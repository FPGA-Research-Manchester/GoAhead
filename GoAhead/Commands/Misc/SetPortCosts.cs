using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.Misc
{
    [CommandDescription(Description = "Sets the cost for a port or ports in one or more tiles.")]
    class SetPortCosts : Command
    {
        [Parameter(Comment = "The tile to set the port cost for")]
        public string TileLocation = "";

        [Parameter(Comment = "The port to set the cost for")]
        public string PortLocation = "";

        [Parameter(Comment = "The cost value")]
        public int NewCost = 0;

        protected override void DoCommandAction()
        {
            List<Tile> tileList = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, TileLocation)).OrderBy(t => t.Location).ToList();
            foreach(Tile t in tileList)
            {
                List<Port> portList = t.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, PortLocation)).OrderBy(p => p.Name).ToList();
                foreach(Port p in portList)
                {
                    p.Cost = NewCost;
                }
            }
        }
        public override void Undo()
        {
            throw new ArgumentException("The method or operation is not implemented.");
        }
    }
}
