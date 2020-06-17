using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    class SelectAllWithFilter : Command
    {
        protected override void DoCommandAction()
        {
            Regex filter = new Regex(this.Filter);

            FPGA.TileSelectionManager.Instance.ClearSelection();
            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles())
            {
                if (!filter.IsMatch(t.Location))
                {
                    continue;
                }
                FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
            }

            FPGA.TileSelectionManager.Instance.SelectionChanged();           
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Only selected those tiles in the given range that match this filter")]
        public String Filter = "^CL";
    }
}
