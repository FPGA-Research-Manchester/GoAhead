using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Do not generate prohibit statements for primitves on this tiles.", Wrapper = false, Publish = true)]
    class SetProhibitExcludeFilter : SetIdentifierCommand
    {
        protected override void DoCommandAction()
        {
            IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.ProhibitExcludeFilter, FamilyRegexp, IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
