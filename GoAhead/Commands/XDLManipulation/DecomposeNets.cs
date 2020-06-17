using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.XDLManipulation
{
    class DecomposeNets : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            NetlistContainer netlistContainer = this.GetNetlistContainer();

            // extract net names as we may not remve during iteration
            List<XDLNet> netNamesToDecompose = new List<XDLNet>();
            foreach(String netName in this.NetNames)
            {
                XDLNet n = (XDLNet) netlistContainer.GetNet(netName);
                netNamesToDecompose.Add(n);
            }

            foreach (XDLNet net in netNamesToDecompose)
            {
                foreach (XDLPip pip in net.Pips)
                {
                    XDLNet arc = new XDLNet(net.Name + "_" + pip.Location + "_" + pip.From + "_" + pip.To);
                    // TODO what about attributes
                    arc.Add(pip);
                    netlistContainer.Add(arc);
                }
                net.ClearPips();
                if (net.NetPinCount == 0)
                {
                    netlistContainer.Remove(new Predicate<Net>(n => n.Name.Equals(net.Name)));
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "All nets whose name matches this regular expression will be decomposed (e.g. ^module removes all nets woth prefix module, $netname$ remove a particular net name)")]
        public List<String> NetNames = new List<String>();
    }

}
