using System;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Remove primitives and routing inside the selected area.", Wrapper = false, Publish = true)]
    class CutOff : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    CutOffISE();
                    break;
                case FPGATypes.BackendType.Vivado:
                    CutOffVivado();
                    break;
            }
        }
       
        private void CutOffISE()
        {
            XDLContainer nlc = (XDLContainer)GetNetlistContainer();
            int netsDone = 0;
            foreach (XDLNet n in nlc.Nets)
            {
                ProgressInfo.Progress = (int)((double)netsDone++ / (double)nlc.NetCount * 100);
                int pipCount = n.PipCount;

                //List<XDLPip> removedPips = 
                n.Remove(p => Remove(p));
                n.RemoveAllPinStatements(np => Remove(np, nlc));

                // pip count changed -> probably a PRLink that may be decomposed
                if (pipCount != n.PipCount)
                {
                    n.PRLink = true;
                }
            }

            nlc.Remove(inst => Remove(inst));
            // remove all nets that are now empty
            nlc.Remove(n => n.PipCount == 0 && n.InpinCount == 0 && n.OutpinCount == 0);
        }

        private void CutOffVivado()
        {
            TCLContainer nlc = (TCLContainer) GetNetlistContainer();
            int netsDone = 0;
            foreach (TCLNet net in nlc.Nets)
            {
                ProgressInfo.Progress = (int)((double)netsDone++ / (double)nlc.NetCount * 100);
                // only flatten in we will really remove something, otherwise keep the tree structure to save processing later
                if(net.RoutingTree.GetAllRoutingNodes().Any(n => !n.VirtualNode && TileSelectionManager.Instance.IsSelected(n.Tile)))
                {
                    net.FlattenNet();
                    net.Remove(node => !node.VirtualNode && TileSelectionManager.Instance.IsSelected(node.Tile));
                }

            }
            nlc.Remove(inst => Remove(inst));
        }

        private bool Remove(XDLPip pip)
        {
            return TileSelectionManager.Instance.IsSelected(FPGA.FPGA.Instance.GetTile(pip.Location).TileKey);
        }

        private bool Remove(TCLRoutingTreeNode node)
        {
            return TileSelectionManager.Instance.IsSelected(node.Tile.TileKey);
        }

        private bool Remove(NetPin np, XDLContainer netlistContainer)
        {
            return TileSelectionManager.Instance.IsSelected(FPGA.FPGA.Instance.GetTile(netlistContainer.GetInstance(np).Location).TileKey);
        }    

        private bool Remove(Instance inst)
        {           
            return TileSelectionManager.Instance.IsSelected(FPGA.FPGA.Instance.GetTile(inst.Location).TileKey);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
