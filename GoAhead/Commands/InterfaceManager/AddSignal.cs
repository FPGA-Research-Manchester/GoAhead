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
            SignalName = s.SignalName;
            SignalMode = s.SignalMode;
            SignalDirection = s.SignalDirection.ToString();
            PartialRegion = s.PartialRegion;
            Column = s.Column;
        }

        protected override void DoCommandAction()
        {
            FPGATypes.InterfaceDirection dir = (FPGATypes.InterfaceDirection)Enum.Parse(typeof(FPGATypes.InterfaceDirection), SignalDirection);
           
            Signal s = new Signal(SignalName, SignalMode, dir, PartialRegion, Column);

            Objects.InterfaceManager.Instance.Add(s);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of signal")]
        public string SignalName = "s";

        [Parameter(Comment = "The signal direction (in, out, stream)")]
        public string SignalMode = "stream";

        [Parameter(Comment = "The signal direction (e.g. East)")]
        public string SignalDirection = "East";

        [Parameter(Comment = "The name of partial region this interface signal belongs to")]
        public string PartialRegion = "";

        [Parameter(Comment = "The column index for interleaving")]
        public int Column = 0;
    }
}
