using GoAhead.FPGA;
using GoAhead.Objects;
using System.Collections.Generic;
using System.IO;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Export Architecture Wires", Wrapper = false, Publish = true)]
    class ExportArchitectureWires : ExportArchitectureCommand
    {
        public bool ExcludeNonSMWires = false;
        public bool ExcludeStopoverWires = false;

        public new static string[] InternalFormatCodes { get; } = { "tile1", "port1", "tile2", "port2" };

        protected override void Export(IEnumerable<Tile> tiles)
        {
            string temp = "";

            int[] columnWidths = { ColumnWidth };

            // Comment lines
            string[] formatDesc = new string[4];
            for (int k = 0; k < Columns.Length; k++) formatDesc[k] = "{" + Columns[k] + "}";
            formatDesc[0] = cSym + formatDesc[0];
            string formatLine = GetColumnsFormattedLine(formatDesc, columnWidths);
            string dashLine = cSym;
            for (int k = 0; k < ColumnWidth * Columns.Length - 2; k++) dashLine += "-";

            Dictionary<string, string> entriesDic;
            //List<string> lines = new List<string>();
            StreamWriter sw = new StreamWriter("result.txt", Append);

            foreach (var tile in tiles)
            {
                string line = "";

                if(FormattingMethod)
                {
                    line += dashLine + NL + NL + cSym + "TILE: " + tile.Location + "" + NL + NL;
                    line += cSym + "Outgoing Wires" + NL + NL + formatLine + NL;
                }
                                        
                foreach (var outwire in tile.WireList)
                {
                    if (SkipWire(outwire, tile)) continue;

                    var destTile = Navigator.GetDestinationByWire(tile, outwire);

                    // Map internal parameters to dynamic data
                    entriesDic = new Dictionary<string, string>
                    {
                        { InternalFormatCodes[0], tile.Location },
                        { InternalFormatCodes[1], outwire.LocalPip },
                        { InternalFormatCodes[2], destTile.Location },
                        { InternalFormatCodes[3], outwire.PipOnOtherTile }
                    };

                    // Order the mappings
                    string[] entries = new string[4];
                    for (int j = 0; j < entries.Length; j++)
                        entries[j] = entriesDic[Columns[j]];

                    if (FormattingMethod)
                        temp = GetColumnsFormattedLine(entries, columnWidths);
                    else
                    {
                        temp = "";
                        for(int j = 0; j < entries.Length; j++)
                            temp += entries[j] + Delims[j];
                    }

                    line += temp + "" + NL;
                }
                if (FormattingMethod)
                    line += "" + NL + NL;

                //lines.Add(line);
                //OutputManager.WriteOutput(line);
                sw.Write(line);
            }
            sw.Close();
            // Write file
            //foreach (var l in lines)
            //    OutputManager.WriteOutput(l);
        }

        private bool SkipWire(Wire wire, Tile tile)
        {
            if (ExcludeNonSMWires)
            {
                if (!tile.SwitchMatrix.Contains(wire.LocalPip))
                {
                    return true;
                }
            }

            if (ExcludeStopoverWires)
            {
                if (tile.IsPortBlocked(wire.LocalPip, Tile.BlockReason.Stopover))
                {
                    return true;
                }
            }

            return false;
        }
    }

}
