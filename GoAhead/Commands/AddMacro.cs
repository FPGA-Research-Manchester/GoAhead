using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.MacroGeneration
{
	class AddMacro : Command
	{
        public AddMacro()
        {
            this.MacroName = AddMacro.GetUniquqMacroName();
        }

        public AddMacro(String macroName)
        {
            this.MacroName = macroName;
        }

		public override void Do()
		{
			MacroManager.Instance.Add(new Objects.Macro(this.MacroName));
		}

		public override void Undo()
		{
			MacroManager.Instance.RemoveMacro(this.MacroName);
		}

        private static String GetUniquqMacroName()
        {
            return "macro_" + AddMacro.m_macroIndex++;
        }

        private static int m_macroIndex = 0;

        [ParamterField(Comment = "The name of the macro")]
        public String MacroName;
	}
}


