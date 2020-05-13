using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Select a fence around the current selection for blocking", Wrapper = false, Publish = true)]
    class SelectFence : Command
    {
        protected override void DoCommandAction()
        {
            TileSet fence = new TileSet();
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles().Where(t => t.WireList != null))
            {
                foreach (Wire wire in tile.WireList.Where(w => w.LocalPipIsDriver))
                {
                    foreach(Location loc in Navigator.GetDestinations(tile.Location, wire.LocalPip).Where(l => !TileSelectionManager.Instance.IsSelected(l.Tile)))
                    {
                        if (!fence.Contains(loc.Tile))
                        {
                            fence.Add(loc.Tile);
                        }
                    }
                }
            }
           

            CommandExecuter.Instance.Execute(new Commands.Selection.ClearSelection());
            foreach(Tile t in fence)
            {
                TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
            }
            CommandExecuter.Instance.Execute(new Commands.Selection.ExpandSelection());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        //[Parameter(Comment = "Whether or not exclude ports from blocking that can not be used to re-enter the current selection. Enabling this parameter may lead to smaller blockers.")]
        //public bool ExcludeNonReEntryPortsFromBlocking = false;
    }
}
