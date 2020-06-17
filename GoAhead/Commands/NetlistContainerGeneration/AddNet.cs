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
            this.NetName = AddNet.GetUniqueNetName();
        }

        public AddNet(String netlistContainerName)
        {
            this.NetlistContainerName = netlistContainerName;
            this.NetName = AddNet.GetUniqueNetName();
        }

        protected override void DoCommandAction()
		{
            NetlistContainer netlistContainer = this.GetNetlistContainer();
            netlistContainer.Add(Net.CreateNet(this.NetName));
		}

		public override void Undo()
		{
            NetlistContainer netlistContainer = this.GetNetlistContainer();
            netlistContainer.Remove(new Predicate<Net>(n => n.Name.Equals(this.NetName)));
		}

        private static String GetUniqueNetName()
        {
            return "net_" + AddNet.m_netIndex++;
        }

        private static int m_netIndex = 0;

        [Parameter(Comment = "The name of the net")]
		public String NetName = "";
	}
}


