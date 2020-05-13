using System;
using GoAhead.Commands.Data;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Add a regexp pair that GoAhead will use to identify wires going into " +
        "BELs for the given Family. The expressions must follow a certain format, see the example.", Wrapper = false, Publish = true)]
    class AddBELInwireIdentifierRegexp : AddBELWireIdentifierRegexp
    {
        protected override IdentifierManager.RegexTypes WireRegexType
        { get { return IdentifierManager.RegexTypes.BEL_inwire; } }

        protected override WireHelper.IncludeFlag IncludeFlag
        { get { return WireHelper.IncludeFlag.BELInWires; } }

        protected override string GetPinKeyword()
        {
            return WireHelper.keyword_inpin;
        }

        /*protected override void SetPinKeyword()
        {
            WireHelper.keyword_inpin = PinKeyword;
        }*/
    }
}
