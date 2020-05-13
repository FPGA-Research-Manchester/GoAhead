using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.MacroGeneration
{
    class AddRam : Command
    {
        public AddRam()
        {
        }

        public AddRam(String locationString)
		{
            // do not store tile in ctor already!
            this.m_locationString = locationString;
		}

		public override void Do()
		{
            MacroManager.Instance.CurrentMacro.Add(FPGA.FPGA.Instance.GetTile(this.m_locationString));
		}

		public override void Undo()
		{
            throw new NotImplementedException();
		}

        private String m_locationString;
    }
}
