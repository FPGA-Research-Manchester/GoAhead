using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GUI
{
    [CommandDescription(Description = "Save the current FPGA view as PNG", Wrapper = false, Publish = true)]
    class SaveFPGAViewAsPNG : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            if (ShowGUI.FPGAView == null)
            {
                throw new ArgumentException("Can not save the FPGA view as no GUI is open");
            }

            ShowGUI.FPGAView.SaveFPGAViewAsPNG(this.FileName);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
