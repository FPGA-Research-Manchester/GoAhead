using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.FPGA;

namespace GoAhead.Code.XDL.ResourceDescription
{
    public class XDLTileParser
    {
        public void ParseTile(string line, XDLStreamReaderWithUndo sr)
        {
            int yPos;
            int xPos;
            string location;
            GetTileHeaderData(line, out yPos, out xPos, out location);

            TileKey key = new TileKey(xPos, yPos);
            Tile tile = new Tile(key, location);

            SwitchMatrix switchMatrix = new SwitchMatrix();
            //add tile to FPGA
            FPGA.FPGA.Instance.Add(tile);

            //read until end of tile
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("pip"))
                {
                    // \t\t(pip TIOB_X1Y63 TIOB_DIFFO_OUT0 -> TIOB_DIFFO_IN1)
                    // scan for the first bracket in case \t is replaced by spaces!
                    int firstBracket = line.IndexOf('(', 0);
                    int firstBlank = line.IndexOf(' ', firstBracket);
                    int secondBlank = line.IndexOf(' ', firstBlank + 1);
                    int thirdBlank = line.IndexOf(' ', secondBlank + 1);
                    int fourthBlank = line.IndexOf(' ', thirdBlank + 1);

                    //String tileStr = line.Substring(firstBlank + 1, secondBlank - firstBlank);
                    string fromStr = line.Substring(secondBlank + 1, thirdBlank - secondBlank - 1);
                    string pipOperator = line.Substring(thirdBlank + 1, fourthBlank - thirdBlank - 1);
                    string toStr = line.Substring(fourthBlank + 1, line.Length - fourthBlank - 2);

                    if (toStr.Contains("(_ROUTE"))
                    {
                        toStr = toStr.Replace("(_ROUTE", "# (_ROUTE");
                    }

                    Port fromPort = new Port(fromStr);
                    Port toPort = new Port(toStr);

                    // there are unidirectional and bidirectional wires
                    // save string comparison:
                    // if (pipAtoms[4].Equals("->"))
                    // else if (pipAtoms[4].Equals("=-"))

                    switchMatrix.Add(fromPort, toPort);
                    if (pipOperator.Equals("=-"))
                    {
                        switchMatrix.Add(toPort, fromPort);
                    }
                }
                else if (line.Contains("conn"))
                {
                }
                else if (line.Contains("wire"))
                {
                }
                else if (line.Contains("primitive_site"))
                {
                    InPortOutPortMapping nextPortMapping = new InPortOutPortMapping();
                    Slice nextSlice = XDLSliceParser.Parse(tile, nextPortMapping, line, sr);

                    // share in out port mappings
                    if (!FPGA.FPGA.Instance.Contains(nextPortMapping))
                    {
                        FPGA.FPGA.Instance.Add(nextPortMapping);
                    }
                    int hashCode = nextPortMapping.GetHashCode();
                    nextSlice.InPortOutPortMappingHashCode = hashCode;
                }
                else if (line.Contains("tile_summary"))
                {
                    StoreAndShareSwitchMatrix(tile, switchMatrix);

                    //consume closing bracket and exit
                    line = sr.ReadLine();
                    return;
                }
            }
        }

        public static void StoreAndShareSwitchMatrix(Tile tile, SwitchMatrix switchMatrix)
        {
            // string compare the current SM with the previous read in
            bool equalSmFound = false;
            foreach (SwitchMatrix sm in FPGA.FPGA.Instance.GetAllSwitchMatrices().Where(s => s.ArcCount == switchMatrix.ArcCount))
            {
                if (sm.Equals(switchMatrix))
                {
                    switchMatrix.HashCode = sm.HashCode;
                    equalSmFound = true;
                    break;
                }
            }
            if (!equalSmFound)
            {
                switchMatrix.HashCode = FPGA.FPGA.Instance.SwitchMatrixCount;
            }

            //reached end of tile -> share common switch matrices
            if (!FPGA.FPGA.Instance.Contains(switchMatrix))
            {
                FPGA.FPGA.Instance.Add(switchMatrix.HashCode, switchMatrix);
            }

            tile.SwitchMatrixHashCode = switchMatrix.HashCode;
        }

        public static void GetTileHeaderData(string line, out int yPos, out int xPos, out string location)
        {
            //first line of tile like: (tile 0 0 TL UL 2
            string[] atoms = line.Split(' ', '(');

            if (atoms.Length != 7)
                throw new ArgumentException("expecting 7 element in line " + line);

            yPos = int.Parse(atoms[2]);
            xPos = int.Parse(atoms[3]);
            location = atoms[4];
        }

        /// <summary>
        /// The FPGA is already read in, this time only read wire statements
        /// </summary>
        /// <param name="line"></param>
        /// <param name="sr"></param>
        public void ParseWire(Tile tile, XDLStreamReaderWithUndo sr, UnresolvedWires unresWires)
        {
            WireList wireList = new WireList();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("(wire"))
                {
                    wireList = XDLWireParser.Parse(tile, sr, unresWires);
                }
                else if (line.Contains("tile_summary") && unresWires == null)
                {
                    StoreAndShareWireList(tile, wireList);

                    //consume closing bracket and exit
                    line = sr.ReadLine();

                    return;
                }
                else if (line.Contains("tile_summary") && unresWires != null)
                {
                    //consume closing bracket and exit
                    line = sr.ReadLine();

                    return;
                }
            }
        }

        public static void StoreAndShareWireList(Tile tile, WireList wireList)
        {
            // reached end of wire statements ->
            // clean up after switch matrix is read in
            // no need to remove wires, as the filtering takes place during wire parsing
            //wires.RemoveNonExistingWires(tile);

            bool equalWLFound = false;

            // try finding an equal wire list
            foreach (WireList other in FPGA.FPGA.Instance.GetAllWireLists().Where(wl => wl.GetHashCode() == wireList.GetHashCode()))
            {
                if (other.Equals(wireList))
                {
                    wireList.Key = other.Key;
                    equalWLFound = true;
                    break;
                }
            }

            if (!equalWLFound)
            {
                foreach (WireList other in FPGA.FPGA.Instance.GetAllWireLists().Where(wl => wl.Count == wireList.Count))
                {
                    if (other.Equals(wireList))
                    {
                        wireList.Key = other.Key;
                        equalWLFound = true;
                        break;
                    }
                }
            }
            if (!equalWLFound)
            {
                wireList.Key = FPGA.FPGA.Instance.WireListCount;
            }

            // promote wire list key to tile
            tile.WireListHashCode = wireList.Key;

            // now share common wire list
            if (!FPGA.FPGA.Instance.ContainsWireList(tile.WireListHashCode))
            {
                FPGA.FPGA.Instance.Add(tile.WireListHashCode, wireList);
            }
            else
            {
            }
        }
    }
}