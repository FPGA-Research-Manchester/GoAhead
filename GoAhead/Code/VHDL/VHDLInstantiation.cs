using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.Commands;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.VHDL
{
    public class VHDLInstantiation
    {
        public VHDLInstantiation(VHDLFile container, LibElemInst instantiation, LibraryElement libElement, Command callee)
        {
            m_container = container;
            m_instantation = instantiation;
            m_libraryElement = libElement;
            IntergrateIntoContainer(callee);
        }

        public override string ToString()
        {
            return m_instanceCode.ToString();
        }

        private void IntergrateIntoContainer(Command callee)
        {
            m_instanceCode.AppendLine("-- instantiation of " + m_instantation.InstanceName);
            m_instanceCode.AppendLine(m_instantation.InstanceName + " : " + m_instantation.GetLibraryElement().PrimitiveName);
            if (!string.IsNullOrEmpty(m_libraryElement.VHDLGenericMap))
            {
                m_instanceCode.AppendLine(m_libraryElement.VHDLGenericMap);
            }
            m_instanceCode.AppendLine("port map (");

            List<string> mappings = new List<string>();

            foreach (XDLPort port in ((XDLContainer)m_libraryElement.Containter).Ports)
            {
                if (!m_instantation.PortMapper.HasKindMapping(port.ExternalName))
                {
                    callee.OutputManager.WriteOutput("Warning: Could not find a signal mapping for port " + port.ExternalName + ". Misspelled mapping? Note that the mapping is case sensitive.");
                    continue;
                }

                PortMapper mapper = m_instantation.PortMapper;
                PortMapper.MappingKind mappingKind = mapper.GetMapping(port.ExternalName);
                string rightHandSide = mapper.GetSignalName(port.ExternalName);

                // do not vectorize already indeced ports
                //bool vector = Regex.IsMatch(port.ExternalName, @"\d+$") && !Regex.IsMatch(rightHandSide, @"\(\d+\)$");

                if (rightHandSide.Equals("0") || rightHandSide.Equals("1"))
                {
                    mappings.Add("\t" + port.ExternalName + " => " + "'" + rightHandSide + "',");
                }
                else if (rightHandSide.Equals("open"))
                {
                    mappings.Add("\t" + port.ExternalName + " => open,");
                }
                else if (!rightHandSide.Equals("open"))
                {
                    VHDLSignalList signalList = null;
                    switch (mappingKind)
                    {
                        case PortMapper.MappingKind.NoVector:
                        case PortMapper.MappingKind.External:
                            {
                                // in case of entity signals add direction
                                signalList = m_container.Entity;
                                m_container.Entity.SetDirection(rightHandSide, port.Direction);
                                break;
                            }
                        case PortMapper.MappingKind.Internal:
                            {
                                signalList = m_container.SignalDeclaration;
                                break;
                            }

                        default:
                            throw new ArgumentException("Found port mapped to " + mappingKind + ". This mapping is not supported. Use either public or external, see command AddPortMapping");
                    }

                    if (!signalList.HasSignal(rightHandSide))
                    {
                        if (m_instantation.PortMapper.HasKindMapping(port.ExternalName))
                        {
                            signalList.Add(rightHandSide, 1, m_instantation.PortMapper.GetMapping(port.ExternalName));
                        }
                        else
                        {
                            signalList.Add(rightHandSide, 1);
                        }
                    }

                    switch (mappingKind)
                    {
                        case PortMapper.MappingKind.Internal:
                        case PortMapper.MappingKind.External:
                            {
                                //int index = signalList.GetSignalWidth(rightHandSide) - 1;
                                int index = mapper.GetIndex(port.ExternalName);
                                mappings.Add("\t" + port.ExternalName + " => " + rightHandSide + "(" + index + "),");

                                if (!signalList.HasSignal(rightHandSide))
                                {
                                    signalList.Add(rightHandSide, -1);
                                }
                                if (signalList.GetSignalWidth(rightHandSide) <= index)
                                {
                                    signalList.SetSignalWidth(rightHandSide, index + 1);
                                }

                                // store the index generated during VHDL generation for interface checks
                                //this.m_instantation.GetPortMapper().SetIndex(port.ExternalName, index);
                                break;
                            }
                        case PortMapper.MappingKind.NoVector:
                            {
                                mappings.Add("\t" + port.ExternalName + " => " + rightHandSide + ",");
                                break;
                            }

                        default:
                            throw new ArgumentException("Found port mapped to " + mappingKind + ". This mapping is not supported. Use either public or external, see command AddPortMapping");
                    }
                }
            }

            if (mappings.Count > 0)
            {
                mappings[mappings.Count - 1] = Regex.Replace(mappings[mappings.Count - 1], ",$", "");
            }

            // update tool info
            Tile t = FPGA.FPGA.Instance.GetTile(m_instantation.AnchorLocation);
            Blackboard.Instance.ClearToolTipInfo(t);

            foreach (string str in mappings)
            {
                m_instanceCode.AppendLine(str);

                // update tool info
                if (!str.EndsWith(" => ,"))
                {
                    string toolTip = str;
                    toolTip = Regex.Replace(toolTip, @"^\s+", "");
                    toolTip = Regex.Replace(toolTip, ",", "");
                    toolTip += Environment.NewLine;
                    Blackboard.Instance.AddToolTipInfo(t, toolTip);
                }
            }

            m_instanceCode.AppendLine(");");
        }

        private StringBuilder m_instanceCode = new StringBuilder();
        private readonly LibElemInst m_instantation;
        private readonly LibraryElement m_libraryElement;
        private readonly VHDLFile m_container;
    }
}