using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description="Move all arcs from all nets into the first net with an outpin and without attributes. Outpins and inpins will not be changed", Wrapper=false)]
    class MoveAllArcsToFirstNet : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            NetlistContainer nlc = GetNetlistContainer();

            // look for net with outping
            XDLNet anyNet = (XDLNet) nlc.Nets.FirstOrDefault(n => n.OutpinCount == 1 && string.IsNullOrEmpty(((XDLNet) n).HeaderExtension));

            if (anyNet == null)
            {
                throw new ArgumentException("Could not a net with an outpin in " + NetlistContainerName);
            }

            // add outpin and inpin statements
            foreach (XDLNet net in nlc.Nets.Where(net => OtherArcsFilter(anyNet, net)))
            {
                anyNet.Add(net, false);
                // ports remain blocked
                net.ClearPips();
                net.ClearPins();
            }          
        }

        private bool OtherArcsFilter(Net sink, Net other)
        {
            if (OnlyMoveArcsFromNetsWithoutOutpin)
            {
                return !other.Name.Equals(sink.Name) && other.OutpinCount == 0;
            }
            else
            {
                return !other.Name.Equals(sink.Name);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Whether to only move those arcs whose nets do not have outpins (false: all arcs from all nets will be moved")]
        public bool OnlyMoveArcsFromNetsWithoutOutpin = false;
    }
}
