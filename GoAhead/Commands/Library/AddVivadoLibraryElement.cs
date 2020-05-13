using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.FPGA;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Add a Vivado connection primitive.", Wrapper = false, Publish = true)]
    class AddVivadoLibraryElement : Command
    {
        protected override void DoCommandAction()
        {
            TCLContainer nlc = new TCLContainer("VivadoLibraryElement");
            DesignParser parser = DesignParser.CreateDesignParser(FileName);

            try
            {
                parser.ParseDesign(nlc, this);
            }
            catch (Exception e)
            {
                
                throw new ArgumentException("Error during parsing the design " + FileName + ": " + e.Message + ". Are you trying to open the design on the correct device?");
            }
            
            LibraryElement libElement = new LibraryElement();
            libElement.VivadoConnectionPrimitive = false;
            libElement.PrimitiveName = Path.GetFileNameWithoutExtension(FileName);
            libElement.Containter = nlc;
            libElement.Name = Path.GetFileNameWithoutExtension(FileName);
            libElement.LoadCommand = ToString();

            libElement.ResourceShape = new Shape();

            foreach(Instance inst in nlc.Instances)
            {
                libElement.ResourceShape.Add(inst.Location);
            }

            // X Y coordinates
            int minX = nlc.Instances.Select(i =>i.TileKey.X).Min();
            int maxX = nlc.Instances.Select(i =>i.TileKey.X).Max();
            int minY = nlc.Instances.Select(i =>i.TileKey.Y).Min();
            int maxY = nlc.Instances.Select(i =>i.TileKey.Y).Max();

            // get covering rectangle
            IEnumerable<Tile> rectangle = 
                from tile in FPGA.FPGA.Instance.GetAllTiles()
                where 
                    (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB) || 
                     IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.BRAM) || 
                     IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.DSP)) &&
                    tile.TileKey.X >= minX && tile.TileKey.X <= maxX &&
                    tile.TileKey.Y >= minY && tile.TileKey.Y <= maxY
                select tile;

            // Row Column
            int minRX = rectangle.Select(t => t.LocationX).Min();
            int maxCY = rectangle.Select(t => t.LocationY).Max();


            IEnumerable<Tile> possibleAnchors =
                from tile in rectangle
                where tile.LocationX == minRX && tile.LocationY == maxCY
                select tile;

            Tile anchor = possibleAnchors.OrderBy(t => t.TileKey.X).FirstOrDefault();
            if (anchor == null)
            {
                throw new ArgumentException("No upper left tile of either type CLB, DSP or BRAM found. Can not derive any anchor.");
            }

            libElement.ResourceShape.Anchor.AnchorTileLocation = anchor.Location;
            libElement.ResourceShape.Anchor.AnchorLocationX = anchor.LocationX;
            libElement.ResourceShape.Anchor.AnchorLocationY = anchor.LocationY;
            libElement.ResourceShape.Anchor.AnchorSliceNumber = 0;
            libElement.ResourceShape.Anchor.AnchorSliceName = anchor.Slices[0].SliceName;

            Objects.Library.Instance.Add(libElement);
        
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = "The vivado netlist to read in (*.viv_rpt)")]
        public string FileName = "design.xdl";
    }
}
