using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.Vivado
{
    class PreRoutePRLink : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            int slicePortIndex = 0;
            foreach (LibElemInst inst in Objects.LibraryElementInstanceManager.Instance.GetAllInstantiations())
            {
                PortMapper portMapper = inst.PortMapper;
                Slice s = FPGA.FPGA.Instance.GetSlice(inst.SliceName);
                Tile clb = s.ContainingTile;

                Tuple<String, String, PortMapper.MappingKind> mapping = portMapper.GetMappings().Where(m => Regex.IsMatch(m.Item2, this.SignalName)).FirstOrDefault();

                string signalName = portMapper.GetSignalName(mapping.Item1);
                int index = portMapper.GetIndex(mapping.Item1);
                string netName = this.Prefix + signalName + "[" + index + "]";
                Tuple<Port, Port> arc = clb.SwitchMatrix.GetAllArcs().FirstOrDefault(a => Regex.IsMatch(a.Item1.Name, this.SliceOutPorts[slicePortIndex]));
                slicePortIndex++;
                slicePortIndex %= this.SliceOutPorts.Count;
                if (arc == null)
                {
                    throw new ArgumentException("Cannot start pre route from tile " + clb.Location);
                }
                Location current = Navigator.GetDestinations(clb.Location, arc.Item2.Name).First();
                bool startIsUserSelected = FPGA.TileSelectionManager.Instance.IsUserSelected(current.Tile.TileKey, this.UserSelectionType);
                bool continuePath = true;
                string routingConstraint = @"set_property ROUTE { " + arc.Item1 + " " + arc.Item2 + " ";
                do
                {
                    Port port = current.Tile.SwitchMatrix.GetDrivenPorts(current.Pip).Where(p => Regex.IsMatch(p.Name, this.RoutingResources)).FirstOrDefault();
                    if (port == null)
                    {
                        throw new ArgumentException("Cannot continue pre route from " + current + ". Is the tile at the border of the device?");
                    }
                    routingConstraint += port.Name + " ";
                    current = Navigator.GetDestinations(current.Tile.Location, port.Name).FirstOrDefault();

                    continuePath =
                        (startIsUserSelected && FPGA.TileSelectionManager.Instance.IsUserSelected(current.Tile.TileKey, this.UserSelectionType)) ||
                        (!startIsUserSelected && !FPGA.TileSelectionManager.Instance.IsUserSelected(current.Tile.TileKey, this.UserSelectionType));
                }
                while (continuePath);

                routingConstraint += "} [get_nets " + netName + "]" + Environment.NewLine; ;
                routingConstraint += "set_property IS_ROUTE_FIXED TRUE [get_nets " + netName + "]";
                this.OutputManager.WriteOutput(routingConstraint);

            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the signal to route. The net names will be SignalName[index]")]
        public string SignalName = "p2s";

        [Parameter(Comment = "The name of the user selection to route out of")]
        public string UserSelectionType = "PartialArea";

        [Parameter(Comment = "The slice out ports where the routing paths start")]
        public List<string> SliceOutPorts = new List<string> { "_L_A$", "_L_B$", "_L_C$", "_L_D$" };

        [Parameter(Comment = "The routing resources to use")]
        public string RoutingResources = "WW2BEG[0-3]";

        [Parameter(Comment = "Prefix for support hierarchical nets, e.g. system_i/SW2LED_0/U0/")]
        public string Prefix = "";
    }
}
