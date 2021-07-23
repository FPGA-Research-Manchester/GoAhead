using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using GoAhead.FPGA;
using GoAhead.FPGA.Slices;
using GoAhead.Code.XDL.ResourceDescription;
using GoAhead.Objects;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Read a textual FPGA description (Vivado)", Wrapper = true)]
    class ReadVivadoFPGA : Command
    {
        protected override void DoCommandAction()
        {
            // reset PRIOR to reading to reset high lighter 
            CommandExecuter.Instance.Execute(new Reset());

            FPGA.FPGA.Instance.Reset();

            FPGA.FPGA.Instance.BackendType = FPGATypes.BackendType.Vivado;

            // create reader & open file
            StreamReader sr = new StreamReader(FileName);

            FileInfo fi = new FileInfo(FileName);
            long charCount = 0;
            long lineCount = 0;
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                lineCount++;
                charCount += line.Length;
                if (PrintProgress)
                {
                    ProgressInfo.Progress = (int)(((double)charCount / (double)fi.Length) * (ExcludePipsToBidirectionalWiresFromBlocking ? 50 : 100));
                }

                if (m_commentRegexp.IsMatch(line))
                {
                    continue;
                }

                int length = line.IndexOf('=');
                string prefix = line.Substring(0, length);
                switch (prefix)
                {
                    case "Device" : 
                        Watch.Start("ProcessDevice");
                        ProcessDevice(line);
                        Watch.Stop("ProcessDevice");
                        break;
                    case "R":
                        Watch.Start("ProcessTile");
                        ProcessTile(line);
                        Watch.Stop("ProcessTile");
                        break;
                    case "Site":
                        Watch.Start("ProcessSite");
                        ProcessSite(line);
                        Watch.Stop("ProcessSite");
                        break;
                    case "Pips":
                        Watch.Start("ProcessPips");
                        ProcessPips(line);
                        Watch.Stop("ProcessPips");
                        break;
                    case "Wire":
                        Watch.Start("ProcessWire");
                        ProcessWires(line, sr, ref charCount, fi.Length);
                        Watch.Stop("ProcessWire");
                        break;
                    default:
                    {
                        throw new ArgumentException("Unknown line type: " + line + " (line" + lineCount + ")");
                    }
                }
            }
            sr.Close();
            ReadVivadoFPGADebugger.CloseStream();

            WireList emptyWl = new WireList();
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => t.WireList == null))
            {
                XDLTileParser.StoreAndShareWireList(tile, emptyWl);
            }

            if (ExcludePipsToBidirectionalWiresFromBlocking)
            {
                ExcludePipsToBidirectionalWiresFromBlocking exclCmd = new ExcludePipsToBidirectionalWiresFromBlocking();
                exclCmd.Profile = Profile;
                exclCmd.PrintProgress = PrintProgress;
                exclCmd.ProgressStart = 50;
                exclCmd.ProgressShare = 50;
                exclCmd.FileName = "";
                CommandExecuter.Instance.Execute(exclCmd);
            }

            CommandExecuter.Instance.Execute(new Reset());

            // no LoadFPGAFamilyScript here! LoadFPGAFamilyScript is called through Reset

            // remember for other stuff how we read in this FPGA
            Blackboard.Instance.LastLoadCommandForFPGA = ToString();

        }

        private void ProcessDevice(string line)
        {
            if (line.Length < 5 || !line.Contains('='))
            {
                throw new ArgumentException("Error in device line " + line);
            }
            string[] pairs = line.Split('=');
            string prefix = pairs[1].Substring(0, 4);
            switch (prefix)
            {
                case "xcku":
                case "xcvu":
                case "xczu":
                case "xqku": { FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.UltraScale; break; }
                case "xc7k": { FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.Kintex7; break; }
                case "xc7a":
                case "xa7a": { FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.Artix7; break; }
                case "xa7z":
                case "xc7z": { FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.Zynq; break; }
                case "xc7v": { FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.Virtex7; break; }
                default: { throw new ArgumentException("Unsupported device " + line); }
            }
            FPGA.FPGA.Instance.DeviceName = pairs[1];
        }

        private void ProcessTile(string line)
        {
            // R=40,C=10,Name=CLBLL_L_X2Y161,Sites=SLICE_X1Y161 SLICE_X0Y161
            string[] pairs = line.Split(',');

            int r = -1;
            int c = -1;
            string cr = "unknown";
            string name = "";
            List<string> siteNames = new List<string>();
            List<string> siteTypes = new List<string>();
            foreach (string pair in pairs)
            {
                string[] atoms = pair.Split('=');
                if (atoms.Length != 2)
                {
                    throw new ArgumentException("Error in line " + line);
                }
                switch (atoms[0])
                {
                    case "R":
                        r = int.Parse(atoms[1]);
                        break;
                    case "C":
                        c = int.Parse(atoms[1]);
                        break;
                    case "Name":
                        name = atoms[1];
                        break;
                    case "Sites":
                        siteNames.AddRange(atoms[1].Split(' ').Where(s => !string.IsNullOrEmpty(s)));
                        break;
                    case "Types":
                        siteTypes.AddRange(atoms[1].Split(' ').Where(s => !string.IsNullOrEmpty(s)));
                        break;
                    case "ClockRegion":
                        cr = atoms[1];
                        break;
                }
            }

            if (siteNames.Count != siteTypes.Count)
            {
                throw new ArgumentException("The number of site names does not match the number of site types in line " + line);
            }

            TileKey key = new TileKey(c, r);
            Tile tile = new Tile(key, name);
            tile.ClockRegion = cr;
            FPGA.FPGA.Instance.Add(tile);

            for(int i=0;i<siteNames.Count;i++)
            {
                Slice s = Slice.GetSlice(tile, siteNames[i], siteTypes[i]);
                tile.Slices.Add(s);

                m_sliceNames.Add(siteNames[i], s);
            }
        }

        private void ProcessSite(string line)
        {            
            // Site=TIEOFF_X18Y107 with HARD0GND/0 HARD1VCC/1
            string[] pairs = line.Split(',');
            Slice slice = null;
            InPortOutPortMapping portMapping = new InPortOutPortMapping();
            
            foreach (string pair in pairs)
            {
                int index = pair.IndexOf('=');
                if (index <= 0)
                {
                    throw new ArgumentException("Unexpected line " + pair);
                }

                string prefix = pair.Substring(0, index);
                string data = pair.Substring(index + 1, pair.Length - index - 1);

                switch (prefix)
                {
                    case "Site":
                        // GetSlice has no index structure -> most processing time is spent here
                        //slice = FPGA.FPGA.Instance.GetSlice(data);
                        slice = m_sliceNames[data];
                        break;
                    case "Inpins":
                        ExtractPins(data, portMapping, FPGATypes.PortDirection.In, slice);
                        break;
                    case "Outpins":
                        ExtractPins(data, portMapping, FPGATypes.PortDirection.Out, slice);
                        break;
                }
            }
            // share in out port mappings
            if (!FPGA.FPGA.Instance.Contains(portMapping))
            {
                FPGA.FPGA.Instance.Add(portMapping);
            }
            int hashCode = portMapping.GetHashCode();
            slice.InPortOutPortMappingHashCode = hashCode;
        }

        private void ExtractPins(string pins, InPortOutPortMapping pm, FPGATypes.PortDirection direction, Slice slice)
        {
            string[] pinPaths = pins.Split(' ');
            foreach (string pinPath in pinPaths.Where(s => !string.IsNullOrEmpty(s)))
            {
                int index = pinPath.IndexOf('/');
                if (index <= 0)
                {
                    throw new ArgumentException("Unexpected line " + pinPath);
                }

                string bel = pinPath.Substring(0, index);
                string pin = pinPath.Substring(index+1, pinPath.Length - index - 1);
                Port p = new Port(pin);
                pm.AddSlicePort(p, direction);
                pm.Add(bel, p);
                slice.AddBel(bel);

                /*
                string[] components = pinPath.Split('/');               
                Port p = new Port(components[1]);
                pm.AddSlicePort(p, direction);
                pm.Add(components[0], p);
                slice.AddBel(components[0]);
                */
            }
        }

        private void ProcessPips(string line)
        {
            string[] pairs = line.Split(',');
            if (pairs.Length != 2)
            {
                throw new ArgumentException("Error in line " + line);
            }
            int startIndex = pairs[0].IndexOf('=');
            string tileName = pairs[0].Remove(0, startIndex + 1);
            Tile t = FPGA.FPGA.Instance.GetTile(tileName);
            SwitchMatrix sm = new SwitchMatrix();

            string[] switching = pairs[1].Split(' ');

            foreach (string entry in switching.Where(s => !string.IsNullOrEmpty(s)))
            {
                // "LIOI3_SING.IOI_BYP6_0->IOI_IDELAY0_CINVCTRL"
                int fromEnd = entry.IndexOf('-');
                int toStart = entry.LastIndexOf('>'); // last due to ->>              

                string from = entry.Substring(0, fromEnd);
                bool biDirectional = from.EndsWith("<<");
                // handle LH0<<->>LH12
                while (from.EndsWith(">") || from.EndsWith("<"))
                {
                    from = from.Remove(from.Length - 1);
                }
                string to = entry.Substring(toStart + 1);
                Port p1 = new Port(from);
                Port p2 = new Port(to);
                sm.Add(p1, p2);

                if (biDirectional && WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.BiDirectionalPips))
                {
                    sm.Add(p2, p1);
                }
            }
            XDLTileParser.StoreAndShareSwitchMatrix(t, sm);
        }

        private void ProcessWires(string line, StreamReader sr, ref long currentcharCount, long totalCharCount)
        {
            string currentTileName = "";
            Tile currentTile = null;

            WireList wl = new WireList();
            string wireLine = line;

            WireHelper wireHelper = new WireHelper();

            do
            {
                currentcharCount += wireLine.Length;
                if (PrintProgress)
                {
                    ProgressInfo.Progress = (int)(((double)currentcharCount / (double)totalCharCount) * (ExcludePipsToBidirectionalWiresFromBlocking ? 50 : 100));
                }

                if (m_commentRegexp.IsMatch(wireLine))
                {
                    Console.WriteLine(wireLine);
                    continue;
                }

                int equalIndex = wireLine.IndexOf('=');
                int sepIndex = wireLine.IndexOf('>', equalIndex);
                string left = wireLine.Substring(equalIndex + 1, sepIndex - equalIndex - 2);

                int slashIndexLeft = left.IndexOf("/");
                string fromTile = left.Substring(0, slashIndexLeft);
                string fromPip = left.Substring(slashIndexLeft + 1);//, left.Length - slashIndexLeft-1);

                if (string.IsNullOrEmpty(currentTileName))
                {
                    currentTileName = fromTile;
                    currentTile = FPGA.FPGA.Instance.GetTile(currentTileName);
                }
                else if (!fromTile.Equals(currentTileName))
                {
                    if (currentTile.WireList != null)
                    {
                        throw new ArgumentException("Wirelist should be null");
                    }
                    XDLTileParser.StoreAndShareWireList(currentTile, wl);
                    currentTileName = fromTile;
                    currentTile = FPGA.FPGA.Instance.GetTile(currentTileName);
                   
                    wl = new WireList();
                }

                string right = wireLine.Substring(sepIndex + 1);//, wireLine.Length - sepIndex - 1);
                int slashIndexRight = right.IndexOf("/");
                string toTile = right.Substring(0, slashIndexRight);
                string toPip = right.Substring(slashIndexRight + 1);//, right.Length - slashIndexRight - 1);

                Tile tile = FPGA.FPGA.Instance.GetTile(fromTile);
                Tile target = FPGA.FPGA.Instance.GetTile(toTile);

                uint localPipKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(fromPip);

                if (WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.WiresTrajectoriesData)) 
                    tile.AddWireTrajectoryData(localPipKey, FPGA.FPGA.Instance.IdentifierListLookup.GetKey(toTile));
                
                if (!currentTile.SwitchMatrix.Contains(fromPip))
                {
                    if (!WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.BELOutWires) ||
                        !wireHelper.IsBELOutPip(currentTile, fromPip))
                    {
                        ReadVivadoFPGADebugger.DebugWire(fromTile, fromPip, toTile, toPip);
                        continue;
                    }
                }

                short xIncr = (short)(target.TileKey.X - currentTile.TileKey.X);
                short yIncr = (short)(target.TileKey.Y - currentTile.TileKey.Y);

                // Check if we should consider U-turn wires
                bool condition = WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.UTurnWires);
                condition = condition ? fromTile != toTile || fromPip != toPip : xIncr != 0 || yIncr != 0;
                if (!condition)
                {
                    ReadVivadoFPGADebugger.DebugWire(fromTile, fromPip, toTile, toPip);
                    continue;
                }

                if (!target.SwitchMatrix.Contains(toPip))
                {
                    if (!WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.BELInWires) ||
                        !wireHelper.IsBELInPip(target, toPip))
                    {
                        ReadVivadoFPGADebugger.DebugWire(fromTile, fromPip, toTile, toPip);
                        continue;
                    }
                }

                uint pipOnOtherTileKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(toPip);
                Port from = new Port(fromPip);
                //Port to = new Port(toPip);
                bool fromIsBegin = currentTile.SwitchMatrix.ContainsRight(from);
                Wire w = new Wire(localPipKey, pipOnOtherTileKey, fromIsBegin, xIncr, yIncr);
                wl.Add(w);

                if (WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.IncomingWires))
                    target.AddIncomingWire(w);
            }
            while ((wireLine = sr.ReadLine()) != null);

            wireHelper.ProcessStopoverArcs();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private readonly Regex m_commentRegexp = new Regex(@"^\s*#", RegexOptions.Compiled);

        [Parameter(Comment = "Whether run a ExcludePipsToBidirectionalWiresFromBlocking command after reading")]
        public bool ExcludePipsToBidirectionalWiresFromBlocking = true;

        [Parameter(Comment = "The file to read in")]
        public string FileName = "xc7k70tfbg676.viv_rpt";

        /// <summary>
        /// public index  
        /// </summary>
        private Dictionary<string, Slice> m_sliceNames = new Dictionary<string, Slice>();
    }
}
