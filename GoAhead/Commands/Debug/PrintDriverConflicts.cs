using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print all driver conflicts (if any) in the given netlist container. If no output is created, there are no conflicts.", Wrapper = false, Publish = true)]
    class PrintDriverConflicts : NetlistContainerGeneration.NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer m = this.GetNetlistContainer();

            // build mapping: Location -> XDLNet
            Dictionary<String, Dictionary<String, XDLNet>> netConflicts = new Dictionary<String, Dictionary<String, XDLNet>>();
            Dictionary<String, Dictionary<String, XDLPip>> pipConflicts = new Dictionary<String, Dictionary<String, XDLPip>>();
            foreach (XDLNet n in m.Nets)
            {
                foreach (XDLPip pip in n.Pips)
                {
                    if (!pip.Operator.Equals("->"))
                    {
                        continue;
                    }

                    if (!netConflicts.ContainsKey(pip.Location))
                    {
                        netConflicts.Add(pip.Location, new Dictionary<String, XDLNet>());
                        pipConflicts.Add(pip.Location, new Dictionary<String, XDLPip>());
                    }

                    // to
                    if (!netConflicts[pip.Location].ContainsKey(pip.To))
                    {
                        netConflicts[pip.Location].Add(pip.To, n);
                        pipConflicts[pip.Location].Add(pip.To, pip);
                    }
                    else
                    {
                        XDLNet conflictingNet = netConflicts[pip.Location][pip.To];
                        XDLPip conflictingPip = pipConflicts[pip.Location][pip.To];

                        String conflict = " in " + pip.ToString() + " and " + conflictingPip;
                        if (n.Name.Equals(conflictingNet.Name))
                        {
                            this.OutputManager.WriteOutput("Detected driver conflift in net " + n.Name + conflict );
                        }
                        else
                        {
                            this.OutputManager.WriteOutput("Detected driver conflift between nets " + n.Name + " and " + netConflicts[pip.Location][pip.To].Name + conflict);
                        }
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
