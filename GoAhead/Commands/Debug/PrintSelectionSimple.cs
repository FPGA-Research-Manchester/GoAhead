using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Commands;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print the identifiers of all currently selected tiles", Wrapper = true, Publish = true)]
    class PrintSelection : Command
    {
        protected override void DoCommandAction()
        {
            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                this.OutputManager.WriteOutput(t.Location);                              
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
