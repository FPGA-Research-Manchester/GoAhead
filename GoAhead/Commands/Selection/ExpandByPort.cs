using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Selection
{
    class ExpandByPort : Command
    {
        protected override void DoCommandAction()
        {
            if (!TileSelectionManager.Instance.UserSelectionTypes.Any(s => s.Equals(UserSelectionType)))
            {
                throw new ArgumentException("UserSelectionType " + UserSelectionType + " does not exist");
            }

            List<Tile> expansion = new List<Tile>();
            
            foreach (Tile clb in TileSelectionManager.Instance.GetSelectedTiles().Where(t =>
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
            {
                Tile interconnect = FPGATypes.GetInterconnectTile(clb);
                Port port = interconnect.SwitchMatrix.GetDrivenPorts().FirstOrDefault(p => p.Name.Equals(Begin));
                bool startIsUserSelected = TileSelectionManager.Instance.IsUserSelected(interconnect.TileKey, UserSelectionType);
                bool continuePath = true;
                
                do
                {
                    expansion.Add(interconnect);
                    Location loc = Navigator.GetDestinations(interconnect, port).FirstOrDefault(l => l.Pip.Name.Equals(this.End));
                    if (loc == null)
                    {
                        throw new ArgumentException("Can not route via " + this.Begin + " from " + interconnect + " to " + this.End);
                    }
                    interconnect = loc.Tile;
                    continuePath =
                            (startIsUserSelected && FPGA.TileSelectionManager.Instance.IsUserSelected(interconnect.TileKey, this.UserSelectionType)) ||
                            (!startIsUserSelected && !FPGA.TileSelectionManager.Instance.IsUserSelected(interconnect.TileKey, this.UserSelectionType));
                } while (continuePath);
            }

            expansion.ForEach(t => FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the user selection that limits the extent of the selection expanding")]
        public String UserSelectionType = "PartialArea";


        [Parameter(Comment = "The name of the port to expand the selection until reaching or leaving the user selecton")]
        public String Begin = "EE2BEG0";

        [Parameter(Comment = "The name of the port to expand the selection until reaching or leaving the user selecton. We need to provide this parameter as the BEG pip might have several taps (e.g., mid)")]
        public String End = "EE2END0";
    }
}
