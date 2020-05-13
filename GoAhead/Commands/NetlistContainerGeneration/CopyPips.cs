using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Copy the pips from all nets whose names match SourceNetsRegexp to the net (one) whose name matches TargetNetsRegexp", Wrapper = false, Publish = true)]
    class CopyPips : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE); 

            NetlistContainer netlistContainer = GetNetlistContainer();

            Regex sourceNetFilter = new Regex(SourceNetsRegexp, RegexOptions.Compiled);
            Regex targetNetFilter = new Regex(TargetNetsRegexp, RegexOptions.Compiled);
            Regex pipFilter = new Regex(PipFilter, RegexOptions.Compiled);

            XDLNet targetNet = (XDLNet) netlistContainer.Nets.FirstOrDefault(n => targetNetFilter.IsMatch(n.Name));
            if (targetNet == null)
            {
                throw new ArgumentException("Could not find a target net");
            }

            foreach (XDLNet sourceNet in netlistContainer.Nets.Where(n => sourceNetFilter.IsMatch(n.Name)))
            {
                foreach (XDLPip pip in sourceNet.Pips.Where(p => pipFilter.IsMatch(p.Location)))
                {
                    XDLPip copy = new XDLPip(pip.Location, pip.From, pip.Operator, pip.To);
                    targetNet.Add(copy);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = "Only copy pips that are located on tiles that match this regular expression")]
        public string PipFilter = "^INT_";

        [Parameter(Comment = "Copy pips from all nets whose identifier match this regular expression")]
        public string SourceNetsRegexp = "index_[0-3]_loop";

        [Parameter(Comment = "Copy pips to the first net whose identifier matches this regular expression")]
        public string TargetNetsRegexp = "^RBB_Blocker";
    }
}
