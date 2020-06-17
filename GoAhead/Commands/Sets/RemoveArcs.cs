using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Sets
{
    [CommandDescription(Description = "Remove all arcs in all nets that reside in the current selection AND that include a port that matches PortNameFilter")]
    class RemoveArcs : Command
    {
        public RemoveArcs()
        {
            this.PortNameFilter = "";
        }

        public RemoveArcs(String portNameFilter)
        {
            this.PortNameFilter = portNameFilter;
        }

        protected override void DoCommandAction()
        {
            Regex filter = new Regex(this.PortNameFilter, RegexOptions.Compiled);

            foreach (Tile where in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                foreach (NetlistContainer netlistContainer in NetlistContainerManager.Instance.NetlistContainer)
                {
                    foreach (XDLNet net in netlistContainer.Nets)
                    {
                        net.Remove(pip => pip.Location.Equals(where.Location) && (filter.IsMatch(pip.From) || filter.IsMatch(pip.To)));
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "If an arc contains this port it will be removed")]
        public String PortNameFilter = "EE2";
    }
}
