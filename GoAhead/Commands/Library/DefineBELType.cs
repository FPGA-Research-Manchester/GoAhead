using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Define a BEL Type for the AddVivadoConnectionPrimitive command.", Wrapper = false, Publish = true)]
    class DefineBELType : Command
    {
        [Parameter(Comment = "Name for the BEL Type")]
        public string Name = "LUT6";

        [Parameter(Comment = "VHDL Generic Map")]
        public string VHDLGenericMap = "generic map ( INIT => X\"ABCDABCDABCDABCD\" )";

        [Parameter(Comment = "Output Ports")]
        public List<string> OutputPorts = new List<string> { "O" };

        [Parameter(Comment = "Input Ports")]
        public List<string> InputPorts = new List<string> { "I0", "I1", "I2", "I3", "I4", "I5" };

        [Parameter(Comment = "Should the inputs be made constant. Default=false")]
        public bool InputsConstantValue = false;

        //[Parameter(Comment = "")]
        //public bool TraverseBackwards = false;

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void DoCommandAction()
        {
            VivadoBELManager.Instance.AddBELType(Name, new BELType
            {
                VHDLGenericMap = VHDLGenericMap,
                outputNames = OutputPorts,
                inputNames = InputPorts,
                inputsConstantValue = InputsConstantValue
            });
        }
    }
}
