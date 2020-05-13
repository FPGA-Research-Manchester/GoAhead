using System;
using System.Linq;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Mark all end pips reachable form all nets in the given netlist container as blocked", Wrapper = false)]
    class ExpandBlockAttribute : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = GetNetlistContainer();

            foreach (XDLNet n in netlistContainer.Nets)
            {
                foreach (XDLPip p in n.Pips)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(p.Location);
                    foreach(Wire wire in t.WireList.Where(w => w.LocalPipIsDriver && w.LocalPip.Equals(p.To)))
                    {
                        Tile dest = Navigator.GetDestinationByWire(t, wire);
                        if (!dest.IsPortBlocked(wire.PipOnOtherTile))
                        {
                            dest.BlockPort(wire.PipOnOtherTile, Tile.BlockReason.Blocked);
                        }
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
