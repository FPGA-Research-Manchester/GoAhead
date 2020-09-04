using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    class PrintInterfaceConstraintsForSelection : Command
    {
        private const string MODE_ROW_WISE = "row-wise";
        private const string HORIZONTAL_LEFT_TO_RIGHT = "left-to-right";
        private const string VERTICAL_TOP_DOWN = "top-down";

        private const string NORTH = "North";
        private const string EAST = "East";
        private const string SOUTH = "South";
        private const string WEST = "West";

        private const string INPUT = "In";
        private const string OUTPUT = "Out";

        private const string PORT_KIND_BEGIN = "Begin";
        private const string PORT_KIND_END = "End";

        private const int INDEX_DIRECTION = 0;
        private const int INDEX_LENGTH = 1;
        private const int INDEX_SIGNAL_NAME = 2;
        private const int INDEX_SWITCHBOX = 3;

        protected override void DoCommandAction()
        {
            CheckParameters();

            List<Tile> backupSelection = TileSelectionManager.Instance.GetSelectedTiles().ToList();

            bool append = Append;

            foreach (string spec in InterfaceSpecs)
            {
                List<TileKey> intTiles = new List<TileKey>();

                // filter interconnect tiles from selection
                foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles().Where(
                         tile => (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect))))
                {
                    if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect) && IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.SubInterconnect))
                        continue;
                        
                    
                    intTiles.Add(t.TileKey);
                }

                int indexOffset = StartIndex;

                string[] split = spec.Split(':');

                string direction = split[INDEX_DIRECTION];
                string signalName = split[INDEX_SIGNAL_NAME];

                string[] sLengths = split[INDEX_LENGTH].Split('-');
                int[] lengths = Array.ConvertAll(sLengths, s => int.Parse(s));

                string switchboxToUse = "W";

                if (split.Length > INDEX_SWITCHBOX)
                {
                    switchboxToUse = split[INDEX_SWITCHBOX];
                    switchboxToUse = switchboxToUse.ToUpper();
                }

                foreach (int length in lengths)
                {
                    IEnumerable<IGrouping<int, TileKey>> clusters = null;

                    if (Border.Equals(WEST) || Border.Equals(EAST))
                    {
                        clusters =
                            from key in intTiles
                            group key by key.X into cluster
                            select cluster;
                    }
                    else if (Border.Equals(NORTH) || Border.Equals(SOUTH))
                    {
                        clusters =
                            from key in intTiles
                            group key by key.Y into cluster
                            select cluster;
                    }
                    else
                    {
                        throw new ArgumentException("Unexpected format in parameter InterfaceSpecs.");
                    }

                    if (Border.Equals(NORTH) || Border.Equals(WEST))
                    {
                        clusters = clusters.OrderBy(c => c.Key).Take(length);
                    }
                    else if (Border.Equals(EAST) || Border.Equals(SOUTH))
                    {
                        clusters = clusters.OrderByDescending(c => c.Key).Take(length);
                    }

                    List<Tile> interfaceTiles = new List<Tile>();

                    foreach (IGrouping<int, TileKey> group in clusters)
                    {
                        foreach (TileKey key in group)
                        {
                            interfaceTiles.Add(FPGA.FPGA.Instance.GetTile(key));
                        }
                    }

                    TileSelectionManager.Instance.ClearSelection();

                    // select interface tiles
                    foreach (Tile t in interfaceTiles)
                    {
                        TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                    }

                    PrintPartitionPinConstraintsForSelection command = new PrintPartitionPinConstraintsForSelection();
                    command.Mode = MODE_ROW_WISE;
                    command.Horizontal = HORIZONTAL_LEFT_TO_RIGHT;
                    command.Vertical = VERTICAL_TOP_DOWN;
                    command.CardinalDirection = GetCardinalDirection(Border, direction);
                    command.Length = length;
                    command.IndexOffset = indexOffset;
                    command.PortKind = GetPortKind(direction);
                    command.NumberOfSignals = NumberOfSignals;
                    command.FileName = FileName;
                    command.InstanceName = InstanceName;
                    command.SignalName = $"{SignalPrefix}_{signalName}";
                    command.PreventBlocking = PreventWiresFromBlocking;
                    command.Append = append;
                    command.CreateBackupFile = CreateBackupFile;
                    command.EastWestSwitchbox = switchboxToUse;
                    command.MaxSignalsPerTile = SignalsPerTile;

                    if (NumberOfSignals == 1)
                        command.PrintParameterizedSignal = false;
                    else
                        command.PrintParameterizedSignal = true;
                    
                    CommandExecuter.Instance.Execute(command);

                    // restore selection
                    TileSelectionManager.Instance.ClearSelection();

                    foreach (Tile t in backupSelection)
                    {
                        TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                    }

                    indexOffset += interfaceTiles.Count * SignalsPerTile;

                    append = true;
                }
            }
        }

        private string GetPortKind(string dir)
        {
            string portkind;

            switch(dir)
            {
                case OUTPUT:
                    portkind = PORT_KIND_BEGIN;
                    break;
                case INPUT:
                    portkind = PORT_KIND_END;
                    break;
                default:
                    throw new ArgumentException("Unexpected format in parameter InterfaceSpecs.");
            }

            return portkind;
        }

        private string GetCardinalDirection(string border, string dir)
        {
            string cardinalDirection;

            if(dir.Equals(OUTPUT))
            {
                cardinalDirection = border;
            }
            else if(dir.Equals(INPUT))
            {
                switch(border)
                {
                    case EAST:
                        cardinalDirection = WEST;
                        break;
                    case WEST:
                        cardinalDirection = EAST;
                        break;
                    case NORTH:
                        cardinalDirection = SOUTH;
                        break;
                    case SOUTH:
                        cardinalDirection = NORTH;
                        break;
                    default:
                        throw new ArgumentException("Unexpected format in parameter InterfaceSpecs.");
                }
            }
            else
            {
                throw new ArgumentException("Unexpected format in parameter InterfaceSpecs.");
            }

            return cardinalDirection;
        }

        private void CheckParameters()
        {
            bool interfaceSpecsIsCorrect = InterfaceSpecs.Count > 0;
            bool borderIsCorrect = Border.Equals(WEST) ||
                                   Border.Equals(EAST) ||
                                   Border.Equals(SOUTH) ||
                                   Border.Equals(NORTH);

            bool instanceNameIsCorrect = !string.IsNullOrEmpty(InstanceName);
            bool signalPrefixIsCorrect = !string.IsNullOrEmpty(SignalPrefix);
            bool filenameIsCorrect = !string.IsNullOrEmpty(FileName);
            bool startIndexIsCorrect = StartIndex >= 0;

            if (!interfaceSpecsIsCorrect || !borderIsCorrect || !instanceNameIsCorrect || !signalPrefixIsCorrect || !filenameIsCorrect || !startIndexIsCorrect)
            {
                throw new ArgumentException("Unexpected format in one of the parameters.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "format: <directions>:<wire lengths>:<signal name>:<switchbox to use> :: example; In:2-4:s2p,Out:2-4:p2s:E; where <switchbox to use> is an option for some architecture families and supports the options 'E' and 'W'")]
        public List<string> InterfaceSpecs = new List<string>();

        [Parameter(Comment = "The border on the slot (North, East, South, or West)")]
        public string Border = "West";

        [Parameter(Comment = "Instance name of the component")]
        public string InstanceName = "inst_ConnMacro";

        [Parameter(Comment = "The name of the signal")]
        public string SignalPrefix = "x0y0";

        [Parameter(Comment = "The name of the file.")]
        public string FileName = "";

        [Parameter(Comment = "Whether to append the content to the existing file.")]
        public bool Append = true;

        [Parameter(Comment = "Whether to create a backup file of FileName with the extension .bak.")]
        public bool CreateBackupFile = true;

        [Parameter(Comment = "The number of signals. Should be a multiple of 4.")]
        public int NumberOfSignals = 128;

        [Parameter(Comment = "Prevent the interface wires from blocking.")]
        public bool PreventWiresFromBlocking = true;

        [Parameter(Comment = "Provide start index number for enumerating vectorised signals.")]
        public int StartIndex = 0;

        [Parameter(Comment = "Signal per tile. Should be less than 8.")]
        public int SignalsPerTile = 8;
    }
}
