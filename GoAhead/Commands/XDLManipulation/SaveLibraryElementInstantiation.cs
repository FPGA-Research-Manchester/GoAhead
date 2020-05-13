using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Objects;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.XDLManipulation
{
    [CommandDescription(Description = "Save an instantiation of a library element into a netlist container", Wrapper = false, Publish = true)]
    class SaveLibraryElementInstantiation : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            LibElemInst inst = LibraryElementInstanceManager.Instance.GetInstantiation(InstanceName);
            LibraryElement libElement = Objects.Library.Instance.GetElement(inst.LibraryElementName);
            Tile anchorCLB = FPGA.FPGA.Instance.GetTile(inst.AnchorLocation);
            NetlistContainer netlistContainer = GetNetlistContainer();

            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    RelocateInstancesForXDL(libElement, anchorCLB, (XDLContainer)netlistContainer);
                    RelocateNetsForXDL(libElement, anchorCLB, (XDLContainer)netlistContainer);

                    // add design config
                    if (AddDesignConfig && libElement.Containter is XDLContainer && ((XDLContainer)netlistContainer).GetDesignConfig().Length == 0)
                    {
                        ((XDLContainer)netlistContainer).AddDesignConfig(((XDLContainer)libElement.Containter).GetDesignConfig());
                    }
                    break;
                case FPGATypes.BackendType.Vivado:
                    RelocateInstancesForTCL(libElement, anchorCLB, (TCLContainer)netlistContainer);
                    RelocateNetsForTCL(libElement, anchorCLB, netlistContainer);
                    break;
            }           
        }
        
        private void RelocateInstancesForTCL(LibraryElement libElement, Tile anchorCLB, TCLContainer netlistContainer)
        {
            foreach (Tuple<Instance, Tile> tileSliceTupel in libElement.GetInstanceTiles(anchorCLB, libElement))
            {
                TCLInstance newInstance = new TCLInstance((TCLInstance)tileSliceTupel.Item1);
                // change LOC property and the other fields carries out the actual relocation
                Slice targetSlice = tileSliceTupel.Item2.Slices[(int)newInstance.SliceNumber];
                newInstance.Properties.SetProperty("LOC", targetSlice.SliceName, false);
                if (InsertPrefix)
                {
                    newInstance.Name = InstanceName + "_" + newInstance.Name; // do not use / to avoid creating a new hierarchy for which w do not have a refernce cell
                }
                newInstance.SliceName = targetSlice.SliceName;
                newInstance.SliceType = targetSlice.SliceType;
                newInstance.SliceNumber = targetSlice.ContainingTile.GetSliceNumberByName(targetSlice.SliceName);
                newInstance.TileKey = targetSlice.ContainingTile.TileKey;
                newInstance.Location = targetSlice.ContainingTile.Location;
                newInstance.LocationX = targetSlice.ContainingTile.LocationX;
                newInstance.LocationY = targetSlice.ContainingTile.LocationY;                
                newInstance.OmitPlaceCommand = true; // TODO we only support GND primitves, overwork this when placing module
                netlistContainer.Add(newInstance);
            }
        }

        private void RelocateNetsForTCL(LibraryElement libElement, Tile anchorCLB, NetlistContainer netlistContainer)
        {
            foreach (TCLNet net in libElement.Containter.Nets)
            {                
                TCLNet relocatedNet = TCLNet.Relocate(net, libElement, anchorCLB);
                relocatedNet.Name = InstanceName + relocatedNet.Name;

                // relocate NetPins
                foreach (NetPin pin in relocatedNet.NetPins)
                {                    
                    if (InsertPrefix)                        
                    {
                        pin.InstanceName = InstanceName + pin.InstanceName;
                    }
                }

                netlistContainer.Add(relocatedNet);
            }
        }

        private void RelocateInstancesForXDL(LibraryElement libElement, Tile anchorCLB, XDLContainer netlistContainer)
        {
            // reli
            foreach (Tuple<Instance, Tile> tileSliceTupel in libElement.GetInstanceTiles(anchorCLB, libElement))
            {
                int sliceNumber = tileSliceTupel.Item1.SliceNumber;
                Slice slice = tileSliceTupel.Item2.Slices[sliceNumber];

                string xdlCode = tileSliceTupel.Item1.ToString();

                /* in TODO init.goa auslagern
                 * @"placed.*SLICE_X\d+Y\d+";
                 * @"placed.*TIEOFF_X\d+Y\d+";
                 * @"placed.*RAMB16_X\d+Y\d+"
                 * 
                */

                if (IdentifierManager.Instance.IsMatch(tileSliceTupel.Item1.Location, IdentifierManager.RegexTypes.CLB))
                {
                    // replace placement information in: inst "right" "SLICEX", placed CLEXL_X9Y33 SLICE_X13Y33
                    string newPlacement = "placed " + tileSliceTupel.Item2.Location + " " + slice.SliceName;
                    string oldPlacement = @"placed.*SLICE_X\d+Y\d+";
                    xdlCode = Regex.Replace(xdlCode, oldPlacement, newPlacement);
                }
                else if (IdentifierManager.Instance.IsMatch(tileSliceTupel.Item1.Location, IdentifierManager.RegexTypes.Interconnect))
                {
                    string newPlacement = "placed " + tileSliceTupel.Item2.Location + " " + slice.SliceName;
                    string oldPlacement = @"placed.*TIEOFF_X\d+Y\d+";
                    xdlCode = Regex.Replace(xdlCode, oldPlacement, newPlacement);
                }
                else if (IdentifierManager.Instance.IsMatch(tileSliceTupel.Item1.Location, IdentifierManager.RegexTypes.DSP))
                {
                    int underScoreLocation = tileSliceTupel.Item2.Location.IndexOf("_");
                    string locationPrefix = tileSliceTupel.Item2.Location.Substring(0, underScoreLocation);

                    string newPlacement = "placed " + tileSliceTupel.Item2.Location + " " + slice.SliceName;
                    string oldPlacement = "placed.*" + locationPrefix + @"_X\d+Y\d+.*" + tileSliceTupel.Item1.SliceName;

                    xdlCode = Regex.Replace(xdlCode, oldPlacement, newPlacement);
                }
                else if (IdentifierManager.Instance.IsMatch(tileSliceTupel.Item1.Location, IdentifierManager.RegexTypes.BRAM))
                {
                    int underScoreLocation = tileSliceTupel.Item2.Location.IndexOf("_");
                    string locationPrefix = tileSliceTupel.Item2.Location.Substring(0, underScoreLocation);

                    string newPlacement = "placed " + tileSliceTupel.Item2.Location + " " + slice.SliceName;
                    string oldPlacement = "placed.*" + locationPrefix + @"_X\d+Y\d+.*" + tileSliceTupel.Item1.SliceName;
                    //String oldPlacement = @"placed.*RAMB16_X\d+Y\d+";
                    xdlCode = Regex.Replace(xdlCode, oldPlacement, newPlacement);
                }

                if (InsertPrefix)
                {
                    // insert instance prefix
                    // remove greedy between double quotes
                    MatchCollection matches = Regex.Matches(xdlCode, "(\".*?\")");
                    string oldInstanceName = matches[0].ToString();
                    oldInstanceName = Regex.Replace(oldInstanceName, "\"", "");
                    string newInstanceName = InstanceName + Regex.Replace(oldInstanceName, "\"", "");

                    // replace both the instance name AND slice configuration
                    xdlCode = xdlCode.Replace("\"" + oldInstanceName, "\"" + newInstanceName);

                    if (IdentifierManager.Instance.IsMatch(tileSliceTupel.Item1.Location, IdentifierManager.RegexTypes.BRAM))
                    {
                        xdlCode = xdlCode.Replace("RAMB16BWER:" + oldInstanceName, "RAMB16BWER:" + newInstanceName);
                    }

                    // XDL Shape??
                    xdlCode = xdlCode.Replace("Shape_", InstanceName + "Shape_");
                }

                netlistContainer.AddSliceCodeBlock(xdlCode);
            }
        }

        private void RelocateNetsForXDL(LibraryElement libElement, Tile anchorCLB, XDLContainer netlistContainer)
        {
            foreach (XDLNet net in libElement.Containter.Nets)
            {
                // insert instance prefix
                XDLNet relocatedNet = new XDLNet(InstanceName + net.Name);
                relocatedNet.HeaderExtension = net.HeaderExtension;

                foreach (NetPin pin in net.NetPins)
                {
                    NetPin copy = NetPin.Copy(pin);
                    if (InsertPrefix)
                    {
                        // insert instance prefix
                        // remove greedy between double quotes
                        string oldInstanceName = pin.InstanceName;
                        string newInstanceName = "\"" + InstanceName + Regex.Replace(oldInstanceName, "\"", "") + "\"";
                        //xdlCode = Regex.Replace(xdlCode, oldInstanceName, newInstanceName);
                        copy.InstanceName = newInstanceName;
                        copy.InstanceName = copy.InstanceName.Replace("\"", "");
                    }
                    relocatedNet.Add(copy);
                }

                //foreach (NetSegment seg in originalNet.GetAllSegments())
                foreach (XDLPip pip in net.Pips)
                {
                    string targetLocation;
                    bool success = libElement.GetTargetLocation(pip.Location, anchorCLB, out targetLocation);

                    Tile targetTile = null;
                    if (FPGA.FPGA.Instance.Contains(targetLocation))
                    {
                        targetTile = FPGA.FPGA.Instance.GetTile(targetLocation);
                    }
                    else
                    {
                        throw new ArgumentException("Error during relocation of pip " + pip + " to " + targetLocation);
                    }

                    XDLPip relocatedSegment = null;
                    if (targetTile.SwitchMatrix.Contains(pip.From, pip.To))
                    {
                        // we do not need to transform identifiers
                        relocatedSegment = new XDLPip(targetTile.Location, pip.From, pip.Operator, pip.To);
                    }
                    else
                    {
                        // naming fun
                        relocatedSegment = FPGATypes.RelocatePip(targetTile, pip, relocatedNet);
                    }

                    if (relocatedSegment == null)
                    {
                        throw new ArgumentException("Could not relocate " + pip.ToString() + " to tile " + targetLocation);
                    }

                    if (!targetTile.SwitchMatrix.Contains(relocatedSegment.From, relocatedSegment.To))
                    {
                        throw new ArgumentException("Could not relocate " + pip.ToString() + " to tile " + targetLocation);
                    }

                    relocatedNet.Add(relocatedSegment);
                }

                if (netlistContainer.Nets.Any(n => n.Name.Equals(relocatedNet.Name)))
                {
                    throw new ArgumentException("A net named " + relocatedNet.Name + " is alredy inserted to netlist " + netlistContainer.Name + ". Did you try to join two instances of the same macro in one?");
                }

                netlistContainer.Add(relocatedNet);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The instance name")]
        public string InstanceName = "inst_BM_S6_L4_R4_double_5";

        [Parameter(Comment = "Whether to insert the InstanceName as a prefix in slices, ports, and nets")]
        public bool InsertPrefix = true;

        [Parameter(Comment = "Wheter to add the Design Config (if any) or not. Only the Design Config encountered will be set. Use AddDesignConfig=true for relocating modules (ISE only)")]
        public bool AddDesignConfig = false;
    }
}
