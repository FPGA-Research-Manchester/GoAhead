using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.CodeGeneration;

namespace GoAhead.Commands.MacroGeneration
{
	class AddPort : Command
	{
        public AddPort()
        {
        }

		public AddPort(String portName, int sliceNumber, String portString)
		{
            this.PortName = portName;
			this.SliceNumber = sliceNumber;
            this.PortString = portString;
		}

		public override void Do()
		{
			Slice where = FPGA.FPGA.Instance.Current.Slices[this.SliceNumber];
			m_addedPort = new XDLPort(this.PortName, new Port(this.PortString), where);
			MacroManager.Instance.CurrentMacro.Add(this.m_addedPort);
		}

		public override void Undo()
		{
            throw new NotImplementedException();
		}

        [ParamterField(Comment = "The XDL port name to be exported")]
        public String PortName;
        [ParamterField(Comment = "The name of the FPGA pip")]
        public String PortString;
        [ParamterField(Comment = "The index of the slice on which the port resides")]
        public int SliceNumber;


		private XDLPort m_addedPort;
	}
}


