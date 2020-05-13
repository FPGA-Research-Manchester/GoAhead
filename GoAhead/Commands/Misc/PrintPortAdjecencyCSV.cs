using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Misc
{
    class PrintPortAdjacencyCSV : CommandWithFileOutput
    {
        class Adjacency
        {
            public Adjacency(List<string> sinks, string firstSectionFilter, string secondSectionFilter)
            {
                m_sinks = sinks;
                m_1stSection = firstSectionFilter;
                m_2ndSection = secondSectionFilter;
            }

            public void AddConnection(string from, string to)
            {
                if (!m_connections.ContainsKey(from))
                {
                    m_connections.Add(from, new Dictionary<string, bool>());

                    foreach (string sink in m_sinks.OrderBy(s => s))
                    {
                        m_connections[from][sink] = false;
                    }
                }

                m_connections[from][to] = true;
            }

            public override string ToString()
            {
                StringBuilder buffer = new StringBuilder();

                string header = "endport,";
                foreach (string sink in m_sinks.OrderBy(s => s))
                {
                    header += sink + ",";
                }
                buffer.AppendLine(header);

                foreach (KeyValuePair<string, Dictionary<string, bool>> tuple in m_connections.Where(t => Regex.IsMatch(t.Key, m_1stSection)).OrderBy(t => t.Key[t.Key.Length - 1]))
                {
                    AddLine(buffer, tuple);
                }

                foreach (KeyValuePair<string, Dictionary<string, bool>> tuple in m_connections.Where(t => Regex.IsMatch(t.Key, m_2ndSection)).OrderBy(t => t.Key))
                {
                    AddLine(buffer, tuple);
                }
                
                return buffer.ToString();
            }

            private void AddLine(StringBuilder buffer, KeyValuePair<string, Dictionary<string, bool>> tuple)
            {
                string line = tuple.Key + ",";
                foreach (KeyValuePair<string, bool> s in tuple.Value)
                {
                    line += (s.Value ? "1" : "0") + ",";
                }
                buffer.AppendLine(line);
            }

            private readonly string m_1stSection;
            private readonly string m_2ndSection;

            private readonly List<string> m_sinks;
            private Dictionary<string, Dictionary<string, bool>> m_connections = new Dictionary<string, Dictionary<string, bool>>();
        }

        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            Tile interconnect = FPGATypes.GetInterconnectTile(clb);

            List<string> lutInPorts = new List<string>();
            foreach (Port lutInPort in clb.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, LUTInPortFilter)))
            {
                lutInPorts.Add(lutInPort.Name);
            }
            
            Adjacency adjacency = new Adjacency(lutInPorts, EndPortFilter, LUTOutPortFilter);

            // section 1
            foreach(Port endPort in interconnect.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, EndPortFilter)).OrderBy(p => p.Name[p.Name.Length-1]))
            {
                foreach (Port logicIn in interconnect.SwitchMatrix.GetDrivenPorts(endPort))
                {
                    // goto CLB
                    foreach (Location loc in Navigator.GetDestinations(interconnect, logicIn).Where(l => l.Tile.Location.Equals(clb.Location)))
                    {
                        foreach (Port lutInPort in clb.SwitchMatrix.GetDrivenPorts(loc.Pip).Where(l => Regex.IsMatch(l.Name, LUTInPortFilter)))
                        {
                            adjacency.AddConnection(endPort.Name, lutInPort.Name);
                        }                        
                    }
                }
            }

            // section 2
            foreach (Port lutOutPort in clb.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, LUTOutPortFilter)))
            {
                foreach (Port logicOut in clb.SwitchMatrix.GetDrivenPorts(lutOutPort))
                {
                    // goto int
                    foreach (Location locInt in Navigator.GetDestinations(clb, logicOut).Where(l => l.Tile.Location.Equals(interconnect.Location)))
                    {
                        foreach (Port logicIn in interconnect.SwitchMatrix.GetDrivenPorts(locInt.Pip))
                        {
                            foreach (Location locClb in Navigator.GetDestinations(interconnect, logicIn).Where(l => l.Tile.Location.Equals(clb.Location)))
                            {
                                foreach (Port lutInPort in clb.SwitchMatrix.GetDrivenPorts(locClb.Pip).Where(l => Regex.IsMatch(l.Name, LUTInPortFilter)))
                                {
                                    adjacency.AddConnection(lutOutPort.Name, lutInPort.Name);
                                }  
                            }
                        }
                    }
                }
            }
            OutputManager.WriteOutput(adjacency.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Which ports to use for section endport->LUT")]
        public string EndPortFilter = "(EE)|(WW)E2[0-3]";

        [Parameter(Comment = "Which ports to use for section LUT->LUT")]
        public string LUTOutPortFilter = "(MUX)|([A-D]$)";

        [Parameter(Comment = "Which ports to use")]
        public string LUTInPortFilter = "[A-D][1-6]";
    }
}
