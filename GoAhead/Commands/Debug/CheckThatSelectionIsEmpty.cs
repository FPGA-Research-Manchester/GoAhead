using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Checkt that netlist container does not have any primitives (see AllowedPrimitives for exceptions) in the current selection. Violations will be printed out", Wrapper = false, Publish = true)]
    class CheckThatSelectionIsEmpty : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer design = GetNetlistContainer();

            Regex filter = new Regex(AllowedPrimitives);

            foreach (XDLInstance inst in design.Instances)
            {
                // do we have a filter?
                if (!string.IsNullOrEmpty(AllowedPrimitives))
                {
                    if (filter.IsMatch(inst.Name))
                    {
                        // ignore this one
                        continue;
                    }
                }
                Tile t = FPGA.FPGA.Instance.GetTile(inst.Location);
                if (TileSelectionManager.Instance.IsSelected(t.TileKey))
                {
                    OutputManager.WriteOutput("Primitive " + inst.Name + " is in selection");
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Primitives whose identifiers match this regular epxression are not considered as erroneously placed. Leave empty if no primitives are allowed at all")]
        public string AllowedPrimitives = "";
    }
}
