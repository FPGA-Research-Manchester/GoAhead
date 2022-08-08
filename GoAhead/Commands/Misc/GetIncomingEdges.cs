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
    internal class GetIncomingEdges : Command
    {
        [Parameter(Comment = "Tile to start command at")]
        public string TileLocation = "";
        [Parameter(Comment = "Port to start command at")]
        public string PortLocation = "";
        [Parameter(Comment = "Choose whether to print edges or predecessor nodes")]
        public bool GetPredecessors = false;

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
                    OutputManager.WriteOutput($"Incoming edges to location {t.Location}.{p}: ");
                    var locs = Navigator.GetDestinations(t, p, false);
                    foreach(Location loc in locs)
                    {
                        OutputManager.WriteOutput(GetPredecessors ? loc.ToString() : loc + " -> " + t.Location + "." + p);
                    }
                    var arcs = t.SwitchMatrix.GetAllArcs().Where(tuple => tuple.Item2.Equals(p));
                    foreach (var arc in arcs)
                    {
                        OutputManager.WriteOutput(GetPredecessors ? t.Location + "." + arc.Item1 : t.Location + "." + arc.Item1 + " -> " + t.Location + "." + p);
                    }
                }
            }
        }
    }
}
