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

		public AddPort(string netlistContainerName, string portName, int sliceNumber, string portString)
		{
            NetlistContainerName = netlistContainerName;
            PortName = portName;
			SliceNumber = sliceNumber;
            PortString = portString;
		}

        protected override void DoCommandAction()
		{
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

			Slice where = FPGA.FPGA.Instance.Current.Slices[SliceNumber];
            XDLMacroPort addedPort = new XDLMacroPort(PortName, new Port(PortString), where);
            XDLContainer netlistContainer = (XDLContainer) GetNetlistContainer();

            netlistContainer.Add(addedPort);
		}

		public override void Undo()
		{
            throw new NotImplementedException();
		}

        [Parameter(Comment = "The XDL port name to be exported")]
        public string PortName;

        [Parameter(Comment = "The name of the FPGA pip")]
        public string PortString;

        [Parameter(Comment = "The index of the slice on which the port resides")]
        public int SliceNumber;
	}
}


