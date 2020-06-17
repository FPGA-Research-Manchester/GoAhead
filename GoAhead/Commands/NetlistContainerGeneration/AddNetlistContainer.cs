using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;
using GoAhead.Code.TCL;
using GoAhead.Commands;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class AddNetlistContainer : NetlistContainerCommand
	{
        public AddNetlistContainer()
        {
            this.NetlistContainerName = AddNetlistContainer.GetUniquqMacroName();
        }

        public AddNetlistContainer(String netlistContainerName)
        {
            this.NetlistContainerName = netlistContainerName;
        }

        protected override void DoCommandAction()
		{
            switch (FPGA.FPGA.Instance.BackendType)
            {                
                case FPGATypes.BackendType.ISE:
                    NetlistContainerManager.Instance.Add(new XDLContainer(this.NetlistContainerName));
                    break;
                case FPGATypes.BackendType.Vivado:
                    NetlistContainerManager.Instance.Add(new TCLContainer(this.NetlistContainerName));
                    break;
                default:
                    throw new ArgumentException("Unsuuported backend type " + FPGA.FPGA.Instance.BackendType);
            }
		}

		public override void Undo()
		{
            throw new NotImplementedException("Not implemented yet");
		}

        private static String GetUniquqMacroName()
        {
            return "netlist_container_" + AddNetlistContainer.m_macroIndex++;
        }

        private static int m_macroIndex = 0;
	}
}
