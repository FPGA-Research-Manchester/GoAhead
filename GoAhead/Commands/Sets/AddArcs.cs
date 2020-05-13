using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Commands;
using GoAhead.Objects;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.Sets
{
    [CommandDescription(Description = "Add the arc (if existing) specified by From and To to a net from the given netlist container in the current selection of tiles.", Wrapper = false, Publish = true)]
    class AddArcs : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            NetlistContainer netlistContainer = GetNetlistContainer();
            
            bool arcAdded = false;

            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    arcAdded = AddArcsXDL(netlistContainer);
                    break;
                case FPGATypes.BackendType.Vivado:
                    arcAdded = AddArcsVivado((TCLContainer)netlistContainer);
                    break;
            }            

            if (!arcAdded)
            {
                throw new ArgumentException("Did not find any tile in the current selection that contains the arcs " + From + " -> " + To + ". Misspelled From or To?");
            }
        }

        private bool AddArcsVivado(TCLContainer netlistContainer)
        {
            // which net to extend?
            TCLNet target;
            if (netlistContainer.Nets.Any(n => n.Name.Equals(Netname)))
            {
                target = (TCLNet)netlistContainer.GetNet(Netname);
            }           
            else
            {
                target = (TCLNet)Net.CreateNet(Netname);
                target.IsBlockerNet = true;
                target.RoutingTree = new TCLRoutingTree();
                TCLRoutingTreeNode root = new TCLRoutingTreeNode(null, null);
                root.VirtualNode = true;
                target.RoutingTree.Root = root;

                netlistContainer.Add(target);
                netlistContainer.AddGndPrimitive(target);
            }

            Port from = new Port(From);
            Port to = new Port(To);

            bool arcAdded = false;
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles().Where(t => AddArcOnThisTile(from, to, t)))
            {
                TCLRoutingTreeNode driverNode = target.RoutingTree.Root.Children.FirstOrDefault(tile.Location, From);
                if (driverNode == null)
                {
                    driverNode = new TCLRoutingTreeNode(tile, from);
                    //driverNode.VivadoPipConnector = orderEl.VivadoPipConnector;
                    target.RoutingTree.Root.Children.Add(driverNode);
                }
                TCLRoutingTreeNode leaveNode = new TCLRoutingTreeNode(tile, to);
                driverNode.Children.Add(leaveNode);

                // block ports
                if (!tile.IsPortBlocked(from, Tile.BlockReason.Blocked))
                {
                    tile.BlockPort(from, Tile.BlockReason.Blocked);
                }
                tile.BlockPort(to, Tile.BlockReason.Blocked);
                arcAdded = true;
            }

            return arcAdded;
        }

        private bool AddArcsXDL(NetlistContainer netlistContainer)
        {
            // which net to extend?
            XDLNet target;
            if (netlistContainer.Nets.Any(n => n.Name.Equals(Netname)))
            {
                target = (XDLNet)netlistContainer.GetNet(Netname);
            }
            else
            {
                target = (XDLNet)netlistContainer.GetAnyNet();
            }

            Port from = new Port(From);
            Port to = new Port(To);

            bool arcAdded = false;
            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles().Where(t => AddArcOnThisTile(from, to, t)))
            {
                target.Add(CommentForPip);
                target.Add(t, from, to);
                if (!t.IsPortBlocked(from, Tile.BlockReason.Blocked))
                {
                    t.BlockPort(from, Tile.BlockReason.Blocked);
                }
                t.BlockPort(to, Tile.BlockReason.Blocked);
                arcAdded = true;
            }

            return arcAdded;
        }

        private  bool AddArcOnThisTile(Port from, Port to, Tile t)
        {
            return 
                t.SwitchMatrix.Contains(to) &&                
                t.SwitchMatrix.Contains(from, to) &&                
                !(t.IsPortBlocked(to, Tile.BlockReason.Blocked) || t.IsPortBlocked(to, Tile.BlockReason.OccupiedByMacro));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The driver to use to block To")]
        public string From = "";

        [Parameter(Comment = "This comment will be added to the pip")]
        public string CommentForPip = "added_by_AddArcs";

        [Parameter(Comment = "For each selected tile, add a arc driving this port to a net from the given blocker")]
        public string To = "";

        [Parameter(Comment = "The name of the net to extend (leave empty to extend the first net with an outpin (ISE) or to create a new net called AddArcs (Vivado))")]
        public string Netname = "";
    }
}
