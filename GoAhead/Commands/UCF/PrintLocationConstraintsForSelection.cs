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

            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                this.ProgressInfo.Progress = this.ProgressStart + (int)((double)tileCount++ / (double)FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles * this.ProgressShare);

                PrintLocationConstraint cmd = new PrintLocationConstraint();
                cmd.InstanceName = this.InstanceName;
                cmd.Location = t.Location;
                cmd.SliceNumber = this.SliceNumber;

                CommandExecuter.Instance.Execute(cmd);

                // copy output
                if (cmd.OutputManager.HasUCFOutput)
                {
                    this.OutputManager.WriteWrapperOutput(cmd.OutputManager.GetUCFOuput());
                }
            }
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The index of the slice the blocker will use")]
        public int SliceNumber;
        [Parameter(Comment = "The instance name")]
        public String InstanceName;
    }
}
