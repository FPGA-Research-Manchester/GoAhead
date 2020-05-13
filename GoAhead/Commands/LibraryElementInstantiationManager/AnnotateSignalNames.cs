using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.LibraryElementInstantiationManager
{
    [CommandDescription(Description = "Define for each port of a library element instantiation to 1) which signal it is connected to, 2) whether this signal is external", Wrapper = false)]
    class AnnotateSignalNames : Command
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE, FPGA.FPGATypes.BackendType.Vivado);

            // map a library element port to 
            // kind (internal, external, no vecotr
            Dictionary<string, string> signals = new Dictionary<string, string>();
            // and to a signal name
            Dictionary<string, PortMapper.MappingKind> mappings = new Dictionary<string, PortMapper.MappingKind>();
            Dictionary<string, List<int>> stepWidth = new Dictionary<string, List<int>>();            

            foreach(string triplet in PortMapping)
            {
                string[] atoms = triplet.Split(':');
                if(atoms.Length < 2)
                {
                    throw new ArgumentException("Unexpected format in PortMapping. See parameter description for an example");
                }
                string libraryElementPortNameRegexp = atoms[0];
                string connectedSignal = atoms[1];

                // external is the default value
                PortMapper.MappingKind mapping = PortMapper.MappingKind.External;
                if (atoms.Length == 3)
                {
                    switch (atoms[2])
                    {
                        case "internal":
                            {
                                mapping = PortMapper.MappingKind.Internal;
                                break;
                            }
                        case "external":
                            {
                                mapping = PortMapper.MappingKind.External;
                                break;
                            }
                        case "no_vector":
                            {
                                mapping = PortMapper.MappingKind.NoVector;
                                break;
                            }
                        default:
                            {
                                throw new ArgumentException("Unexpected value " + triplet + "in PortMapping. See parameter description for an example");
                            }
                    }
                }

                signals[libraryElementPortNameRegexp] = connectedSignal;
                mappings[libraryElementPortNameRegexp] = mapping;
                stepWidth[libraryElementPortNameRegexp] = new List<int>();
                if (atoms.Length == 4)
                {
                    foreach (string intAsString in atoms[3].Split('-'))
                    {
                        stepWidth[libraryElementPortNameRegexp].Add(int.Parse(intAsString));
                    }
                }
            }

            if (LibraryElementFilter.Contains("LUT"))
            {

            }

            foreach (string libraryElementPortNameRegexp in signals.Keys)
            {
                // index with every signal 
                // two use case: increment or stepwidth
                int index = StartIndex;
                int wraps = 0;
                foreach (LibElemInst inst in LibraryElementInstanceManager.Instance.GetAllInstantiations().Where(inst =>
                    Regex.IsMatch(inst.InstanceName, InstantiationFilter) &&
                    Regex.IsMatch(inst.LibraryElementName, LibraryElementFilter)))
                {
                    LibraryElement libElement = Objects.Library.Instance.GetElement(inst.LibraryElementName);
                    foreach (XDLPort port in libElement.Containter.Ports.Where(p => Regex.IsMatch(p.ExternalName, libraryElementPortNameRegexp)).OrderBy(p => p.ExternalName))
                    {
                        if (port.ConstantValuePort)
                        {
                            inst.PortMapper.AddMapping(port.ExternalName, "0", PortMapper.MappingKind.External, index);
                        }
                        else
                        {
                            if (index >= stepWidth[libraryElementPortNameRegexp].Count && stepWidth[libraryElementPortNameRegexp].Count > 0)
                            {
                                wraps++;
                                index = 0;
                            }
                            int nextIndex = stepWidth[libraryElementPortNameRegexp].Count == 0 ? index : stepWidth[libraryElementPortNameRegexp].Count*wraps + stepWidth[libraryElementPortNameRegexp][index];
                            inst.PortMapper.AddMapping(
                                port.ExternalName, 
                                signals[libraryElementPortNameRegexp], 
                                mappings[libraryElementPortNameRegexp],
                                nextIndex);
                            index += StepWidth;
                            
                        }
                    }
                }
            }             
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Only consider those library element instantiations whose name matches this filter")]
        public string InstantiationFilter = ".*";

        [Parameter(Comment = "Only consider those library element instantiations of this type")]
        public string LibraryElementFilter = ".*";

        [Parameter(Comment = "The start of the signal index")]
        public int StartIndex = 0;

        [Parameter(Comment = "The step width for incrementing signal index")]
        public int StepWidth = 1;

        [Parameter(Comment = "How to map library element instantiation ports, e.g. H:1,I:RES_1:external,O:not_used_0:internal,CLK:CLK:no_vector")]
        public List<string> PortMapping = new List<string>();
    }
}
