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
            NetlistContainer netlistContainer = this.GetNetlistContainer();

            foreach (Tile probe in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                if (probe.SwitchMatrix.GetAllArcs().Any(tupel => tupel.Item1.Name.Equals(this.From) && tupel.Item2.Name.Equals(this.To)))
                {
                    XDLNet n = new XDLNet(probe.Location + "_" + this.From + "_" + this.To);
                    n.Add(this.CommentForPip);
                    n.Add(probe, new Port(this.From), new Port(this.To));
                    /*
                    probe.BlockPort(new Port(this.From), false);
                    probe.BlockPort(new Port(this.To), false);*/
                    netlistContainer.Add(n);
                }
                else
                {
                    this.OutputManager.WriteOutput("Warning: Arc " + this.From + " -> " + this.To + " not found on tile " + probe.Location);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The driver")]
        public String From = "EE2B0";

        [Parameter(Comment = "This comment will be added to the pip")]
        public String CommentForPip = "added_by_AddArcs";

        [Parameter(Comment = "The sink")]
        public String To = "EE2E0";
    }
}
