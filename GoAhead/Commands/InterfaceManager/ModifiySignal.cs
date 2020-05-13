
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
            NewSignalName = s.SignalName;
            NewSignalMode = s.SignalMode;
            NewSignaDirection = s.SignalDirection.ToString();
            PartialRegion = s.PartialRegion;
            Columns = s.Column;
        }

        protected override void DoCommandAction()
        {
            Signal signalToMidify = Objects.InterfaceManager.Instance.GetSignal(CurrentSignalName);
            FPGATypes.InterfaceDirection dir = (FPGATypes.InterfaceDirection)Enum.Parse(typeof(FPGATypes.InterfaceDirection), NewSignaDirection);
            
            signalToMidify.SignalName = NewSignalName;
            signalToMidify.SignalMode = NewSignalMode;
            signalToMidify.SignalDirection = dir;
            signalToMidify.PartialRegion = PartialRegion;
            signalToMidify.Column = Columns;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The current of signal to modify")]
        public string CurrentSignalName = "s";

        [Parameter(Comment = "The new name of the signal")]
        public string NewSignalName = "s";

        [Parameter(Comment = "The new signal direction (in, out, stream)")]
        public string NewSignalMode = "stram";

        [Parameter(Comment = "The new signal widht in bits")]
        public int NewSignalWidth = 32;

        [Parameter(Comment = "The new signal direction (e.g. East)")]
        public string NewSignaDirection = "East";

        [Parameter(Comment = "The new partial region")]
        public string PartialRegion = "";

        [Parameter(Comment = "The new column value")]
        public int Columns = 0;
    }
}
