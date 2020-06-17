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
            this.PortMapping = Regex.Replace(this.PortMapping, "\\\"", "");
            this.InstantiationFilter = Regex.Replace(this.InstantiationFilter, "\\\"", "");

            Queue<LibElemInst> instantiations = new Queue<LibElemInst>();
            Dictionary<String, bool> libraryElementNames = new Dictionary<String, bool>();
            foreach (LibElemInst inst in Objects.LibraryElementInstanceManager.Instance.GetAllInstantiations())
            {
                // collect all macro names
                if (!libraryElementNames.ContainsKey(inst.LibraryElementName))
                {
                    libraryElementNames.Add(inst.LibraryElementName, false);
                }

                // filter
                if (Regex.IsMatch(inst.InstanceName, this.InstantiationFilter))
                {
                    instantiations.Enqueue(inst);
                }
            }

            Dictionary<String, String> portMapping = PortMappingHandler.GetPortMapping(this.PortMapping);
            Dictionary<String, int> indeces = new Dictionary<String, int>();

            foreach (String key in portMapping.Keys)
            {
                indeces.Add(key, 0);
            }

            StringBuilder instanceCode = new StringBuilder();

            SortedDictionary<String, int> signalWidths = new SortedDictionary<String, int>();
            Dictionary<String, String> directions = new Dictionary<String, String>();

            while (instantiations.Count > 0)
            {
                LibElemInst inst = instantiations.Dequeue();
                LibraryElement libElement = Objects.Library.Instance.GetElement(inst.LibraryElementName);

                instanceCode.AppendLine("-- instantiation of " + inst.InstanceName);
                instanceCode.AppendLine(inst.InstanceName + " : " + libElement.PrimitiveName);
                instanceCode.AppendLine("Port Map (");

                List<String> mappings = new List<String>();

                foreach (XDLPort port in ((XDLContainer)libElement.Containter).Ports)
                {
                    String key;
                    if (PortMappingHandler.HasMapping(port, portMapping, out key))
                    {
                        String rightHandSide = portMapping[key];

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
                        this.OutputManager.WriteOutput("Warning: Could not find a right hand side for port " + port.ExternalName + ". Misspelled mapping? Note that the mapping is case sensistive.");

                        //mappings.Add("\t" + port.ExternalName + " => ,");
                    }
                }

                mappings[mappings.Count - 1] = Regex.Replace(mappings[mappings.Count - 1], ",$", "");

                // update tool info 
                Tile t = FPGA.FPGA.Instance.GetTile(inst.AnchorLocation);
                Objects.Blackboard.Instance.ClearToolTipInfo(t);

                foreach (String str in mappings)
                {
                    instanceCode.AppendLine(str);

                    // update tool info 
                    if (!str.EndsWith(" => ,"))
                    {
                        String toolTip = str;
                        toolTip = Regex.Replace(toolTip, @"^\s+", "");
                        toolTip = Regex.Replace(toolTip, ",", "");
                        toolTip += Environment.NewLine;
                        Objects.Blackboard.Instance.AddToolTipInfo(t, toolTip);
                    }
                }

                instanceCode.AppendLine(");");
            }

            if (this.EmbedInstantionInEntity)
            {
                this.OutputManager.WriteVHDLOutput("library IEEE;");
                this.OutputManager.WriteVHDLOutput("use IEEE.STD_LOGIC_1164.ALL;");
                this.OutputManager.WriteVHDLOutput("");

                this.OutputManager.WriteVHDLOutput("entity " + this.EntityName + " is port (");

                // we need signalWidths as a list ...
                List<KeyValuePair<String, int>> interfaceSignals = new List<KeyValuePair<String, int>>();
                foreach (KeyValuePair<String, int> tupel in signalWidths)
                {
                    interfaceSignals.Add(tupel);
                }
                for (int i = 0; i < interfaceSignals.Count; i++)
                {
                    String signalName = interfaceSignals[i].Key;
                    String line = signalName + " : " + directions[signalName] + " std_logic_vector(" + interfaceSignals[i].Value + " downto 0)";
                    // ... to find the last index
                    if (i < interfaceSignals.Count - 1)
                    {
                        line += ";";
                    }
                    else
                    {
                        line += ");";
                    }
                    this.OutputManager.WriteVHDLOutput(line);
                }
                this.OutputManager.WriteVHDLOutput("end " + this.EntityName + ";");
                this.OutputManager.WriteVHDLOutput("");
                this.OutputManager.WriteVHDLOutput("architecture Behavioral of " + this.EntityName + " is");
                this.OutputManager.WriteVHDLOutput("");

                // use command
                foreach (String libraryElementName in libraryElementNames.Keys)
                {
                    PrintComponentDeclaration printWrapperCmd = new PrintComponentDeclaration();
                    printWrapperCmd.LibraryElement = libraryElementName;
                    printWrapperCmd.Do();
                    this.OutputManager.WriteVHDLOutput(printWrapperCmd.OutputManager.GetVHDLOuput());
                }

                this.OutputManager.WriteVHDLOutput("begin");
            }


            // print out signals declarations
            if (this.PrintSignalDeclarations)
            {
                this.OutputManager.WriteVHDLOutput("--attribute s : string;");

                foreach (KeyValuePair<String, int> tupel in signalWidths)
                {
                    String decl = "signal " + tupel.Key + " : std_logic_vector(" + tupel.Value + " downto 0) := (others => '1');";
                    this.OutputManager.WriteVHDLOutput(decl);
                }

                foreach (KeyValuePair<String, int> tupel in signalWidths)
                {
                    String attr = "attribute s of " + tupel.Key + " : signal is \"true\";";
                    this.OutputManager.WriteVHDLOutput(attr);
                }
            }

            // print out instances
            if (this.PrintInstantiations)
            {
                this.OutputManager.WriteVHDLOutput(instanceCode.ToString());
            }

            if (this.EmbedInstantionInEntity)
            {
                this.OutputManager.WriteVHDLOutput("end architecture Behavioral;");
            }
        }

        
        public override void Undo()
        {
        }

        [Parameter(Comment = "How to map macro ports to VHDL signals. e.g. LI:staticToPartial,LO:static_from_partial,L_RST=>0")]
        public String PortMapping = "I:StaticToPartial,O:StaticFromPartial,H:1";

        [Parameter(Comment = "Only consider those library lelement instantiations whose name matches this filter")]
        public String InstantiationFilter = ".*";

        [Parameter(Comment = "Wheter to print out the instantiations")]
        public bool PrintInstantiations = true;

        [Parameter(Comment = "Wheter to print out signal delcarations")]
        public bool PrintSignalDeclarations = true;

        [Parameter(Comment = "Wheter to embed the instantiation in an entity and architecture")]
        public bool EmbedInstantionInEntity = false;

        [Parameter(Comment = "The name of the entity to embed the macor instantiations in, applies only if EmbeddInstantionInEntity is true")]
        public String EntityName = "PartialSubsystem";
    }
}
