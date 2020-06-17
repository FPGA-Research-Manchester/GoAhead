using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Add all arcs from the given file to the given net (arcs inside the net will remina unique)")]
    class AddPipsToNet : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE);

            NetlistContainer nlc = this.GetNetlistContainer();

            if (!nlc.Nets.Any(n => n.Name.Equals (this.NetName)))
            {
                throw new ArgumentException(nlc.Name + " does not contain a net named " + this.NetName);
            }

            XDLNet net = (XDLNet) nlc.GetNet(this.NetName);

            String line = "";
            TextReader tr = new StreamReader(this.FileName);
            while ((line = tr.ReadLine()) != null)
            {
                if(Regex.IsMatch(line, @"^\s*#"))
                {
                    continue;
                }
                String[] atoms = line.Split(' ');

                String location = atoms[1];
                String from = atoms[2];
                String arrow = atoms[3];
                String to = atoms[4].Replace(",", "");

                XDLPip pip = new XDLPip(location, from, arrow, to);

                bool pipContainedInNet = false;
                bool pipContainedInNetlsitContainer = false;
                bool pipIsDrivenByOtherNet = false;

                pipContainedInNet = net.Contains(p => p.Equals(pip));
                if (!pipContainedInNet)
                {
                    pipContainedInNetlsitContainer = nlc.Nets.Any(n => ((XDLNet) n).Contains(p => p.Equals(pip)));
                }
                if (!pipContainedInNet && !pipContainedInNetlsitContainer)
                {
                    pipIsDrivenByOtherNet = nlc.Nets.Any(n => ((XDLNet)n).Contains(p => p.Location.Equals(pip.Location) && p.To.Equals(pip.To)));
                }

                if (pipContainedInNet)
                {
                    this.OutputManager.WriteOutput("not adding " + pip.ToString() + " as it is already part of the net");
                }
                else if(pipContainedInNetlsitContainer)
                {
                    this.OutputManager.WriteOutput("not adding " + pip.ToString() + " as it is already part of another net");
                }
                else if (pipIsDrivenByOtherNet)
                {
                    this.OutputManager.WriteOutput("not adding " + pip.ToString() + " as right hand side is driven by other net");
                }
                else
                {
                    net.Add(pip);
                }
            }
            tr.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file with pips")]
        public String FileName = "pips.txt";

        [Parameter(Comment = "The full name of the net to extend")]
        public String NetName = "RBB_Blocker";
    }
}
