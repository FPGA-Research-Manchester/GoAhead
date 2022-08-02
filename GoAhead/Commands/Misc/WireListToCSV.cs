using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.Selection;

namespace GoAhead.Commands.Misc
{
    [CommandDescription(Description = "Output the switch matrix connections for a selection of tiles to a CSV", Wrapper = true)]
    class WireListToCSV : CommandWithFileOutput
    {
        [Parameter(Comment = "The start tile of the rectangular selection")]
        public string FromTile = "";

        [Parameter(Comment = "The end tile of the rectangular selection")]
        public string ToTile = "";

        protected override void DoCommandAction()
        {
            List<Tile> tileList = new List<Tile>();
            if (FileName == "")
            {
                throw new ArgumentException("Output CSV filename must be provided.");
            }
            if (FromTile == "" || ToTile == "")
            {
                // Uses current user selection if a required argument is not supplied.
                tileList = TileSelectionManager.Instance.GetSelectedTiles().ToList();
            }
            else
            {
                // Find tiles represented by input strings.
                Tile fromTile = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, FromTile)).OrderBy(t => t.Location).ToList().FirstOrDefault();
                Tile toTile = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, ToTile)).OrderBy(t => t.Location).ToList().FirstOrDefault();

                // Calculate the vertices of the tile selection.
                int startX = Math.Min(fromTile.TileKey.X, toTile.TileKey.X);
                int endX = Math.Max(fromTile.TileKey.X, toTile.TileKey.X);

                int startY = Math.Min(fromTile.TileKey.Y, toTile.TileKey.Y);
                int endY = Math.Max(fromTile.TileKey.Y, toTile.TileKey.Y);

                AddToSelectionXY selectionCmd = new AddToSelectionXY(startX, startY, endX, endY);
                selectionCmd.Do();
                ExpandSelection expandCmd = new ExpandSelection();
                expandCmd.Do();

                tileList = TileSelectionManager.Instance.GetSelectedTiles().ToList();
            }
            // If selection is empty...
            if (tileList.Count() == 0)
            {
                throw new ArgumentException("No tiles found in the selection.");
            }
            StringBuilder csv = new StringBuilder();
            int numTilesWritten = 0;
            foreach (Tile t in tileList)
            {
                // Avoids outputting useless tiles.
                if(!t.ToString().Contains("NULL"))
                {
                    // Outputs tile name, then information on every wire in the tile's WireList object.
                    csv.AppendLine(t.ToString());
                    csv.AppendLine("Local Pip,Local Pip Is Driver,Pip On Other Tile,XIncr,YIncr,Target Pip");
                    foreach (Wire w in t.WireList)
                    {
                        Tile target = Navigator.GetDestinationByWire(t, w);
                        csv.AppendLine($"{w.LocalPip},{w.LocalPipIsDriver},{w.PipOnOtherTile},{w.XIncr},{w.YIncr},{target}");
                    }
                    csv.Append("\n");
                    numTilesWritten++;
                }
            }

            // Output contents of StringBuilder to specified file location, overwriting any file that is present in that location with the same name.
            using (StreamWriter sw = File.CreateText(FileName))
            {
                sw.Write(csv);
            }

            // Write helpful success message.
            Console.WriteLine($"Wire information for {numTilesWritten} tiles written to {Directory.GetCurrentDirectory() + "\\" + FileName}.");

        }
        public override void Undo()
        {
            throw new ArgumentException("The method or operation is not implemented.");
        }
    }
}
