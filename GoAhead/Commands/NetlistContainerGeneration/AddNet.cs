using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class AddNet : NetlistContainerCommand
	{
        public AddNet()
        {
            NetName = GetUniqueNetName();
        }

        public AddNet(string netlistContainerName)
        {
            NetlistContainerName = netlistContainerName;
            NetName = GetUniqueNetName();
        }

        protected override void DoCommandAction()
		{
            NetlistContainer netlistContainer = GetNetlistContainer();
            netlistContainer.Add(Net.CreateNet(NetName));
		}

		public override void Undo()
		{
            NetlistContainer netlistContainer = GetNetlistContainer();
            netlistContainer.Remove(new Predicate<Net>(n => n.Name.Equals(NetName)));
		}

        private static string GetUniqueNetName()
        {
            return "net_" + m_netIndex++;
        }

        private static int m_netIndex = 0;

        [Parameter(Comment = "The name of the net")]
		public string NetName = "";
	}
}


