using GoAhead.FPGA;
using System;
using System.Collections.Generic;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Export Architecture BELs", Wrapper = false, Publish = true)]
    class ExportArchitectureBELs : ExportArchitectureCommand
    {
        public new static string[] InternalFormatCodes { get; } = { "tile", "slice", "slicetype", "bel", "port", "porttype" };

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
                    line += dashLine + NL + NL + cSym + "TILE: " + tile.Location + "" + NL + NL;

                foreach (var slice in tile.Slices)
                {
                    if (FormattingMethod)
                    {
                        line += cSym + "Slice: " + slice.SliceName + "" + NL;
                        line += cSym + "Type: " + slice.SliceType + NL + NL + formatLine + NL;
                    }                         

                    foreach (var bel in slice.PortMapping.SliceElements)
                    {
                        foreach (var port in slice.PortMapping.GetPorts(bel))
                        {
                            // Map internal parameters to dynamic data
                            entriesDic = new Dictionary<string, string>
                            {
                                { InternalFormatCodes[0], tile.Location },
                                { InternalFormatCodes[1], slice.SliceName },
                                { InternalFormatCodes[2], slice.SliceType },
                                { InternalFormatCodes[3], bel },
                                { InternalFormatCodes[4], port.Name },
                                { InternalFormatCodes[5], slice.PortMapping.IsSliceInPort(port) ? "input" : "output" }
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
                    }
                }

                OutputManager.WriteOutput(line);
            }                
        }
    }

}
