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
    [CommandDescription(Description = "Remove nets from the given macro (XLD-Container) and optionally write the net code to file", Wrapper = false, Publish = true)]
    class RemoveNets : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = GetNetlistContainer();

            Regex netFilter = new Regex(NetNameRegexp, RegexOptions.Compiled);

            // output to be removed nets for later writing them to file
            foreach (Net netToRemove in netlistContainer.Nets.Where(n => netFilter.IsMatch(n.Name)))
            {
                OutputManager.WriteOutput(netToRemove.ToString());
            }

            netlistContainer.Remove(new Predicate<Net>(n => netFilter.IsMatch(n.Name)));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "All nets whose name matches this regular expression will be removed (e.g. ^module removes all nets woth prefix module, ^netname$ remove a particular net name)")]
        public string NetNameRegexp = "^module";
    }
}
