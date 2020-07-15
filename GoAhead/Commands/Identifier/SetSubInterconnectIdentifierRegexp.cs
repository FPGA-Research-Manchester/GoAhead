using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Set the regexp GoAhead uses to identify sub-interconnect tiles (e.g INT_INTF_ for Ultrascale) for the given Family", Wrapper = false, Publish = true)]
    class SetSubInterconnectIdentifierRegexp : SetIdentifierCommand
    {
        protected override void DoCommandAction()
        {
            IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.SubInterconnect, FamilyRegexp, IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}