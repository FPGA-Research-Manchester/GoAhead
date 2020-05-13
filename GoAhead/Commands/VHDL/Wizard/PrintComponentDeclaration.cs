using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.Objects;
using GoAhead.FPGA;
using GoAhead.Code.XDL;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    class PrintComponentDeclaration : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            Objects.LibraryElement libraryElement = Objects.Library.Instance.GetElement(LibraryElement);

            VHDLComponent vhdlComponent = new VHDLComponent(libraryElement);
            OutputManager.WriteVHDLOutput(vhdlComponent.ToString());

            /*
            List<String> buffer = new List<String>();
            buffer.Add("component " + this.LibraryElement + " is Port (");

            foreach (XDLPort port in libraryElement.Containter.Ports)
            {
                buffer.Add("\t" + port.ExternalName + " : " + (port.Direction == FPGATypes.PortDirection.In ? "in" : "out") + " std_logic;");
            }

            buffer[buffer.Count - 1] = Regex.Replace(buffer[buffer.Count - 1], ";", "");
            buffer.Add(");");
            buffer.Add("end component " + this.LibraryElement + ";");

            foreach (String s in buffer)
            {
                this.OutputManager.WriteVHDLOutput(s);
            }*/
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the library element to use")]
        public string LibraryElement = "";
    }
}
