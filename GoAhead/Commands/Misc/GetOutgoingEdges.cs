using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.Commands.Misc
{
    internal class GetOutgoingEdges : Command
    {
        [Parameter(Comment = "Tile to start command at")]
        public string TileLocation = "";
        [Parameter(Comment = "Port to start command at")]
        public string PortLocation = "";
        [Parameter(Comment = "Choose whether to print edges or neighbour nodes")]
        public bool GetNeighbours = false;

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void DoCommandAction()
        {
            foreach(Tile t in FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, TileLocation)))
            {
                foreach (Port p in t.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.ToString(), PortLocation)))
                {
                    OutputManager.WriteOutput($"Outgoing edges from location {t.Location}.{p}: ");
                    var locs = Navigator.GetDestinations(t, p, true);
                    foreach(Location loc in locs)
                    {
                        OutputManager.WriteOutput(GetNeighbours ? loc.ToString() : t.Location + "." + p + " -> " + loc.ToString());
                    }
                    
                    var arcs = t.SwitchMatrix.GetAllArcs().Where(tuple => tuple.Item1.Equals(p));
                    foreach (var arc in arcs)
                    {
                        OutputManager.WriteOutput(GetNeighbours ? t.Location + "." + arc.Item2 : t.Location + "." + p + " -> " + t.Location + "." + arc.Item2);
                    }
                }
            }
        }
    }
}
