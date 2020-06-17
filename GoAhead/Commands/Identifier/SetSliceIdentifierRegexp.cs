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
            Objects.IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.Slice, this.FamilyRegexp, this.IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
