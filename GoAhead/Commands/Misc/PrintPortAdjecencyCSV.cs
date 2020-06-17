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
            public Adjacency(List<String> sinks, String firstSectionFilter, String secondSectionFilter)
            {
                this.m_sinks = sinks;
                this.m_1stSection = firstSectionFilter;
                this.m_2ndSection = secondSectionFilter;
            }

            public void AddConnection(String from, String to)
            {
                if (!this.m_connections.ContainsKey(from))
                {
                    this.m_connections.Add(from, new Dictionary<String, bool>());

                    foreach (String sink in this.m_sinks.OrderBy(s => s))
                    {
                        this.m_connections[from][sink] = false;
                    }
                }

                this.m_connections[from][to] = true;
            }

            public override string ToString()
            {
                StringBuilder buffer = new StringBuilder();

                String header = "endport,";
                foreach (String sink in this.m_sinks.OrderBy(s => s))
                {
                    header += sink + ",";
                }
                buffer.AppendLine(header);

                foreach (KeyValuePair<String, Dictionary<String, bool>> tuple in this.m_connections.Where(t => Regex.IsMatch(t.Key, this.m_1stSection)).OrderBy(t => t.Key[t.Key.Length - 1]))
                {
                    this.AddLine(buffer, tuple);
                }

                foreach (KeyValuePair<String, Dictionary<String, bool>> tuple in this.m_connections.Where(t => Regex.IsMatch(t.Key, this.m_2ndSection)).OrderBy(t => t.Key))
                {
                    this.AddLine(buffer, tuple);
                }
                
                return buffer.ToString();
            }

            private void AddLine(StringBuilder buffer, KeyValuePair<String, Dictionary<String, bool>> tuple)
            {
                String line = tuple.Key + ",";
                foreach (KeyValuePair<String, bool> s in tuple.Value)
                {
                    line += (s.Value ? "1" : "0") + ",";
                }
                buffer.AppendLine(line);
            }

            private readonly String m_1stSection;
            private readonly String m_2ndSection;

            private readonly List<String> m_sinks;
            private Dictionary<String, Dictionary<String, bool>> m_connections = new Dictionary<String, Dictionary<String, bool>>();
        }

        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => Objects.IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            Tile interconnect = FPGA.FPGATypes.GetInterconnectTile(clb);

            List<String> lutInPorts = new List<String>();
            foreach (Port lutInPort in clb.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, this.LUTInPortFilter)))
            {
                lutInPorts.Add(lutInPort.Name);
            }
            
            Adjacency adjacency = new Adjacency(lutInPorts, this.EndPortFilter, this.LUTOutPortFilter);

            // section 1
            foreach(Port endPort in interconnect.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, this.EndPortFilter)).OrderBy(p => p.Name[p.Name.Length-1]))
            {
                foreach (Port logicIn in interconnect.SwitchMatrix.GetDrivenPorts(endPort))
                {
                    // goto CLB
                    foreach (Location loc in Navigator.GetDestinations(interconnect, logicIn).Where(l => l.Tile.Location.Equals(clb.Location)))
                    {
                        foreach (Port lutInPort in clb.SwitchMatrix.GetDrivenPorts(loc.Pip).Where(l => Regex.IsMatch(l.Name, this.LUTInPortFilter)))
                        {
                            adjacency.AddConnection(endPort.Name, lutInPort.Name);
                        }                        
                    }
                }
            }

            // section 2
            foreach (Port lutOutPort in clb.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, this.LUTOutPortFilter)))
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
                                foreach (Port lutInPort in clb.SwitchMatrix.GetDrivenPorts(locClb.Pip).Where(l => Regex.IsMatch(l.Name, this.LUTInPortFilter)))
                                {
                                    adjacency.AddConnection(lutOutPort.Name, lutInPort.Name);
                                }  
                            }
                        }
                    }
                }
            }
            this.OutputManager.WriteOutput(adjacency.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Which ports to use for section endport->LUT")]
        public String EndPortFilter = "(EE)|(WW)E2[0-3]";

        [Parameter(Comment = "Which ports to use for section LUT->LUT")]
        public String LUTOutPortFilter = "(MUX)|([A-D]$)";

        [Parameter(Comment = "Which ports to use")]
        public String LUTInPortFilter = "[A-D][1-6]";
    }
}
