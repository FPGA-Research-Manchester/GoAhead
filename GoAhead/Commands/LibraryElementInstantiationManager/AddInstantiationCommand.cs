using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.FPGA;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.LibraryElementInstantiation
{
    [CommandDescription(Description = "A common base class for commands that instantiate library elements")]
    abstract class AddInstantiationCommand : NetlistContainerCommand
    {     
        protected void AutoClearModuleSlotBeforeInstantiation(LibraryElement libraryElement, IEnumerable<Tile> upperLeftAnchors, int progressStart = 0, int progressShare = 100)
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            if (libraryElement.ResourceShape == null)
            {
                throw new ArgumentException("Library element " + libraryElement.Name + " does not provide any module shape information");
            }

            NetlistContainer nlc = GetNetlistContainer();

            Dictionary<string, bool> targetLocations = new Dictionary<string, bool>();
            foreach (string originalTileIdnetifier in libraryElement.ResourceShape.GetContainedTileIdentifier().Where(s => !s.StartsWith("NULL")))
            {
                bool validTargetTileFound = false;
                foreach (Tile anchor in upperLeftAnchors)
                {
                    string targetLocation = "";
                    bool success = libraryElement.GetTargetLocation(originalTileIdnetifier, anchor, out targetLocation);
                    if (!targetLocations.ContainsKey(targetLocation))
                    {
                        targetLocations.Add(targetLocation, true);
                    }
                    if (success)
                    {
                        validTargetTileFound = true;
                    }
                }
                if (!validTargetTileFound)
                {
                    OutputManager.WriteWarning("Could not relocate " + originalTileIdnetifier);
                }
            }

            int netsDone = 0;
            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    foreach (XDLNet net in nlc.Nets.Where(n => !n.ReadOnly))
                    {
                        ProgressInfo.Progress = progressStart + (int)((double)netsDone++ / (double)nlc.NetCount * progressShare);
                        RemovePipsFromNet((XDLContainer)nlc, targetLocations, net);
                    }
                    // remove all nets that are now empty
                    nlc.Remove(new Predicate<Net>(n => n.PipCount == 0 && n.InpinCount == 0 && n.OutpinCount == 0));
                    break;
                case FPGATypes.BackendType.Vivado:
                    foreach (TCLNet net in nlc.Nets)
                    {
                        ProgressInfo.Progress = (int)((double)netsDone++ / (double)nlc.NetCount * 100);
                        // only flatten in we will really remove something, otherwise keep the tree structure to save processing later
                        if(net.RoutingTree.GetAllRoutingNodes().Any(n => !n.VirtualNode && TileSelectionManager.Instance.IsSelected(n.Tile)))
                        {
                            net.FlattenNet();
                            net.Remove(node => !node.VirtualNode && TileSelectionManager.Instance.IsSelected(node.Tile));
                            net.Remove(new Predicate<NetPin>(np => TileSelectionManager.Instance.IsSelected(np.TileName)));

                            // remove the outpins instance if it is selected
                            if (net.OutpinInstance != null)
                            {
                                if (TileSelectionManager.Instance.IsSelected(net.OutpinInstance.Location))
                                {
                                    net.OutpinInstance = null;
                                }

                            }
                        }
                    }                    
                    break;
            }

            // handle instances equally (XDL and TCL), only nets differ
            nlc.Remove(inst => targetLocations.ContainsKey(inst.Location));
        }

        private void RemovePipsFromNet(XDLContainer netlistContainer, Dictionary<string, bool> targetLocations, XDLNet net)
        {           
            int pipCount = net.PipCount;
          
            net.Remove(p => targetLocations.ContainsKey(p.Location));
            net.RemoveAllPinStatements(np => targetLocations.ContainsKey(netlistContainer.GetInstance(np).Location));

            // pip count changed -> probably a PRLink that may be decomposed
            if (pipCount != net.PipCount)
            {
                net.PRLink = true;
            }            
        }

        [Parameter(Comment = "Whether to call the fuse command at the end of this command", PrintParameter = false)]
        public bool AutoFuse = true;

        [Parameter(Comment = "The instance name, e.g BM_S6_L4_R4_double. For multiple instantiations and index will be added as a postifx")]
        public string InstanceName = "instance_name";

        [Parameter(Comment = "The hiearchy to add before the instance name")]
        public string Hierarchy = "";

        [Parameter(Comment = "The name of the library element, e.g. BM_S6_L4_R4_double")]
        public string LibraryElementName = "BM_S6_L4_R4_double";

        [Parameter(Comment = "Whether to automatically free the resource slot denoted by the anchor")]
        public bool AutoClearModuleSlot = false;
    }
}
