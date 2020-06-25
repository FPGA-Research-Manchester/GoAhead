using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.GridStyle
{
    class AddLUTConnectionPrimitive : Command
    {
        private const int LUT_SIZE = 6;

        protected override void DoCommandAction()
        {
            CheckParameters();

            LibraryElement libraryElement = GetLibraryElement();

            Objects.Library.Instance.Add(libraryElement);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private LibraryElement GetLibraryElement()
        {
            LibraryElement el = new LibraryElement();
            el.Name = Name;
            el.PrimitiveName = BELType;
            el.VHDLGenericMap = "generic map ( INIT => X\"ABCDABCDABCDABCD\" )";
            el.Containter = new XDLModule();
            el.VivadoConnectionPrimitive = true;

            // add outpin
            XDLPort outPort = new XDLPort();
            outPort.Direction = FPGATypes.PortDirection.Out;
            outPort.ExternalName = "O";
            outPort.InstanceName = "unknown";
            outPort.SlicePort = "unknown";
            el.Containter.Add(outPort);

            // add inpins
            for (int i = 0; i < LUT_SIZE; i++)
            {
                AddXDLPort(el, $"I{i}", FPGATypes.PortDirection.In);
            }

            return el;
        }

        private void AddXDLPort(LibraryElement el, string portName, FPGATypes.PortDirection dir)
        {
            XDLPort p = new XDLPort();
            p.Direction = dir;
            p.ExternalName = portName;
            p.InstanceName = "unknown";
            p.SlicePort = "unknown";
            p.ConstantValuePort = false;
            el.Containter.Add(p);
        }

        private void CheckParameters()
        {
            bool nameIsCorrect = !string.IsNullOrEmpty(Name);
            bool belTypeIsCorrect = !string.IsNullOrEmpty(BELType);

            if(!nameIsCorrect || !belTypeIsCorrect)
            {
                throw new ArgumentException("Unexpected format in one of the parameters.");
            }
        }

        [Parameter(Comment = "The name of the connection primitive.")]
        public string Name = "VivadoConnectionPrimitive";

        [Parameter(Comment = "The type of BEL that is used.")]
        public string BELType = "LUT6";
    }
}
