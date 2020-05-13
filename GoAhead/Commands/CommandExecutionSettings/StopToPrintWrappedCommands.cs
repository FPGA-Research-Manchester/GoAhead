using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Settings;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Do no longer print wrapped commands to command trace", Publish = true, Wrapper = false)]
    class StopToPrintWrappedCommands : Command
    {
        protected override void DoCommandAction()
        {
            m_previousValue = CommandExecuter.Instance.PrintWrappedCommands;
            CommandExecuter.Instance.PrintWrappedCommands = false;
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
