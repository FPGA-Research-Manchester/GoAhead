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

            FPGA.FPGA.Instance.BackendType = FPGA.FPGATypes.BackendType.Vivado;

            // create reader & open file
            StreamReader sr = new StreamReader(this.FileName);

            FileInfo fi = new FileInfo(this.FileName);
            long charCount = 0;
            long lineCount = 0;
            String line = "";
            while ((line = sr.ReadLine()) != null)
            {
                lineCount++;
                charCount += line.Length;
                if (this.PrintProgress)
                {
                    this.ProgressInfo.Progress = (int)(((double)charCount / (double)fi.Length) * (this.ExcludePipsToBidirectionalWiresFromBlocking ? 50 : 100));
                }

                if (this.m_commentRegexp.IsMatch(line))
                {
                    continue;
                }

                int length = line.IndexOf('=');
                String prefix = line.Substring(0, length);
                switch (prefix)
                {
                    case "Device" : 
                        this.Watch.Start("ProcessDevice");
                        this.ProcessDevice(line);
                        this.Watch.Stop("ProcessDevice");
                        break;
                    case "R":
                        this.Watch.Start("ProcessTile");
                        this.ProcessTile(line);
                        this.Watch.Stop("ProcessTile");
                        break;
                    case "Site":
                        this.Watch.Start("ProcessSite");
                        this.ProcessSite(line);
                        this.Watch.Stop("ProcessSite");
                        break;
                    case "Pips":
                        this.Watch.Start("ProcessPips");
                        this.ProcessPips(line);
                        this.Watch.Stop("ProcessPips");
                        break;
                    case "Wire":
                        this.Watch.Start("ProcessWire");
                        this.ProcessWires(line, sr, ref charCount, fi.Length);
                        this.Watch.Stop("ProcessWire");
                        break;
                    default:
                    {
                        throw new ArgumentException("Unknown line type: " + line + " (line" + lineCount + ")");
                    }
                }
            }
            sr.Close();

            WireList emptyWl = new WireList();
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => t.WireList == null))
            {
                XDLTileParser.StoreAndShareWireList(tile, emptyWl);
            }

            if (this.ExcludePipsToBidirectionalWiresFromBlocking)
            {
                ExcludePipsToBidirectionalWiresFromBlocking exclCmd = new ExcludePipsToBidirectionalWiresFromBlocking();
                exclCmd.Profile = this.Profile;
                exclCmd.PrintProgress = this.PrintProgress;
                exclCmd.ProgressStart = 50;
                exclCmd.ProgressShare = 50;
                exclCmd.FileName = "";
                Commands.CommandExecuter.Instance.Execute(exclCmd);
            }

            Commands.CommandExecuter.Instance.Execute(new Reset());

            // no LoadFPGAFamilyScript here! LoadFPGAFamilyScript is called through Reset

            // remember for other stuff how we read in this FPGA
        
            Objects.Blackboard.Instance.LastLoadCommandForFPGA = this.ToString();
        }

        private void ProcessDevice(String line)
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

        private void ProcessTile(String line)
        {
            // R=40,C=10,Name=CLBLL_L_X2Y161,Sites=SLICE_X1Y161 SLICE_X0Y161
            String[] pairs = line.Split(',');

            int r = -1;
            int c = -1;
            string cr = "unknown";
            String name = "";
            List<string> siteNames = new List<string>();
            List<string> siteTypes = new List<string>();
            foreach (string pair in pairs)
            {
                String[] atoms = pair.Split('=');
                if (atoms.Length != 2)
                {
                    throw new ArgumentException("Error in line " + line);
                }
                switch (atoms[0])
                {
                    case "R":
                        r = Int32.Parse(atoms[1]);
                        break;
                    case "C":
                        c = Int32.Parse(atoms[1]);
                        break;
                    case "Name":
                        name = atoms[1];
                        break;
                    case "Sites":
                        siteNames.AddRange(atoms[1].Split(' ').Where(s => !String.IsNullOrEmpty(s)));
                        break;
                    case "Types":
                        siteTypes.AddRange(atoms[1].Split(' ').Where(s => !String.IsNullOrEmpty(s)));
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

                this.m_sliceNames.Add(siteNames[i], s);
            }
        }

        private void ProcessSite(String line)
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
                        slice = this.m_sliceNames[data];
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

        private void ExtractPins(String pins, InPortOutPortMapping pm, FPGATypes.PortDirection direction, Slice slice)
        {
            string[] pinPaths = pins.Split(' ');
            foreach (string pinPath in pinPaths.Where(s => !String.IsNullOrEmpty(s)))
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

        private void ProcessPips(String line)
        {
            String[] pairs = line.Split(',');
            if (pairs.Length != 2)
            {
                throw new ArgumentException("Error in line " + line);
            }
            int startIndex = pairs[0].IndexOf('=');
            string tileName = pairs[0].Remove(0, startIndex + 1);
            Tile t = FPGA.FPGA.Instance.GetTile(tileName);
            SwitchMatrix sm = new SwitchMatrix();

            String[] switching = pairs[1].Split(' ');

            foreach (String entry in switching.Where(s => !String.IsNullOrEmpty(s)))
            {
                // "LIOI3_SING.IOI_BYP6_0->IOI_IDELAY0_CINVCTRL"
                int fromEnd = entry.IndexOf('-');
                int toStart = entry.LastIndexOf('>'); // last due to ->>              

                String from = entry.Substring(0, fromEnd);
                // handle LH0<<->>LH12
                while (from.EndsWith(">") || from.EndsWith("<"))
                {
                    from = from.Remove(from.Length - 1);
                }
                String to = entry.Substring(toStart + 1);
                sm.Add(new Port(from), new Port(to));

                // TODO bidirectional pip if (pipOperator.Equals("-->"))
            }
            XDLTileParser.StoreAndShareSwitchMatrix(t, sm);
        }

        private void ProcessWires(String line, StreamReader sr, ref long currentcharCount, long totalCharCount)
        {
            string currentTileName = "";
            Tile currentTile = null;

            WireList wl = new WireList();
            String wireLine = line;

            WireHelper wireHelper = new WireHelper();

            do
            {
                currentcharCount += wireLine.Length;
                if (this.PrintProgress)
                {
                    this.ProgressInfo.Progress = (int)(((double)currentcharCount / (double)totalCharCount) * (this.ExcludePipsToBidirectionalWiresFromBlocking ? 50 : 100));
                }

                if (this.m_commentRegexp.IsMatch(wireLine))
                {
                    continue;
                }

                int equalIndex = wireLine.IndexOf('=');
                int sepIndex = wireLine.IndexOf('>', equalIndex);
                String left = wireLine.Substring(equalIndex + 1, sepIndex - equalIndex - 2);

                int slashIndexLeft = left.IndexOf("/");
                String fromTile = left.Substring(0, slashIndexLeft);
                String fromPip = left.Substring(slashIndexLeft + 1);//, left.Length - slashIndexLeft-1);

                if (String.IsNullOrEmpty(currentTileName))
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

                String right = wireLine.Substring(sepIndex + 1);//, wireLine.Length - sepIndex - 1);
                int slashIndexRight = right.IndexOf("/");
                String toTile = right.Substring(0, slashIndexRight);
                String toPip = right.Substring(slashIndexRight + 1);//, right.Length - slashIndexRight - 1);

                Tile tile = FPGA.FPGA.Instance.GetTile(fromTile);
                Tile target = FPGA.FPGA.Instance.GetTile(toTile);

                uint localPipKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(fromPip);

                if (WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.WiresTrajectoriesData)) 
                    tile.AddWireTrajectoryData(localPipKey, FPGA.FPGA.Instance.IdentifierListLookup.GetKey(toTile));
                
                if (!currentTile.SwitchMatrix.Contains(fromPip))
                {
                    if (!WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.BELOutWires)) continue;
                    if (!wireHelper.IsBELOutPip(currentTile, fromPip)) continue;
                }

                short xIncr = (short)(target.TileKey.X - currentTile.TileKey.X);
                short yIncr = (short)(target.TileKey.Y - currentTile.TileKey.Y);

                // Check if we should consider U-turn wires
                bool condition = WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.UTurnWires);
                condition = condition ? fromTile != toTile || fromPip != toPip : xIncr != 0 || yIncr != 0;
                if (!condition) continue;

                if (!target.SwitchMatrix.Contains(toPip))
                {
                    if (!WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.BELInWires)) continue;
                    if (!wireHelper.IsBELInPip(target, toPip)) continue;
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
        public String FileName = "xc7k70tfbg676.viv_rpt";

        /// <summary>
        /// public index  
        /// </summary>
        private Dictionary<String, Slice> m_sliceNames = new Dictionary<string, Slice>();
    }
}
