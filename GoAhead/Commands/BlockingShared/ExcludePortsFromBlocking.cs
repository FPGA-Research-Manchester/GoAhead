using System;
using System.Linq;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Prevent future blocking of the specified port on a specific tile")]
    class ExcludePortsFromBlocking : Command
    {
        protected override void DoCommandAction() 
        {
            BlockPortAndReachablePorts(Location, PortName, CheckForExistence, IncludeAllPorts);
        }

        public static void BlockPortAndReachablePorts(string location, string portName, bool checkForExistence, bool includeAllPorts)
        {
            Tile where = FPGA.FPGA.Instance.GetTile(location);

            // block 
            BlockPort(where, portName, checkForExistence);

            // follow wire if required
            if (includeAllPorts && where.WireList != null)
            {
                foreach (Location l in Navigator.GetDestinations(where.Location, portName))
                {
                    BlockPort(l.Tile, l.Pip.Name, checkForExistence);
                }
            }
        }

        public static void BlockPort(Tile where, string portName, bool checkForExistence)
        {
            if (checkForExistence && !where.SwitchMatrix.Contains(portName))
            {
                throw new ArgumentException("Tile " + where.Location + " does not contain port " + portName);
            }
            else if (where.SwitchMatrix.Contains(portName) && !where.IsPortBlocked(portName))
            {
                where.BlockPort(portName, Tile.BlockReason.ExcludedFromBlocking);
            }                     

        }

        public override void Undo() 
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The port name to be blocked, e.g. E2BEG5")]
        public string PortName = "E2BEG5";

        [Parameter(Comment = "The location string, e.g. INT_X2Y34")]
        public string Location = "INT_X2Y34";

        [Parameter(Comment = "Check for existence of ports")]
        public bool CheckForExistence = false;

        [Parameter(Comment = "Whether to follow up wires on a port and also block the reachable ports in other tiles")]
        public bool IncludeAllPorts = true;        
    }
}
