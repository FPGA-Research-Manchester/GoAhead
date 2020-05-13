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

            NetlistContainer nlc = GetNetlistContainer();

            if (!nlc.Nets.Any(n => n.Name.Equals (NetName)))
            {
                throw new ArgumentException(nlc.Name + " does not contain a net named " + NetName);
            }

            XDLNet net = (XDLNet) nlc.GetNet(NetName);

            string line = "";
            TextReader tr = new StreamReader(FileName);
            while ((line = tr.ReadLine()) != null)
            {
                if(Regex.IsMatch(line, @"^\s*#"))
                {
                    continue;
                }
                string[] atoms = line.Split(' ');

                string location = atoms[1];
                string from = atoms[2];
                string arrow = atoms[3];
                string to = atoms[4].Replace(",", "");

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
                    OutputManager.WriteOutput("not adding " + pip.ToString() + " as it is already part of the net");
                }
                else if(pipContainedInNetlsitContainer)
                {
                    OutputManager.WriteOutput("not adding " + pip.ToString() + " as it is already part of another net");
                }
                else if (pipIsDrivenByOtherNet)
                {
                    OutputManager.WriteOutput("not adding " + pip.ToString() + " as right hand side is driven by other net");
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
        public string FileName = "pips.txt";

        [Parameter(Comment = "The full name of the net to extend")]
        public string NetName = "RBB_Blocker";
    }
}
