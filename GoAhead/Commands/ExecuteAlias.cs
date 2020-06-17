using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description="An public command that executes aliased commands", Publish=true, Wrapper=true)]
    class ExecuteAlias : Command
    {
        public ExecuteAlias()
        {
        }

        public ExecuteAlias(List<String> overriddenDefaults)
        {
            this.m_overriddenDefaults = overriddenDefaults;
        }

        protected override void DoCommandAction()
        {
            CommandStringParser parser = new CommandStringParser(this.Commands);
            foreach(String cmdString in parser.Parse())
            {
                Command cmd = null;
                String errorDescr = "";
                parser.ParseCommand(cmdString, true, out cmd, out errorDescr);
                // only the alias it self shall appear in the command trace
                cmd.UpdateCommandTrace = false;                
                string errorDescription = "";
                parser.SetParamters(cmd, true, this.m_overriddenDefaults, ref errorDescription);
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.AliasName + (this.AliasName.EndsWith(";") ? "" : ";");
        }


        [Parameter(Comment = "The name of alias")]
        public String AliasName = "select_an_area";

        [Parameter(Comment = "A semicolon seperated list of GoAhead commands that will be executed if the alias name is used as a command.")]
        public String Commands = "ClearSelection;AddToSelectionXY UpperLeftX=138 UpperLeftY=85 LowerRightX=145 LowerRightY=125;ExpandSelection;";

        private readonly List<String> m_overriddenDefaults = new List<string>();
    }
}
