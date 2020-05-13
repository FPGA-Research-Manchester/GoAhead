using System;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.XDL.ResourceDescription
{
    public class XDLWireParser
    {
        //private static Regex m_closingBrackes = new Regex(@"^\s+\)", RegexOptions.Compiled);
        //private static Regex m_summary = new Regex("(pip)|(tile_summary)", RegexOptions.Compiled);

        public static WireList Parse(Tile containingTile, XDLStreamReaderWithUndo sr, UnresolvedWires unresWires)
        {
            WireList wires = new WireList();
            sr.UndoLastRead();

            while (true)
            {
                //   		(wire WS5BEG1 3
                //	            (conn INT_BUFS_R_X18Y78 INT_BUFS_WS5B1)
                //	            (conn INT_X18Y78 WS5A1)
                //	            (conn CLBLM_X18Y78 CLB_WS5A1)
                //          )

                string line = sr.ReadLine();
                bool conn = line.Contains("(conn");
                if (!conn)
                {
                    if (line.Contains("pip") || line.Contains("tile_summary"))
                    {
                        sr.UndoLastRead();
                        break;
                    }
                }

                // skip closing brackets
                //if (WireParser.m_closingBrackes.IsMatch(nextLine))
                if (line.EndsWith(")"))
                {
                    continue;
                }

                int firstBracketWire = line.IndexOf('(', 0);
                int firstBlankWire = line.IndexOf(' ', firstBracketWire);
                int secondBlankWire = line.IndexOf(' ', firstBlankWire + 1);

                // e.g. (wire WS5BEG1 3
                // wireName becomes WS5BEG1
                string wireName = line.Substring(firstBlankWire + 1, secondBlankWire - firstBlankWire - 1);
                // wireCount becomes 3
                string wireCountStr = line.Substring(secondBlankWire + 1, line.Length - secondBlankWire - 1);
                int wireCount = int.Parse(wireCountStr);

                // wire classification
                bool beginPip = false;
                bool endPip = false;

                beginPip = containingTile.SwitchMatrix.ContainsRight(new Port(wireName));

                // HasArcs is expensive, only search if the the pip is a not begin pip
                if (!beginPip)
                {
                    endPip = containingTile.SwitchMatrix.ContainsLeft(new Port(wireName));
                }

                for (int i = 0; i < wireCount; i++)
                {
                    line = sr.ReadLine();

                    if (beginPip || endPip)
                    {
                        Tile targetTile;
                        Wire w = GetWire(line, containingTile, wireName, beginPip, false, unresWires, out targetTile);
                        if (w == null)
                        {
                            continue;
                        }

                        bool destinationExists = Navigator.DestinationAndWireExists(containingTile, w);
                        if (destinationExists && unresWires == null)
                        {
                            // regular wire (bipartite case: wire, pip, wire, pip)
                            wires.Add(w);
                        }
                        else if (!destinationExists && unresWires != null)
                        {
                            // store wire for later
                            unresWires.Add(containingTile, w);
                        }
                    }
                    else if (unresWires != null)
                    {
                        Tile targetTile;
                        Wire w = GetWire(line, containingTile, wireName, beginPip, true, unresWires, out targetTile);
                        if (w == null)
                        {
                            continue;
                        }

                        // store wire for later
                        unresWires.Add(containingTile, w);
                    }
                }
            }

            return wires;
        }

        private static Wire GetWire(string line, Tile containingTile, string wireName, bool beginPip, bool allowSkip, UnresolvedWires unresWires, out Tile targetTile)
        {
            targetTile = null;

            int firstBracketConn = line.IndexOf('(', 0);
            int firstBlankConn = line.IndexOf(' ', firstBracketConn);
            int secondBlankConn = line.IndexOf(' ', firstBlankConn + 1);

            string targetLocationString = line.Substring(firstBlankConn + 1, secondBlankConn - firstBlankConn - 1);

            // when parsing in incomplete files
            if (!FPGA.FPGA.Instance.Contains(targetLocationString))
            {
                return null;
            }

            string targetLocationWireName = line.Substring(secondBlankConn + 1, line.Length - secondBlankConn - 2);
            targetTile = FPGA.FPGA.Instance.GetTile(targetLocationString);
            int xIncr = (targetTile.TileKey.X - containingTile.TileKey.X);
            int yIncr = (targetTile.TileKey.Y - containingTile.TileKey.Y);

            if (allowSkip && false)
            {
                if (!unresWires.ValidIdentifier(wireName) && !unresWires.ValidIdentifier(targetLocationWireName))
                {
                    return null;
                }

                if (Math.Abs(xIncr) > 2 || Math.Abs(yIncr) > 2)
                {
                    return null;
                }
            }

            uint wireNameKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(wireName);
            uint targetLocationWireNameKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(targetLocationWireName);

            Wire w = new Wire(
                wireNameKey,
                targetLocationWireNameKey,
                beginPip,
                xIncr,
                yIncr);
            return w;
        }
    }
}