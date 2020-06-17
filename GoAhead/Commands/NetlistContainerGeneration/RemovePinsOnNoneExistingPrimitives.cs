using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Remove all inpins and outpins statements that reside on non existing primitives", Wrapper = false, Publish = true)]
    class RemovePinsOnNoneExistingPrimitives : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = this.GetNetlistContainer();

            // find inpins/outpins which reside on unknown primitives 
            int netCount = 0;
            foreach (XDLNet net in nlc.Nets)
            {
                this.ProgressInfo.Progress = (int)((double)(netCount++) / (double)nlc.NetCount * 100);
                net.RemoveAllPinStatements(np => !nlc.Instances.Any(i => i.SliceName.Equals(np.InstanceName)));              
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
