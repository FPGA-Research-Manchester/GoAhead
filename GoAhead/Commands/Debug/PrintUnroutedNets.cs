using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print the names of all unrouted netlists (i.e. nets that provide one outpin and one or more inpins, however no pips)", Wrapper = false)]
    class PrintUnroutedNets : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {            
            NetlistContainer nlc = GetNetlistContainer();
            foreach (XDLNet net in nlc.Nets.Where(n => n.OutpinCount == 1 && n.InpinCount > 0 && n.PipCount == 0))
            {
                OutputManager.WriteOutput(net.Name);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
