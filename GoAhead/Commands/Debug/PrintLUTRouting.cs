using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    class PrintLUTRouting : Command
    {
        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            Tile interconnect = FPGATypes.GetInterconnectTile(clb);

            Dictionary<Port, List<Tuple<Port, Port>>> result = new Dictionary<Port, List<Tuple<Port, Port>>>();

            foreach (string portName in this.EndPorts)
            {
                Port endPort = new Port(portName);
                result.Add(endPort, new List<Tuple<Port, Port>>());

                foreach (Port imux in interconnect.SwitchMatrix.GetDrivenPorts(endPort).Where(p => Regex.IsMatch(p.Name, this.IMUX)))
                {
                    Location l = Navigator.GetDestinations(interconnect, imux).FirstOrDefault();

                    foreach (Port lutInPort in clb.SwitchMatrix.GetDrivenPorts(l.Pip).Where(p => Regex.IsMatch(p.Name, this.LUTInPortFilter)))
                    {
                        result[endPort].Add(new Tuple<Port,Port>(imux,lutInPort));
                    }
                }
            }

            // valid
            bool noMatches = result.Values.Any(l => l.Count == 0);
            if (noMatches)
            {
                this.OutputManager.WriteWarning("Could not enter LUT via all ports -> exit command");
                return;
            }

            // look for unique paths
            foreach (KeyValuePair<Port, List<Tuple<Port, Port>>> portTupleList in result)
            {
                IEnumerable<KeyValuePair<Port, List<Tuple<Port, Port>>>> others = result.Where(t => !t.Key.Equals(portTupleList.Key));
                Port takenInput = null;
                foreach (Tuple<Port, Port> tuple in portTupleList.Value)
                {
                    // t => t.Value.Count == 0 becomes true if the already took some ports
                    if (others.All(t => t.Value.Count == 0 || t.Value.Any(p => !p.Equals(tuple.Item2))))
                    {
                        // found mapping 
                        this.OutputManager.WriteOutput(portTupleList.Key.Name + " -> " + tuple.Item1.Name + " -> " + tuple.Item2.Name);
                        takenInput = tuple.Item2;
                        break;
                    }
                    else
                    {
                        // try next one
                    }
                }
                if(takenInput == null)
                {
                    this.OutputManager.WriteWarning("Did not find an unique LUT inpt for " + portTupleList.Key.Name + " -> exit command");
                    return;
                }
                foreach (KeyValuePair<Port, List<Tuple<Port, Port>>> t in result)
                {
                    t.Value.RemoveAll(p => p.Equals(takenInput));
                }
                // all other tuples must have a LUT in port different from the current one
                //bool unique = result.Where(other => tuple.Key.Equals(other.Key)).All(t => t.Value.FirstOrDefault(p => p.NameKey != tuple.Value[0]));
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = "The end ports")]
        public List<string> EndPorts = new List<string>();

        [Parameter(Comment = "How to get from end port to LUT in")]
        public string IMUX = "IMUX";

        [Parameter(Comment = "Filtr LUT in ports")]
        public string LUTInPortFilter = "_L_A";
    }
}
