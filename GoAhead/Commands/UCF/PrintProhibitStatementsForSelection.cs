using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.UCF
{
    [CommandDescription(Description = "Apply PrintProhibitStatement to all currently selected tiles", Wrapper = true)]
    class PrintProhibitStatementsForSelection : UCFCommand
    {
        protected override void DoCommandAction()
        {
            // progress 
            int tileCount = 0;            
            
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles())
            {
                ProgressInfo.Progress = ProgressStart + (int)((double)tileCount++ / (double)TileSelectionManager.Instance.NumberOfSelectedTiles * ProgressShare);

                foreach (string prohibitStatment in PrintProhibitStatement.GetProhibitStatments(tile.Location, ExcludeUsedSlices))
                {
                    OutputManager.WriteUCFOutput(prohibitStatment);
                }

                /*
                PrintProhibitStatement cmd = new PrintProhibitStatement();
                cmd.Location = tile.Location;
                cmd.Mute = this.Mute;
                cmd.ExcludeUsedSlices = this.ExcludeUsedSlices;               
                CommandExecuter.Instance.Execute(cmd);

                // copy output
                if (cmd.OutputManager.HasUCFOutput)
                {
                    this.OutputManager.WriteWrapperOutput(cmd.OutputManager.GetUCFOuput());
                }*/
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Wheter to exclude slices used by user instantiated macros")]
        public bool ExcludeUsedSlices = true;
    }
}
