using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.GUI.TileView;
using GoAhead.Objects;

namespace GoAhead.Commands.GUI
{
    [CommandDescription(Description = "Opens the Tile View window for the provided Location. If the location is invalid, attempts to find it by the (X,Y) coordinates.", Wrapper = true)]
    class OpenTileView : Command
    {
        [Parameter(Comment = "Tile location, e.g. INT_X4Y75.")]
        public string Location = "";

        [Parameter(Comment = "GoAhead X coordinate for a tile.")]
        public int X;

        [Parameter(Comment = "GoAhead Y coordinate for a tile.")]
        public int Y;

        [Parameter(Comment = "Check if ExpandSelectionMode is ON.")]
        public bool doExpand;

        public override void Undo()
        {
        }

        protected override void DoCommandAction()
        {
            if (!string.IsNullOrEmpty(Location))
            {
                Tile tile = FPGA.FPGA.Instance.GetTile(Location);
                if (tile != null)
                {
                    //Check if expand is activated.
                    if(doExpand)
                    {
                        TileViewForm tileView = new TileViewForm(tile);
                        tileView.Show();
                    }
                    else
                    {
                        SingleTileViewForm tileView = new SingleTileViewForm(tile);
                        tileView.Show();

                    }


                    
                    return;
                }
                Console.WriteLine("Tile Location=" + Location + " doesn't exist. Attempting to find tile by (X,Y) coordinates...");
            }

            if (FPGA.FPGA.Instance.Contains(X, Y))
            {
                if(doExpand)
                {
                    TileViewForm tileView = new TileViewForm(FPGA.FPGA.Instance.GetTile(X, Y));
                    tileView.Show();
                }
                else
                {
                    SingleTileViewForm tileView = new SingleTileViewForm(FPGA.FPGA.Instance.GetTile(X, Y));
                    tileView.Show();
                }
                
            }
            else
            {
                Console.WriteLine("Tile doesn't exist at X=" + X + " Y=" + Y);
            }
        }
    }
}
