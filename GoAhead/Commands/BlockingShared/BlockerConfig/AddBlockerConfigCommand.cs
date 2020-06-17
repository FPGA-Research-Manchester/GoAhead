using System;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    [CommandDescription(Description = "Base class")]
    public abstract class AddBlockerConfigCommand : Command
    {
        [Parameter(Comment = "The FPGA Family this command applies to")]
        public String FamilyRegexp = "";
    }
}