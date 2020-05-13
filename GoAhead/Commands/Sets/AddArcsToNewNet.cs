using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;


namespace GoAhead.Commands.Sets
{
    [CommandDescription(Description = "Add the arc (if existing) specified by From and To to a new net names Tile_From_To in the given macro.", Wrapper = false, Publish = true)]
    class AddArcsToNewNet : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = GetNetlistContainer();

            foreach (Tile probe in TileSelectionManager.Instance.GetSelectedTiles())
            {
                if (probe.SwitchMatrix.GetAllArcs().Any(tupel => tupel.Item1.Name.Equals(From) && tupel.Item2.Name.Equals(To)))
                {
                    XDLNet n = new XDLNet(probe.Location + "_" + From + "_" + To);
                    n.Add(CommentForPip);
                    n.Add(probe, new Port(From), new Port(To));
                    /*
                    probe.BlockPort(new Port(this.From), false);
                    probe.BlockPort(new Port(this.To), false);*/
                    netlistContainer.Add(n);
                }
                else
                {
                    OutputManager.WriteOutput("Warning: Arc " + From + " -> " + To + " not found on tile " + probe.Location);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The driver")]
        public string From = "EE2B0";

        [Parameter(Comment = "This comment will be added to the pip")]
        public string CommentForPip = "added_by_AddArcs";

        [Parameter(Comment = "The sink")]
        public string To = "EE2E0";
    }
}
