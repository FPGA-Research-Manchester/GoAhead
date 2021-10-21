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

namespace GoAhead.Commands.Misc
{
    [CommandDescription(Description = "Output the switch matrix connections for a selection of tiles to a CSV", Wrapper = true)]
    class WireListToCSV : CommandWithFileOutput
    {
        [Parameter(Comment = "The start tile of the rectangular selection")]
        public string FromTile = "";

        [Parameter(Comment = "The end tile of the rectangular selection")]
        public string ToTile = "";

        [Parameter(Comment = "Whether to specify tiles or use GUI selection. If both FromTile and ToTile are empty but this argument is False or omitted then it will be treated as True.")]
        public bool UseSelection = false;

        protected override void DoCommandAction()
        {
            List<Tile> tileList = new List<Tile>();
            if (FileName == "")
            {
                throw new ArgumentException("Output CSV filename must be provided.");
            }
            if (UseSelection || (FromTile == "" && ToTile == ""))
            {
                if(TileSelectionManager.Instance.GetSelectedTiles().Count() == 0)
                {
                    throw new ArgumentException("No tiles found in the selection.");
                }
                foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
                {
                    tileList.Add(t);
                }
            }
            else
            {
                Tile fromTile = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, FromTile)).OrderBy(t => t.Location).ToList().FirstOrDefault();
                Tile toTile = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, ToTile)).OrderBy(t => t.Location).ToList().FirstOrDefault();

                int startX = Math.Min(fromTile.TileKey.X, toTile.TileKey.X);
                int endX = Math.Max(fromTile.TileKey.X, toTile.TileKey.X);

                int startY = Math.Min(fromTile.TileKey.Y, toTile.TileKey.Y);
                int endY = Math.Max(fromTile.TileKey.Y, toTile.TileKey.Y);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        TileKey tk = new TileKey(x, y);
                        if (FPGA.FPGA.Instance.Contains(tk))
                        {
                            Tile t = FPGA.FPGA.Instance.GetTile(tk);
                            if (!tileList.Contains(t))
                            {
                                tileList.Add(t);
                            }
                        }
                    }
                }
            }
            StringBuilder csv = new StringBuilder();
            int numTilesWritten = 0;
            foreach (Tile t in tileList)
            {
                if(!t.ToString().Contains("NULL"))
                {
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

            using (StreamWriter sw = File.CreateText(FileName))
            {
                sw.Write(csv);
            }

            Console.WriteLine($"Wire information for {numTilesWritten} tiles written to {Directory.GetCurrentDirectory() + "\\" + FileName}.");

        }
        public override void Undo()
        {
            throw new ArgumentException("The method or operation is not implemented.");
        }
    }
}
