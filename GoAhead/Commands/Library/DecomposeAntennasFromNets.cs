using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Decompose all nets that are marked as PRLinks by the command CutOff or AddInstantiation (refer to these commands for further info)")]
    class DecomposeAntennasFromNets : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
            {
                return;
            }

            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            NetlistContainer nlc = this.GetNetlistContainer();

            int workload = this.GetNetsToDecomposeWithOutpin().Count();
            int count = 0;

            List<XDLNet> newNets = new List<XDLNet>();

            foreach(XDLNet net in this.GetNetsToDecomposeWithOutpin().Where(n => n.PRLink))
            {
                this.ProgressInfo.Progress = this.ProgressStart + (int)((double)count++ / (double)workload * this.ProgressShare);

                Dictionary<String, List<XDLPip>> pipsToRemove = null;

                // decompose nets without outpin.
                // e.g., placing a module on connection macros wil remove outpins from certain I/O bar wires
                if (net.NetPins.Where(np => np is NetOutpin).Count() == 0)
                {
                    pipsToRemove = new Dictionary<string, List<XDLPip>>();                                    
                    foreach (XDLPip pip in net.Pips)
                    {
                        if (!pipsToRemove.ContainsKey(pip.Location))
                        {
                            pipsToRemove.Add(pip.Location, new List<XDLPip>());
                        }
                        pipsToRemove[pip.Location].Add(pip);
                    }
                }
                else
                {
                    bool antenna = net.IsAntenna(out pipsToRemove);
                }

                bool firstArc = true;
                // values are all non empty litst
                foreach (List<XDLPip> l in pipsToRemove.Values)
                {
                    foreach(XDLPip pip in l)
                    {
                        if (firstArc)
                        {
                            firstArc = false;
                            //this.OutputManager.WriteOutput("Decomposing net " + net.Name);
                        }

                        XDLNet arc = new XDLNet(net.Name + "_arc_" + pip.Location + "_" + pip.From + "_" + pip.To);
                        //arc.AddComment("decomposed from net (with outpin) " + net.Name);
                        // TODO what about attributes?
                        arc.Add(pip);

                        // move inpins
                        List<NetPin> netPinsToRemove = new List<NetPin>();
                        foreach (NetPin netpin in net.NetPins.Where(np => np is NetInpin))
                        {
                            XDLInstance inst = (XDLInstance) nlc.GetInstanceByName(netpin.InstanceName);
                            Tile pipTile = FPGA.FPGA.Instance.GetTile(pip.Location);
                   
                            if(pipTile.TileKey.Equals(inst.TileKey))
                            {
                                //netpin.Comment += "taken from " + net.Name;
                                arc.Add(netpin);
                                // store net pip for later removal as we may not change the collection during iterating over it
                                netPinsToRemove.Add(netpin);
                            }
                        }
                        // remove the inpins from the original net ...
                        net.RemoveAllPinStatements(np => netPinsToRemove.Contains(np));
                        // ... and remove the arc from the original net
                        newNets.Add(arc);
                    }
                }
                // only invoke Remove once per net (blocker is very slow)
                net.Remove(p => this.PipFilter(p, pipsToRemove));                     
            }

            // decompose blocker net 
            foreach (XDLNet net in this.GetNetsToDecomposeWithoutOutpin())
            {
                foreach (XDLPip pip in net.Pips)
                {
                    XDLNet arc = new XDLNet(net.Name + "_arc_" + pip.Location + "_" + pip.From + "_" + pip.To);
                    //arc.AddComment("decomposed from net (without outpin) " + net.Name);
                    // TODO what about attributes?
                    arc.Add(pip);
                    newNets.Add(arc);

                }
                // remove all pips
                net.ClearPips();
            }

            // add arcs
            foreach (XDLNet n in newNets)
            {
                nlc.Add(n);
            }
        }

        private IEnumerable<XDLNet> GetNetsToDecomposeWithoutOutpin()
        {
            NetlistContainer macro = this.GetNetlistContainer();

            foreach (XDLNet net in macro.Nets.Where(n => n.OutpinCount == 0 && n.InpinCount == 0 && n.PipCount > 0))
            {
                yield return net;
            }
        }

        private IEnumerable<XDLNet> GetNetsToDecomposeWithOutpin()
        {
            NetlistContainer macro = this.GetNetlistContainer();

            foreach (XDLNet net in macro.Nets.Where(n => n.OutpinCount == 1 && n.PipCount > 0))
            {
                yield return net;
            }
            foreach (XDLNet net in macro.Nets.Where(n => n.OutpinCount == 0 && n.InpinCount > 0 && n.PipCount > 0))
            {
                yield return net;
            }
        }


        private bool PipFilter(XDLPip pip, Dictionary<String, List<XDLPip>> pipsToRemove)
        {
            if (!pipsToRemove.ContainsKey(pip.Location))
            {
                return false;
            }
            return pipsToRemove[pip.Location].Contains(pip);
        }
    
        public override void Undo()
        {
            throw new NotImplementedException();
        }

    }
}
