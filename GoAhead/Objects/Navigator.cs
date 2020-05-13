using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    public class Location : IComparable
    {
        public Location(Location other)
        {
            m_tile = other.Tile;
            m_pip = other.Pip;
            if (other.Path != null)
            {
                Path = new List<Location>();
                other.Path.ForEach(l => Path.Add(l));
            }
        }

        public Location(Tile tile, Port pip)
        {
            m_tile = tile;
            m_pip = pip;
        }

        public Tile Tile
        {
            get { return m_tile; }
        }

        public Port Pip
        {
            get { return m_pip; }
        }

        public virtual int CompareTo(object obj)
        {
            return string.CompareOrdinal(ToString(), obj.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
                return false;

            Location other = (Location)obj;

            bool result = other.Pip.Name.Equals(Pip.Name) && other.Tile.Location.Equals(Tile.Location);
            return result;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Tile.Location + "." + Pip.Name;
        }

        public List<Location> Path = null;

        private readonly Tile m_tile;
        private readonly Port m_pip;
    }

    public class Navigator
    {
        public static Tile GetNextCLB(Tile current, int xIncrement, int yIncrement)
        {
            return GetNextCLB(current, xIncrement, yIncrement, null);
        }

        public static Tile GetNextCLB(Tile current, int xIncrement, int yIncrement, List<Tile> traversedTile)
        {
            int currentX = current.TileKey.X;
            int currentY = current.TileKey.Y;
            while (true)
            {
                // move coordinates to next tile
                currentX += xIncrement;
                currentY += yIncrement;
                // check for match
                Tile target = FPGA.FPGA.Instance.GetTile(currentX, currentY);
                if (traversedTile != null)
                {
                    traversedTile.Add(target);
                }
                if (IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.CLB))
                {
                    return target;
                }
            }
        }

        public static bool DestinationExists(Tile start, Wire wire)
        {
            TileKey targetKey = GetTargetLocation(start, wire);

            return FPGA.FPGA.Instance.Contains(targetKey);
        }

        public static bool DestinationAndWireExists(Tile start, Wire wire)
        {
            TileKey targetKey = GetTargetLocation(start, wire);

            if (!FPGA.FPGA.Instance.Contains(targetKey))
            {
                return false;
            }

            Tile target = FPGA.FPGA.Instance.GetTile(targetKey);
            return target.SwitchMatrix.Contains(wire.PipOnOtherTile);
        }

        /// <summary>
        /// Get destination in forward direction
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public static IEnumerable<Location> GetDestinations(string startLocation, string portName)
        {
            return GetDestinations(startLocation, portName, true);
        }

        public static IEnumerable<Location> GetDestinations(Location l)
        {
            return GetDestinations(l.Tile, l.Pip, true);
        }

        public static IEnumerable<Location> GetDestinations(string startLocation, string portName, bool forward)
        {
            Tile t = FPGA.FPGA.Instance.GetTile(startLocation);
            return GetDestinations(t, new Port(portName), forward);
        }

        public static IEnumerable<Location> GetDestinations(Tile start, Port port)
        {
            return GetDestinations(start, port, true);
        }

        public static IEnumerable<Location> GetDestinations(Tile start, Port port, bool forward)
        {
            //foreach (Wire wire in start.WireList.GetAllWires().Where(w => (forward ? w.LocalPipIsDriver : !w.LocalPipIsDriver) && w.LocalPip.Equals(port.Name)))
            foreach (Wire wire in start.WireList.GetAllWires(port).Where(w => (forward ? w.LocalPipIsDriver : !w.LocalPipIsDriver)))
            {
                TileKey targetKey = GetTargetLocation(start, wire);
                if (FPGA.FPGA.Instance.Contains(targetKey))
                {
                    Tile target = FPGA.FPGA.Instance.GetTile(targetKey);
                    Location loc = new Location(target, new Port(wire.PipOnOtherTile));
                    yield return loc;

                    // return stopovers such as NL1B0 -> NL1E_S0 that are not part of the switch matrix but are part of wire lists
                    foreach (Wire reverseWire in target.WireList.GetAllWires(wire.PipOnOtherTile).
                        Where(rw => !rw.PipOnOtherTile.Equals(port.Name) && !rw.LocalPipIsDriver && wire.XIncr + rw.XIncr == 0 && wire.YIncr + rw.YIncr == 0))
                    {
                        Location returnLoc = new Location(start, new Port(reverseWire.PipOnOtherTile));
                        yield return returnLoc;
                    }
                    /*
                    // return stopovers such as NL1B0 -> NL1E_S0 that are not part of the switch matrix but are part of wire lists
                    foreach (Wire rw in target.WireList.GetAllWires())
                    {
                        if (!rw.PipOnOtherTile.Equals(port.Name) && !rw.LocalPipIsDriver && rw.LocalPip.Equals(wire.PipOnOtherTile) && wire.XIncr + rw.XIncr == 0 && wire.YIncr + rw.YIncr == 0)
                        {
                            Location returnLoc = new Location(start, new Port(rw.PipOnOtherTile));
                            yield return returnLoc;
                        }
                    }*/
                }
            }
        }

        public static Tile GetDestinationByWire(Tile start, Wire wire, bool forward = true)
        {
            TileKey targetKey = GetTargetLocation(start, wire, forward);

            if (FPGA.FPGA.Instance.Contains(targetKey))
            {
                Tile target = FPGA.FPGA.Instance.GetTile(targetKey);
                return target;
            }
            else
            {
                return FPGA.FPGA.Instance.GetTile(new TileKey(0, 0));
            }
        }

        private static TileKey GetTargetLocation(Tile start, Wire wire, bool forward = true)
        {
            int x = wire.XIncr;
            int y = wire.YIncr;

            if(!forward)
            {
                x *= -1;
                y *= -1;
            }

            TileKey targetKey = new TileKey(start.TileKey.X + x, start.TileKey.Y + y);
            return targetKey;
        }
    }
}