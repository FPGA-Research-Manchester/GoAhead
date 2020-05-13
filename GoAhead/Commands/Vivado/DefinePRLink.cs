using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.FPGA;


namespace GoAhead.Commands.Vivado
{
    class DefinePRLink : NetlistContainerCommandWithFileOutput
    {
        private enum SortMode { Undefined, R, C }
        private enum HMode { Undefined, L2R, R2L }
        private enum VMode { Undefined, TD, BU }

        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            SortMode mode = SortMode.Undefined;
            HMode hMode = HMode.Undefined;
            VMode vMode = VMode.Undefined;

            SetSortModes(ref mode, ref hMode, ref vMode);

            List<TileKey> keys = new List<TileKey>();
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles().Where(t =>
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                keys.Add(tile.TileKey);
            }

            var preOrderedKey =
               from key in keys
               group key by (mode == SortMode.R ? key.Y : key.X) into g
               select g;

            List<Tile> tilesInFinalOrder = new List<Tile>();

            if (mode == SortMode.R)
            {
                foreach (IGrouping<int, TileKey> group in (vMode == VMode.TD ? preOrderedKey.OrderBy(g => g.Key) : preOrderedKey.OrderByDescending(g => g.Key)))
                {
                    foreach (TileKey key in (hMode == HMode.L2R ? group.OrderBy(k => k.X) : group.OrderBy(k => k.X).Reverse()))
                    {
                        tilesInFinalOrder.Add(FPGA.FPGA.Instance.GetTile(key));
                    }
                }
            }
            else
            {
                foreach (IGrouping<int, TileKey> group in (hMode == HMode.L2R ? preOrderedKey.OrderBy(g => g.Key) : preOrderedKey.OrderByDescending(g => g.Key)))
                {
                    foreach (TileKey key in (vMode == VMode.TD ? group.OrderBy(k => k.Y) : group.OrderBy(k => k.Y).Reverse()))
                    {
                        tilesInFinalOrder.Add(FPGA.FPGA.Instance.GetTile(key));
                    }
                }
            }

            int index = StartIndex;
            foreach (Tile tile in tilesInFinalOrder)
            {
                foreach (string path in Paths)
                {
                    string netName = Prefix + SignalName + "[" + index++ + "]";
                    PRLink link = new PRLink(tile, netName);

                    string[] portNames = path.Split(':');
                    foreach (string portName in portNames)
                    {
                        if (!tile.SwitchMatrix.Contains(portName))
                        {
                            throw new ArgumentException("Port " + portName + " not found on " + tile.Location);
                        }
                        link.Add(new Port(portName));
                        tile.BlockPort(portName, Tile.BlockReason.ExcludedFromBlocking);
                    }
                    PRLinkManager.Instance.Add(link);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private void SetSortModes(ref SortMode mode, ref HMode hMode, ref VMode vMode)
        {
            if (Mode.ToLower().Equals("row-wise")) { mode = SortMode.R; }
            else if (Mode.ToLower().Equals("column-wise")) { mode = SortMode.C; }
            else { throw new ArgumentException("Invalid value for Mode " + Mode + ". Use either row-wise or column-wise"); }

            if (Horizontal.ToLower().Equals("left-to-right")) { hMode = HMode.L2R; }
            else if (Horizontal.ToLower().Equals("right-to-left")) { hMode = HMode.R2L; }
            else { throw new ArgumentException("Invalid value for Mode " + Horizontal + ". Use either left-to-right or right-to-left"); }

            if (Vertical.ToLower().Equals("top-down")) { vMode = VMode.TD; }
            else if (Vertical.ToLower().Equals("bottom-up")) { vMode = VMode.BU; }
            else { throw new ArgumentException("Invalid value for Mode " + Horizontal + ". Use either top-down orbottom-up"); }
        }

        [Parameter(Comment = "The index to start counting, e.g. SignalName[index]")]
        public int StartIndex = 0;

        [Parameter(Comment = "The name of the signal to route. The net names will be SignalName[index]")]
        public string SignalName = "p2s";
        
        [Parameter(Comment = "The routing resources to use")]
        public List<string> Paths = new List<string>();

        [Parameter(Comment = "Either row-wise or column-wise")]
        public string Mode = "row-wise";

        [Parameter(Comment = "Either left-to-right or right-to-left")]
        public string Horizontal = "left-to-right";

        [Parameter(Comment = "Either top-down or bottom-up")]
        public string Vertical = "top-down";

        [Parameter(Comment = "Prefix for support hierarchical nets, e.g. system_i/SW2LED_0/U0/")]
        public string Prefix = "";
    }
}
