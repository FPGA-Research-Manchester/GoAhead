using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Mark nets as write protected and optionally write the affected nets to file", Wrapper = false, Publish = true)]
    class SetWriteProtection : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = this.GetNetlistContainer();

            Regex netFilter = new Regex(this.NetNameRegexp, RegexOptions.Compiled);

            // output to be removed nets for later writing them to file
            foreach (XDLNet net in netlistContainer.Nets.Where(n => netFilter.IsMatch(n.Name)))
            {
                net.ReadOnly = true;
                this.OutputManager.WriteOutput("Setting write protection for net " + net.Name);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "All nets whose name matches this regular expression will write protected")]
        public String NetNameRegexp = "^module";
    }
}
