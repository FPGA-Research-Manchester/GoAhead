using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.MacroGeneration
{
	class AddNet : Command
	{
		public AddNet()
		{
            this.NetName = AddNet.GetUniqueNetName();
		}

        public AddNet(String netName)
        {
            this.NetName = netName;
        }

  		public override void Do()
		{
			MacroManager.Instance.CurrentMacro.Add(new Net(this.NetName));
		}

		public override void Undo()
		{
            MacroManager.Instance.CurrentMacro.Remove(this.NetName);
		}

        private static String GetUniqueNetName()
        {
            return "net_" + AddNet.m_netIndex++;
        }

        private static int m_netIndex = 0;

        [ParamterField(Comment = "The name of the net")]
		public String NetName = "";
	}
}


