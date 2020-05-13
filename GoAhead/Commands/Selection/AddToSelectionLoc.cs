using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    class AddToSelectionLoc : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {
            // click done out of fpga range
            if (!FPGA.FPGA.Instance.Contains(Location))
            {
                return;
            }

            Tile where = FPGA.FPGA.Instance.GetTile(Location);

            // deselect or add the selected tile 
            // in comd umwandeln
            if (TileSelectionManager.Instance.IsSelected(where.TileKey))
            {
                TileSelectionManager.Instance.RemoveFromSelection(where.TileKey, Notify);
            }
            else
            {
                TileSelectionManager.Instance.AddToSelection(where.TileKey, Notify);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The location string of the tile to be added to selection, e.g INT_X10Y24")]
        public string Location = "INT_X10Y24";

        /// <summary>
        /// Set to false for multi use (GUI only) set to false for speed up
        /// </summary>
        public bool Notify = true;
    }
}
