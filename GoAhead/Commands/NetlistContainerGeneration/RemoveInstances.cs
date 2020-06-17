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
    [CommandDescription(Description = "Remove nets from the given netlist ontainer and (optionally) write the instances code to file", Wrapper = false, Publish = true)]
    class RemoveInstances : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = this.GetNetlistContainer();

            Regex filter = new Regex(this.InstanceNameRegexp, RegexOptions.Compiled);

            // output to be removed nets for later writing them to file
            foreach (Instance inst in netlistContainer.Instances.Where(n => filter.IsMatch(n.Name)))
            {
                this.OutputManager.WriteOutput(inst.ToString());
            }

            netlistContainer.Remove(new Predicate<Instance>(n => filter.IsMatch(n.Name)));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }        
        
        [Parameter(Comment = "All instances whose name matches this regular expression will be removed")]
        public String InstanceNameRegexp = "^inst$";
    }
}
