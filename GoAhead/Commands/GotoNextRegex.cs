using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Search;

namespace GoAhead.Commands.MacroGeneration
{
    class GotoNextRegex : Command
    {
        public GotoNextRegex()
        {
        }

        public GotoNextRegex(String regex, String direction)
        {
            // store direction string for ToString()
            this.DirectionString = direction;
            this.Regex = regex;
        }

        public override void Do()
        {
            this.m_oldFocus = FPGA.FPGA.Instance.Current;
            Tile current = FPGA.FPGA.Instance.Current;
            Types.Direction direction = Types.ParseDirectionFromString(this.DirectionString);

            while (true)
            {
                TileKey targetKey = V5Navigator.GetDestination(current, direction, 1);
                if (!FPGA.FPGA.Instance.Contains(targetKey))
                    throw new ArgumentException("Left FPGA during navigation from " + current + " in direction " + direction);

                current = FPGA.FPGA.Instance.GetTile(targetKey);
                if (!FPGA.FPGA.Instance.Contains(current.TileKey))
                {
                    throw new ArgumentException("Leaving FPGA");
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(current.Location, this.Regex))
                {
                    FPGA.FPGA.Instance.Current = current;
                    break;
                }
            }
        }

        public override void Undo()
        {
            FPGA.FPGA.Instance.Current = this.m_oldFocus;
        }

        [ParamterField(Comment = "The direction to navigate to, either east, west, south, or north")]
        public String DirectionString;
        [ParamterField(Comment = "The regex used to match the target tile")]
        public String Regex;
        private Tile m_oldFocus;

    }
}
