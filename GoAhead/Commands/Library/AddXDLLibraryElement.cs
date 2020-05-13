using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Add an XDL netlist, an XDL design, module, or a connection primitive to the libary of placeable elements.", Wrapper = true, Publish = true)]
    class AddXDLLibraryElement : Command
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            // read file
            DesignParser parser = DesignParser.CreateDesignParser(FileName);

            XDLContainer container = new XDLContainer();
            // into design            
            parser.ParseDesign(container, this);

            // derive name from file
            string elementName = Path.GetFileNameWithoutExtension(FileName);

            if (container.ModuleCount != 0)
            {
                // find ports to block and assign them to slices
                // as we want to either
                // connect these ports (if used) or
                // drive a '1' to (if unused)
                foreach (XDLModule module in container.Modules)
                {
                    // new library element to be added to library   
                    LibraryElement libElement = new LibraryElement();
                    libElement.Containter = module;
                    libElement.Name = elementName;
                    libElement.PrimitiveName = elementName;
                    libElement.LoadCommand = ToString();

                    // add lib element to library, BEFORE deriving block data as command SaveXDLLibraryElementAsBinaryLibraryElement access the element in the library
                    Objects.Library.Instance.Add(libElement);
                    DeriveBlockingData(libElement);
                }
            }
            else
            {
                // new library element to be added to library
                LibraryElement libElement = new LibraryElement();
                libElement.Containter = container;
                libElement.Name = elementName;
                libElement.PrimitiveName = elementName;
                libElement.LoadCommand = ToString();

                // add lib element to library, BEFORE deriving block data as command SaveXDLLibraryElementAsBinaryLibraryElement access the element in the library
                Objects.Library.Instance.Add(libElement);
                DeriveBlockingData(libElement);
            }
        }

        private void DeriveBlockingData(LibraryElement libElement)
        {
            ////////////////////////////
            // find LOGICIN and LOGICOUT
            foreach (XDLPort xdlPort in ((XDLContainer)libElement.Containter).Ports)
            {
                XDLInstance inst = (XDLInstance) libElement.Containter.GetInstanceByName(xdlPort.InstanceName);
                Tile clb = FPGA.FPGA.Instance.GetTile(inst.Location);
                Tile interconnectTile = FPGATypes.GetInterconnectTile(clb);
                Slice slice = clb.GetSliceByName(inst.SliceName);

                // true: in, false: out (no needed)
                Dictionary<uint, bool> portMapping = new Dictionary<uint, bool>();
                foreach(Port inPort in slice.PortMapping.GetPorts(FPGATypes.PortDirection.In))
                {
                    portMapping.Add(inPort.NameKey, true);
                }

                // 1 store all ports we need to connect the switchmatrx with macro in ports
                foreach (Tuple<Port, Port> intArc in interconnectTile.SwitchMatrix.GetAllArcs())
                {
                    bool portFound = false;
                    // find the arc that is driven by the xdlPort && make sure that we get the correct slice
                    foreach (Location loc in Navigator.GetDestinations(interconnectTile, intArc.Item2).Where(l => l.Tile.Location.Equals(clb.Location)))
                    {
                        foreach (Port clbPort in clb.SwitchMatrix.GetDrivenPorts(loc.Pip).Where(drivenPort => portMapping.ContainsKey(drivenPort.NameKey) && drivenPort.Name.EndsWith(xdlPort.SlicePort)))
                        {
                            libElement.AddPortToBlock(interconnectTile, intArc.Item2);
                            portFound = true;
                            break;
                        }
                        if (portFound)
                        {
                            break;
                        }
                    }
                }

                // 2 store all ports we need to connect the macro out ports to the switch matrix (LOGICOUT\d+)
                // // find the arc that is driven by the xdlPort &&  make sure that we get the correct slice
                foreach (Tuple<Port, Port> arc in clb.SwitchMatrix.GetAllArcs().Where(a => slice.PortMapping.IsSliceOutPort(a.Item1) && a.Item1.Name.EndsWith(xdlPort.SlicePort)))
                { 
                    foreach (Location loc in Navigator.GetDestinations(clb, arc.Item2))
                    {
                        libElement.AddPortToBlock(loc.Tile, loc.Pip);
                    }
                }
            }

            ////////////////////////////
            // extract begin pips to block
            foreach (XDLNet net in libElement.Containter.Nets)
            {
                foreach (XDLPip pip in net.Pips)
                {
                    libElement.AddPortToBlock(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.From));
                    libElement.AddPortToBlock(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.To));
                }
            }

            // sideeffect, trigger calculation of slice number to reuse this macro on other devices            
            foreach (XDLInstance inst in libElement.Containter.Instances)
            {
                inst.DeriveTileKeyAndSliceNumber();
            }          

            // CLB anchor
            if (((XDLContainer)libElement.Containter).ExplicitAnchorFound)
            {
                if (((XDLContainer)libElement.Containter).HasInstanceByName(((XDLContainer)libElement.Containter).Anchor))
                {
                    XDLInstance anchorInst = (XDLInstance) libElement.Containter.GetInstanceByName(((XDLContainer)libElement.Containter).Anchor);

                    libElement.ResourceShape.Anchor.AnchorLocationX = anchorInst.LocationX;
                    libElement.ResourceShape.Anchor.AnchorLocationY = anchorInst.LocationY;
                    libElement.ResourceShape.Anchor.AnchorSliceName = anchorInst.SliceName;
                    libElement.ResourceShape.Anchor.AnchorSliceNumber = anchorInst.SliceNumber;
                    libElement.ResourceShape.Anchor.AnchorTileLocation = anchorInst.Location;
                }
                else
                {
                    Slice anchorSlice = FPGA.FPGA.Instance.GetSlice(((XDLContainer)libElement.Containter).Anchor);
                    libElement.ResourceShape.Anchor.AnchorLocationX = anchorSlice.ContainingTile.LocationX;
                    libElement.ResourceShape.Anchor.AnchorLocationY = anchorSlice.ContainingTile.LocationY;
                    libElement.ResourceShape.Anchor.AnchorSliceName = anchorSlice.SliceName;
                    libElement.ResourceShape.Anchor.AnchorSliceNumber = 0; // see CutOffFromDesign
                    libElement.ResourceShape.Anchor.AnchorTileLocation = anchorSlice.ContainingTile.Location;
                }
            }
            else
            {
                // TODO der Slot muss ausgewaehlt sein!
                SetResourceShapeInfo setCmd = new SetResourceShapeInfo();
                setCmd.LibraryElementName = libElement.Name;
                CommandExecuter.Instance.Execute(setCmd);
            }
        }

        public override void Undo()
        {
        }
        
        [Parameter(Comment = "The XDL netlist to read in")]
        public string FileName = "design.xdl";
    }
}
