using GoAhead.Commands.LibraryElementInstantiationManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    class AnnotateSignalNamesToConnectionPrimitives : Command
    {
        private const string SEPERATOR = ":";

        private const string REGEX_MATCH_ALL = ".*";

        private const string MAPPING_KIND_EXTERNAL = "external";
        private const string MAPPING_KIND_INTERNAL = "internal";
 
        private const string VCC = "1";
        private const string GROUND = "0";

        private const int START_INDEX = 0;
        private const int STEP_WIDTH = 1;

        protected override void DoCommandAction()
        {
            CheckParameters();

            AnnotateSignalNames annotateSignalNames = new AnnotateSignalNames();
            annotateSignalNames.InstantiationFilter = InstantiationFilter;
            annotateSignalNames.LibraryElementFilter = REGEX_MATCH_ALL;
            annotateSignalNames.StartIndex = START_INDEX;
            annotateSignalNames.StepWidth = STEP_WIDTH;
            annotateSignalNames.PortMapping = GetPortMapping(LookupTableInputPort.ToString());
            CommandExecuter.Instance.Execute(annotateSignalNames);
        }

        private List<string> GetPortMapping(string portNumber)
        {
            List<string> portMapping = new List<string>();

            string portMap = string.Empty;
            portMap += $"I({portNumber})";
            portMap += SEPERATOR;
            portMap += GetSignalNameWithPrefix(InputSignalName);
            portMap += SEPERATOR;
            portMap += InputMappingKind;

            portMapping.Add(portMap);

            portMap = string.Empty;
            portMap += GetConstantInputPortsPrefix(portNumber);
            portMap += SEPERATOR;
            portMap += GROUND;
            portMap += SEPERATOR;
            portMap += InputMappingKind;

            portMapping.Add(portMap);

            portMap = string.Empty;
            portMap += "O";
            portMap += SEPERATOR;
            portMap += GetSignalNameWithPrefix(OutputSignalName);
            portMap += SEPERATOR;
            portMap += OutputMappingKind;

            portMapping.Add(portMap);

            return portMapping;
        }

        private string GetSignalNameWithPrefix(string name)
        {
            string nameWithPrefix;

            if(SignalPrefix.Equals(string.Empty) || name.Equals(VCC) || name.Equals(GROUND))
            {
                nameWithPrefix = name;
            }
            else
            {
                nameWithPrefix = $"{SignalPrefix}_{name}"; 
            }

            return nameWithPrefix;
        }

        private string GetConstantInputPortsPrefix(string portNumber)
        {
            string inputPorts = "012345";

            inputPorts = inputPorts.Replace(portNumber, string.Empty);
            inputPorts = string.Join("|", inputPorts.ToCharArray());
           
            return $"I({inputPorts})";
        }

        private void CheckParameters()
        {
            bool instantiationFilterIsCorrect = !string.IsNullOrEmpty(InstantiationFilter);
            bool inputMappingKindIsCorrect = InputMappingKind.Equals(MAPPING_KIND_EXTERNAL) || InputMappingKind.Equals(MAPPING_KIND_INTERNAL);
            bool outputMappingKindIsCorrect = OutputMappingKind.Equals(MAPPING_KIND_EXTERNAL) || OutputMappingKind.Equals(MAPPING_KIND_INTERNAL);
            bool inputSignalNameIsCorrect = !string.IsNullOrEmpty(InputSignalName);
            bool outputSignalNameIsCorrect = !string.IsNullOrEmpty(OutputSignalName);
            bool lookupTableInputPortIsCorrect = LookupTableInputPort >= 0 && LookupTableInputPort <= 5;
            bool signalPrefixIsCorrect = !string.IsNullOrEmpty(SignalPrefix);

            if (!instantiationFilterIsCorrect || !inputMappingKindIsCorrect || !outputMappingKindIsCorrect || !inputSignalNameIsCorrect ||
               !outputSignalNameIsCorrect || !lookupTableInputPortIsCorrect || !signalPrefixIsCorrect)
            {
                throw new ArgumentException("Unexpected format in one of the parameters.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Only consider instantiations that match this filter.")]
        public string InstantiationFilter = ".*";

        [Parameter(Comment = "The mapping kind of the signal connected to the input of the LUTs. Internal signals will be defined within the VHDL component, where external signals will be defined in the entity of the VHDL component.")]
        public string InputMappingKind = "external";

        [Parameter(Comment = "The mapping kind of the signal connected to the output of the LUTs. Internal signals will be defined within the VHDL component, where external signals will be defined in the entity of the VHDL component.")]
        public string OutputMappingKind = "external";

        [Parameter(Comment = "The name of the signal connected to the input of the LUTs")]
        public string InputSignalName = "s2p";

        [Parameter(Comment = "The name of the signal connected to the output of the LUTs")]
        public string OutputSignalName = "p2s";

        [Parameter(Comment = "The input port of the LUTs to connect the input signal to.")]
        public int LookupTableInputPort = 3;

        [Parameter(Comment = "The prefix of the input and output signals")]
        public string SignalPrefix = "x0y0";
    }
}
