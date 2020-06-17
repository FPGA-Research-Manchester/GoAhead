using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.Vivado
{
    [CommandDescription(Description = "Check the given net uses the given rouing resources to leave or enter the given user selection")]
    class CheckPRLink : NetlistContainerCommand
    {
        private enum Mode { Undefined, Static, Module }

        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.Vivado);

            Mode mode = Mode.Undefined;
            if (this.NetlistType.ToLower().Equals("static")) { mode = Mode.Static; }
            else if (this.NetlistType.ToLower().Equals("module")) { mode = Mode.Module; }
            else { throw new ArgumentException("Invalid value for NetlistType " + this.NetlistType + ". Use either static or module"); }

            this.CheckRoutingResourceUsageInTunnel(mode);
            this.CheckEnterLeave();
        }

        private void CheckRoutingResourceUsageInTunnel(Mode mode)
        {
            NetlistContainer nlc = this.GetNetlistContainer();
            Regex validRoutingResources = new Regex(this.RoutingResources);
            foreach (TCLNet net in nlc.Nets.Where(n => Regex.IsMatch(n.Name, this.Nets)))
            {
                foreach (TCLRoutingTreeNode node in net.RoutingTree.GetAllRoutingNodes(net.RoutingTree.Root))
                {
                    if (node.Tile == null)
                    {
                        this.OutputManager.WriteWarning("Incomplete routing tree " + net.Name);
                        break;
                    }
                    bool insideTunnel = false;
                    bool interConnectedTile = IdentifierManager.Instance.IsMatch(node.Tile.Location, IdentifierManager.RegexTypes.Interconnect);;

                    switch (mode)
                    {
                        case Mode.Static:
                            insideTunnel = 
                                FPGA.TileSelectionManager.Instance.IsUserSelected(node.Tile, this.Tunnel) &&
                                FPGA.TileSelectionManager.Instance.IsUserSelected(node.Tile, this.ModulArea);
                            break;
                        case Mode.Module:
                            insideTunnel =
                                FPGA.TileSelectionManager.Instance.IsUserSelected(node.Tile, this.Tunnel) &&
                                !FPGA.TileSelectionManager.Instance.IsUserSelected(node.Tile, this.ModulArea);
                            break;
                    }
                    if (insideTunnel && interConnectedTile && !validRoutingResources.IsMatch(node.Port.Name))
                    {
                        this.OutputManager.WriteWarning("Unexpected routing resource " + node.ToString() + " in net " + net.Name + " inside tunnel " + this.Tunnel);
                    }
                }
            }
        }

        private void CheckEnterLeave()
        {
            NetlistContainer nlc = this.GetNetlistContainer();
            foreach (TCLNet net in nlc.Nets.Where(n => Regex.IsMatch(n.Name, this.Nets)))
            {
                Slice startSlice = FPGA.FPGA.Instance.GetSlice(net.OutpinInstance.SliceName);
                TCLRoutingTreeNode startPort = net.RoutingTree.Root.Children.First();
                while (!Regex.IsMatch(startPort.Port.Name, this.StartPort))
                {
                    startPort = startPort.Children.First();
                }
                Location startLoc = new Location(startSlice.ContainingTile, startPort.Port);
                bool startLocIsUserSelected = FPGA.TileSelectionManager.Instance.IsUserSelected(startLoc.Tile.TileKey, this.ModulArea);
                Location currentLoc = startLoc;
                Location lastLoc = currentLoc;
                TCLRoutingTreeNode currentNode = startPort;
                TCLRoutingTreeNode violator;
                int hopCount = 0;
                while (true)
                {
                    // TODO foreach
                    lastLoc = currentLoc;
                    currentLoc = Navigator.GetDestinations(currentLoc).FirstOrDefault();
                    violator = currentNode;
                    currentNode = currentNode.Children.FirstOrDefault();
                    if (currentNode == null)
                    {
                        this.OutputManager.WriteWarning("Can not follow routing in net " + net.Name + " after using routing resources " + violator.Port.Name);
                        break;
                    }
                    // stop over
                    if (currentLoc == null)
                    {
                        Tuple<Port, Port> hop = lastLoc.Tile.SwitchMatrix.GetAllArcs().FirstOrDefault(t =>
                            t.Item1.Name.Equals(lastLoc.Pip.Name) && t.Item2.Name.Equals(currentNode.Port.Name));
                        currentLoc = new Location(lastLoc.Tile, hop.Item2);
                    }

                    if (currentLoc == null)
                    {
                        this.OutputManager.WriteWarning("Can not follow routing in net " + net.Name + " after using routing resources " + violator.Port.Name);
                        break;
                    }

                    bool currentLocIsUserSelected = FPGA.TileSelectionManager.Instance.IsUserSelected(currentLoc.Tile.TileKey, this.ModulArea);
                    bool crossingBoundary = (startLocIsUserSelected && !currentLocIsUserSelected) || (!startLocIsUserSelected && currentLocIsUserSelected);
                    if (crossingBoundary)
                    {
                        if (!Regex.IsMatch(violator.Port.Name, this.RoutingResources))
                        {
                            this.OutputManager.WriteWarning("Unexpected routing resource " + violator.Port.Name + " in net " + net.Name + "used to enter/leave the reconfigurable area");
                        }
                        break;
                    }

                    // update pip
                    currentLoc = new Location(currentLoc.Tile, currentNode.Port);
                    hopCount++;
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "A regular expression to define the nets that will be checked ")]
        public String Nets = "p2s";

        [Parameter(Comment = "A regular expression to define valid routing resources")]
        public String RoutingResources = "(EE2BEG)|(IMUX)";

        [Parameter(Comment = "A regular expression to define the start port form where to check")]
        public String StartPort = "LOGIC_OUTS";

        [Parameter(Comment = "The name of the user seletcion the PR link enters or leaves")]
        public string ModulArea = "PartialArea";

        [Parameter(Comment = "The name of the user seletcion denoting the tunnel")]
        public string Tunnel = "Tunnel";

        [Parameter(Comment = "Whether to check the 'static' netlist or the 'module' netlist")]
        public string NetlistType = "static";
    }
}
