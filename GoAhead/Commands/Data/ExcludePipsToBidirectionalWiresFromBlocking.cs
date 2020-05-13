using System;
using System.Linq;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Detect pips that drive bidirectional wires and exclude them from blocking (see TileView for results)", Wrapper = true)]
    class ExcludePipsToBidirectionalWiresFromBlocking : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            int tileCount = 0;

            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => t.HasNonstopoverBlockedPorts))
            {
                OutputManager.WriteOutput("Tile " + tile.Location + " already contains ports that are excluded from blocking");
            }

            TileSet tilesWithBidirectionalWires = new TileSet();

            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles())
            {
                ProgressInfo.Progress = ProgressStart + ((int)((double)tileCount++ / (double)FPGA.FPGA.Instance.TileCount * ProgressShare));

                if (t.WireList == null)
                {
                    continue;
                }

                foreach (Wire wire in t.WireList)
                {                                                                                     
                    Tile other = Navigator.GetDestinationByWire(t, wire);
                    foreach(Wire otherWire in other.WireList.Where(w => w.LocalPip.Equals(wire.PipOnOtherTile)))
                    {
                        // bidirectional wire detected
                        if(wire.LocalPipIsDriver && otherWire.LocalPipIsDriver)
                        {
                            if (!tilesWithBidirectionalWires.Contains(t))
                            {
                                tilesWithBidirectionalWires.Add(t);
                            }
                            if (!tilesWithBidirectionalWires.Contains(other))
                            {
                                tilesWithBidirectionalWires.Add(other);
                            }
                            //this.OutputManager.WriteOutput(this.GetType().Name + " Excluding " + wire.LocalPip + " from " + t.Location);
                            //this.OutputManager.WriteOutput(this.GetType().Name + " Excluding " + otherWire.LocalPip + " from " + other.Location);
                            
                            t.BlockPort(wire.LocalPip, Tile.BlockReason.ExcludedFromBlocking);
                            other.BlockPort(otherWire.LocalPip, Tile.BlockReason.ExcludedFromBlocking);
                        }
                    }
                }
            }

            foreach (Tile t in tilesWithBidirectionalWires)
            {                
                OutputManager.WriteOutput("Excluded ports from blocking on " + t.Location);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
