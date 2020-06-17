using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Commands;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print information on the selected tiles", Wrapper = true, Publish = true)]
    class PrintTileInfoForSelection : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                PrintTileInfo printCmd = new PrintTileInfo();
                printCmd.Location = t.Location;
                CommandExecuter.Instance.Execute(printCmd);

                this.OutputManager.WriteOutput(printCmd.OutputManager.GetOutput());
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
