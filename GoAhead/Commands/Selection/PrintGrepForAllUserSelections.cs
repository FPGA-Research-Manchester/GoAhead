using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Print a grep command that matches on all user selected tiles. This grep may be used to analyse e.g. pips in certain areas", Wrapper = true)]
    class PrintGrepForAllUserSelections : Command
    {
        public override void Do()
        {
            StringBuilder buffer = new StringBuilder();
            bool firstTile = true;
            foreach (String userSel in FPGA.TileSelectionManager.Instance.UserSelectionTypes)
            {
                foreach (Tile tile in FPGA.TileSelectionManager.Instance.GetAllUserSelectedTiles(userSel).Where(t => Regex.IsMatch(t.Location, this.TileFilter)))
                {
                    if (!firstTile)
                    {
                        buffer.Append("|");
                    }
                    else
                    {
                        firstTile = false;
                    }
                    buffer.Append("(" + tile.Location + ")");
                }
            }

            this.WriteOutput("grep " + buffer.ToString());
        }

        public override void Undo()
        {
        }

        [Paramter(Comment = "Only consider tiles that match this filter")]
        public String TileFilter = "";
    }
}
