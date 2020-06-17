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
            NetlistContainer netlistContainer = this.GetNetlistContainer();

            Regex netFilter = new Regex(this.NetNameRegexp, RegexOptions.Compiled);

            // output to be removed nets for later writing them to file
            foreach (XDLNet netToRemove in netlistContainer.Nets.Where(n => netFilter.IsMatch(n.Name)))
            {
                this.OutputManager.WriteOutput(netToRemove.ToString());
            }
        }

        private NetlistContainer GetNetlistContainer()
        {
            String netlistContainerName = String.IsNullOrEmpty(this.NetlistContainerName) ? NetlistContainerManager.DefaultNetlistContainerName : this.NetlistContainerName;

            return Objects.NetlistContainerManager.Instance.Get(netlistContainerName);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The netlist container this command works on. Leave this parameter empty to work on the default netlist")]
        public String NetlistContainerName = "";

        [Parameter(Comment = "All nets whose name matches this regular expression will be written to file)")]
        public String NetNameRegexp = "^module";
    }
}
