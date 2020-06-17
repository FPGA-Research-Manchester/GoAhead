using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Debug
{
    class PrintAverageNumberOfReachablePips : Command
    {
        protected override void DoCommandAction()
        {
            double sum = 0;
            double count = 0;
            foreach(Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(
                tile => Objects.IdentifierManager.Instance.IsMatch(tile.Location, Objects.IdentifierManager.RegexTypes.Interconnect)))
            {
                Dictionary<String, int> connectedWire = new Dictionary<String,int>();
                foreach (Wire wire in tile.WireList.Where(w => w.LocalPipIsDriver))
                {
                    if (!connectedWire.ContainsKey(wire.LocalPip))
                    {
                        connectedWire.Add(wire.LocalPip, 0);
                    }
                    connectedWire[wire.LocalPip]++;
                }
                double avg = (double) connectedWire.Sum(t => t.Value) / (double) connectedWire.Count;
                if (avg > 0)
                {
                    count++;
                    sum += avg;
                }
            }

            double result = sum / count;
            this.OutputManager.WriteOutput(result.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
