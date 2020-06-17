using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.GridStyle
{
    class BlockLUTInputPortsInSelection : Command
    {
        protected override void DoCommandAction()
        {
            CheckParameters();

            List<Tile> selectionCLB = new List<Tile>();

            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles().Where(tile =>
                     IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB)))
            {
                selectionCLB.Add(t);
            }

            List<Tile> selectionInt = new List<Tile>();

            foreach(Tile clb in selectionCLB)
            {
                selectionInt.Add(FPGATypes.GetInterconnectTile(clb));
            }

            foreach(Tile clb in selectionCLB)
            {
                Tile interconnect = FPGATypes.GetInterconnectTile(clb);

                foreach (Tuple<Port, Port> t in clb.SwitchMatrix.GetAllArcs().Where(a => Regex.IsMatch(a.Item2.Name, InputPortsRegex)))
                {
                    foreach (Wire w in interconnect.WireList.Where(w => w.PipOnOtherTile.Equals(t.Item1.Name)))
                    {
                        interconnect.BlockPort(new FPGA.Port(w.LocalPip), Tile.BlockReason.ExcludedFromBlocking);
                    }
                }
            }
        }

        private void CheckParameters()
        {
            bool inputPortsRegexIsCorrect = !string.IsNullOrEmpty(InputPortsRegex);

            if(!inputPortsRegexIsCorrect)
            {
                throw new ArgumentException("Unvalid parameter InputPortsRegex.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the input ports in regular expression.")]
        public string InputPortsRegex = ".*(L|M)*_(A|B|C|D)4";

    }
}
