using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.FPGA;
namespace GoAhead.Commands.Vivado
{
    class SavePRLinksAsTCL : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            foreach (PRLink link in PRLinkManager.Instance)
            {
                string path = "";
                int index = 0;
                foreach (Port port in link.Ports)
                {
                    path += (index++ == 0 ? link.Tile.Location + "/" : " ") + port.Name + " ";
                }

                string routingConstraint = @"set_property FIXED_ROUTE { GAP " + path + "} [get_nets " + link.NetName + "]" + Environment.NewLine;
                OutputManager.WriteTCLOutput(routingConstraint);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
