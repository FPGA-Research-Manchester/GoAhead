
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.InterfaceManager
{
    class ModifiySignal : Command
    {
        public ModifiySignal()
        {
        }

        public ModifiySignal(Signal s)
        {
            this.NewSignalName = s.SignalName;
            this.NewSignalMode = s.SignalMode;
            this.NewSignaDirection = s.SignalDirection.ToString();
            this.PartialRegion = s.PartialRegion;
            this.Columns = s.Column;
        }

        protected override void DoCommandAction()
        {
            Signal signalToMidify = Objects.InterfaceManager.Instance.GetSignal(this.CurrentSignalName);
            FPGATypes.InterfaceDirection dir = (FPGATypes.InterfaceDirection)Enum.Parse(typeof(FPGATypes.InterfaceDirection), this.NewSignaDirection);
            
            signalToMidify.SignalName = this.NewSignalName;
            signalToMidify.SignalMode = this.NewSignalMode;
            signalToMidify.SignalDirection = dir;
            signalToMidify.PartialRegion = this.PartialRegion;
            signalToMidify.Column = this.Columns;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The current of signal to modify")]
        public String CurrentSignalName = "s";

        [Parameter(Comment = "The new name of the signal")]
        public String NewSignalName = "s";

        [Parameter(Comment = "The new signal direction (in, out, stream)")]
        public String NewSignalMode = "stram";

        [Parameter(Comment = "The new signal widht in bits")]
        public int NewSignalWidth = 32;

        [Parameter(Comment = "The new signal direction (e.g. East)")]
        public String NewSignaDirection = "East";

        [Parameter(Comment = "The new partial region")]
        public String PartialRegion = "";

        [Parameter(Comment = "The new column value")]
        public int Columns = 0;
    }
}
