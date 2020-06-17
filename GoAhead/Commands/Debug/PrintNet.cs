using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.Code;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print all nets whose name matches the given net name", Wrapper = false, Publish = true)]
    class PrintNet : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = this.GetNetlistContainer();
            Net net = nlc.Nets.FirstOrDefault(n => n.Name.Equals(this.NetName));
            if(net == null)
            {
                throw new ArgumentException("Net " + this.NetName + " not found");
            }
            this.OutputManager.WriteOutput(net.Name + ":");
            this.OutputManager.WriteOutput(net.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the net that will be printed ")]
        public String NetName = "quote(p2s[9])";
    }
}
