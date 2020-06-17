using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description="Clear the current selection and select a fence around a user selection (for blocking a fence)", Wrapper=true, Publish=true)]
    class SelectFenceAroundUserSelection : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {           
            Tile ul = FPGA.TileSelectionManager.Instance.GetUserSelectedTile("", this.UserSelectionType, FPGATypes.Placement.UpperLeft);
            Tile lr = FPGA.TileSelectionManager.Instance.GetUserSelectedTile("", this.UserSelectionType, FPGATypes.Placement.LowerRight);

            List<Tile> fenceTiles = new List<Tile>();
            for (int x = ul.TileKey.X - this.Size; x <= lr.TileKey.X + this.Size; x++)
            {
                for (int y = ul.TileKey.Y - this.Size; y <= lr.TileKey.Y + this.Size; y++)
                {
                    TileKey key = new TileKey(x, y);
                   
                    if(!FPGA.TileSelectionManager.Instance.IsUserSelected(key, this.UserSelectionType) && FPGA.FPGA.Instance.Contains(x, y))
                    {
                        Tile t = FPGA.FPGA.Instance.GetTile(key);
                        bool resMatch =
                            Objects.IdentifierManager.Instance.IsMatch(t.Location, Objects.IdentifierManager.RegexTypes.Interconnect) ||
                            Objects.IdentifierManager.Instance.IsMatch(t.Location, Objects.IdentifierManager.RegexTypes.CLB) ||
                            Objects.IdentifierManager.Instance.IsMatch(t.Location, Objects.IdentifierManager.RegexTypes.DSP) ||
                            Objects.IdentifierManager.Instance.IsMatch(t.Location, Objects.IdentifierManager.RegexTypes.BRAM);
                        // prevent RUG-Creation by only adding "well known" tiles
                        if (resMatch)
                        {
                            fenceTiles.Add(t);
                        }
                    }
                }
            }

            ClearSelection clearCmd = new ClearSelection();
            CommandExecuter.Instance.Execute(clearCmd);

            foreach (Tile t in fenceTiles)
            {
                AddToSelectionLoc addCmd = new AddToSelectionLoc();
                addCmd.Location = t.Location;
                CommandExecuter.Instance.Execute(addCmd);
            }

            ExpandSelection expandCmd = new ExpandSelection();
            CommandExecuter.Instance.Execute(expandCmd);
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The name of the user selection type to select around")]
        public String UserSelectionType = "PartialArea";

        [Parameter(Comment = "The size of the fence in tiles")]
        public int Size = 5;

    }
}
