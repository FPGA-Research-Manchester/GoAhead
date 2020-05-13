using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Search
{
    public class Location  : IComparable
    {
        public Location(Location other)
        {
            this.m_tile = other.Tile;
            this.m_pip = other.Pip;
            if(other.Path!= null)
            {
                this.Path = new List<Location>();
                other.Path.ForEach(l => this.Path.Add(l));
            }
        }

        public Location(Tile tile, Port pip)
        {
            this.m_tile = tile;
            this.m_pip = pip;
        }

        public Tile Tile
        {
            get { return this.m_tile; }
        }

        public Port Pip
        {
            get { return this.m_pip; }
        }

        public virtual int CompareTo(object obj)
        {
            return String.CompareOrdinal(this.ToString(), obj.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
                return false;

            Location other = (Location)obj;

            bool result = other.Pip.Name.Equals(this.Pip.Name) && other.Tile.Location.Equals(this.Tile.Location);
            return result;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return this.Tile.Location + "." + this.Pip.Name;
        }

        public List<Location> Path = null;

        private readonly Tile m_tile;
        private readonly Port m_pip;
    }

    class Navigator
    {
        public static Tile GetNextCLB(Tile current, int xIncrement, int yIncrement)
        {
            return Navigator.GetNextCLB(current, xIncrement, yIncrement, null);
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
                if (IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.CLBRegex))
                {
                    return target;
                }
            }
        }


        public static bool DestinationExists(Tile start, Wire wire)
        {
            TileKey targetKey = Navigator.GetTargetLocation(start, wire);

            return FPGA.FPGA.Instance.Contains(targetKey);
        }

        public static bool DestinationAndWireExists(Tile start, Wire wire)
        {
            TileKey targetKey = Navigator.GetTargetLocation(start, wire);

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
        public static IEnumerable<Location> GetDestinations(String startLocation, String portName)
        {
            return Navigator.GetDestinations(startLocation, portName, true);
        }
        
        public static IEnumerable<Location> GetDestinations(String startLocation, String portName, bool forward)
        {
            Tile t = FPGA.FPGA.Instance.GetTile(startLocation);
            return Navigator.GetDestinations(t, new Port(portName), forward);
        }

        public static IEnumerable<Location> GetDestinations(Tile start, Port port)
        {
            return Navigator.GetDestinations(start, port, true);
        }

        public static IEnumerable<Location> GetDestinations(Tile start, Port port, bool forward)
        {
            //foreach (Wire wire in start.WireList.GetAllWires().Where(w => (forward ? w.LocalPipIsDriver : !w.LocalPipIsDriver) && w.LocalPip.Equals(port.Name)))
            foreach (Wire wire in start.WireList.GetAllWires(port).Where(w => (forward ? w.LocalPipIsDriver : !w.LocalPipIsDriver)))
            {
                TileKey targetKey = Navigator.GetTargetLocation(start, wire);
                if(FPGA.FPGA.Instance.Contains(targetKey))
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
              
        public static Tile GetDestinationByWire(Tile start, Wire wire)
        {
            TileKey targetKey = Navigator.GetTargetLocation(start, wire);

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

        private static TileKey GetTargetLocation(Tile start, Wire wire)
        {
            TileKey targetKey = new TileKey(start.TileKey.X + wire.XIncr, start.TileKey.Y + wire.YIncr);
            return targetKey;
            /*
            String locationPrefix = Regex.Split(start.Location, @"_X\d+Y\d+")[0];
            String targetLocationString = locationPrefix + "_X" + (start.LocationX + wire.XIncr) + "Y" + (start.LocationY + wire.YIncr);
            return targetLocationString;
            */
        }
    }

	class V5Navigator                                                                   
	{
        public static IEnumerable<Location> GetDestination(Tile where, Port exit)
        {
            //SE5BEG2
            //SL2BEG1
            //SL5BEG2

            String portName = exit.ToString();

            if (Regex.IsMatch(portName, "(E|N|S|W)(E|N|S|W|L|R)(2|5)BEG") && !Regex.IsMatch(portName, "_"))
            {

                int distance = int.Parse(portName.Substring(2, 1));
                //int index = int.Parse(portName.Substring(6, portName.Length - 6));

                //mid
                FPGATypes.Direction dir1 = FPGA.FPGATypes.GetDirectionFromString(portName.Substring(0, 1));

                int startX = where.LocationX;
                int startY = where.LocationY;

                int midX;
                int midY;

                //double lines
                if (dir1.Equals(FPGATypes.Direction.East) && distance == 2)
                {
                    midX = startX + 1;
                    midY = startY;
                }
                else if (dir1.Equals(FPGATypes.Direction.North) && distance == 2)
                {
                    midX = startX;
                    midY = startY + 1;
                }
                else if (dir1.Equals(FPGATypes.Direction.South) && distance == 2)
                {
                    midX = startX;
                    midY = startY - 1;
                }
                else if (dir1.Equals(FPGATypes.Direction.West) && distance == 2)
                {
                    midX = startX - 1;
                    midY = startY;
                }
                //pent lines
                else if (dir1.Equals(FPGATypes.Direction.East) && distance == 5)
                {
                    midX = startX + 3;
                    midY = startY;
                }
                else if (dir1.Equals(FPGATypes.Direction.North) && distance == 5)
                {
                    midX = startX;
                    midY = startY + 3;
                }
                else if (dir1.Equals(FPGATypes.Direction.South) && distance == 5)
                {
                    midX = startX;
                    midY = startY - 3;
                }
                else if (dir1.Equals(FPGATypes.Direction.West) && distance == 5)
                {
                    midX = startX - 3;
                    midY = startY;
                }
                else
                {
                    throw new ArgumentException("Can not handle port " + exit);
                }

                String locationFirstPart = Regex.Split(where.Location, "_")[0];
                String midTargetLocation = locationFirstPart + "_X" + midX.ToString() + "Y" + midY.ToString();

                if (FPGA.FPGA.Instance.Contains(midTargetLocation))
                {
                    Tile midTarget = FPGA.FPGA.Instance.GetTile(midTargetLocation);
                    Port midEnter = new Port(Regex.Replace(portName, "BEG", "MID"));

                    //return mid
                    yield return new Location(midTarget, midEnter);
                }

                int endX;
                int endY;

                FPGATypes.Direction dir2;
                String dir2String = portName.Substring(1, 1);
                if (Regex.IsMatch(dir2String, "(R|L)"))
                {
                    dir2 = dir1;
                }
                else
                {
                    dir2 = FPGA.FPGATypes.GetDirectionFromString(dir2String);
                }

                //double lines
                if (dir2.Equals(FPGATypes.Direction.East) && distance == 2)
                {
                    endX = midX + 1;
                    endY = midY;
                }
                else if (dir2.Equals(FPGATypes.Direction.North) && distance == 2)
                {
                    endX = midX;
                    endY = midY + 1;
                }
                else if (dir2.Equals(FPGATypes.Direction.South) && distance == 2)
                {
                    endX = midX;
                    endY = midY - 1;
                }
                else if (dir2.Equals(FPGATypes.Direction.West) && distance == 2)
                {
                    endX = midX - 1;
                    endY = midY;
                }
                //pent lines
                else if (dir2.Equals(FPGATypes.Direction.East) && distance == 5)
                {
                    endX = midX + 2;
                    endY = midY;
                }
                else if (dir2.Equals(FPGATypes.Direction.North) && distance == 5)
                {
                    endX = midX;
                    endY = midY + 2;
                }
                else if (dir2.Equals(FPGATypes.Direction.South) && distance == 5)
                {
                    endX = midX;
                    endY = midY - 2;
                }
                else if (dir2.Equals(FPGATypes.Direction.West) && distance == 5)
                {
                    endX = midX - 2;
                    endY = midY;
                }
                else
                {
                    throw new ArgumentException("Can not handle port " + exit);
                }

                String endTargetLocation = locationFirstPart + "_X" + endX.ToString() + "Y" + endY.ToString();

                if (FPGA.FPGA.Instance.Contains(endTargetLocation))
                {
                    Tile endTarget = FPGA.FPGA.Instance.GetTile(endTargetLocation);
                    Port endEnter = new Port(Regex.Replace(portName, "BEG", "END"));

                    //return mid
                    yield return new Location(endTarget, endEnter);
                }
            }
            else if (Regex.IsMatch(portName, "^L(V|H)(0|18)$"))
            {
                /*
                foreach (Tuple<String, String> nextWire in where.GetAllConnectedWires(portName))
                {
                    if (Regex.IsMatch(nextWire.Value, "^L(V|H)(0|6|12|18)"))
                    {
                        if (FPGA.FPGA.Instance.Contains(nextWire.Key))
                        {
                            Tile targetTile = FPGA.FPGA.Instance.GetTile(nextWire.Key);
                            Port targetPort = new Port(nextWire.Value);
                            yield return new Location(targetTile, targetPort);
                        }
                        else
                        {
                        }

                    }
                }
                 * */
            }
        }


        public static TileKey GetDestination(Tile start, FPGATypes.Direction direction, int stepWidth)
		{
            TileKey targetKey;
            if (direction.Equals(FPGATypes.Direction.North))
			{
				targetKey = new TileKey(start.TileKey.X, start.TileKey.Y - stepWidth);
                return targetKey;
				//return FPGA.FPGA.Instance.GetTile(targetKey);
			}
            else if (direction.Equals(FPGATypes.Direction.South))
			{
				targetKey = new TileKey(start.TileKey.X, start.TileKey.Y + stepWidth);
                return targetKey;
				//return FPGA.FPGA.Instance.GetTile(targetKey);
			}
            else if (direction.Equals(FPGATypes.Direction.East))
			{
				targetKey = new TileKey(start.TileKey.X + stepWidth, start.TileKey.Y);
                return targetKey;
                //return FPGA.FPGA.Instance.GetTile(targetKey);
            }
            else if (direction.Equals(FPGATypes.Direction.West))
			{
				targetKey = new TileKey(start.TileKey.X - stepWidth, start.TileKey.Y);
                return targetKey;
                //return FPGA.FPGA.Instance.GetTile(targetKey);
            }
			else
			{
				throw new Exception("Unknown direction: " + direction);
			}
		}

    }
}


