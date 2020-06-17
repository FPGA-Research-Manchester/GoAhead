using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.Code.XDL;
using GoAhead.FPGA;

namespace GoAhead.Commands.Debug
{
    class PrintAllNetsInSelection : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = this.GetNetlistContainer();

            foreach (XDLNet net in nlc.Nets)
            {
                bool netCrossesSelection = false;
                foreach (XDLPip pip in net.Pips)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                    if (FPGA.TileSelectionManager.Instance.IsSelected(t.TileKey))
                    {
                        netCrossesSelection = true;
                        break;
                    }
                }
                if (netCrossesSelection)
                {
                    this.OutputManager.WriteOutput(net.Name);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
