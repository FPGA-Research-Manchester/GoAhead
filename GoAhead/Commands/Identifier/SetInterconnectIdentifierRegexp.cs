using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Set the regexp GoAhead uses to identify interconnect tiles for the given Family", Wrapper = false, Publish = true)]
    class SetInterconnectIdentifierRegexp : SetIdentifierCommand
    {
       protected override void DoCommandAction()
        {
            Objects.IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.Interconnect,  this.FamilyRegexp, this.IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
