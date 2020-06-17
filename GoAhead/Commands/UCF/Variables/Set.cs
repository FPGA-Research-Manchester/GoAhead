using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;

namespace GoAhead.Commands.Variables
{
    [CommandDescription(Description = "Set the value of a variable", Wrapper = false, Publish = true)]
    public class Set : Command
    {
        public Set()
        {
        }

        public Set(bool valueIsString)
        {
            this.m_valueIsString = valueIsString;
        }

        protected override void DoCommandAction()
        {
            if (Set.m_digitFilter.IsMatch(this.Value) || this.m_valueIsString)
            {
                // e.g Set Variable=CI_location Value="right";
                VariableManager.Instance.Set(this.Variable, this.Value);
            }
            else
            {
                int evaluationResult = 0;
                bool valid = Set.m_ep.Evaluate(this.Value, out evaluationResult);
                VariableManager.Instance.Set(this.Variable, evaluationResult.ToString());
            }
        }

        public override void Undo()
        {
        }

        private static Regex m_digitFilter = new Regex(@"^\D+(\d|\D)*$", RegexOptions.Compiled);
        private static ExpressionParser m_ep = new ExpressionParser();

        private readonly bool m_valueIsString = false;

        [Parameter(Comment = "The name of the variable to set (e.g. a)")]
        public String Variable = "";

        [Parameter(Comment = "The new value of the variable, a constant value or an arithmetic expression (e.g. %a%+5")]
        public String Value = "";
    }
}
