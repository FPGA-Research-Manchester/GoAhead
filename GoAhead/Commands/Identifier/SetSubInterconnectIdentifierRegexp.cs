using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Set the regexp GoAhead uses to identify interconnect tiles named INTF for the given Family", Wrapper = false, Publish = true)]
    class SetSubInterconnectIdentifierRegexp : SetIdentifierCommand
    {
        protected override void DoCommandAction()
        {
            IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.SubInterconnect_INTF, FamilyRegexp, IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}