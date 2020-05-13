using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Apply command ExcludePortsFromBlocking to the given tile for those ports that match PortNameRegexp", Wrapper = true, Publish=true)]
    class ExcludePortsFromBlockingOnTileByRegexp : Command
    {
        protected override void DoCommandAction()
        {
            Tile tile = FPGA.FPGA.Instance.GetTile(Location);

            Regex filter = new Regex(PortNameRegexp, RegexOptions.Compiled);

            foreach (Port port in tile.SwitchMatrix.Ports.Where(p => filter.IsMatch(p.Name)))
            {
                ExcludePortsFromBlocking cmd = new ExcludePortsFromBlocking();
                cmd.CheckForExistence = CheckForExistence;
                cmd.IncludeAllPorts = IncludeAllPorts;
                cmd.Location = tile.Location;
                cmd.PortName = port.Name;
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The port name to be blocked, e.g. E2BEG5")]
        public string PortNameRegexp = "";

        [Parameter(Comment = "Check for existence of ports")]
        public bool CheckForExistence = false;

        [Parameter(Comment = "Wheter to follow up wires on a port and also block the reachable ports in other tiles")]
        public bool IncludeAllPorts = true;

        [Parameter(Comment = "The location string  of the tile to block, e.g CLBLL_X2Y78")]
        public string Location = "CLBLL_X2Y78";
    }
}