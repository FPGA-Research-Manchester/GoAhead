﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code.TCL;
using GoAhead.Objects;
using GoAhead.FPGA;
using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.XDLManipulation;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.LibraryElementInstantiation
{
    [CommandDescription(Description = "Add instantiations to all selected reference points and mark the resources used by the library element as blocked.", Wrapper = true, Publish = true)]
    class AddInstantiationInSelectedTiles : AddInstantiationCommand
    {
        public enum SortMode { Undefined, R, C }
        public enum HMode { Undefined, L2R, R2L }
        public enum VMode { Undefined, TD, BU }

        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            SortMode mode = SortMode.Undefined;
            HMode hMode = HMode.Undefined;
            VMode vMode = VMode.Undefined;

            SetSortModes(ref mode, ref hMode, ref vMode);

            List<TileKey> keys = new List<TileKey>();
            foreach (Tile clb in TileSelectionManager.Instance.GetSelectedTiles().Where(t =>
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB) || 
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.DSP) || 
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.BRAM))) 
            {
                keys.Add(clb.TileKey);
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

            // apply filter
            tilesInFinalOrder.RemoveAll(t => !Regex.IsMatch(t.Location, Filter));

            /*
            // check prior to plaecment of valid placement
            foreach (Tile t in tilesInFinalOrder)
            {
                LibraryElement libElement = Objects.Library.Instance.GetElement(this.LibraryElementName);
                StringBuilder errorList = null;                
                bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(t, libElement, out errorList);
                if (!placementOk)
                {
                    throw new ArgumentException("Macro " + this.LibraryElementName + " can not be placed at " + t + ": " + errorList.ToString());
                }
            }
            */
            
            if (AutoClearModuleSlot)
            {
                LibraryElement libElement = Objects.Library.Instance.GetElement(LibraryElementName);
                AutoClearModuleSlotBeforeInstantiation(libElement, tilesInFinalOrder);
            }

            foreach (Tile t in tilesInFinalOrder)
            {
                // check again prior to placement and now consider already placed tiles
                
                /*
                StringBuilder errorList = null;
                bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(t, libElement, out errorList);
                if (!placementOk)
                {
                    throw new ArgumentException("Library element " + this.LibraryElementName + " can not be placed at " + t + ": " + errorList.ToString());
                }*/
                switch (FPGA.FPGA.Instance.BackendType)
                {
                    case FPGATypes.BackendType.ISE:
                        AddISEInstantiation(t);
                        break;
                    case FPGATypes.BackendType.Vivado:
                        AddVivadoInstantiation(t);
                        break;
                }
            }

            if (AutoFuse)
            {
                FuseNets fuseCmd = new FuseNets();
                fuseCmd.NetlistContainerName = NetlistContainerName;
                fuseCmd.Mute = Mute;
                fuseCmd.Profile = Profile;
                fuseCmd.PrintProgress = PrintProgress;
                CommandExecuter.Instance.Execute(fuseCmd);
            }
        }

        private void AddISEInstantiation(Tile t)
        {
            LibElemInst instance = new LibElemInst();
            instance.AnchorLocation = t.Location;
            instance.InstanceName = GetNextInstanceName();
            instance.LibraryElementName = LibraryElementName;
            instance.SliceNumber = SliceNumber;
            instance.SliceName = t.Slices[(int)SliceNumber].SliceName;
            LibraryElementInstanceManager.Instance.Add(instance);

            // mark source as blocked
            ExcludeInstantiationSourcesFromBlocking markSrc = new ExcludeInstantiationSourcesFromBlocking();
            markSrc.AnchorLocation = t.Location;
            markSrc.LibraryElementName = LibraryElementName;
            CommandExecuter.Instance.Execute(markSrc);

            SaveLibraryElementInstantiation saveCmd = new SaveLibraryElementInstantiation();
            saveCmd.AddDesignConfig = false;
            saveCmd.InsertPrefix = true;
            saveCmd.InstanceName = instance.InstanceName;
            saveCmd.NetlistContainerName = NetlistContainerName;
            CommandExecuter.Instance.Execute(saveCmd);
        }

        private void AddVivadoInstantiation(Tile t)
        {            
            LibraryElement libElement = Objects.Library.Instance.GetElement(LibraryElementName);

            if (libElement.VivadoConnectionPrimitive)
            {
                // bei Vivado 4 LUT6-Instanzen erstellen, eine mit Inputs und Output, drei davon dann nur mit Output
                // only VivaodConnection primitves contains SubElements
                bool checkResources = true;
                foreach (LibraryElement other in libElement.SubElements)
                {
                    LibElemInst instance = new LibElemInst();
                    instance.AnchorLocation = t.Location;
                    instance.InstanceName = GetNextInstanceName();
                    instance.LibraryElementName = other.Name;
                    instance.SliceNumber = SliceNumber;
                    instance.SliceName = t.Slices[(int)SliceNumber].SliceName;
                    LibraryElementInstanceManager.Instance.Add(instance);

                    ExcludeInstantiationSourcesFromBlocking markSrc = new ExcludeInstantiationSourcesFromBlocking();
                    markSrc.AnchorLocation = t.Location;
                    markSrc.LibraryElementName = other.Name;
                    markSrc.CheckResources = checkResources;
                    // mark source as blocked, only check onces
                    checkResources = false;
                    CommandExecuter.Instance.Execute(markSrc);

                    SaveLibraryElementInstantiation saveCmd = new SaveLibraryElementInstantiation();
                    saveCmd.AddDesignConfig = false;
                    saveCmd.InsertPrefix = true;
                    saveCmd.InstanceName = instance.InstanceName;
                    saveCmd.NetlistContainerName = NetlistContainerName;
                    CommandExecuter.Instance.Execute(saveCmd);
                }
            }
            else
            {
                // for normal moduleas use ISE code
                AddISEInstantiation(t);
            }
        }

        private string GetNextInstanceName()
        {
            string instanceName = Hierarchy + InstanceName + "_" + m_instanceIndex++;
            while (LibraryElementInstanceManager.Instance.HasInstanceName(instanceName))
            {
                instanceName = InstanceName + "_" + m_instanceIndex++;
            }
            return instanceName;
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

        /// <summary>
        /// used for naming instances
        /// </summary>
        private int m_instanceIndex = 0;

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Either row-wise or column-wise")]
        public string Mode   = "row-wise";

        [Parameter(Comment = "Either left-to-right or right-to-left")]
        public string Horizontal = "left-to-right";

        [Parameter(Comment = "Either top-down or bottom-up")]
        public string Vertical = "top-down";
        
        [Parameter(Comment = "The slice number to instantiate on the anchor")]
        public int SliceNumber = 0;

        [Parameter(Comment = "Only consider those selected tiles that match this regular expression ")]
        public string Filter = ".*";
    }
}
