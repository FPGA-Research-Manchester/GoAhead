using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GoAhead.Code;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Commands.VHDL;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    /// <summary>
    /// Singleton
    /// </summary>
    [Serializable]
    public class Library : Interfaces.IResetable
    {
        private Library()
        {
            Commands.Debug.PrintGoAheadInternals.ObjectsToPrint.Add(this);
        }

        public void Reset()
        {
            m_macros.Clear();
        }

        public static Library Instance = new Library();

        public LibraryElement GetElement(string elementName)
        {
            if (!Contains(elementName))
            {
                throw new ArgumentException("No libray element named " + elementName + " is part of the library");
            }

            return m_macros.First(macro => macro.Name.Equals(elementName));
        }

        public bool Contains(string elementName)
        {
            return m_macros.FirstOrDefault(macro => macro.Name.Equals(elementName)) != null;
        }

        public void Add(LibraryElement element)
        {
            if (Contains(element.Name))
            {
                throw new ArgumentException("A macro named " + element.Name + " is already part of the library");
            }

            m_macros.Add(element);
        }

        public void Remove(string elementName)
        {
            if (!Contains(elementName))
            {
                throw new ArgumentException("No macro named " + elementName + " is part of the library");
            }

            m_macros.Remove(m_macros.First(macro => macro.Name.Equals(elementName)));
        }

        public void Clear()
        {
            m_macros.Clear();
        }

        public IEnumerable<LibraryElement> GetAllElements()
        {
            foreach (LibraryElement el in m_macros)
            {
                yield return el;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (m_macros.Count == 0)
            {
                result.AppendLine("No library elements added");
            }
            else
            {
                result.AppendLine("Added library elements are");
                foreach (LibraryElement libElement in GetAllElements())
                {
                    result.AppendLine(libElement.ToString());
                }
            }
            return result.ToString();
        }

        public BindingList<LibraryElement> LibraryElements
        {
            get { return m_macros; }
        }

        private BindingList<LibraryElement> m_macros = new BindingList<LibraryElement>();
    }

    [Serializable]
    public class LibraryElement
    {
        public void ClearPortsToBlock()
        {
            m_portsToBlock.Clear();
        }

        public void Add(LibraryElement other)
        {
            m_others.Add(other);
        }

        public void AddPortToBlock(Tile where, Port port)
        {
            if (!m_portsToBlock.ContainsKey(where))
            {
                m_portsToBlock.Add(where, new List<string>());
            }

            // do not add duplicates
            if (!m_portsToBlock[where].Contains(port.Name))
            {
                m_portsToBlock[where].Add(port.Name);
            }
        }

        public bool IsRelocationPossible(Tile anchor, out StringBuilder errorList)
        {
            errorList = new StringBuilder();

            foreach (Instance inst in Containter.Instances)
            {
                string targetLocation = "";
                bool success = GetTargetLocation(inst.Location, anchor, out targetLocation);

                if (!success)
                {
                    errorList.Append("GoAhead failed to relocate " + inst.Location + " using anchor " + anchor.Location);
                    // debug only
                    success = GetTargetLocation(inst.Location, anchor, out targetLocation);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Return tupel of an XDLInstance and the tile where the instance will be placed based on the given placementAnchor
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<Instance, Tile>> GetInstanceTiles(Tile anchor, LibraryElement libElement)
        {
            if(FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.ISE || (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado && !libElement.VivadoConnectionPrimitive))
            {
                foreach (Instance inst in Containter.Instances)
                {
                    string targetLocation = "";
                    bool success = GetTargetLocation(inst.Location, anchor, out targetLocation);

                    if (success)
                    {
                        Tile targetTile = FPGA.FPGA.Instance.GetTile(targetLocation);
                        yield return new Tuple<Instance, Tile>(inst, targetTile);
                    }
                    else
                    {
                        throw new ArgumentException("Could not relocate " + inst.Location + " from anchor " + anchor);
                    }
                }
            } 
            else if(FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado && libElement.VivadoConnectionPrimitive)
            {
                TCLInstance inst = new TCLInstance();
                inst.SliceNumber = SliceNumber;
                inst.SliceName = anchor.Slices[SliceNumber].SliceName;
                inst.Location = anchor.Location;
                inst.LocationX = anchor.LocationX;
                inst.LocationY = anchor.LocationY;
                inst.SliceType = anchor.Slices[SliceNumber].SliceType;
                inst.Name = anchor.Location;
                inst.TileKey = anchor.TileKey;
                inst.OmitPlaceCommand = true;
                inst.BELType = libElement.PrimitiveName;
                Tuple<Instance, Tile> result = new Tuple<Instance, Tile>(inst, anchor);
                yield return result;
            }
            else
            {
                throw new ArgumentException("Unsupported branch in GetInstanceTiles");
            }             
        }

        public Shape ResourceShape
        {
            get { return m_resourceShape; }
            set { m_resourceShape = value; }
        }

        /// <summary>
        /// Relocate the tile given by instanceTileLocation from anchor
        /// instanceTileLocation does not have to be part of the FPGA
        /// </summary>
        /// <param name="instanceTileLocation"></param>
        /// <param name="anchor"></param>
        /// <returns></returns>
        ///
        public bool GetTargetLocation(string instanceTileLocation, Tile anchor, out string targetLocation)
        {
            targetLocation = "";

            int x, y;
            FPGATypes.GetXYFromIdentifier(instanceTileLocation, out x, out y);

            int locationIncrX = x - ResourceShape.Anchor.AnchorLocationX;
            int locationIncrY = y - ResourceShape.Anchor.AnchorLocationY;

            int targetLocationX = anchor.LocationX + locationIncrX;
            int targetLocationY = anchor.LocationY + locationIncrY;


            // three chars should match the type CLB/BRAM/DSP
            if (string.IsNullOrEmpty(instanceTileLocation) || instanceTileLocation.Length < 3)
            {
            }
            string prefix = instanceTileLocation.Substring(0, 3);
            Tile targetTile = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => t.Location.StartsWith(prefix) && t.LocationX == targetLocationX && t.LocationY == targetLocationY);


            //int lastUnderScore = instanceTileLocation.LastIndexOf("_");
            //targetLocation += instanceTileLocation.Substring(0, lastUnderScore);
            //targetLocation += "_X" + targetLocationX + "Y" + targetLocationY;
            // naming fun
            //String resolvedTargetLocation = null;
            //bool success = FPGA.FPGATypes.ResolveLMIdentifier(targetLocation, str => !FPGA.FPGA.Instance.Contains(str), out resolvedTargetLocation);

            if (targetTile != null)
            {
                targetLocation = targetTile.Location;
                return true;
            }
            else
            {
                // relocated DSP INT_X3Y5 -> INT_BRAM_X3Y5 or INT_BRAM_BRK_X3Y5
                // move to init.goa
                Tile differentlyNamedTile = FPGA.FPGA.Instance.GetAllTiles().Where(t =>
                    ((t.Location.StartsWith("BRAM_") && FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex6)) ||
                     (t.Location.StartsWith("INT_BRAM_") && FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Spartan6))) &&
                    t.LocationX == targetLocationX &&
                    t.LocationY == targetLocationY).FirstOrDefault();
                if (differentlyNamedTile != null)
                {
                    targetLocation = differentlyNamedTile.Location;
                    return true;
                }
                else
                {
                    targetLocation = "";
                    return false;
                }
                //throw new ArgumentException("Error during relocation: Can not relocate " + instanceTileLocation + " using anchor " + anchor.Location + ". Target " + targetLocation + " not found");
            }
        }
        /*
        /// <summary>
        /// Return tiles along with the ports to block on their original positions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<Tile, List<String>>> GetPortsToBlock()
        {
            foreach (Tile macroTile in this.m_portsToBlock.Keys)
            {
                yield return new Tuple<Tile, List<String>>(macroTile, this.m_portsToBlock[macroTile]);
            }
        }
        */
        /// <summary>
        /// Return tiles along with the ports to block in the relocated positions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<Tile, List<FPGA.Port>>> GetPortsToBlock(Tile anchor)
        {
            if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.ISE)
            {
                foreach (Tile tileAtOriginalPosition in m_portsToBlock.Keys)
                {
                    string targetLocation = "";
                    bool success = GetTargetLocation(tileAtOriginalPosition.Location, anchor, out targetLocation);

                    if (success)
                    {
                        Tile targetTile = FPGA.FPGA.Instance.GetTile(targetLocation);

                        // adapt port names after relocation
                        List<Port> portsToBlock = new List<Port>();
                        foreach (string p in m_portsToBlock[tileAtOriginalPosition])
                        {
                            string resolvedPortName = p;
                            bool relocationSuccess = FPGATypes.ResolveLMIdentifier(resolvedPortName, str => !targetTile.SwitchMatrix.Contains(str), out resolvedPortName);
                            if (!relocationSuccess)
                            {
                                throw new ArgumentException("Error during relocation: Can not relocate " + resolvedPortName + " to its new position from anchor " + anchor.Location);
                            }
                            portsToBlock.Add(new Port(resolvedPortName));
                        }
                        yield return new Tuple<Tile, List<Port>>(targetTile, portsToBlock);
                    }
                    else
                    {
                        success = GetTargetLocation(tileAtOriginalPosition.Location, anchor, out targetLocation);
                        throw new ArgumentException("Error during relocation: Can not relocate " + tileAtOriginalPosition.Location + " to its new position from anchor " + anchor.Location);
                    }
                }
            }
            else if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
            {
                if (!IdentifierManager.Instance.IsMatch(anchor.Location, IdentifierManager.RegexTypes.CLB))
                {
                    throw new ArgumentException("Error during relocation: Can only place on CLBs");
                }
                Tile interconnect = FPGATypes.GetInterconnectTile(anchor);

                Tile clbKey = m_portsToBlock.Keys.FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
                Tile intKey = m_portsToBlock.Keys.FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect));
                if (clbKey != null)
                {
                    Tuple<Tile, List<Port>> clbResult = new Tuple<Tile, List<Port>>(anchor, new List<Port>());

                    m_portsToBlock[clbKey].ForEach(s => clbResult.Item2.Add(new Port(s)));

                    yield return clbResult;
                }
                if (intKey != null)
                {
                    Tuple<Tile, List<Port>> intResult = new Tuple<Tile, List<Port>>(interconnect, new List<Port>());

                    m_portsToBlock[intKey].ForEach(s => intResult.Item2.Add(new Port(s)));

                    yield return intResult;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("Name: " + Name);

            try
            {
                buffer.AppendLine(ResourceShape != null ? "ResourceShape: " + ResourceShape.ToString() : "No resource shape provided");
            }
            catch (NullReferenceException)
            {
                buffer.AppendLine("Warning: This binary libray element seems to be out of date. Regenerate it by reading in the XDL version with the command AddXDLLibraryElement");
            }

            PrintComponentDeclaration printCmd = new PrintComponentDeclaration();
            printCmd.LibraryElement = Name;
            printCmd.Do();
            buffer.AppendLine(printCmd.OutputManager.GetVHDLOuput());

            return buffer.ToString();
        }

        /// <summary>
        /// The unique nameof the library element
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the instantiated primitive
        /// </summary>
        public string PrimitiveName { get; set; }

        /// <summary>
        /// The Command with which this library element was added
        /// </summary>
        public string LoadCommand { get; set; }

        /// <summary>
        /// The VHDL generic map inserted at instantiation
        /// </summary>
        public string VHDLGenericMap { get; set; }

        /// <summary>
        /// The slice element (Vivado only)
        /// </summary>
        public string BEL { get; set; }

        /// <summary>
        /// The slice number (Vivado only)
        /// </summary>
        public int SliceNumber { get; set; }

        /// <summary>
        /// Whether or not this library element is a vivado connection primitive (Vivado only)
        /// </summary>
        public bool VivadoConnectionPrimitive { get; set; }

        /// <summary>
        /// The parsed in XDL Module/Design
        /// </summary>
        public NetlistContainer Containter { get; set; }

        public IEnumerable<LibraryElement> SubElements
        {
            get { return m_others; }
        }

        private Shape m_resourceShape = new Shape();

        private Dictionary<Tile, List<string>> m_portsToBlock = new Dictionary<Tile, List<string>>();
        private List<LibraryElement> m_others = new List<LibraryElement>();
    }

    [Serializable]
    public class Shape
    {
        public void Add(string identifier)
        {
            m_containedTileIdentifier.Add(identifier);
        }

        public IEnumerable<string> GetContainedTileIdentifier()
        {
            return m_containedTileIdentifier;
        }

        /// <summary>
        /// The number of tiles in the module shape
        /// </summary>
        public int NumberOfTiles
        {
            get { return m_containedTileIdentifier.Count; }
        }

        public Anchor Anchor
        {
            get { return m_anchor; }
            set { m_anchor = value; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("ResourceShape");
            foreach (string s in GetContainedTileIdentifier())
            {
                buffer.AppendLine(s);
            }
            buffer.AppendLine("Anchor");
            buffer.AppendLine(m_anchor.ToString());

            return buffer.ToString();
        }

        private Anchor m_anchor = new Anchor();

        private List<string> m_containedTileIdentifier = new List<string>();
    }

    [Serializable]
    public class Anchor
    {
        public string AnchorSliceName { get; set; }
        public int AnchorSliceNumber { get; set; }
        public int AnchorLocationX { get; set; }
        public int AnchorLocationY { get; set; }
        public string AnchorTileLocation { get; set; }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("AnchorSliceName: " + AnchorSliceName);
            buffer.AppendLine("AnchorSliceNumber: " + AnchorSliceNumber);
            buffer.AppendLine("AnchorTileLocation: " + AnchorTileLocation);
            buffer.AppendLine("AnchorLocationX: " + AnchorLocationX);
            buffer.AppendLine("AnchorLocationY: " + AnchorLocationY);
            return buffer.ToString();
        }
    }
}