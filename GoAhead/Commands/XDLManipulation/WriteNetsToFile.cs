using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.XDLManipulation
{
    [CommandDescription(Description = "Write net code to file", Wrapper = false, Publish = true)]
    class WriteNetsToFile : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = GetNetlistContainer();

            Regex netFilter = new Regex(NetNameRegexp, RegexOptions.Compiled);

            // output to be removed nets for later writing them to file
            foreach (XDLNet netToRemove in netlistContainer.Nets.Where(n => netFilter.IsMatch(n.Name)))
            {
                OutputManager.WriteOutput(netToRemove.ToString());
            }
        }

        private NetlistContainer GetNetlistContainer()
        {
            string netlistContainerName = string.IsNullOrEmpty(NetlistContainerName) ? NetlistContainerManager.DefaultNetlistContainerName : NetlistContainerName;

            return NetlistContainerManager.Instance.Get(netlistContainerName);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The netlist container this command works on. Leave this parameter empty to work on the default netlist")]
        public string NetlistContainerName = "";

        [Parameter(Comment = "All nets whose name matches this regular expression will be written to file)")]
        public string NetNameRegexp = "^module";
    }
}
