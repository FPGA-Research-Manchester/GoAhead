using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.VHDL
{
    [CommandDescription(Description = "Deprecated", Wrapper = false)]
    class PrintVHDLInstantiation : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            PortMapping = Regex.Replace(PortMapping, "\\\"", "");
            InstantiationFilter = Regex.Replace(InstantiationFilter, "\\\"", "");

            Queue<LibElemInst> instantiations = new Queue<LibElemInst>();
            Dictionary<string, bool> libraryElementNames = new Dictionary<string, bool>();
            foreach (LibElemInst inst in LibraryElementInstanceManager.Instance.GetAllInstantiations())
            {
                // collect all macro names
                if (!libraryElementNames.ContainsKey(inst.LibraryElementName))
                {
                    libraryElementNames.Add(inst.LibraryElementName, false);
                }

                // filter
                if (Regex.IsMatch(inst.InstanceName, InstantiationFilter))
                {
                    instantiations.Enqueue(inst);
                }
            }

            Dictionary<string, string> portMapping = PortMappingHandler.GetPortMapping(PortMapping);
            Dictionary<string, int> indeces = new Dictionary<string, int>();

            foreach (string key in portMapping.Keys)
            {
                indeces.Add(key, 0);
            }

            StringBuilder instanceCode = new StringBuilder();

            SortedDictionary<string, int> signalWidths = new SortedDictionary<string, int>();
            Dictionary<string, string> directions = new Dictionary<string, string>();

            while (instantiations.Count > 0)
            {
                LibElemInst inst = instantiations.Dequeue();
                LibraryElement libElement = Objects.Library.Instance.GetElement(inst.LibraryElementName);

                instanceCode.AppendLine("-- instantiation of " + inst.InstanceName);
                instanceCode.AppendLine(inst.InstanceName + " : " + libElement.PrimitiveName);
                instanceCode.AppendLine("Port Map (");

                List<string> mappings = new List<string>();

                foreach (XDLPort port in ((XDLContainer)libElement.Containter).Ports)
                {
                    string key;
                    if (PortMappingHandler.HasMapping(port, portMapping, out key))
                    {
                        string rightHandSide = portMapping[key];

                        // do not vectorize allready indeced ports
                        bool vector = Regex.IsMatch(port.ExternalName, @"\d+$") && !Regex.IsMatch(rightHandSide, @"\(\d+\)$");
                        bool constantAssigned = false;

                        if (rightHandSide.Equals("0"))
                        {
                            rightHandSide = "'" + rightHandSide + "'";
                            vector = false;
                            constantAssigned = true;
                        }
                        if (rightHandSide.Equals("1"))
                        {
                            rightHandSide = "'" + rightHandSide + "'";
                            vector = false;
                            constantAssigned = true;
                        }
                        if (vector && !rightHandSide.Equals("open"))
                        {
                            int index = indeces[key]++;
                            mappings.Add("\t" + port.ExternalName + " => " + rightHandSide + "(" + index + "),");

                            // store right hand sides for signal declarations
                            if (!signalWidths.ContainsKey(rightHandSide))
                            {
                                signalWidths.Add(rightHandSide, 0);
                                directions.Add(rightHandSide, port.Direction.ToString().ToLower());
                            }
                            else
                            {
                                signalWidths[rightHandSide]++;
                            }
                        }
                        else
                        {
                            mappings.Add("\t" + port.ExternalName + " => " + rightHandSide + ",");
                            if (!constantAssigned && !rightHandSide.Equals("open"))
                            {
                                if (!signalWidths.ContainsKey(rightHandSide))
                                {
                                    signalWidths.Add(rightHandSide, 0);
                                }
                                if (!directions.ContainsKey(rightHandSide))
                                {
                                    directions.Add(rightHandSide, port.Direction.ToString().ToLower());
                                }
                                else
                                {
                                    if (!directions[rightHandSide].Equals(port.Direction.ToString().ToLower()))
                                    {
                                        throw new ArgumentException("Port direction changed for port " + port.ExternalName);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // no mapping, issue warning
                        OutputManager.WriteOutput("Warning: Could not find a right hand side for port " + port.ExternalName + ". Misspelled mapping? Note that the mapping is case sensistive.");

                        //mappings.Add("\t" + port.ExternalName + " => ,");
                    }
                }

                mappings[mappings.Count - 1] = Regex.Replace(mappings[mappings.Count - 1], ",$", "");

                // update tool info 
                Tile t = FPGA.FPGA.Instance.GetTile(inst.AnchorLocation);
                Blackboard.Instance.ClearToolTipInfo(t);

                foreach (string str in mappings)
                {
                    instanceCode.AppendLine(str);

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

                instanceCode.AppendLine(");");
            }

            if (EmbedInstantionInEntity)
            {
                OutputManager.WriteVHDLOutput("library IEEE;");
                OutputManager.WriteVHDLOutput("use IEEE.STD_LOGIC_1164.ALL;");
                OutputManager.WriteVHDLOutput("");

                OutputManager.WriteVHDLOutput("entity " + EntityName + " is port (");

                // we need signalWidths as a list ...
                List<KeyValuePair<string, int>> interfaceSignals = new List<KeyValuePair<string, int>>();
                foreach (KeyValuePair<string, int> tupel in signalWidths)
                {
                    interfaceSignals.Add(tupel);
                }
                for (int i = 0; i < interfaceSignals.Count; i++)
                {
                    string signalName = interfaceSignals[i].Key;
                    string line = signalName + " : " + directions[signalName] + " std_logic_vector(" + interfaceSignals[i].Value + " downto 0)";
                    // ... to find the last index
                    if (i < interfaceSignals.Count - 1)
                    {
                        line += ";";
                    }
                    else
                    {
                        line += ");";
                    }
                    OutputManager.WriteVHDLOutput(line);
                }
                OutputManager.WriteVHDLOutput("end " + EntityName + ";");
                OutputManager.WriteVHDLOutput("");
                OutputManager.WriteVHDLOutput("architecture Behavioral of " + EntityName + " is");
                OutputManager.WriteVHDLOutput("");

                // use command
                foreach (string libraryElementName in libraryElementNames.Keys)
                {
                    PrintComponentDeclaration printWrapperCmd = new PrintComponentDeclaration();
                    printWrapperCmd.LibraryElement = libraryElementName;
                    printWrapperCmd.Do();
                    OutputManager.WriteVHDLOutput(printWrapperCmd.OutputManager.GetVHDLOuput());
                }

                OutputManager.WriteVHDLOutput("begin");
            }


            // print out signals declarations
            if (PrintSignalDeclarations)
            {
                OutputManager.WriteVHDLOutput("--attribute s : string;");

                foreach (KeyValuePair<string, int> tupel in signalWidths)
                {
                    string decl = "signal " + tupel.Key + " : std_logic_vector(" + tupel.Value + " downto 0) := (others => '1');";
                    OutputManager.WriteVHDLOutput(decl);
                }

                foreach (KeyValuePair<string, int> tupel in signalWidths)
                {
                    string attr = "attribute s of " + tupel.Key + " : signal is \"true\";";
                    OutputManager.WriteVHDLOutput(attr);
                }
            }

            // print out instances
            if (PrintInstantiations)
            {
                OutputManager.WriteVHDLOutput(instanceCode.ToString());
            }

            if (EmbedInstantionInEntity)
            {
                OutputManager.WriteVHDLOutput("end architecture Behavioral;");
            }
        }

        
        public override void Undo()
        {
        }

        [Parameter(Comment = "How to map macro ports to VHDL signals. e.g. LI:staticToPartial,LO:static_from_partial,L_RST=>0")]
        public string PortMapping = "I:StaticToPartial,O:StaticFromPartial,H:1";

        [Parameter(Comment = "Only consider those library lelement instantiations whose name matches this filter")]
        public string InstantiationFilter = ".*";

        [Parameter(Comment = "Wheter to print out the instantiations")]
        public bool PrintInstantiations = true;

        [Parameter(Comment = "Wheter to print out signal delcarations")]
        public bool PrintSignalDeclarations = true;

        [Parameter(Comment = "Wheter to embed the instantiation in an entity and architecture")]
        public bool EmbedInstantionInEntity = false;

        [Parameter(Comment = "The name of the entity to embed the macor instantiations in, applies only if EmbeddInstantionInEntity is true")]
        public string EntityName = "PartialSubsystem";
    }
}
