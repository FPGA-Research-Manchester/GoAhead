using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Add the arc specified by From and To to the last added net of the currently selected macro at the currently selected tile (Walker)", Wrapper = false, Publish = false)]
    class AddArc : NetlistContainerCommand
	{        
        public AddArc()
        {
        }

		public AddArc(String netlistContainerName, String from, String to)
		{
            this.NetlistContainerName = netlistContainerName;
			this.From = from;
			this.To = to;
		}

        protected override void DoCommandAction()
		{
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            XDLContainer netlistContainer = (XDLContainer) this.GetNetlistContainer();
            Port from = new Port(this.From);
            Port to = new Port(this.To);

            if(!FPGA.FPGA.Instance.Current.SwitchMatrix.Contains(from, to))
            {
                throw new ArgumentException("Tile " + FPGA.FPGA.Instance.Current + " does not contain arc " + from + " -> " + to);
            } 

            if (FPGA.FPGA.Instance.Current.IsPortBlocked(from) && !((XDLNet) netlistContainer.LastNetAdded).Contains(FPGA.FPGA.Instance.Current, from))
            {
                throw new ArgumentException("Port " + from + " on slice " + FPGA.FPGA.Instance.Current + " is blocked by another net");
            }
            if (FPGA.FPGA.Instance.Current.IsPortBlocked(to) && !((XDLNet) netlistContainer.LastNetAdded).Contains(FPGA.FPGA.Instance.Current, to))
            {
                throw new ArgumentException("Port " + to + " on slice " + FPGA.FPGA.Instance.Current + " is blocked by another net");
            }

            if (netlistContainer == null)
            {
                throw new ArgumentException("No current macro");
            }
            if (netlistContainer.LastNetAdded == null)
            {
                throw new ArgumentException("No current net");
            }

            ((XDLNet) netlistContainer.LastNetAdded).Add(FPGA.FPGA.Instance.Current, from, to);
            if (!FPGA.FPGA.Instance.Current.IsPortBlocked(from))
            {
                FPGA.FPGA.Instance.Current.BlockPort(from, Tile.BlockReason.Blocked);
            }
            if (!FPGA.FPGA.Instance.Current.IsPortBlocked(to))
            {
                FPGA.FPGA.Instance.Current.BlockPort(to, Tile.BlockReason.Blocked);
            }
		}

		public override void Undo()
		{
            throw new ArgumentException("The method or operation is not implemented.");
		}

        [Parameter(Comment = "The name pip where the arc starts")]
        public String From = "";

        [Parameter(Comment = "The name pip where the arc end")]
        public String To = "";

	}
}


