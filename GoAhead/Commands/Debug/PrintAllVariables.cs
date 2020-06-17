using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print the current value of all variables (optionally including environement variables)", Wrapper = false, Publish = true)]
    class PrintAllVariables : Command
    {
        protected override void DoCommandAction()
        {
            foreach (String varName in VariableManager.Instance.GetAllVariableNames())
            {
                String value = VariableManager.Instance.GetValue(varName);
                this.OutputManager.WriteOutput(varName + " is " + value);
            }

            if (this.PrintEnvironementVariables)
            {
                IDictionary	environmentVariables = Environment.GetEnvironmentVariables();
                foreach (DictionaryEntry de in environmentVariables)
                {
                    this.OutputManager.WriteOutput(de.Key + " is " + de.Value);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Whether to print out environement variables")]
        public bool PrintEnvironementVariables = false;
    }
}
