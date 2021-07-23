using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.IO;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Export Architecture", Wrapper = false, Publish = true)]
    abstract class ExportArchitectureCommand : CommandWithFileOutput
    {
        [Parameter(Comment = "The name of the file to save the architecture to.")]
        public string FileName = "";

        public bool Scope = true; // false = all
        public bool FormattingMethod = false; // false = compact
        public int ColumnWidth;
        public string[] Columns;
        public string[] Delims;

        // Comment symbol
        protected static string cSym = "# ";
        protected static string NL = Environment.NewLine;

        // This should be overriden
        public static string[] InternalFormatCodes { get; } = { "" };

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void DoCommandAction()
        {
            var tiles = Scope ? TileSelectionManager.Instance.GetSelectedTiles() 
                : FPGA.FPGA.Instance.GetAllTiles();

            Export(tiles);
        }

        protected abstract void Export(IEnumerable<Tile> tiles);

        protected void WriteFile(List<string> lines, string path)
        {
            StreamWriter sw = new StreamWriter(path, Append);

            foreach (var l in lines)
                sw.Write(l);

            sw.Close();
        }

        protected string GetColumnsFormattedLine(string[] columns, int[] columnWidths)
        {
            string result = "";
            int cw = columnWidths.Length; // if this is 1 then use it for every column

            if ((columns.Length != cw) && (cw != 1))
                return result;
            
            int counter = columnWidths[0], i = 0;

            for(int j=0; j<columns.Length; j++)
            {
                result += columns[j];

                do result += " ";
                while (result.Length < counter - Delims[j].Length - 1);

                result += Delims[j] + " ";

                i++;
                counter += (cw > 1 && i < cw ? columnWidths[i] : columnWidths[0]);
            }

            return result;
        }
    }

}
