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

        protected override void DoCommandAction()
        {
            this.CheckParameters();

            List<Tile> backupSelection = FPGA.TileSelectionManager.Instance.GetSelectedTiles().ToList();

            bool append = this.Append;

            foreach (string spec in this.InterfaceSpecs)
            {
                List<TileKey> intTiles = new List<TileKey>();

                // filter interconnect tiles from selection
                foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(
                         tile => IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect)))
                {
                    intTiles.Add(t.TileKey);
                }

                int indexOffset = 0;

                string[] split = spec.Split(':');

                string direction = split[INDEX_DIRECTION];
                string signalName = split[INDEX_SIGNAL_NAME];

                string[] sLengths = split[INDEX_LENGTH].Split('-');
                int[] lengths = Array.ConvertAll(sLengths, s => int.Parse(s));

                foreach (int length in lengths)
                {
                    IEnumerable<IGrouping<int, TileKey>> clusters = null;

                    if (this.Border.Equals(WEST) || this.Border.Equals(EAST))
                    {
                        clusters =
                            from key in intTiles
                            group key by key.X into cluster
                            select cluster;
                    }
                    else if (this.Border.Equals(NORTH) || this.Border.Equals(SOUTH))
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

                    if (this.Border.Equals(NORTH) || this.Border.Equals(WEST))
                    {
                        clusters = clusters.OrderBy(c => c.Key).Take(length);
                    }
                    else if (this.Border.Equals(EAST) || this.Border.Equals(SOUTH))
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

                    FPGA.TileSelectionManager.Instance.ClearSelection();

                    // select interface tiles
                    foreach (Tile t in interfaceTiles)
                    {
                        FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                    }

                    PrintPartitionPinConstraintsForSelection command = new PrintPartitionPinConstraintsForSelection();
                    command.Mode = MODE_ROW_WISE;
                    command.Horizontal = HORIZONTAL_LEFT_TO_RIGHT;
                    command.Vertical = VERTICAL_TOP_DOWN;
                    command.CardinalDirection = this.GetCardinalDirection(this.Border, direction);
                    command.Length = length;
                    command.IndexOffset = indexOffset;
                    command.PortKind = this.GetPortKind(direction);
                    command.NumberOfSignals = this.NumberOfSignals;
                    command.FileName = this.FileName;
                    command.InstanceName = this.InstanceName;
                    command.SignalName = $"{this.SignalPrefix}_{signalName}";
                    command.PreventBlocking = this.PreventWiresFromBlocking;
                    command.Append = append;
                    command.CreateBackupFile = this.CreateBackupFile;
                    CommandExecuter.Instance.Execute(command);

                    // restore selection
                    FPGA.TileSelectionManager.Instance.ClearSelection();

                    foreach (Tile t in backupSelection)
                    {
                        FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                    }

                    indexOffset += interfaceTiles.Count * PrintPartitionPinConstraintsForTile.SIGNALS_PER_TILE;

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
            bool interfaceSpecsIsCorrect = this.InterfaceSpecs.Count > 0;
            bool borderIsCorrect = this.Border.Equals(WEST) ||
                                   this.Border.Equals(EAST) ||
                                   this.Border.Equals(SOUTH) ||
                                   this.Border.Equals(NORTH);

            bool instanceNameIsCorrect = !String.IsNullOrEmpty(this.InstanceName);
            bool signalPrefixIsCorrect = !String.IsNullOrEmpty(this.SignalPrefix);
            bool filenameIsCorrect = !String.IsNullOrEmpty(this.FileName);

            if(!interfaceSpecsIsCorrect || !borderIsCorrect || !instanceNameIsCorrect || !signalPrefixIsCorrect || !filenameIsCorrect)
            {
                throw new ArgumentException("Unexpected format in one of the parameters.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "format: <directions>:<wire lengths>:<signal name> :: example; In:2-4:s2p,Out:2-4:p2s")]
        public List<string> InterfaceSpecs = new List<string>();

        [Parameter(Comment = "The border on the slot (North, East, South, or West)")]
        public String Border = "West";

        [Parameter(Comment = "Instance name of the component")]
        public String InstanceName = "inst_ConnMacro";

        [Parameter(Comment = "The name of the signal")]
        public String SignalPrefix = "x0y0";

        [Parameter(Comment = "The name of the file.")]
        public String FileName = "";

        [Parameter(Comment = "Whether to append the content to the existing file.")]
        public bool Append = true;

        [Parameter(Comment = "Whether to create a backup file of FileName with the extension .bak.")]
        public bool CreateBackupFile = true;

        [Parameter(Comment = "The number of signals. Should be a multiple of 4.")]
        public int NumberOfSignals = 128;

        [Parameter(Comment = "Prevent the interface wires from blocking.")]
        public bool PreventWiresFromBlocking = true;
    }
}
