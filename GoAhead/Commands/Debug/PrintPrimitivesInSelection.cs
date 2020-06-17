using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Commands;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print all primitves of all currently selected tiles", Wrapper = false, Publish = true)]
    class PrintPrimitivesInSelection : Command
    {
        protected override void DoCommandAction()
        {
            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                foreach (Slice s in t.Slices)
                {
                    this.OutputManager.WriteOutput(t.Location + "." + s.SliceName);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
