using System;
using System.Collections.Generic;
using System.ComponentModel;
using GoAhead.Objects;

namespace GoAhead.Commands.InterfaceManager
{
    class ClearInterface : Command
    {
       protected override void DoCommandAction()
        {
            // capture current signal set for undo
            m_clearedSignals = Objects.InterfaceManager.Instance.Signals;
            Objects.InterfaceManager.Instance.Reset();
        }

        public override void Undo()
        {
            Objects.InterfaceManager.Instance.Reset();
            foreach (Signal s in m_clearedSignals)
            {
                Objects.InterfaceManager.Instance.Add(s);
            };

            Objects.InterfaceManager.Instance.LoadCommands.Clear();
        }

        /// <summary>
        /// populated before undo
        /// </summary>
        private BindingList<Signal> m_clearedSignals = new BindingList<Signal>();
    }
}
