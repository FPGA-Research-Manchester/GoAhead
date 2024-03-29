﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Prevent future blocking of the specified port within all currently selected tiles", Wrapper = true)]
    class ExcludePortsFromBlockingInSelectionByRegexp : Command
    {
        protected override void DoCommandAction()
        {
            // port filter
            Regex portFilter = new Regex(PortNameRegexp, RegexOptions.Compiled);
            // progress 
            int tileCount = 0;

            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles())
            {
                ProgressInfo.Progress = ProgressStart + (int)((double)tileCount++ / (double)TileSelectionManager.Instance.NumberOfSelectedTiles * ProgressShare);

                foreach (Port port in tile.SwitchMatrix.Ports.Where(p => portFilter.IsMatch(p.Name)))
                {
                    if (IncludeAllPorts)
                    {
                        ExcludePortsFromBlocking cmd = new ExcludePortsFromBlocking();
                        cmd.CheckForExistence = CheckForExistence;
                        cmd.IncludeAllPorts = IncludeAllPorts;
                        cmd.Location = tile.Location;
                        cmd.PortName = port.Name;
                        CommandExecuter.Instance.Execute(cmd);
                    }
                    else
                    {
                        // TODO ExcludePortsFromBlocking.BlockPort
                        ExcludePortsFromBlocking.BlockPort(tile, port.Name, CheckForExistence);
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The port name to be blocked, e.g. E2BEG5")]
        public string PortNameRegexp = "E2BEG5";

        [Parameter(Comment = "Check for existence of ports")]
        public bool CheckForExistence = false;

        [Parameter(Comment = "Whether to follow up wires on a port and also block the reachable ports in other tiles")]
        public bool IncludeAllPorts = true;
    }
}
