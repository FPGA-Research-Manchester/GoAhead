using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    class PrintPartitionPinConstraintsForTile : PrintPartitionPinConstraint
    {
        protected override void DoCommandAction()
        {
            CheckParameters();

            for(int i = 0; i < SignalsForTile + StartIndex ; i++)
            {
                PortIndex = i;

                int signalIndex = StartIndex + i;
                SignalIndex = signalIndex;

                base.DoCommandAction();
            }
        }

        private void CheckParameters()
        {
            bool startIndexIsCorrect = StartIndex >= 0;
            bool signalsForTileIsCorrect = SignalsForTile <= MaxSignalsPerTile;

            if (!startIndexIsCorrect || !signalsForTileIsCorrect)
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

        [Parameter(Comment = "Signals for tile. If this is less than SignalsPerTile, then only these many signals will be printed on this tile")]
        public int SignalsForTile = 8;
    }
}
