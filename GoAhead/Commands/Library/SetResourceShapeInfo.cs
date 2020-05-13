using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Set the resource shape info for the given libray element")]
    class SetResourceShapeInfo : Command
    {
        protected override void DoCommandAction()
        {
            if (!Objects.Library.Instance.Contains(LibraryElementName))
            {
                throw new ArgumentException("A library element named " + LibraryElementName + " was not found");
            }

            if (TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                throw new ArgumentException("No tiles are selected, can not derive module shape");
            }

            LibraryElement libElement = Objects.Library.Instance.GetElement(LibraryElementName);

            // enrich library element with selection command for auto clear            
            libElement.ResourceShape = new Shape();

            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                libElement.ResourceShape.Add(t.Location);
            }

            List<Tile> possibleAnchors = new List<Tile>();
            foreach (IdentifierManager.RegexTypes anchorType in new IdentifierManager.RegexTypes[] { IdentifierManager.RegexTypes.CLB, IdentifierManager.RegexTypes.DSP, IdentifierManager.RegexTypes.BRAM})
            {
                Tile upperLeftTile = TileSelectionManager.Instance.GetSelectedTile(IdentifierManager.Instance.GetRegex(anchorType), FPGATypes.Placement.UpperLeft);
                if(upperLeftTile != null)
                {
                    possibleAnchors.Add(upperLeftTile);
                }
            }

            if (possibleAnchors.Count == 0)
            {
                throw new ArgumentException("No upper left tile of either type CLB, DSP or BRAM found. Can not derive any anchor.");
            }

            Tile anchor = possibleAnchors.OrderBy(t => t.TileKey.X).First();
            libElement.ResourceShape.Anchor.AnchorTileLocation = anchor.Location;
            libElement.ResourceShape.Anchor.AnchorLocationX = anchor.LocationX;
            libElement.ResourceShape.Anchor.AnchorLocationY = anchor.LocationY;
            libElement.ResourceShape.Anchor.AnchorSliceNumber = 0;
            libElement.ResourceShape.Anchor.AnchorSliceName = anchor.Slices[0].SliceName;

            /*
             * Tile upperLeftCLB = TileSelectionManager.Instance.GetSelectedTile(IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.CLBRegex), FPGATypes.Placement.UpperLeft);
            libElement.ResourceShape.AnchorTileLocation = upperLeftCLB.Location;
            libElement.ResourceShape.AnchorLocationX = upperLeftCLB.LocationX;
            libElement.ResourceShape.AnchorLocationY = upperLeftCLB.LocationY;
            libElement.ResourceShape.AnchorSliceNumber = 0;
            libElement.ResourceShape.AnchorSliceName = upperLeftCLB.Slices[0].SliceName;*/
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the library element to modify")]
        public string LibraryElementName = "libelement";
    }
}
