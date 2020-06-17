using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class AddPort : NetlistContainerCommand
	{
        public AddPort()
        {
        }

		public AddPort(String netlistContainerName, String portName, int sliceNumber, String portString)
		{
            this.NetlistContainerName = netlistContainerName;
            this.PortName = portName;
			this.SliceNumber = sliceNumber;
            this.PortString = portString;
		}

        protected override void DoCommandAction()
		{
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

			Slice where = FPGA.FPGA.Instance.Current.Slices[this.SliceNumber];
            XDLMacroPort addedPort = new XDLMacroPort(this.PortName, new Port(this.PortString), where);
            XDLContainer netlistContainer = (XDLContainer) this.GetNetlistContainer();

            netlistContainer.Add(addedPort);
		}

		public override void Undo()
		{
            throw new NotImplementedException();
		}

        [Parameter(Comment = "The XDL port name to be exported")]
        public String PortName;

        [Parameter(Comment = "The name of the FPGA pip")]
        public String PortString;

        [Parameter(Comment = "The index of the slice on which the port resides")]
        public int SliceNumber;
	}
}


