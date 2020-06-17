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

            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                this.ProgressInfo.Progress = this.ProgressStart + (int)((double)tileCount++ / (double)FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles * this.ProgressShare);

                // direct block (fast)
                if (!this.IncludeAllPorts)
                {
                    ExcludePortsFromBlocking.BlockPort(t, this.PortName, this.CheckForExistence);
                }
                else if(t.SwitchMatrix.Contains(this.PortName))
                {
                    ExcludePortsFromBlocking.BlockPortAndReachablePorts(t.Location, this.PortName, this.CheckForExistence, this.IncludeAllPorts);
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
        public String PortName = "";

        [Parameter(Comment = "Check for existence of ports")]
        public bool CheckForExistence = false;

        [Parameter(Comment = "Wheter to follow up wires on a port and also block the reachable ports in other tiles")]
        public bool IncludeAllPorts = true;
    }
}
