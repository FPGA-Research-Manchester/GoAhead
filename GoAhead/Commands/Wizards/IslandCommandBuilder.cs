﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.LibraryElementInstantiation;
using GoAhead.Commands.LibraryElementInstantiationManager;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.Selection;
using GoAhead.Commands.UCF;
using GoAhead.Commands.VHDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Wizards
{
    public class IslandCommandBuilder
    {
        public enum Target { Static = 0, Module = 1 }

        public string IslandName
        {
            get { return m_islandName; }
            set { m_islandName = value; }
        }

        public string VHDLWrapper
        {
            get { return m_vhdWrapper; }
            set { m_vhdWrapper = value; }
        }

        public string UCFFile
        {
            get { return m_ucfFile; }
            set { m_ucfFile = value; }
        }

        public string ConnectionPrimitiveName
        {
            get { return m_macroName; }
            set { m_macroName = value; }
        }

        public Target BuildTarget
        {
            get { return m_target; }
            set { m_target = value; }
        }

        public List<Command> GetCommands()
        {
            switch (BuildTarget)
            {
                case Target.Static: { return GetCommandsForStaticSystem(); }
                case Target.Module: { return GetCommandsForModule(); }
                default: { throw new ArgumentException("BuildTarget " + BuildTarget + " not implemented"); }
            }
        }

        private List<Command> GetCommandsForModule()
        {
            List<Command> cmds = new List<Command>();

            // shared by static and partial
            cmds.AddRange(GetSelectAreaCommands());

            List<Command> tunnelCommnds = new List<Command>();
            // i/o
            foreach (FPGATypes.InterfaceDirection direction in Enum.GetValues(typeof(FPGATypes.InterfaceDirection)))
            {
                // how many columns of connection macros will we instantiate?
                IEnumerable<Signal> signals = Objects.InterfaceManager.Instance.Signals.Where(s => s.SignalDirection.Equals(direction) && s.PartialRegion.Equals(IslandName) && (s.SignalMode.Equals("in") || s.SignalMode.Equals("out")));
                int ioColumns = 1;
                if (signals.Any())
                {
                    ioColumns = 1 + signals.Max(s => s.Column);
                    List<Command> placementCommands = GetPlacementCommands(direction, ConnectionPrimitiveName, IslandName + "_" + direction + "_", ioColumns, 4);

                    cmds.AddRange(GetTunnelCommands(direction, placementCommands));
                    cmds.AddRange(placementCommands);
                }
            }

            PrintVHDLWrapper printCmd = new PrintVHDLWrapper();
            printCmd.InstantiationFilter = "^" + IslandName;
            printCmd.EntityName = "static_placeholder_" + IslandName;
            printCmd.FileName = VHDLWrapper;
            printCmd.Append = false;
            printCmd.CreateBackupFile = true;
            cmds.Add(printCmd);

            InvertSelection invCmd = new InvertSelection();
            invCmd.Comment = "for building the module we need UCF constraints the currenlty selected partial area";
            cmds.Add(invCmd);

            // insert a * to match all if the generate labels
            cmds.AddRange(GetProhibitAndLocationConstraints("*inst_static_placeholder_" + IslandName + "/"));

            SelectFenceAroundUserSelection selFenceCmd = new SelectFenceAroundUserSelection();
            selFenceCmd.UserSelectionType = IslandName;
            selFenceCmd.Size = 12;
            selFenceCmd.Comment = "we could block the current selection, however blocking only a fence around the partial area is faster, thus select a fence around the partial area";
            cmds.Add(selFenceCmd);

            // add the tunnel commands
            cmds.AddRange(tunnelCommnds);

            cmds.AddRange(GetBlockCommands());

            return cmds;
        }

        private List<Command> GetCommandsForStaticSystem()
        {
            List<Command> cmds = new List<Command>();

            cmds.AddRange(GetSelectAreaCommands());

            // i/o
            foreach (FPGATypes.InterfaceDirection direction in Enum.GetValues(typeof(FPGATypes.InterfaceDirection)))
            {
                // how many columns of connection macros will we instantiate?
                IEnumerable<Signal> signals = Objects.InterfaceManager.Instance.Signals.Where(s => s.SignalDirection.Equals(direction) && s.PartialRegion.Equals(IslandName) && (s.SignalMode.Equals("in") || s.SignalMode.Equals("out")));
                int ioColumns = 1;
                if (signals.Any())
                {
                    ioColumns = 1 + signals.Max(s => s.Column);
                    List<Command> placementCommands = GetPlacementCommands(direction, ConnectionPrimitiveName, IslandName + "_" + direction + "_", ioColumns, 4);

                    cmds.AddRange(GetTunnelCommands(direction, placementCommands));
                    cmds.AddRange(placementCommands);
                }
            }

            PrintVHDLWrapper printCmd = new PrintVHDLWrapper();
            printCmd.InstantiationFilter = "^" + IslandName;
            printCmd.EntityName = IslandName;
            printCmd.FileName = VHDLWrapper;
            printCmd.Append = false;
            printCmd.CreateBackupFile = true;
            cmds.Add(printCmd);

            string instantiationTemplate = Path.GetDirectoryName(VHDLWrapper) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(VHDLWrapper) + "_instantation" + Path.GetExtension(VHDLWrapper);
            PrintVHDLWrapperInstantiation printInstCmd = new PrintVHDLWrapperInstantiation();
            printInstCmd.InstantiationFilter = "^" + IslandName;
            printInstCmd.EntityName = IslandName;
            printInstCmd.FileName = instantiationTemplate;
            printInstCmd.Append = false;
            printInstCmd.CreateBackupFile = true;
            cmds.Add(printInstCmd);

            cmds.AddRange(GetProhibitAndLocationConstraints("inst_" + IslandName + "/"));
            cmds.AddRange(GetBlockCommands());

            return cmds;
        }

        private List<Command> GetTunnelCommands(FPGATypes.InterfaceDirection direction, List<Command> placementCommands)
        {
            List<Command> result = new List<Command>();

            // TODO make parameter?
            foreach (Command placeCmd in placementCommands.Where(c => c is AddSingleInstantiationByTile))
            {
                AddSingleInstantiationByTile addCmd = (AddSingleInstantiationByTile)placeCmd;
                Tile clb = FPGA.FPGA.Instance.GetTile(addCmd.AnchorLocation);
                Tile interconnect = FPGATypes.GetInterconnectTile(clb);

                ExcludePortsFromBlockingOnTileByRegexp exclude = new ExcludePortsFromBlockingOnTileByRegexp();
                exclude.CheckForExistence = false;
                exclude.IncludeAllPorts = false;
                exclude.Location = interconnect.Location;

                if (BuildTarget == Target.Static && direction == FPGATypes.InterfaceDirection.East)
                {
                    exclude.PortNameRegexp = "(WW2E[0|1|2|3])|(EE2B[0|1|2|3])";
                }
                else if (BuildTarget == Target.Static && direction == FPGATypes.InterfaceDirection.West)
                {
                    exclude.PortNameRegexp = "(EE2E[0|1|2|3])|(WW2B[0|1|2|3])";
                }
                else if (BuildTarget == Target.Module && direction == FPGATypes.InterfaceDirection.East)
                {
                    exclude.PortNameRegexp = "(EE2E[0|1|2|3])|(WW2B[0|1|2|3])";
                }
                else if (BuildTarget == Target.Module && direction == FPGATypes.InterfaceDirection.West)
                {
                    exclude.PortNameRegexp = "(WW2E[0|1|2|3])|(EE2B[0|1|2|3])";
                }
                else
                {
                    throw new ArgumentException("Direction " + direction + " not implemented");
                }
                //exclude.PortNameRegexp = "((EE)|(WW))2(B|E)[0|1|2|3]"; // the epxression parser cant handle [0-3]

                if (result.Count == 0)
                {
                    exclude.Comment = " exclude tunnel wires from blocking";
                }

                result.Add(exclude);
            }
            return result;
        }

        private List<Command> GetProhibitAndLocationConstraints(string instancePrefix)
        {
            List<Command> cmds = new List<Command>();

            // print prohibt statements
            PrintProhibitStatementsForSelection prohibitCmd = new PrintProhibitStatementsForSelection();
            prohibitCmd.Append = true;
            prohibitCmd.Mute = true;
            prohibitCmd.ExcludeUsedSlices = true;
            prohibitCmd.FileName = UCFFile;
            prohibitCmd.Comment = "create prohibit statements that prevent the placer from using logic within the (currently selected) partial area " + IslandName;
            cmds.Add(prohibitCmd);

            // print placement constraints for macros
            PrintLocationConstraints placeCmd = new PrintLocationConstraints();
            placeCmd.Append = true;
            placeCmd.Mute = true;
            placeCmd.FileName = UCFFile;
            placeCmd.HierarchyPrefix = instancePrefix;
            placeCmd.InstantiationFilter = IslandName;
            placeCmd.Comment = "create placement constraints for the connection primitives and append them to the UCF file used above ";
            cmds.Add(placeCmd);

            return cmds;
        }

        /// <summary>
        /// shared between static and partial
        /// </summary>
        /// <returns></returns>
        private List<Command> GetBlockCommands()
        {
            List<Command> cmds = new List<Command>();
            // create blocker
            string blockerName = IslandName + "_blocker";
            AddNetlistContainer addCmd = new AddNetlistContainer();
            addCmd.NetlistContainerName = blockerName;
            addCmd.Comment = "add a netlist container that will contain the blocker";
            cmds.Add(addCmd);

            // TODO release tunnel

            BlockSelection blockCmd = new BlockSelection();
            blockCmd.BlockWithEndPips = true;
            blockCmd.NetlistContainerName = blockerName;
            blockCmd.SliceNumber = 0;
            blockCmd.Prefix = "RBB_Blocker";
            blockCmd.PrintUnblockedPorts = false;
            blockCmd.Comment = "Block all routing resources but the before spared out tunnel";
            cmds.Add(blockCmd);

            return cmds;
        }

        private List<Command> GetSelectAreaCommands()
        {
            List<Command> cmds = new List<Command>();
            // recreate user selection
            ClearSelection clearCmd = new ClearSelection();
            clearCmd.Comment = "clear selections from previous commands";
            cmds.Add(clearCmd);

            List<Command> addSelCmds = TileSelectionManager.Instance.GetListOfAddToSelectionXYCommandsForUserSelection(IslandName);
            foreach (Command cmd in addSelCmds)
            {
                cmd.Comment = "select tiles in the reconfigurable area " + IslandName;
            }
            cmds.AddRange(addSelCmds);
            // the last command is not the ExpandSelection,
            cmds[cmds.Count - 1].Comment = "expand the current selection such that we always select interconnect tiles along with the CLBs and always full DSP and RAM blocks";

            StoreCurrentSelectionAs storeCmd = new StoreCurrentSelectionAs();
            storeCmd.UserSelectionType = IslandName;
            storeCmd.Comment = "store the above selected tiles as a user selection named " + IslandName + ". we may later refer to this name";
            cmds.Add(storeCmd);

            return cmds;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="connectionPrimitiveName"></param>
        /// <param name="instance"></param>
        /// <param name="columns"></param>
        /// <param name="maxWiresPerRow"></param>
        /// <param name="prefix">The prefix that can be sued for subsequent AnnotateSignal commands</param>
        /// <returns></returns>
        private List<Command> GetPlacementCommands(FPGA.FPGATypes.InterfaceDirection dir, string connectionPrimitiveName, string instance, int columns, int maxWiresPerRow)
        {
            LibraryElement libElement = Objects.Library.Instance.GetElement(connectionPrimitiveName);
            List<Tile> tilesToReleaseWiresOn = null;

            List<Tile> anchors = GetAnchors(dir, columns, false, out tilesToReleaseWiresOn);

            Dictionary<int, List<Signal>> inputsPerColumn = new Dictionary<int, List<Signal>>();
            Dictionary<int, List<Signal>> outputsPerColumn = new Dictionary<int, List<Signal>>();

            //Console.WriteLine(dir);
            //Console.WriteLine(instance);

            for (int column = 0; column < columns; column++)
            {
                List<Signal> inputs = GetSignals(dir, column, "in");
                List<Signal> outputs = GetSignals(dir, column, "out");

                // equalize both list with open signals, the signal paramteres are dont care
                while (inputs.Count % maxWiresPerRow != 0 || inputs.Count < outputs.Count)
                {
                    inputs.Add(new Signal("open", "in", dir, IslandName, column));
                }
                while (outputs.Count < inputs.Count)
                {
                    outputs.Add(new Signal("open", "out", dir, IslandName, column));
                }

                inputsPerColumn[column] = inputs;
                outputsPerColumn[column] = outputs;
            }

            Dictionary<Tile, List<Signal>> inputsOnTile = new Dictionary<Tile, List<Signal>>();
            Dictionary<Tile, List<Signal>> outputsOnTile = new Dictionary<Tile, List<Signal>>();
            List<Tile> connectionPrimitiveCLBs = new List<Tile>();
            // 1 collect inputs in this tile

            // iterate row wise
            for (int column = 0; column < columns; column++)
            {
                Tile currentAnchor = anchors[column];
                for (int i = 0; i < inputsPerColumn[column].Count; i += maxWiresPerRow)
                {
                    inputsOnTile.Add(currentAnchor, new List<Signal>());
                    outputsOnTile.Add(currentAnchor, new List<Signal>());
                    connectionPrimitiveCLBs.Add(currentAnchor);

                    for (int j = 0; j < maxWiresPerRow; j++)
                    {
                        Signal input = inputsPerColumn[column][i + j];
                        inputsOnTile[currentAnchor].Add(input);

                        Signal output = outputsPerColumn[column][i + j];
                        outputsOnTile[currentAnchor].Add(output);
                    }

                    currentAnchor = MoveAnchorToNextColumn(dir, currentAnchor);
                }
            }

            CheckMacroTiles(dir, connectionPrimitiveCLBs);

            string prefix = instance + connectionPrimitiveName;
            // do connection primitive instantiation
            List<Command> result = new List<Command>();
            foreach (Tile anchor in connectionPrimitiveCLBs)
            {
                List<Signal> localInputs = inputsOnTile[anchor];
                List<Signal> localOutputs = outputsOnTile[anchor];

                // do we need to place a macro here
                bool placeMacro = HasNonOpenSignal(localInputs) || HasNonOpenSignal(localOutputs);
                if (!placeMacro)
                {
                    continue;
                }

                // check that the added signal names are all identical
                if (localOutputs.Select(s => s.SignalNameWithoutBraces).Distinct().Count() != 1)
                {
                    throw new ArgumentException("Signal names may only change at a boundary of 4. Check signal " + localInputs[0]);
                }
                if (localInputs.Select(s => s.SignalNameWithoutBraces).Distinct().Count() != 1)
                {
                    throw new ArgumentException("Signal names may only change at a boundary of 4. Check signal " + localOutputs[0]);
                }

                // can we use this tile for placement?
                StringBuilder errorList = null;
                bool validPlacement = DesignRuleChecker.CheckLibraryElementPlacement(anchor, libElement, out errorList);
                if (!validPlacement)
                {
                    throw new ArgumentException("Can not place the library element at the desired postion (check the output window)" + errorList.ToString());
                }

                // finally, place the macro
                AddSingleInstantiationByTile addByTile = new AddSingleInstantiationByTile();
                addByTile.AnchorLocation = anchor.Location;
                addByTile.AutoClearModuleSlot = false;
                addByTile.InstanceName = prefix + "_" + result.Count;
                addByTile.LibraryElementName = connectionPrimitiveName;
                if (result.Count == 0)
                {
                    addByTile.Comment = "instantiate a connection primitive at tile " + addByTile.AnchorLocation + ". An expert may want to relocate the instantiations";
                }
                result.Add(addByTile);

                AnnotateSignalNames annotateCmd = new AnnotateSignalNames();
                annotateCmd.InstantiationFilter = "^" + addByTile.InstanceName;

                string inputSource = BuildTarget == Target.Static ? "I:" : "O:";
                string outputSource = BuildTarget == Target.Static ? "O:" : "I:";
                annotateCmd.PortMapping.Add(inputSource + localInputs[0].SignalNameWithoutBraces + ":external");
                annotateCmd.PortMapping.Add(outputSource + localOutputs[0].SignalNameWithoutBraces + ":external");
                annotateCmd.PortMapping.Add("H:H:internal");
                result.Add(annotateCmd);
            }

            return result;
        }

        private Tile MoveAnchorToNextColumn(FPGA.FPGATypes.InterfaceDirection dir, Tile currentAnchor)
        {
            switch (dir)
            {
                case FPGATypes.InterfaceDirection.East: { currentAnchor = Navigator.GetNextCLB(currentAnchor, 0, 1); break; } // go left
                case FPGATypes.InterfaceDirection.West: { currentAnchor = Navigator.GetNextCLB(currentAnchor, 0, 1); break; } // go left
                default: { throw new ArgumentException("Can not handle direction " + dir); }
            }
            return currentAnchor;
        }

        /// <summary>
        /// Check that all passed tiles ar in (stati) or out of (modul) the partial area
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="macroTiles"></param>
        private void CheckMacroTiles(FPGA.FPGATypes.InterfaceDirection dir, List<Tile> macroTiles)
        {
            foreach (Tile anchor in macroTiles)
            {
                if (BuildTarget == Target.Static)
                {
                    if (!TileSelectionManager.Instance.IsUserSelected(anchor.TileKey, IslandName))
                    {
                        throw new ArgumentException("The interface of " + IslandName + " is too large to be placed in the selected area (" + dir + ")");
                    }
                }
                if (BuildTarget == Target.Module)
                {
                    if (TileSelectionManager.Instance.IsUserSelected(anchor.TileKey, IslandName))
                    {
                        throw new ArgumentException("The interface of " + IslandName + " is too large to be placed in the selected area (" + dir + ")");
                    }
                }
            }
        }

        private List<Signal> GetSignals(FPGA.FPGATypes.InterfaceDirection dir, int column, string mode)
        {
            List<Signal> inputs = Objects.InterfaceManager.Instance.GetFlatSignalList(
                s => s.SignalMode.Equals(mode) && s.Column == column &&
                     s.PartialRegion.Equals(IslandName) && s.SignalDirection.Equals(dir));
            return inputs;
        }

        private bool HasNonOpenSignal(List<Signal> list)
        {
            foreach (Signal s in list)
            {
                if (!s.SignalName.StartsWith("open"))
                {
                    return true;
                }
            }

            return false;
        }

        private List<Tile> GetAnchors(FPGA.FPGATypes.InterfaceDirection dir, int columns, bool streaming, out List<Tile> tilesToReleaseWiresOn)
        {
            List<Tile> result = new List<Tile>();
            tilesToReleaseWiresOn = new List<Tile>();

            FPGATypes.Placement placement = 0;
            switch (dir)
            {
                case FPGATypes.InterfaceDirection.East: { placement = FPGATypes.Placement.UpperRight; break; }
                case FPGATypes.InterfaceDirection.West: { placement = FPGATypes.Placement.UpperLeft; break; }
                default: { throw new ArgumentException("Can not hanlde direction " + dir); }
            }

            Tile anchor = TileSelectionManager.Instance.GetUserSelectedTile(
                IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.CLB),
                IslandName,
                placement);

            // move this way to collect anchors for columns
            int xIncrement = 0;
            int yIncrement = 0;
            switch (dir)
            {
                case FPGATypes.InterfaceDirection.East: { xIncrement = -1; yIncrement = 0; break; }
                case FPGATypes.InterfaceDirection.West: { xIncrement = 1; yIncrement = 0; break; }
            }
            xIncrement *= m_columnStepWidth;
            yIncrement *= m_columnStepWidth;

            // for module anchors move the anchors out of the partial area
            if (BuildTarget == Target.Module)
            {
                if (m_islandName.Equals("pr5"))
                {
                }

                int xAnchorShift = 0;
                int yAnchorShift = 0;
                switch (dir)
                {
                    case FPGATypes.InterfaceDirection.East: { xAnchorShift = 1; yAnchorShift = 0; break; }
                    case FPGATypes.InterfaceDirection.West: { xAnchorShift = -1; yAnchorShift = 0; break; }
                }
                xAnchorShift *= m_columnStepWidth;
                yAnchorShift *= m_columnStepWidth;

                // TODO here, we assume double lines
                Tile start = anchor;
                for (int column = 0; column < columns; column++)
                {
                    // besser mit DoubleLine (parameter) von Anker auf den naechsten CLB gehen

                    /*
                    do
                    {
                        anchor = Search.Navigator.GetNextCLB(anchor, xAnchorShift, yAnchorShift, tilesToReleaseWiresOn);
                    }
                    while (Math.Abs(start.LocationX - anchor.LocationX) % 2 != 0);
                     * */
                    anchor = Navigator.GetNextCLB(anchor, xAnchorShift, yAnchorShift, tilesToReleaseWiresOn);
                    anchor = Navigator.GetNextCLB(anchor, xAnchorShift, yAnchorShift, tilesToReleaseWiresOn);
                }
            }

            // first anchor
            result.Add(anchor);
            Tile current = anchor;
            // start with 1 as we have the anchor added already
            for (int column = 1; column < columns; column++)
            {
                current = Navigator.GetNextCLB(current, xIncrement, yIncrement, tilesToReleaseWiresOn);
                result.Add(current);
            }

            // East 0 1 2
            // West 2 1 0 -> Reverse
            if (dir == FPGATypes.InterfaceDirection.East)
            {
                result.Reverse();
            }

            return result;
        }

        private string m_islandName = "";
        private string m_vhdWrapper = "";
        private string m_ucfFile = "";
        private string m_macroName = "";
        private Target m_target = Target.Static;
        private int m_columnStepWidth = 1;
    }
}