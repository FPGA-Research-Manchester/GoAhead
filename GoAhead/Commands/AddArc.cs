using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.MacroGeneration
{
	class AddArc : Command
	{
        public AddArc()
        {
        }

		public AddArc(String from, String to)
		{
			this.From = from;
			this.To = to;
		}

		public override void Do()
		{
            Port from = new Port(this.From);
            Port to = new Port(this.To);

            if(!FPGA.FPGA.Instance.Current.SwitchMatrix.Contains(from, to))
            {
                throw new Exception("Tile " + FPGA.FPGA.Instance.Current + " does not contain arc " + from + " -> " + to);
            } 

            if (FPGA.FPGA.Instance.Current.IsPortBlocked(from) &&
                !MacroManager.Instance.CurrentMacro.LastNetAdded.Contains(FPGA.FPGA.Instance.Current, from))
            {
                throw new Exception("Port " + from + " on slice " + FPGA.FPGA.Instance.Current + " is blocked by another net");
            }
            if (FPGA.FPGA.Instance.Current.IsPortBlocked(to) &&
                !MacroManager.Instance.CurrentMacro.LastNetAdded.Contains(FPGA.FPGA.Instance.Current, to))
            {
                throw new Exception("Port " + to + " on slice " + FPGA.FPGA.Instance.Current + " is blocked by another net");
            }

            if (MacroManager.Instance.CurrentMacro == null)
            {
                throw new Exception("No current macro");
            }
            if (MacroManager.Instance.CurrentMacro.LastNetAdded == null)
            {
                throw new Exception("No current net");
            }

            MacroManager.Instance.CurrentMacro.LastNetAdded.Add(FPGA.FPGA.Instance.Current, from, to);
            if (!FPGA.FPGA.Instance.Current.IsPortBlocked(from))
                FPGA.FPGA.Instance.Current.BlockPort(from);
            if (!FPGA.FPGA.Instance.Current.IsPortBlocked(to))
                FPGA.FPGA.Instance.Current.BlockPort(to);

		}

		public override void Undo()
		{
            throw new Exception("The method or operation is not implemented.");
		}

        [ParamterField(Comment = "The name pip where the arc starts")]
        public String From;
        [ParamterField(Comment = "The name pip where the arc end")]
        public String To;

	}
}


