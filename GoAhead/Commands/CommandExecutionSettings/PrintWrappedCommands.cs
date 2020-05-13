using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Settings;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description="For debugging purposes print all commands to command that are wrapped in an other command", Publish=true, Wrapper=false)]
    class PrintWrappedCommands : Command
    {
        protected override void DoCommandAction()
        {
            m_previousValue = CommandExecuter.Instance.PrintWrappedCommands;
            CommandExecuter.Instance.PrintWrappedCommands = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.PrintWrappedCommands = m_previousValue;
        }

        /// <summary>
        /// for undo only
        /// </summary>
        private bool m_previousValue = false;
    }
}
