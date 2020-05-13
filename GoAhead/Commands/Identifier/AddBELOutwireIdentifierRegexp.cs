using System;
using GoAhead.Commands.Data;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Add a regexp pair that GoAhead will use to identify wires coming out of " +
        "BELs for the given Family. The expressions must follow a certain format, see the example.", Wrapper = false, Publish = true)]
    class AddBELOutwireIdentifierRegexp : AddBELWireIdentifierRegexp
    {
        protected override IdentifierManager.RegexTypes WireRegexType
        { get { return IdentifierManager.RegexTypes.BEL_outwire; } }

        protected override WireHelper.IncludeFlag IncludeFlag
        { get { return WireHelper.IncludeFlag.BELOutWires; } }

        protected override string GetPinKeyword()
        {
            return WireHelper.keyword_outpin;
        }

        /*protected override void SetPinKeyword()
        {
            WireHelper.keyword_outpin = PinKeyword;
        }*/
    }
}
