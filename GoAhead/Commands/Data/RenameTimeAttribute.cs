using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Renames an existing time attribute in the model.", Wrapper = true)]
    public class RenameTimeAttribute : Command
    {
        [Parameter(Comment = "The current name of the attibute.")]
        public string OldName = "";

        [Parameter(Comment = "The required new name for the attribute.")]
        public string NewName = "";

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void DoCommandAction()
        {
            Tile.TimeAttributes att = FPGA.FPGA.Instance.GetTimeAttribute(OldName);

            bool[] presentAtts = Tile.GetPresentTimeAttributes();
            if (presentAtts == null || !presentAtts[(int)att])
            {
                Console.WriteLine("The provided time attibute " + OldName + " was not found in the model.");
                return;
            }

            FPGA.FPGA.Instance.SetTimeModelAttributeName(att, NewName);
            Console.WriteLine("Successfully renamed " + OldName + " to " + NewName);
        }
    }
}
