using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Set the regexp GoAhead uses to identify Slices for the given Family", Wrapper = false, Publish = true)]
    class SetSliceIdentifierRegexp : SetIdentifierCommand
    {
       protected override void DoCommandAction()
        {
            IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.Slice, FamilyRegexp, IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
