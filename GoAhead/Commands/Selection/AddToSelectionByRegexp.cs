using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Add all tiles to the current selection whose identifier matches the given filter", Wrapper = false)]
    class AddToSelectionByRegexp : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {
            Regex filter = new Regex(Filter, RegexOptions.Compiled);
            foreach (Tile where in FPGA.FPGA.Instance.GetAllTiles().Where(t => filter.IsMatch(t.Location)))
            {
                //deselect or add the selected tile 
                if (TileSelectionManager.Instance.IsSelected(where.TileKey))
                {
                    TileSelectionManager.Instance.RemoveFromSelection(where.TileKey, true);
                }
                else
                {
                    TileSelectionManager.Instance.AddToSelection(where.TileKey);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "All tiles that match this regular expression will be added")]
        public string Filter = "";
    }
}
