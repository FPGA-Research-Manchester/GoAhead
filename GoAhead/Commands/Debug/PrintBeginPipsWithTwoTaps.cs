using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    class PrintBeginPipsWithMoreThanOneTap : Command
    {
        protected override void DoCommandAction()
        {
            Tile t = FPGA.FPGA.Instance.GetTile(Location);
            foreach (Port port in t.SwitchMatrix.GetAllDrivers())
            {
                IEnumerable<Wire> wires = t.WireList.GetAllWires(port).Where(w => w.LocalPipIsDriver);
                if (wires.Count() > 1)
                {
                    foreach (Wire w in wires)
                    {
                        Tile other = Navigator.GetDestinationByWire(t, w);
                        OutputManager.WriteOutput(t.Location + "." + port.Name + " -> " + other.Location + "." + w.PipOnOtherTile);
                    }
                }

            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The tile")]
        public string Location = "INT_X34Y10";
    }
}
