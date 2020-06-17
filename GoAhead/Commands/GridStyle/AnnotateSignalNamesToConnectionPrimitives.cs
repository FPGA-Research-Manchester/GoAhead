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
            this.CheckParameters();

            AnnotateSignalNames annotateSignalNames = new AnnotateSignalNames();
            annotateSignalNames.InstantiationFilter = this.InstantiationFilter;
            annotateSignalNames.LibraryElementFilter = REGEX_MATCH_ALL;
            annotateSignalNames.StartIndex = START_INDEX;
            annotateSignalNames.StepWidth = STEP_WIDTH;
            annotateSignalNames.PortMapping = this.GetPortMapping(this.LookupTableInputPort.ToString());
            CommandExecuter.Instance.Execute(annotateSignalNames);
        }

        private List<String> GetPortMapping(String portNumber)
        {
            List<String> portMapping = new List<String>();

            String portMap = String.Empty;
            portMap += $"I({portNumber})";
            portMap += SEPERATOR;
            portMap += this.GetSignalNameWithPrefix(this.InputSignalName);
            portMap += SEPERATOR;
            portMap += this.InputMappingKind;

            portMapping.Add(portMap);

            portMap = String.Empty;
            portMap += this.GetConstantInputPortsPrefix(portNumber);
            portMap += SEPERATOR;
            portMap += GROUND;
            portMap += SEPERATOR;
            portMap += this.InputMappingKind;

            portMapping.Add(portMap);

            portMap = String.Empty;
            portMap += "O";
            portMap += SEPERATOR;
            portMap += this.GetSignalNameWithPrefix(this.OutputSignalName);
            portMap += SEPERATOR;
            portMap += this.OutputMappingKind;

            portMapping.Add(portMap);

            return portMapping;
        }

        private String GetSignalNameWithPrefix(String name)
        {
            String nameWithPrefix;

            if(this.SignalPrefix.Equals(String.Empty) || name.Equals(VCC) || name.Equals(GROUND))
            {
                nameWithPrefix = name;
            }
            else
            {
                nameWithPrefix = $"{this.SignalPrefix}_{name}"; 
            }

            return nameWithPrefix;
        }

        private String GetConstantInputPortsPrefix(String portNumber)
        {
            String inputPorts = "012345";

            inputPorts = inputPorts.Replace(portNumber, String.Empty);
            inputPorts = String.Join("|", inputPorts.ToCharArray());
           
            return $"I({inputPorts})";
        }

        private void CheckParameters()
        {
            bool instantiationFilterIsCorrect = !String.IsNullOrEmpty(this.InstantiationFilter);
            bool inputMappingKindIsCorrect = this.InputMappingKind.Equals(MAPPING_KIND_EXTERNAL) || this.InputMappingKind.Equals(MAPPING_KIND_INTERNAL);
            bool outputMappingKindIsCorrect = this.OutputMappingKind.Equals(MAPPING_KIND_EXTERNAL) || this.OutputMappingKind.Equals(MAPPING_KIND_INTERNAL);
            bool inputSignalNameIsCorrect = !String.IsNullOrEmpty(this.InputSignalName);
            bool outputSignalNameIsCorrect = !String.IsNullOrEmpty(this.OutputSignalName);
            bool lookupTableInputPortIsCorrect = this.LookupTableInputPort >= 0 && this.LookupTableInputPort <= 5;
            bool signalPrefixIsCorrect = !String.IsNullOrEmpty(this.SignalPrefix);

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
        public String InstantiationFilter = ".*";

        [Parameter(Comment = "The mapping kind of the signal connected to the input of the LUTs. Internal signals will be defined within the VHDL component, where external signals will be defined in the entity of the VHDL component.")]
        public String InputMappingKind = "external";

        [Parameter(Comment = "The mapping kind of the signal connected to the output of the LUTs. Internal signals will be defined within the VHDL component, where external signals will be defined in the entity of the VHDL component.")]
        public String OutputMappingKind = "external";

        [Parameter(Comment = "The name of the signal connected to the input of the LUTs")]
        public String InputSignalName = "s2p";

        [Parameter(Comment = "The name of the signal connected to the output of the LUTs")]
        public String OutputSignalName = "p2s";

        [Parameter(Comment = "The input port of the LUTs to connect the input signal to.")]
        public int LookupTableInputPort = 3;

        [Parameter(Comment = "The prefix of the input and output signals")]
        public String SignalPrefix = "x0y0";
    }
}
