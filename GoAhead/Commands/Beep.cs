using System;
using System.Media;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Beep", Wrapper = false)]
    public class Beep : Command
    {
        protected override void DoCommandAction()
        {
            SystemSounds.Beep.Play();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}