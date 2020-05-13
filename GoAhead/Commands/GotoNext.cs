using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Search;

namespace GoAhead.Commands.MacroGeneration
{
    class GotoNext : Command
    {
        public GotoNext()
        {
        }

        public GotoNext(String targetPrefix, String direction)
		{
            // store direction string for ToString()
            this.DirectionString = direction;
            this.TargetPrefix = targetPrefix;
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
                if(current.Location.StartsWith(this.TargetPrefix))
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
        [ParamterField(Comment = "The name of the file to save the FPGA in")]
        public String TargetPrefix;
		private Tile m_oldFocus;

    }
}
