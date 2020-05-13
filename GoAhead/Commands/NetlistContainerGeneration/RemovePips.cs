using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Remove pips from the given net in the given container and optionally write the removed pips to file", Wrapper = false, Publish = true)]
    class RemovePips : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            NetlistContainer netlistContainer = GetNetlistContainer();
            XDLNet net = (XDLNet ) netlistContainer.GetNet(Netname);
            Regex filter = new Regex(PipRegexp, RegexOptions.Compiled);
            net.Remove(pip => filter.IsMatch(pip.ToString()));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Remove pips in this net")]
        public string Netname = "blocker";

        [Parameter(Comment = "Remove all pips that match this regular expression")]
        public string PipRegexp = "^module";
    }
}
