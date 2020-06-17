using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using GoAhead.Commands.GUI;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Add a user menu to the GUI with special debug commands", Wrapper = false, Publish = true)]
    class AddDebugMenu : Command
    {
        protected override void DoCommandAction()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type type in asm.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Command)) && !t.Name.Equals(this.GetType().Name)))
            {
                // Namespace may be null -> do not include in Where statement
                if (type.Namespace.EndsWith("Debug"))
                {
                    Command cmd = (Command)Activator.CreateInstance(type);

                    AddUserMenu addMenuCmd = new AddUserMenu();
                    addMenuCmd.Name = type.Name;
                    addMenuCmd.Command = type.Name + ";";
                    addMenuCmd.ToolTip = cmd.GetCommandDescription();
                    CommandExecuter.Instance.Execute(addMenuCmd);
                }
            }            
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
