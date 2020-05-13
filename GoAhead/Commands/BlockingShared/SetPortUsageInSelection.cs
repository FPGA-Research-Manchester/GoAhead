using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Set the usage of the specified port within all currently selected tiles", Wrapper = true)]
    class SetPortUsageInSelection : Command 
    {
        protected override void DoCommandAction()
        {
            int tileCount = 0;

            bool unblock = PortUsage == "Free";

            if(!Enum.TryParse(PortUsage, out Tile.BlockReason blockReason) && !unblock)
            {
                throw new ArgumentException("Invalid port usage provided");
            }

            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                ProgressInfo.Progress = ProgressStart + (int)((double)tileCount++ / (double)TileSelectionManager.Instance.NumberOfSelectedTiles * ProgressShare);

                if(t.SwitchMatrix.Contains(PortName))
                {
                    if (unblock) t.UnblockPort(PortName);
                    else t.BlockPort(PortName, blockReason);
                }
                else if(CheckForExistence)
                {
                    throw new ArgumentException("Port " + PortName + " does not exist in tile " + t.Location);
                }

                if(IncludeReachablePorts)
                {
                    foreach (Location l in Navigator.GetDestinations(t.Location, PortName))
                    {
                        if (unblock) l.Tile.UnblockPort(l.Pip);
                        else l.Tile.BlockPort(l.Pip, blockReason);
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The port name to be blocked, e.g. E2BEG5")]
        public string PortName = "";

        [Parameter(Comment = "Check for existence of ports")]
        public bool CheckForExistence = false;

        [Parameter(Comment = "Wheter to follow up wires on a port and also block the reachable ports in other tiles")]
        public bool IncludeReachablePorts = true;

        [Parameter(Comment = "Port usage to set. Options: Blocked, ExcludedFromBlocking, OccupiedByMacro, Stopover, ToBeBlocked")]
        public string PortUsage = "";
        
    }
}
