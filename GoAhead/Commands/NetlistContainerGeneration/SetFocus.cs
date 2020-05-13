using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.NetlistContainerGeneration
{
	class SetFocus : Command
	{
        public SetFocus()
        {
        }

        public SetFocus(string locationString)
        {
            Location = locationString;
        }

        protected override void DoCommandAction()
		{
            Tile newFocus = FPGA.FPGA.Instance.GetTile(Location);

			m_oldFocus = FPGA.FPGA.Instance.Current;
			FPGA.FPGA.Instance.Current = newFocus;
		}

		public override void Undo()
		{
			FPGA.FPGA.Instance.Current = m_oldFocus;
		}

        [Parameter(Comment = "The location string  of the tile to set the focus to, e.g INT_X3Y12")]
        public string Location;
        private Tile m_oldFocus;
	}
}


