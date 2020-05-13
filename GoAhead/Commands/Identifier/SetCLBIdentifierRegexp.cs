using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Set the regexp GoAhead uses to identify 'Configurable Logic Block' tiles for the given Family", Wrapper = false, Publish = true)]
    class SetCLBIdentifierRegexp : SetIdentifierCommand
    {
        protected override void DoCommandAction()
        {
            IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.CLB, FamilyRegexp, IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
