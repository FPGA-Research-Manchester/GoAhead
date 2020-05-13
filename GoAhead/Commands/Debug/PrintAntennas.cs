using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description="Print all nets names that match the given filter and contain islands, i.e. pips that are not continued", Publish=true, Wrapper=false)]
    class PrintAntennas : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            int netsDone = 0;
            int netCount = GetAllNets().Count();

            foreach (XDLNet net in GetAllNets().Where(n => n.OutpinCount == 0))
            { 
                Print("Found net without outpin: " + net.Name );
            }           

            foreach (XDLNet net in GetAllNets())
            {           
                ProgressInfo.Progress = ProgressStart + (int)((double)netsDone++ / (double)netCount * ProgressShare);

                foreach (XDLPip pip in net.Pips)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                    if(t.IsSliceOutPort(pip.From) || t.IsSliceInPort(pip.To) || pip.Operator.Equals("=-"))
                    {
                        continue;
                    }

                    List<Location> reachable = new List<Location>();
                    foreach (Location loc in Navigator.GetDestinations(pip.Location, pip.To))
                    {
                        reachable.Add(loc);
                    }

                    bool antennaHead = true;
                    foreach (Location loc in reachable)
                    {
                        // other pip
                        if(net.HasPip(p => p.Location.Equals(loc.Tile.Location) && p.From.Equals(loc.Pip.Name)))
                        {
                            antennaHead = false;
                            break;
                        }
                        // other long line, e.g. LH0 =- LV16,
                        if(net.HasPip(p => p.Location.Equals(loc.Tile.Location) && p.To.Equals(loc.Pip.Name) && p.Operator.Equals("=-")))
                        {
                            antennaHead = false;
                            break;
                        }
                    }

                    // stop over
                    if (net.HasPip(p => p.Location.Equals(pip.Location) && p.From.Equals(pip.To)))
                    {
                        antennaHead = false;
                        break;
                    }

                    if (antennaHead)
                    {
                        Print("-------- Antenna heads --------");
                        if (reachable.Count > 0)
                        {
                            Print("Net " + net.Name + " has island via " + pip + " as " + pip.To + " is connected to:");
                            foreach (Location l in reachable)
                            {
                                Print("--> " + l.ToString() + " which is not continued within the net");
                            }
                        }
                        else
                        {
                            Print("Net " + net.Name + " has island via " + pip + " which is not continued within the net");
                        }
                    }
                }
            }
        }

        private void Print(string output)
        {
            if (m_firstPrint)
            {
                m_firstPrint = false;
                NetlistContainer nlc = GetNetlistContainer();
                OutputManager.WriteOutput("Report for " + nlc.Name);
            }
            OutputManager.WriteOutput(output);
        }

        public IEnumerable<XDLNet> GetAllNets()
        {
            NetlistContainer nlc = GetNetlistContainer();

            foreach (XDLNet net in nlc.Nets.Where(n => Positive((XDLNet)n) && !Negative((XDLNet) n)))
            {
                yield return net;
            }
        }

        private bool Positive(XDLNet net)
        {
            return Regex.IsMatch(net.Name, PositiveFilter);
        }

        private bool Negative(XDLNet net)
        {
            if(string.IsNullOrEmpty(NegativeFilter))
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(net.Name, NegativeFilter);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private bool m_firstPrint = true;

        [Parameter(Comment = "Consider only those nets whose names match this regular expression")]
        public string PositiveFilter = ".*";

        [Parameter(Comment = "Do not consider those nets whose names match this regular expression (leave empty to disable this filter")]
        public string NegativeFilter = "";
    }
}
