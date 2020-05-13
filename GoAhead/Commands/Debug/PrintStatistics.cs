using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.Code;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print some statistics about the given netlist container", Wrapper = true, Publish = true)]
    class PrintStatistics : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = GetNetlistContainer();

            OutputManager.WriteOutput("Name: " + nlc.Name);
            OutputManager.WriteOutput("Instances: " + nlc.InstanceCount);
            OutputManager.WriteOutput("Nets: " + nlc.Nets.Count());
            if (nlc.NetCount > 0)
            {
                OutputManager.WriteOutput("Average pip count per net: " + nlc.Nets.Average(n => n.PipCount));
                OutputManager.WriteOutput("Minimal pip count: " + nlc.Nets.Min(n => n.PipCount));
                OutputManager.WriteOutput("Maximal pip count: " + nlc.Nets.Max(n => n.PipCount));

                OutputManager.WriteOutput("-----------------------------------------");
                IEnumerable<Net> netsWithoutOutpin = nlc.Nets.Where(n => n.OutpinCount == 0);
                OutputManager.WriteOutput("Nets without Outpin: " + netsWithoutOutpin.Count());
                PrintHits(netsWithoutOutpin);
                OutputManager.WriteOutput("-----------------------------------------");

                IEnumerable<Net> netsWithoutOutpinWithMoreThanOnePip = nlc.Nets.Where(n => n.OutpinCount == 0 && n.PipCount > 0);
                OutputManager.WriteOutput("Nets without Outpin and more than one pip: " + netsWithoutOutpinWithMoreThanOnePip.Count());
                PrintHits(netsWithoutOutpinWithMoreThanOnePip);
                OutputManager.WriteOutput("-----------------------------------------");

                IEnumerable<Net> netsWithoutIntpin = nlc.Nets.Where(n => n.InpinCount == 0);
                OutputManager.WriteOutput("Nets without Inpin: " + netsWithoutIntpin.Count());
                PrintHits(netsWithoutIntpin);

                OutputManager.WriteOutput("-----------------------------------------");
                IEnumerable<Net> singelInpinNets = nlc.Nets.Where(n => n.InpinCount == 1 && n.OutpinCount == 0 && n.PipCount == 0);
                OutputManager.WriteOutput("Single inpin nets (dummy nets): " + singelInpinNets.Count());
                PrintHits(singelInpinNets);
            }

            if (PrintAntennas)
            {
                PrintAntennas printAntennasCmd = new PrintAntennas();
                printAntennasCmd.NetlistContainerName = NetlistContainerName;
                // consider all nets
                printAntennasCmd.PositiveFilter = ".*";
                printAntennasCmd.NegativeFilter = "";

                CommandExecuter.Instance.Execute(printAntennasCmd);
            }
        }

        private void PrintHits(IEnumerable<Net> nets)
        {
            foreach (Net n in nets.Take(NetNameLimit))
            {
                OutputManager.WriteOutput(n.Name);
            }
            if (nets.Count() > NetNameLimit)
            {
                OutputManager.WriteWarning(" ... more follows. Increase NetNameLimit to " + nets.Count());
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Print which nets have antennas. The last pip of the net is also printed.")]
        public bool PrintAntennas = false;

        [Parameter(Comment = "Do not print print more that NetNameLimit net names per statistic")]
        public int NetNameLimit = 10;
    }
}
