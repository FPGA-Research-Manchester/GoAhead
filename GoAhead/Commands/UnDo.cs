using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    class UnDo : Command
    {
       protected override void DoCommandAction()
        {
            if (CommandExecuter.Instance.CommandCount == 0)
            {
                throw new ArgumentException("Command stack empty, UnDo is not possible");
            }

            Command lastCommand = CommandExecuter.Instance.PopLastCommand();
            try
            {
                lastCommand.Undo();
            }
            catch (NotImplementedException error)
            {
                throw new ArgumentException("Undo not implemented for " + lastCommand.ToString() + ": " + error.Message);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
