using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.VHDL;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.InterfaceManager
{
    [CommandDescription(Description = "Reads in an placed XDL netlist, extracts its interface and store the interface as a CSV", Wrapper = false, Publish = true)]
    class PrintInterfaceCSVFromXDLNetlist : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            DesignParser parser = DesignParser.CreateDesignParser(this.XDLFile);
            XDLContainer container = new XDLContainer();
            parser.ParseDesign(container, this);

            VHDLParser moduleParser = new VHDLParser(this.VHDLModule);
            VHDLParserEntity ent = moduleParser.GetEntity(0);

            Dictionary<int, List<Signal>> east = new Dictionary<int, List<Signal>>();
            Dictionary<int, List<Signal>> west = new Dictionary<int, List<Signal>>();

            double xCenter, yCenter;
            FPGA.TileSelectionManager.Instance.GetCenterOfSelection(t => FPGA.TileSelectionManager.Instance.IsSelected(t.TileKey), out xCenter, out yCenter);

            foreach (HDLEntitySignal signal in ent.InterfaceSignals)
            {
                foreach (XDLNet net in container.Nets.Where(n => n.Name.StartsWith(signal.SignalName) && ((XDLNet) n).HasIndex()).OrderBy(n => ((XDLNet) n).GetIndex()))
                {
                    Tile fromTile;
                    Tile toTile;
                    this.GetSourceAndSink(container, net, out fromTile, out toTile);

                    this.GetSourceAndSink(container, net, out fromTile, out toTile);

                    Tile innerTile = null;
                    Tile outerTile = null;
                    String signalMode = "";
                    if (!FPGA.TileSelectionManager.Instance.IsSelected(fromTile.TileKey) && FPGA.TileSelectionManager.Instance.IsSelected(toTile.TileKey))
                    {
                        innerTile = toTile;
                        outerTile = fromTile;
                        signalMode = "in";
                    }
                    else if (FPGA.TileSelectionManager.Instance.IsSelected(fromTile.TileKey) && !FPGA.TileSelectionManager.Instance.IsSelected(toTile.TileKey))
                    {
                        outerTile = toTile;
                        innerTile = fromTile;
                        signalMode = "out";
                    }
                    else
                    {
                        throw new ArgumentException("Expecting an instance inside the current selection");
                    }

                    FPGATypes.InterfaceDirection dir = outerTile.TileKey.X < (int)xCenter ? FPGATypes.InterfaceDirection.East : FPGATypes.InterfaceDirection.West;

                    Dictionary<int, List<Signal>> signalCollection = dir.Equals(FPGATypes.InterfaceDirection.East) ? east : west;
                    if(!signalCollection.ContainsKey(innerTile.TileKey.Y))
                    {
                        signalCollection.Add(innerTile.TileKey.Y, new List<Signal>());
                    }

                    Signal s = new Signal();
                    s.Column = -1;
                    s.SignalDirection = dir;
                    s.SignalMode = signalMode;
                    s.SignalName = net.Name;

                    signalCollection[innerTile.TileKey.Y].Add(s);

                    // weiter: vor verlaesst das gummiband die partielle flaeche?
                    // vektoren nach osten oder westen?
                }
            }


            bool interleaveEast = east.Any(t => t.Value.Count > 4);
            bool interleaveWest = west.Any(t => t.Value.Count > 4);

            Dictionary<FPGA.FPGATypes.Direction, Dictionary<int, List<Signal>>> interfaces = new Dictionary<FPGATypes.Direction, Dictionary<int, List<Signal>>>();
            interfaces.Add(FPGATypes.Direction.East, new Dictionary<int, List<Signal>>());
            interfaces[FPGATypes.Direction.East][0] = new List<Signal>();
            interfaces[FPGATypes.Direction.East][1] = new List<Signal>();
            interfaces.Add(FPGATypes.Direction.West, new Dictionary<int, List<Signal>>());
            interfaces[FPGATypes.Direction.West][0] = new List<Signal>();
            interfaces[FPGATypes.Direction.West][1] = new List<Signal>();


            if (interleaveEast)
            {
                int columnIndex = 0;
                foreach (KeyValuePair<int, List<Signal>> tupel in east)
                {
                    foreach (Signal s in tupel.Value)
                    {
                        Signal copy = new Signal(s.SignalName, s.SignalMode, s.SignalDirection, "", columnIndex);
                        interfaces[FPGATypes.Direction.East][columnIndex].Add(copy);

                        columnIndex++;
                        columnIndex %= 2;
                    }
                }
            }

                //ent.InterfaceSignals
        }

       private void GetSourceAndSink(NetlistContainer container, XDLNet net, out Tile source, out Tile sink)
        {
            // where is the outpin
            NetOutpin outPin = (NetOutpin)net.NetPins.Where(p => p is NetOutpin).First();
            source = this.GetTile(container, outPin);

            List<Tile> tilesWithInpins = new List<Tile>();

            if (FPGA.TileSelectionManager.Instance.IsSelected(source.TileKey))
            {
                foreach (NetPin pin in net.NetPins.Where(p => p is NetInpin && !this.IsInside(container, p)))
                {
                    tilesWithInpins.Add(this.GetTile(container, pin));
                }
            }
            else
            {
                foreach (NetPin pin in net.NetPins.Where(p => p is NetInpin && this.IsInside(container, p)))
                {
                    tilesWithInpins.Add(this.GetTile(container, pin));
                }
            }
            double x, y;
            FPGA.TileSelectionManager.Instance.GetCenterOfTiles(tilesWithInpins, out x, out y);
            sink = FPGA.FPGA.Instance.GetTile((int)x, (int)y);
        }

        private bool IsInside(NetlistContainer container, NetPin pin)
        {
            Tile instanceTile = this.GetTile(container, pin);
            return FPGA.TileSelectionManager.Instance.IsSelected(instanceTile.TileKey);
        }

        private Tile GetTile(NetlistContainer container, NetPin pin)
        {
            Instance instance = container.GetInstanceByName(pin.InstanceName);
            Tile instanceTile = FPGA.FPGA.Instance.GetTile(instance.Location);
            return instanceTile;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The placed XDL netlist to read in")]
        public String XDLFile = "top_map.xdl";

        [Parameter(Comment = "The VHDL file with the moduel entity")]
        public String VHDLModule = "module.vhd";
    }
}
