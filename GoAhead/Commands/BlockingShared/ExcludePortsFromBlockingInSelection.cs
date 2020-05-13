using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GoAhead.FPGA;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Prevent future blocking of the specified port within all currently selected tiles", Wrapper = true)]
    class ExcludePortsFromBlockingInSelection : Command 
    {
        protected override void DoCommandAction()
        {
            int tileCount = 0;

            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                ProgressInfo.Progress = ProgressStart + (int)((double)tileCount++ / (double)TileSelectionManager.Instance.NumberOfSelectedTiles * ProgressShare);

                // direct block (fast)
                if (!IncludeAllPorts)
                {
                    ExcludePortsFromBlocking.BlockPort(t, PortName, CheckForExistence);
                }
                else if(t.SwitchMatrix.Contains(PortName))
                {
                    ExcludePortsFromBlocking.BlockPortAndReachablePorts(t.Location, PortName, CheckForExistence, IncludeAllPorts);
                    /*
                    ExcludePortsFromBlocking cmd = new ExcludePortsFromBlocking();
                    cmd.CheckForExistence = this.CheckForExistence;
                    cmd.IncludeAllPorts = this.IncludeAllPorts;
                    cmd.Location = t.Location;
                    cmd.PortName = this.PortName;
                    //cmd.Profile = this.Profile;
                    CommandExecuter.Instance.Execute(cmd);
                     * */
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
        public bool IncludeAllPorts = true;
    }
}
