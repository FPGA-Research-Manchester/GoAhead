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
    [CommandDescription(Description = "remove all routing informations (i.e., pip statements) but keep inpins and outpins", Wrapper = false, Publish = true)]
    class UnrouteNets : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = GetNetlistContainer();

            Regex netFilter = new Regex(NetNameRegexp, RegexOptions.Compiled);

            foreach (XDLNet netToUnrotue in netlistContainer.Nets.Where(n => netFilter.IsMatch(n.Name) && !n.ReadOnly))
            {
                netToUnrotue.ClearPips(true);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "All nets whose name matches this regular expression will be unrouted (e.g. ^module removes all nets woth prefix module, $netname$ remove a particular net name)")]
        public string NetNameRegexp = "^module";
    }
}
