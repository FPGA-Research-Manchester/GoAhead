using System;
using GoAhead.Commands;

namespace GoAhead.Objects
{
    public class CommandStackManager
    {
        private CommandStackManager()
        {
        }

        public static CommandStackManager Instance = new CommandStackManager();

        public string Up()
        {
            return Instance.MoveInCommandStack(-1);
        }

        public string Down()
        {
            return Instance.MoveInCommandStack(1);
        }

        /// <summary>
        /// update index to point to last command
        /// </summary>
        public void Execute()
        {
            m_currentInputIndex = CommandExecuter.Instance.CommandCount - 1;
        }

        private string MoveInCommandStack(int incr)
        {
            if (m_currentInputIndex < CommandExecuter.Instance.CommandCount)
            {
                string storedInput = CommandExecuter.Instance.GetCommandString(m_currentInputIndex);

                m_currentInputIndex += incr;

                // wrap around
                if (m_currentInputIndex < 0)
                {
                    m_currentInputIndex = CommandExecuter.Instance.CommandCount - 1;
                }
                if (m_currentInputIndex >= CommandExecuter.Instance.CommandCount)
                {
                    m_currentInputIndex = 0;
                }

                return storedInput;
            }
            else
            {
                return "";
            }
        }

        private int m_currentInputIndex = 0;
    }
}