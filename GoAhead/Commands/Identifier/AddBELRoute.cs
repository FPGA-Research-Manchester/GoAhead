using System;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Add a regexp pair that GoAhead will use to mimic a route/link from " +
        "IdentifierRegexp_InBEL to IdentifierRegexp_OutBEL, used for adding arcs to the switch matrix.", Wrapper = false, Publish = true)]
    class AddBELRoute : Command
    {
        [Parameter(Comment = "The FPGA Family this command applies to")]
        public string FamilyRegexp = "";
        [Parameter(Comment = "The regexp of the BEL that wires are going into")]
        public string IdentifierRegexp_InBEL = "";
        [Parameter(Comment = "The regexp of the BEL that wires are going out of")]
        public string IdentifierRegexp_OutBEL = "";

        protected override void DoCommandAction()
        {
            Tuple<IdentifierManager.RegexTypes, IdentifierManager.RegexTypes> typePair =
                new Tuple<IdentifierManager.RegexTypes, IdentifierManager.RegexTypes>
                (IdentifierManager.RegexTypes.BEL, IdentifierManager.RegexTypes.BEL);
            Tuple<string, string> regexPair = new Tuple<string, string>(IdentifierRegexp_InBEL, IdentifierRegexp_OutBEL);

            IdentifierManager.Instance.AddRegexMultipair(typePair, FamilyRegexp, regexPair);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
