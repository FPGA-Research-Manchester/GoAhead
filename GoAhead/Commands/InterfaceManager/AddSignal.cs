using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.InterfaceManager
{
    class AddSignal : Command
    {
        public AddSignal()
        {
        }

        public AddSignal(Signal s)
        {
            this.SignalName = s.SignalName;
            this.SignalMode = s.SignalMode;
            this.SignalDirection = s.SignalDirection.ToString();
            this.PartialRegion = s.PartialRegion;
            this.Column = s.Column;
        }

        protected override void DoCommandAction()
        {
            FPGATypes.InterfaceDirection dir = (FPGATypes.InterfaceDirection)Enum.Parse(typeof(FPGATypes.InterfaceDirection), this.SignalDirection);
           
            Signal s = new Signal(this.SignalName, this.SignalMode, dir, this.PartialRegion, this.Column);

            Objects.InterfaceManager.Instance.Add(s);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of signal")]
        public String SignalName = "s";

        [Parameter(Comment = "The signal direction (in, out, stream)")]
        public String SignalMode = "stream";

        [Parameter(Comment = "The signal direction (e.g. East)")]
        public String SignalDirection = "East";

        [Parameter(Comment = "The name of partial region this interface signal belongs to")]
        public String PartialRegion = "";

        [Parameter(Comment = "The column index for interleaving")]
        public int Column = 0;
    }
}
