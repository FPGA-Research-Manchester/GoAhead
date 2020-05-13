using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Commands.MacroGeneration
{
    class SelectMacro : Command
    {
        public SelectMacro()
        {
        }

        public SelectMacro(String macroName)
        {
            this.MacroName = macroName;
        }

        public override void Do()
        {
            // store last macro for undo
            this.m_lastMacro = Objects.MacroManager.Instance.CurrentMacro;

            Objects.MacroManager.Instance.CurrentMacro = Objects.MacroManager.Instance.GetMacro(this.MacroName);
        }

        public override void Undo()
        {
            Objects.MacroManager.Instance.CurrentMacro = this.m_lastMacro;
        }

        [ParamterField(Comment = "The name of the macro to select")]
        public String MacroName;

        private Objects.Macro m_lastMacro;
    }
}
