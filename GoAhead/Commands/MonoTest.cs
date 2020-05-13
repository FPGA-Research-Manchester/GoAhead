using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;


namespace GoAhead.Commands
{
    class MonoTest : Command
    {
        protected override void DoCommandAction()
        {
            GoAhead.GUI.MonoTestForm monoTest = new GoAhead.GUI.MonoTestForm();
            monoTest.Show();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
