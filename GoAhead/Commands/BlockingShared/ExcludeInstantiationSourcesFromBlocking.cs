using System;
using System.Linq;
using System.Collections.Generic;
using GoAhead.Code;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.BlockingShared
{
    public class ExcludeInstantiationSourcesFromBlocking : Command
    {
        protected override void DoCommandAction()
        {
            LibraryElement libElement = Objects.Library.Instance.GetElement(LibraryElementName);

            if (!FPGA.FPGA.Instance.Contains(AnchorLocation))
            {
                throw new ArgumentException("FPGA does not contain location " + AnchorLocation);
            }

            Tile anchor = FPGA.FPGA.Instance.GetTile(AnchorLocation);
            foreach (Tuple<Tile, List<Port>> t in libElement.GetPortsToBlock(anchor))
            {
                t.Item2.ForEach(p => t.Item1.BlockPort(p, Tile.BlockReason.OccupiedByMacro));
            }

            // for ISE we could instantly mark the slices as Macro, but for Vivado we visit the same slice multiple times
            // store the to be marked slice in here and set their usage to Macro at the END of the command
            List<Slice> targets = new List<Slice>();

            // attach usage to slice
            foreach (Tuple<Instance, Tile> t in libElement.GetInstanceTiles(anchor, libElement))
            {
                int sliceNumber = t.Item1.SliceNumber;
                Slice target = t.Item2.Slices[(int)sliceNumber];               
                switch (FPGA.FPGA.Instance.BackendType)
	            {
		            case FPGATypes.BackendType.ISE:
                        if (target.Usage != FPGATypes.SliceUsage.Free && CheckResources)
                        {
                            throw new ArgumentException("Can not place library element at " + target + " as this slice is already used");
                            // TODO warum geht das beim BRAM nicht
                        }
                        targets.Add(target);                        
                        break;
                    case FPGATypes.BackendType.Vivado:
                        // in Vivado we set usage at the bel level, however, the slice shoule be free
                        if (target.Usage != FPGATypes.SliceUsage.Free && CheckResources)
                        {
                            throw new ArgumentException("Can not place library element at " + target + " as this slice is already used");
                        }

                        if (target.SliceName.Equals("SLICE_X33Y88"))
                        {

                        }

                        // can be NULL in modules, should not be NULL in connection primitives
                        if (libElement.BEL != null)
                        {
                            target.SetBelUsage(libElement.BEL, FPGATypes.SliceUsage.Macro);
                        }
                        foreach (LibraryElement libEl in libElement.SubElements.Where(l => l.BEL != null))
                        {
                            target.SetBelUsage(libElement.BEL, FPGATypes.SliceUsage.Macro);
                        }
                        targets.Add(target);                      
                        break;
	            }

            }
            foreach (Slice t in targets)
            {
                t.Usage = FPGATypes.SliceUsage.Macro;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the library element to use")]
        public string LibraryElementName = "libElement";

        [Parameter(Comment = "The location string of the tile in which the macro anchor resided, e.g CLExL_X4Y43")]
        public string AnchorLocation = "CLEXL_X4Y43";

        [Parameter(Comment = "Whether or not to check that the whole slice is free (rather ISE only")]
        public bool CheckResources = true;
    }
}