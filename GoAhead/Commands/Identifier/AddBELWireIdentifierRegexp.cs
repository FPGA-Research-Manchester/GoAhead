using GoAhead.Commands.Data;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "", Wrapper = false, Publish = true)]
    abstract class AddBELWireIdentifierRegexp : Command
    {
        [Parameter(Comment = "The FPGA Family this command applies to")]
        public string FamilyRegexp = "";
        [Parameter(Comment = "The regexp used to identify the BEL")]
        public string IdentifierRegexp_BEL = "";
        [Parameter(Comment = "The regexp used to identify the wire")]
        public string IdentifierRegexp_Wire = "";
        //[Parameter(Comment = "Optional custom keyword for the pin group")]
        //public string PinKeyword = "";

        protected abstract IdentifierManager.RegexTypes WireRegexType { get; }
        protected abstract WireHelper.IncludeFlag IncludeFlag { get; }

        //protected abstract void SetPinKeyword();
        protected abstract string GetPinKeyword();

        protected override void DoCommandAction()
        {
            //if (!PinKeyword.Equals("")) SetPinKeyword();
            ValidateParameters();

            Tuple<IdentifierManager.RegexTypes, IdentifierManager.RegexTypes> typePair =
                new Tuple<IdentifierManager.RegexTypes, IdentifierManager.RegexTypes>
                (IdentifierManager.RegexTypes.BEL, WireRegexType);
            Tuple<string, string> regexPair = new Tuple<string, string>(IdentifierRegexp_BEL, IdentifierRegexp_Wire);

            IdentifierManager.Instance.AddRegexMultipair(typePair, FamilyRegexp, regexPair);

            WireHelper.AddIncludeFlag(IncludeFlag, FamilyRegexp, true);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private void ValidateParameters()
        {
            // Create regex objects
            Regex belRegex = new Regex(IdentifierRegexp_BEL, RegexOptions.Compiled);
            Regex wireRegex = new Regex(IdentifierRegexp_Wire, RegexOptions.Compiled);

            // Get all group names for each provided parameter
            List<string> belGroupNames = new List<string>(belRegex.GetGroupNames());
            List<string> wireGroupNames = new List<string>(wireRegex.GetGroupNames());

            // Remove group 0
            belGroupNames.Remove(belRegex.GroupNameFromNumber(0));
            wireGroupNames.Remove(wireRegex.GroupNameFromNumber(0));

            string log = "" + Environment.NewLine;
            bool fail = false;

            // Check for pin group
            if (wireGroupNames.Contains(GetPinKeyword())) 
            {
                wireGroupNames.Remove(GetPinKeyword());
                log += "Pin group found: '" + GetPinKeyword() + "'";
            }
            else
            {
                log += "Pin group not found";
            }
            log += Environment.NewLine;

            // Check can we successfully do group mapping
            foreach(string belGroup in belGroupNames)
            {
                if(!wireGroupNames.Contains(belGroup))
                {
                    if (!fail) log += "Group mapping failed! Reasons:" + Environment.NewLine;
                    fail = true;
                    log += "The group '" + belGroup + "' is present in IdentifierRegexp_BEL but not in IdentifierRegexp_Wire" + Environment.NewLine;
                }
            }
            foreach (string wireGroup in wireGroupNames)
            {
                if (!belGroupNames.Contains(wireGroup))
                {
                    if (!fail) log += "Group mapping failed! Reasons:" + Environment.NewLine;
                    fail = true;
                    log += "The group '" + wireGroup + "' is present in IdentifierRegexp_Wire but not in IdentifierRegexp_BEL" + Environment.NewLine;
                }
            }

            if (fail) throw new ArgumentException(log);
        }
    }
}
