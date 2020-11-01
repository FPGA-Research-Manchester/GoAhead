using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.IO;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Export Architecture Wires", Wrapper = false, Publish = true)]
    class ExportArchitectureSMs : ExportArchitectureCommand
    {
        public bool ExcludeStopoverArcs = false;

        public new static string[] InternalFormatCodes { get; } = { "tile", "port1", "port2" };

        protected override void Export(IEnumerable<Tile> tiles)
        {
            string temp = "";
            int codesNum = InternalFormatCodes.Length;

            int[] columnWidths = { ColumnWidth };

            // Comment lines
            string[] formatDesc = new string[codesNum];
            for (int k = 0; k < Columns.Length; k++) formatDesc[k] = "{" + Columns[k] + "}";
            formatDesc[0] = cSym + formatDesc[0];
            string formatLine = GetColumnsFormattedLine(formatDesc, columnWidths);
            string dashLine = cSym;
            for (int k = 0; k < ColumnWidth * Columns.Length - 2; k++) dashLine += "-";

            Dictionary<string, string> entriesDic;

            foreach (var tile in tiles)
            {
                string line = "";

                if(FormattingMethod)
                {
                    line += dashLine + NL + NL + cSym + "TILE: " + tile.Location + "" + NL + NL;
                    line += cSym + "Connections" + NL + NL + formatLine + NL;
                }
                                        
                foreach (Tuple<Port, Port> arc in tile.SwitchMatrix.GetAllArcs())
                {
                    if (SkipArc(arc.Item1, arc.Item2, tile)) continue;

                    // Map internal parameters to dynamic data
                    entriesDic = new Dictionary<string, string>
                    {
                        { InternalFormatCodes[0], tile.Location },
                        { InternalFormatCodes[1], arc.Item1.Name },
                        { InternalFormatCodes[2], arc.Item2.Name }
                    };

                    // Order the mappings
                    string[] entries = new string[codesNum];
                    for (int j = 0; j < entries.Length; j++)
                        entries[j] = entriesDic[Columns[j]];

                    if (FormattingMethod)
                        temp = GetColumnsFormattedLine(entries, columnWidths);
                    else
                    {
                        temp = "";
                        for (int j = 0; j < entries.Length; j++)
                            temp += entries[j] + Delims[j];
                    }

                    line += temp + "" + NL;
                }

                if (FormattingMethod)
                    line += "" + NL + NL;

                OutputManager.WriteOutput(line);
            }  
        }

        private bool SkipArc(Port pi, Port po, Tile tile)
        {
            if (ExcludeStopoverArcs)
            {
                if (tile.IsPortBlocked(pi, Tile.BlockReason.Stopover)
                 || tile.IsPortBlocked(po, Tile.BlockReason.Stopover))
                {
                    return true;
                }
            }

            return false;
        }
    }

}
