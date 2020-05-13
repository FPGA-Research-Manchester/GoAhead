using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print the number of currently selected tiles", Publish = true, Wrapper = false)]
    class PrintNumberOfSelectedTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput(FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
