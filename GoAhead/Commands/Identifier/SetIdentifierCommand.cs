using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Base class", Wrapper=false, Publish=false)]
    abstract class SetIdentifierCommand : Command
    {
        [Parameter(Comment = "e.g. Spartan6")]
        public string FamilyRegexp = "Spartan6";

        [Parameter(Comment = "e.g. (^MACCSITE2)|(^BRAMSITE2)")]
        public string IdentifierRegexp = "(^MACCSITE2)|(^BRAMSITE2)";
    }
}
