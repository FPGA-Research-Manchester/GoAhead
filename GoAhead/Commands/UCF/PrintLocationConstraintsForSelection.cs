using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.UCF
{
    [CommandDescription(Description = "Apply GetLocationConstraint to all currently selected tiles", Wrapper = true)]
    class PrintLocationConstraintsForSelection : UCFCommand
    {
        protected override void DoCommandAction()
        {
            int tileCount = 0;

            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                ProgressInfo.Progress = ProgressStart + (int)((double)tileCount++ / (double)TileSelectionManager.Instance.NumberOfSelectedTiles * ProgressShare);

                PrintLocationConstraint cmd = new PrintLocationConstraint();
                cmd.InstanceName = InstanceName;
                cmd.Location = t.Location;
                cmd.SliceNumber = SliceNumber;

                CommandExecuter.Instance.Execute(cmd);

                // copy output
                if (cmd.OutputManager.HasUCFOutput)
                {
                    OutputManager.WriteWrapperOutput(cmd.OutputManager.GetUCFOuput());
                }
            }
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The index of the slice the blocker will use")]
        public int SliceNumber;
        [Parameter(Comment = "The instance name")]
        public string InstanceName;
    }
}
