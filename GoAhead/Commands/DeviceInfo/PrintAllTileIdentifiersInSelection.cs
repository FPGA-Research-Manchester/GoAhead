using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintAllTileIdentifiersInSelection : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            int maxX = FPGA.FPGA.Instance.MaxX;
            int maxY = FPGA.FPGA.Instance.MaxY;

            StringBuilder buffer = new StringBuilder();

            // loop rowise y in x
            for (int y = 0; y < maxY; y++)
            {
                bool rowExtended = false;
                for (int x = 0; x < maxX; x++)
                {
                    if (FPGA.TileSelectionManager.Instance.IsSelected(x, y))
                    {
                        Tile tile = FPGA.FPGA.Instance.GetTile(x, y);
                        if (rowExtended == true)
                        {
                            // csv
                            buffer.Append(", " + tile.Location);
                        }
                        else
                        {
                            buffer.Append(tile.Location);
                        }
                        rowExtended = true;
                    }
                }
                if (rowExtended)
                {
                    buffer.AppendLine("");
                }
            }

            this.OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
