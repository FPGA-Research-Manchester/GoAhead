using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    class PrintPartitionPinConstraintsForTile : PrintPartitionPinConstraint
    {
        public static int SIGNALS_PER_TILE = 4;

        protected override void DoCommandAction()
        {
            this.CheckParameters();

            for(int i = 0; i < SIGNALS_PER_TILE; i++)
            {
                this.PortIndex = i;

                int signalIndex = this.StartIndex + i;
                this.SignalIndex = signalIndex;

                base.DoCommandAction();
            }
        }

        private void CheckParameters()
        {
            bool startIndexIsCorrect = this.StartIndex >= 0;

            if(!startIndexIsCorrect)
            {
                throw new ArgumentException("Unexpected format in parameters StartIndex.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The start index of the signal")]
        public int StartIndex = 0;
    }
}
