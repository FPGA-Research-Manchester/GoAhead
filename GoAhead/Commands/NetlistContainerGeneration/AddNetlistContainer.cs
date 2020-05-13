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
            NetlistContainerName = GetUniquqMacroName();
        }

        public AddNetlistContainer(string netlistContainerName)
        {
            NetlistContainerName = netlistContainerName;
        }

        protected override void DoCommandAction()
		{
            switch (FPGA.FPGA.Instance.BackendType)
            {                
                case FPGATypes.BackendType.ISE:
                    NetlistContainerManager.Instance.Add(new XDLContainer(NetlistContainerName));
                    break;
                case FPGATypes.BackendType.Vivado:
                    NetlistContainerManager.Instance.Add(new TCLContainer(NetlistContainerName));
                    break;
                default:
                    throw new ArgumentException("Unsuuported backend type " + FPGA.FPGA.Instance.BackendType);
            }
		}

		public override void Undo()
		{
            throw new NotImplementedException("Not implemented yet");
		}

        private static string GetUniquqMacroName()
        {
            return "netlist_container_" + m_macroIndex++;
        }

        private static int m_macroIndex = 0;
	}
}
