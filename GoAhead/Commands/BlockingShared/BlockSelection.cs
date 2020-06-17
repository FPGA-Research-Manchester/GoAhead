using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Block the routing resources and add primitives to tiles in the current selection", Wrapper = true, Publish = true)]
    public class BlockSelection : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            BlockOnlyMarkedPortsScope = BlockOnlyMarkedPorts;

            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            // prevent repeated error message from subcommands
            if (!NetlistContainerManager.Instance.Contains(this.NetlistContainerName))
            {
                throw new ArgumentException("The netlist container " + this.NetlistContainerName + " does not exist. Use the command AddNetlistContainer to add a netlist container.");
            }

            NetlistContainer nlc = this.GetNetlistContainer();
            Net blockerNet = null;

            bool useExistingNet = false;
            if (nlc.Nets.Any(n => n.OutpinCount > 0))
            {
                useExistingNet = true;
                blockerNet = nlc.GetAnyNet();
                this.OutputManager.WriteOutput("Adding blocker pips to already existing net " + blockerNet.Name);
            }
            else
            {
                // create XDL or TCL script
                blockerNet = Net.CreateNet("BlockSelection");

                if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
                {
                    blockerNet = new TCLNet("BlockSelection");
                    //((TCLNet)blockerNet).Properties.SetProperty("IS_ROUTE_FIXED", "TRUE", false);
                    // tag for code generation
                    ((TCLNet)blockerNet).IsBlockerNet = true;
                    ((TCLNet)blockerNet).RoutingTree = new TCLRoutingTree();
                    TCLRoutingTreeNode root = new TCLRoutingTreeNode(null, null);
                    root.VirtualNode = true;
                    ((TCLNet)blockerNet).RoutingTree.Root = root;
                }

                useExistingNet = false;
                bool outPinAdded = false;
                
                // the location string of the tile in which we run the outpin
                String outpinLocation = "";

                // 1 iterate over all not filtered out tiles to instantiate primitves and to find an outpin
                switch (FPGA.FPGA.Instance.BackendType)
                {
                    case FPGATypes.BackendType.ISE:
                        this.AddXDLTemplates(nlc, blockerNet, ref outPinAdded, ref outpinLocation);
                        break;
                    case FPGATypes.BackendType.Vivado:
                        //this.AddTCLInstances(nlc, blockerNet, ref outPinAdded, ref outpinLocation);
                        ((TCLContainer)nlc).AddGndPrimitive(blockerNet);
                        outPinAdded = true;
                        break;
                }

                // 2 name net according to added outpin
                blockerNet.Name = this.Prefix + outpinLocation + "_" + blockerNet.Name;

                if (!outPinAdded)
                {
                    this.OutputManager.WriteOutput("Could not find an outpin");
                }
            }

            // 4 cluster all completely unblocked tiles by their identifiers (do not cluster BEFORE having added and thus blocked an outpin)
            //   tiles with already blocked ports are added to single cluster each and thus treated seperately
            Dictionary<int, List<Tile>> clusteredTiles = new Dictionary<int, List<Tile>>();
            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    this.FindClusteringForISE(clusteredTiles);
                    break;
                case FPGATypes.BackendType.Vivado:
                    this.FindClusteringForVivado(clusteredTiles);
                    break;
            }
            

            // block by
            // 5 paths ...
            int clusterCount = 0;
            foreach (List<Tile> tiles in clusteredTiles.Values)
            {
                this.AddBlockerPaths(blockerNet, tiles[0], tiles);

                this.ProgressInfo.Progress = 0 + (int)((double)clusterCount++ / (double)clusteredTiles.Count * 50);
            }

            // 6 and arcs
            clusterCount = 0;
            foreach (List<Tile> tiles in clusteredTiles.Values)
            {
                this.AddArcs(blockerNet, tiles[0], tiles);

                this.ProgressInfo.Progress = 50 + (int)((double)clusterCount++ / (double)clusteredTiles.Count * 50);
            }

            // 7 check blocking
            if (this.PrintUnblockedPorts)
            {
                foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(
                    t => !BlockerSettings.Instance.SkipTile(t) && IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
                {
                    this.CheckForUnblockedPorts(t);
                }
            }

            // clean up indeces
            foreach (Tile tile in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(t => !BlockerSettings.Instance.SkipTile(t)))
            {
                tile.SwitchMatrix.ClearBlockingPortList();
            }

            // add prefix and store nets
            if (blockerNet.PipCount > 0 && !useExistingNet)
            {
                nlc.Add(blockerNet);
            }
        }

        private void FindClusteringForISE(Dictionary<int, List<Tile>> clusteredTiles)
        {
            foreach (Tile tile in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(t => !BlockerSettings.Instance.SkipTile(t)))
            {
                int clusterKey = tile.HasNonstopoverBlockedPorts ? tile.Location.GetHashCode() : clusterKey = tile.SwitchMatrixHashCode;

                if (!clusteredTiles.ContainsKey(clusterKey))
                {
                    clusteredTiles.Add(clusterKey, new List<Tile>());
                }
                clusteredTiles[clusterKey].Add(tile);
            }
        }

        private void FindClusteringForVivado(Dictionary<int, List<Tile>> clusteredTiles)
        {
            var tiles = FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(t => 
                !BlockerSettings.Instance.SkipTile(t) && 
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect));
            // tiles without blocked ports
            var clusters =
                from tile in tiles
                where !tile.HasNonstopoverBlockedPorts
                group tile by tile.SwitchMatrixHashCode into clusterGroup
                select clusterGroup;
            int clusterKey = 0;
            foreach(var cluster in clusters)
            {
                clusteredTiles.Add(clusterKey, new List<Tile>());
                foreach(Tile t in cluster)
                {
                    clusteredTiles[clusterKey].Add(t);
                }
                clusterKey++;
            }
            // group the tiles with blocked ports by their blocked ports
            var otherClusters =
                from tile in tiles
                where tile.HasNonstopoverBlockedPorts
                group tile by new { tile.SwitchMatrixHashCode, tile.AllBlockedPortsHash} into clusterGroup
                select clusterGroup;

            foreach (var cluster in otherClusters)
            {
                clusteredTiles.Add(clusterKey, new List<Tile>());
                foreach (Tile t in cluster)
                {
                    clusteredTiles[clusterKey].Add(t);
                }
                clusterKey++;
            }
        }

        private void AddXDLTemplates(NetlistContainer nlc, Net blockerNet, ref bool outPinAdded, ref String outpinLocation)
        {
            foreach (Tile tile in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(t => !BlockerSettings.Instance.SkipTile(t)))
            {
                // iterate in order
                for (int i = 0; i < tile.Slices.Count; i++)
                {
                    Slice s = tile.Slices[i];
                    String template = "";
                    if (BlockerSettings.Instance.InsertTemplate(s.SliceName, false, i, out template))
                    {
                        AddTemplateConfig.AddTemplate((XDLContainer)nlc, template, tile.Location, i);

                        // add outpin
                        if (!outPinAdded)
                        {
                            outpinLocation = tile.Location;
                            outPinAdded = BlockSelection.AddXDLOutpin((XDLNet)blockerNet, tile, this.SliceNumber);
                        }
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private void AddArcs(Net blockerNet, Tile first, IEnumerable<Tile> cluster)
        {
            XDLNet xdlNet = null;
            TCLNet TCLNet = null;
            if (blockerNet is XDLNet)
            {
                xdlNet = (XDLNet)blockerNet;
            }
            else if (blockerNet is TCLNet)
            {
                TCLNet = (TCLNet)blockerNet;
            }

            bool firstArcInCluster = true;
            foreach (BlockerOrderElement orderEl in BlockerSettings.Instance.GetBlockerOrder())
            {
                if (!this.BlockWithEndPips && orderEl.EndPip)
                {
                    continue;
                }

                Regex driverMatch = this.GetRegex(orderEl.DriverRegexp);

                // as as many arc as possible
                foreach (Port driver in first.SwitchMatrix.GetAllDriversSortedAscByConnectivity(p =>
                    !first.IsPortBlocked(p) &&
                    driverMatch.IsMatch(p.Name) &&
                    !BlockerSettings.Instance.SkipPort(p) &&
                    !first.IsPortBlocked(p.Name, Tile.BlockReason.ExcludedFromBlocking)))
                {
                    Regex sinkMatch = this.GetRegex(orderEl.SinkRegexp);

                    //foreach (Port drivenPort in first.SwitchMatrix.GetDrivenPortsSortedSortedDescByConnectivity(driver, p =>
                    foreach (Port drivenPort in first.SwitchMatrix.GetDrivenPorts(driver).Where(p =>
                        !first.IsPortBlocked(p) &&
                        sinkMatch.IsMatch(p.Name) &&
                        !BlockerSettings.Instance.SkipPort(p) &&
                        !first.IsPortBlocked(p.Name, Tile.BlockReason.ExcludedFromBlocking)))
                    {
                        foreach (Tile t in cluster)
                        {
                            if (xdlNet != null)
                            {
                                // comment on first application of rule
                                if (firstArcInCluster)
                                {
                                    xdlNet.Add(" added by BlockSelection: " + orderEl.ToString());
                                    firstArcInCluster = false;
                                }
                                // extend net
                                xdlNet.Add(t, driver, drivenPort);
                            }
                            else if (TCLNet != null)
                            {
                                //TCLRoutingTreeNode driverNode = xdlNet.RoutingTree.Root.Children.FirstOrDefault(n => n.Tile.Location.Equals(t.Location) && n.Port.Name.Equals(driver.Name));
                                TCLRoutingTreeNode driverNode = TCLNet.RoutingTree.Root.Children.FirstOrDefault(t.Location, driver.Name);
                                if (driverNode == null)
                                {
                                    driverNode = new TCLRoutingTreeNode(t, driver);
                                    driverNode.VivadoPipConnector = orderEl.VivadoPipConnector;
                                    TCLNet.RoutingTree.Root.Children.Add(driverNode);
                                }
                                TCLRoutingTreeNode leaveNode = new TCLRoutingTreeNode(t, drivenPort);
                                driverNode.Children.Add(leaveNode);
                            }

                            BlockPort(t, driver);
                            BlockPort(t, drivenPort);                           
                        }

                        if (!orderEl.ConnectAll)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void AddBlockerPaths(Net blockerNet, Tile first, IEnumerable<Tile> cluster)
        {
            XDLNet xdlNet = null;
            TCLNet TCLNet = null;
            if (blockerNet is XDLNet)
            {
                xdlNet = (XDLNet)blockerNet;
            }
            else if (blockerNet is TCLNet)
            {
                TCLNet = (TCLNet)blockerNet;
            }

            bool firstArcInCluster = true;
            // user defined paths
            foreach (BlockerPath bp in BlockerSettings.Instance.GetAllBlockerPaths())
            {
                Regex hopRegexp = this.GetRegex(bp.HopRegexp);
                Regex driverRegexp = this.GetRegex(bp.DriverRegexp);
                Regex sinkRegexp = this.GetRegex(bp.SinkRegexp);

                // TODO add port filter
                foreach (Port hop in first.SwitchMatrix.Ports.Where(p => hopRegexp.IsMatch(p.Name) && !first.IsPortBlocked(p)))
                {
                    Tuple<Port, Port> arc1 = first.SwitchMatrix.GetFirstArcOrDefault(
                        p => !first.IsPortBlocked(p) && !BlockerSettings.Instance.SkipPort(p) && driverRegexp.IsMatch(p.Name),
                        arc => arc.Item2.Name.Equals(hop.Name) && !BlockerSettings.Instance.SkipPort(arc.Item2));

                    if (arc1.Item1 != null && arc1.Item2 != null)
                    {
                        Tuple<Port, Port> arc2 = first.SwitchMatrix.GetFirstArcOrDefault(
                            p => p.Name.Equals(hop.Name) && !BlockerSettings.Instance.SkipPort(p),
                            arc =>
                            !BlockerSettings.Instance.SkipPort(arc.Item2) &&
                            sinkRegexp.IsMatch(arc.Item2.Name) &&
                            !first.IsPortBlocked(arc.Item2));

                        if (arc2.Item1 != null && arc2.Item2 != null)
                        {
                            foreach (Tile t in cluster)
                            {
                                if (xdlNet != null)
                                {
                                    // comment on first application of this rule
                                    if (firstArcInCluster)
                                    {
                                        xdlNet.Add("added by BlockSelection.BlockerPath");
                                        firstArcInCluster = false;
                                    }
                                    // extend path
                                    xdlNet.Add(t, arc1.Item1, arc1.Item2);
                                    xdlNet.Add(t, arc2.Item1, arc2.Item2);
                                }
                                if (TCLNet != null)
                                {
                                    firstArcInCluster = false;
                                    TCLRoutingTreeNode driverNode = TCLNet.RoutingTree.Root.Children.FirstOrDefault(t.Location, arc1.Item1.Name);
                                    if (driverNode == null)
                                    {
                                        driverNode = new TCLRoutingTreeNode(t, arc1.Item1);
                                        TCLNet.RoutingTree.Root.Children.Add(driverNode);
                                    }
                                    TCLRoutingTreeNode hopNode = TCLNet.RoutingTree.Root.Children.FirstOrDefault(t.Location, arc1.Item2.Name);
                                    if (hopNode == null)
                                    {
                                        hopNode = new TCLRoutingTreeNode(t, arc1.Item2);
                                        driverNode.Children.Add(hopNode);
                                    }
                                    // sink node should never exist!
                                    TCLRoutingTreeNode sinkNode = new TCLRoutingTreeNode(t, arc2.Item2);
                                    hopNode.Children.Add(sinkNode);
                                }

                                BlockPort(t, arc1.Item1);
                                BlockPort(t, arc1.Item2);
                                BlockPort(t, arc2.Item2);
                            }
                        }
                    }
                }
            }
        }

        public static bool AddXDLOutpin(Net blockerNet, Tile t, int sliceNumber)
        {
            foreach (Port sliceOutPort in BlockSelection.GetAllDrivers(t, sliceNumber))
            {
                foreach (Port target in t.SwitchMatrix.GetDrivenPorts(sliceOutPort).Where(drivenPort => !t.IsPortBlocked(drivenPort) && !BlockerSettings.Instance.SkipPort(drivenPort)))
                {
                    List<Location> locations = new List<Location>();
                    foreach (Location loc in Navigator.GetDestinations(t, target))
                    {
                        locations.Add(loc);
                    }
                    if (locations.Count < 1)
                    {
                        continue;
                    }

                    Tile neighbour = locations[0].Tile;
                    Port driverOnNeighbourTile = locations[0].Pip;

                    if (neighbour.IsPortBlocked(driverOnNeighbourTile))
                    {
                        continue;
                    }

                    Port beginPip = neighbour.SwitchMatrix.GetDrivenPorts(driverOnNeighbourTile).FirstOrDefault(p => !neighbour.IsPortBlocked(p));
                    if (beginPip == null)
                    {
                        continue;
                    }

                    // arc on CLB
                    if (blockerNet is XDLNet)
                    {
                        ((XDLNet)blockerNet).Add(t, sliceOutPort, target);
                    }
                    else
                    {
                        // TODO
                    }

                    BlockPort(t, sliceOutPort);
                    BlockPort(t, target);
                    // ouptin on CLB
                    NetOutpin outpin = new NetOutpin();
                    outpin.InstanceName = t.Slices[sliceNumber].SliceName;
                    outpin.SlicePort = NetPin.GetSlicePortString(sliceOutPort.Name);
                    blockerNet.Add(outpin);
                    // arc in INT
                    if (blockerNet is XDLNet)
                    {
                        ((XDLNet)blockerNet).Add(neighbour, driverOnNeighbourTile, beginPip);
                    }
                    else
                    {
                        // TODO
                    }
                    BlockPort(neighbour, driverOnNeighbourTile);
                    BlockPort(neighbour, beginPip);
                    return true;
                }
            }
            return false;
        }

       

        private void CheckForUnblockedPorts(Tile tile)
        {
            foreach (Port driver in tile.SwitchMatrix.Ports.Where(p =>
                !BlockerSettings.Instance.SkipPort(p) &&
                !tile.IsPortBlocked(p) &&
                !this.GetRegex(this.UnblockedPortsToIgnore).IsMatch(p.Name)))
            {
                // exit
                Port exitPort = tile.SwitchMatrix.GetDrivenPorts(driver).FirstOrDefault(p =>
                    !BlockerSettings.Instance.SkipPort(p) &&
                    !tile.IsPortBlocked(p) &&
                    !this.GetRegex(this.UnblockedPortsToIgnore).IsMatch(p.Name)
                    );

                if (exitPort != null)
                {
                    Location otherInterconnect = Navigator.GetDestinations(tile, exitPort).FirstOrDefault(l =>
                        IdentifierManager.Instance.IsMatch(l.Tile.Location, IdentifierManager.RegexTypes.Interconnect));
                    if (otherInterconnect != null)
                    {
                        this.OutputManager.WriteWarning("Possible violaton on " + tile.Location + ": " + driver.Name + "->" + exitPort.Name);

                        foreach (Tuple<Port, Port> blockingArc in tile.SwitchMatrix.GetAllArcs().Where(a =>
                              !BlockerSettings.Instance.SkipPort(a.Item1) &&
                              !tile.IsPortBlocked(a.Item1) &&
                              a.Item2.Name.Equals(exitPort.Name)))
                        {
                            this.OutputManager.WriteWarning("                 use " + blockingArc.ToString());
                        }
                    }
                }

                Port tunnelPort = tile.SwitchMatrix.GetDrivenPorts(driver).FirstOrDefault(p =>
                   !BlockerSettings.Instance.SkipPort(p) &&
                   tile.IsPortBlocked(p, Tile.BlockReason.ExcludedFromBlocking) &&
                   !this.GetRegex(this.UnblockedPortsToIgnore).IsMatch(p.Name)
                   );

                if (tunnelPort != null)
                {
                    this.OutputManager.WriteWarning("Possible connection into tunnel on " + tile.Location + ": " + driver.Name + "->" + tunnelPort.Name);
                }
            }
        }

        private static IEnumerable<FPGA.Port> GetAllDrivers(Tile tile, int sliceNumber)
        {
            Regex lutoutPortFilter = null;
            switch (FPGA.FPGA.Instance.BackendType)
	        {
		        case FPGATypes.BackendType.ISE:
                    lutoutPortFilter = new Regex("[A-D](Q)?$", RegexOptions.Compiled);
                    break;
                case FPGATypes.BackendType.Vivado:
                    lutoutPortFilter = new Regex("Q$", RegexOptions.Compiled);
                    break;
	        }

            foreach (Port sliceOutPort in tile.SwitchMatrix.GetAllDrivers().Where(p => 
                !tile.IsPortBlocked(p) && //free ports only
                lutoutPortFilter.IsMatch(p.Name) // port filter for lut out ports , flip (no MUX)
                ))
            {
                switch (FPGA.FPGA.Instance.BackendType)
                {
                    case FPGATypes.BackendType.ISE:
                        if (tile.Slices[sliceNumber].PortMapping.IsSliceOutPort(sliceOutPort))
                            yield return sliceOutPort;
                        break;
                    case FPGATypes.BackendType.Vivado:
                        yield return sliceOutPort;
                        break;
                }                
            }
        }

        public Regex GetRegex(string pattern)
        {
            if (!this.m_regex.ContainsKey(pattern))
            {
                Regex r = new Regex(pattern, RegexOptions.Compiled);
                this.m_regex.Add(pattern, r);
            }
            return this.m_regex[pattern];
        }

        private static bool BlockOnlyMarkedPortsScope = false;
        private static void BlockPort(Tile tile, Port port)
        {
            if (BlockOnlyMarkedPortsScope)
            {
                if (tile.IsPortBlocked(port, Tile.BlockReason.ToBeBlocked))
                {
                    tile.UnblockPort(port);
                    tile.BlockPort(port, Tile.BlockReason.Blocked);
                }                
            }
            else
            {
                tile.BlockPort(port, Tile.BlockReason.Blocked);
            }
        }

        private Dictionary<string, Regex> m_regex = new Dictionary<string, Regex>();

        [Parameter(Comment = "Wheter to print out unblocked ports. Unblocked EndPips from which we can only reach blocked ports are considered as blocked.")]
        public bool PrintUnblockedPorts = false;

        [Parameter(Comment = "Do not consider those ports as unblocked (when using PrintUnblockedPorts) that match this regular expression. When empty, all ports are considered")]
        public string UnblockedPortsToIgnore = "";

        [Parameter(Comment = "The prefix for nets and ports")]
        public String Prefix = "RBB_Blocker";

        [Parameter(Comment = "Whether to use end pips as drivers for blocking")]
        public bool BlockWithEndPips = true;

        [Parameter(Comment = "The index of the slice the blocker will use")]
        public int SliceNumber = 0;

        [Parameter(Comment = "")]
        public bool BlockOnlyMarkedPorts = false;
    }
}