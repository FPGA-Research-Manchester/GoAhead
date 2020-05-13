using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Commands.Library;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class LocationOutsideNet
    {
        public LocationOutsideNet(Location l, object routingElement)
        {
            Location = l;
            RoutingElement = routingElement;
        }
        public Location Location = null;
        public object RoutingElement = null;

        public override string ToString()
        {
            return
                (Location != null ? Location.ToString() : "") + " at " + 
                (RoutingElement != null ? RoutingElement.ToString() : "");
        }
    }

    [CommandDescription(Description="Fuse all nets in a netlist container", Wrapper=true, Publish=true)]
    class FuseNets : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = GetNetlistContainer();

            DecomposeAntennasFromNets decompose = new DecomposeAntennasFromNets();
            decompose.NetlistContainerName = NetlistContainerName;
            // no output
            decompose.FileName = "";
            CommandExecuter.Instance.Execute(decompose);

            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    FuseXDLNets(netlistContainer);
                    break;
                case FPGATypes.BackendType.Vivado:
                    FuseTCLNets(netlistContainer);
                    break;
            }
        }

        private void FuseTCLNets(NetlistContainer netlistContainer)
        {
            Queue<Net> netsWithOutpin = FindNetsWithOutpin(netlistContainer);
            Dictionary<string, Net> netsToConsiderForFusing = FindNetsToConsiderForFusing(netlistContainer);

            // build mapping: Location -> XDLNet WEITER HIER
            Dictionary<string, Dictionary<string, List<Net>>> candidateLocations = BuildLocationNetMapping(netlistContainer);

            int startSize = netsWithOutpin.Count;
            while (netsWithOutpin.Count > 0)
            {
                TCLNet current = (TCLNet) netsWithOutpin.Dequeue();

                if (current.Name.Contains("p2s"))
                {
                }

                ProgressInfo.Progress = (int)((double)(startSize - netsWithOutpin.Count) / (double)startSize * 100);

                while (netsToConsiderForFusing.Count > 0)
                {
                    Dictionary<string, Net> fusedNets = new Dictionary<string, Net>();
                    Dictionary<string, LocationOutsideNet> fusePoints = new Dictionary<string, LocationOutsideNet>();

                    // route from end of antennas
                    foreach (LocationOutsideNet los in AllLocationsOutsideNet(current))
                    {
                        Location loc = los.Location;

                        if (!candidateLocations.ContainsKey(loc.Tile.Location))
                        {
                            continue;
                        }
                        if (!candidateLocations[loc.Tile.Location].ContainsKey(loc.Pip.Name))
                        {
                            Dictionary<string, List<Net>> debug = candidateLocations[loc.Tile.Location];
                            continue;
                        }
                        List<string> netNamesToRemove = new List<string>();
                        foreach (Net n in candidateLocations[loc.Tile.Location][loc.Pip.Name])
                        {
                            if (!fusedNets.ContainsKey(n.Name) && !n.Name.Equals(current.Name) && netsToConsiderForFusing.ContainsKey(n.Name))
                            {
                                fusedNets.Add(n.Name, n);
                                fusePoints.Add(n.Name, los);
                                netNamesToRemove.Add(n.Name);
                                //((TCLRoutingTreeNode)los.RoutingElement).Children.Add()
                            }
                        }
                        candidateLocations[loc.Tile.Location][loc.Pip.Name].RemoveAll(net => netNamesToRemove.Contains(net.Name));
                    }

                    // TDOO see "and check for branches branches"
                   

                    foreach (TCLNet net in fusedNets.Values)
                    {
                        OutputManager.WriteOutput("Fusing " + current.Name + " with " + net.Name + " @" + fusePoints[net.Name].ToString());

                        //this.OutputManager.WriteOutput(net.ToString());
                        //this.OutputManager.WriteOutput(net.Foo());
                        //this.OutputManager.WriteOutput("------------------------------------------------");

                        //netlistContainer.RemoveNet(net.Name);
                        netlistContainer.Remove(delegate(Net n) { return n.Name.Equals(net.Name); });
                        netsToConsiderForFusing.Remove(net.Name);

                        foreach (NetPin np in net.NetPins)
                        {
                            current.Add(np);
                        }
                        current.FlattenNet();
                        net.FlattenNet();
                        foreach(TCLRoutingTreeNode child in net.RoutingTree.Root.Children)
                        {
                            current.RoutingTree.Root.Children.Add(child);
                        }
                    }

                    // no futher fusions made, continue with next net
                    if (fusedNets.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        public static IEnumerable<LocationOutsideNet> AllLocationsOutsideNet(Net net)
        {
            Dictionary<string, List<string>> locations = new Dictionary<string, List<string>>();
            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    XDLNet xdlNet = (XDLNet)net;
                    foreach (XDLPip pip in xdlNet.Pips)
                    {
                        if (!locations.ContainsKey(pip.Location))
                        {
                            locations.Add(pip.Location, new List<string>());
                        }
                        locations[pip.Location].Add(pip.From);
                    }

                    foreach (XDLPip pip in xdlNet.Pips)
                    {
                        if (pip.Operator.Equals("=-"))
                        {
                            foreach (Location loc in Navigator.GetDestinations(pip.Location, pip.From))
                            {
                                if (!locations.ContainsKey(loc.Tile.Location))
                                {
                                    yield return new LocationOutsideNet(loc, pip);
                                }
                                else
                                {
                                    if (!locations[loc.Tile.Location].Contains(loc.Pip.Name))
                                    {
                                        yield return new LocationOutsideNet(loc, pip);
                                    }
                                }
                            }
                        }

                        // find all arcs that do not end up in this net
                        foreach (Location loc in Navigator.GetDestinations(pip.Location, pip.To))
                        {
                            if (!locations.ContainsKey(loc.Tile.Location))
                            {
                                yield return new LocationOutsideNet(loc, pip);
                            }
                            else
                            {
                                if (!locations[loc.Tile.Location].Contains(loc.Pip.Name))
                                {
                                    yield return new LocationOutsideNet(loc, pip);
                                }
                            }
                        }

                        // consider stopovers 
                        yield return new LocationOutsideNet(new Location(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.To)), pip);
                    }
                    break;
                case FPGATypes.BackendType.Vivado:
                    TCLNet tclNet = (TCLNet)net;

                    foreach (TCLRoutingTreeNode node in tclNet.RoutingTree.GetAllRoutingNodes().Where(n => !n.VirtualNode))
                    {
                        if (!locations.ContainsKey(node.Tile.Location))
                        {
                            locations.Add(node.Tile.Location, new List<string>());
                        }
                        locations[node.Tile.Location].Add(node.Port.Name);
                    }                  

                    // find all arcs that do not end up in this net
                    foreach (TCLRoutingTreeNode node in tclNet.RoutingTree.GetAllRoutingNodes().Where(n => !n.VirtualNode))
                    {
                        List<Port> driven = new List<Port>();
                        // outside
                        driven.Add(node.Port);
                        // inside switch matrix
                        driven.AddRange(node.Tile.SwitchMatrix.GetDrivenPorts(node.Port));

                        foreach (Port p in driven)
                        {
                            foreach (Location loc in Navigator.GetDestinations(node.Tile.Location, p.Name))
                            {
                                if (!locations.ContainsKey(loc.Tile.Location))
                                {
                                    yield return new LocationOutsideNet(loc, node);
                                    foreach (Port reachable in loc.Tile.SwitchMatrix.GetDrivenPorts(loc.Pip))
                                    {
                                        if (reachable.Name.Contains("EE2"))
                                        {
                                        }

                                        Location other = new Location(loc.Tile, reachable);
                                        yield return new LocationOutsideNet(other, node);
                                    }
                                }
                                else
                                {
                                    if (!locations[loc.Tile.Location].Contains(loc.Pip.Name))
                                    {
                                        yield return new LocationOutsideNet(loc, node);                                        
                                    }
                                   
                                }
                            }
                        }
                        // TODO
                        // consider stopovers 
                        // ?? yield return new LocationOutsideNet(new Location(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.To)), pip);
                    }


                    break;
            }
           
        }

        private void FuseXDLNets(NetlistContainer netlistContainer)
        {
            Queue<Net> netsWithOutpin = FindNetsWithOutpin(netlistContainer);
            Dictionary<string, Net> netsToConsiderForFusing = FindNetsToConsiderForFusing(netlistContainer);

            // build mapping: Location -> XDLNet
            Dictionary<string, Dictionary<string, List<Net>>> candidateLocations = BuildLocationNetMapping(netlistContainer);

            int startSize = netsWithOutpin.Count;
            while (netsWithOutpin.Count > 0)
            {
                XDLNet current = (XDLNet ) netsWithOutpin.Dequeue();

                ProgressInfo.Progress = (int)((double)(startSize - netsWithOutpin.Count) / (double)startSize * 100);

                while (netsToConsiderForFusing.Count > 0)
                {
                    Dictionary<string, XDLNet> fusedNets = new Dictionary<string, XDLNet>();
                    Dictionary<string, Location> fusePoints = new Dictionary<string, Location>();

                    // route from end of antennas
                    foreach (LocationOutsideNet los in AllLocationsOutsideNet(current))
                    {
                        Location loc = los.Location;

                        if (!candidateLocations.ContainsKey(loc.Tile.Location))
                        {
                            continue;
                        }
                        if (!candidateLocations[loc.Tile.Location].ContainsKey(loc.Pip.Name))
                        {
                            continue;
                        }
                        List<string> netNamesToRemove = new List<string>();
                        foreach (XDLNet n in candidateLocations[loc.Tile.Location][loc.Pip.Name])
                        {
                            if (!fusedNets.ContainsKey(n.Name) && !n.Name.Equals(current.Name) && netsToConsiderForFusing.ContainsKey(n.Name))
                            {
                                fusedNets.Add(n.Name, n);
                                fusePoints.Add(n.Name, loc);
                                netNamesToRemove.Add(n.Name);
                            }
                        }
                        candidateLocations[loc.Tile.Location][loc.Pip.Name].RemoveAll(net => netNamesToRemove.Contains(net.Name));
                    }

                    // and check for branches branches                    
                    foreach (XDLPip pip in current.Pips)
                    {
                        if (!candidateLocations.ContainsKey(pip.Location))
                        {
                            continue;
                        }
                        if (!candidateLocations[pip.Location].ContainsKey(pip.From))
                        {
                            continue;
                        }
                        List<string> netNamesToRemove = new List<string>();
                        foreach (XDLNet n in candidateLocations[pip.Location][pip.From])
                        {
                            if (!fusedNets.ContainsKey(n.Name) && !n.Name.Equals(current.Name) && netsToConsiderForFusing.ContainsKey(n.Name))
                            {
                                fusedNets.Add(n.Name, n);
                                fusePoints.Add(n.Name, new Location(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.From)));
                                netNamesToRemove.Add(n.Name);
                            }
                        }
                        candidateLocations[pip.Location][pip.From].RemoveAll(net => netNamesToRemove.Contains(net.Name));
                    }

                    foreach (XDLNet net in fusedNets.Values)
                    {
                        // do not add comments to pips or nets to save memory

                        // TODO nur die nasen dranhaengen, die vom current treiber erreichbar sind
                        // TODO attribute fusionieren
                        current.Add(net, false, "");
                        //current.Add(net, true, "taken from " + net.Name);

                        if (!string.IsNullOrEmpty(net.Header))
                        {
                            if (net.Header.Contains("vcc") || net.Header.Contains("gnd"))
                            {
                                OutputManager.WriteWarning("Attribute lost: " + net.Header);
                            }
                        }

                        OutputManager.WriteOutput("Fusing " + current.Name + " with " + net.Name + " @" + fusePoints[net.Name].ToString());

                        //this.OutputManager.WriteOutput(net.ToString());
                        //this.OutputManager.WriteOutput(net.Foo());
                        //this.OutputManager.WriteOutput("------------------------------------------------");

                        //netlistContainer.RemoveNet(net.Name);
                        netlistContainer.Remove(delegate(Net n) { return n.Name.Equals(net.Name); });
                        netsToConsiderForFusing.Remove(net.Name);
                    }

                    // no futher fusions made, continue with next net
                    if (fusedNets.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        private Dictionary<string, Dictionary<string, List<Net>>> BuildLocationNetMapping(NetlistContainer netlistContainer)
        {
            Dictionary<string, Dictionary<string, List<Net>>> candidateLocations = new Dictionary<string, Dictionary<string, List<Net>>>();
            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    foreach (XDLNet candidate in netlistContainer.Nets)
                    {
                        foreach (XDLPip pip in candidate.Pips)
                        {
                            if (!candidateLocations.ContainsKey(pip.Location))
                            {
                                candidateLocations.Add(pip.Location, new Dictionary<string, List<Net>>());
                            }
                            // from
                            if (!candidateLocations[pip.Location].ContainsKey(pip.From))
                            {
                                candidateLocations[pip.Location].Add(pip.From, new List<Net>());
                            }
                            candidateLocations[pip.Location][pip.From].Add(candidate);
                            // to in case of bidirectional wires
                            if (pip.Operator.Equals("=-"))
                            {
                                if (!candidateLocations[pip.Location].ContainsKey(pip.To))
                                {
                                    candidateLocations[pip.Location].Add(pip.To, new List<Net>());
                                }
                                candidateLocations[pip.Location][pip.To].Add(candidate);
                            }
                        }
                    }
                    break;
                case FPGATypes.BackendType.Vivado:
                    foreach (TCLNet candidate in netlistContainer.Nets)
                    {
                        foreach (TCLRoutingTreeNode node in candidate.RoutingTree.GetAllRoutingNodes().Where(n => !n.VirtualNode))
                        {
                            if (node.Tile.Location.Contains("X10Y94"))
                            {
                            }

                            if (!candidateLocations.ContainsKey(node.Tile.Location))
                            {
                                candidateLocations.Add(node.Tile.Location, new Dictionary<string, List<Net>>());
                            }
                            // from
                            if (!candidateLocations[node.Tile.Location].ContainsKey(node.Port.Name))
                            {
                                candidateLocations[node.Tile.Location].Add(node.Port.Name, new List<Net>());
                            }
                            candidateLocations[node.Tile.Location][node.Port.Name].Add(candidate);
                            // to in case of bidirectional wires ?? TODO See XDL solution
                        }

                    }
                    break;
            }
            return candidateLocations;
        }

        private Dictionary<string, Net> FindNetsToConsiderForFusing(NetlistContainer netlistContainer)
        {
            Dictionary<string, Net> netsToConsiderForFusing = new Dictionary<string, Net>();

            foreach (Net other in netlistContainer.Nets.Where(n => n.OutpinCount == 0 && n.PipCount > 0))
            {
                netsToConsiderForFusing.Add(other.Name, other);
            }
            return netsToConsiderForFusing;
        }

        private Queue<Net> FindNetsWithOutpin(NetlistContainer netlistContainer)
        {
            Queue<Net> netsWithOutpin = new Queue<Net>();

            // find all nets with an outpin and at least one wire and thus reduce the search space  
            foreach (Net net in netlistContainer.Nets)
            {
                if(net.OutpinCount == 1 && net.PipCount > 0)
                {
                    netsWithOutpin.Enqueue(net);
                }
            }

            // bugfix: also consider net with one single pin such as EE2E3 -> LOGICIN_B9 as source for fusing
            foreach (Net net in netlistContainer.Nets.Where(n => n.OutpinCount == 0 && n.InpinCount == 0 & n.PipCount > 0))
            {
                netsWithOutpin.Enqueue(net);
            }

            return netsWithOutpin;
        }       


        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
