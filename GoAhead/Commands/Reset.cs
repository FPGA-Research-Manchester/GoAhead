using System;
using System.Reflection;
using GoAhead.FPGA;
using GoAhead.Objects;
using System.Collections.Generic;
using GoAhead.Commands.Data;
using GoAhead.Commands.NetlistContainerGeneration;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Discard any changes since loading the FPGA and run the FPGA family hook script (if any). The following objectd will also be cleared: Netlist container, Define, Variables, Selection, Instantiations, Library, Interfaces", Wrapper = true)]
    class Reset : Command
    {
        protected override void DoCommandAction()
        {
            if (FPGA.FPGA.Instance != null)
            {
                foreach (Tile next in FPGA.FPGA.Instance.GetAllTiles())
                {
                    next.Reset();
                }
            }

            NetlistContainerManager.Instance.Reset();
            TileSelectionManager.Instance.Reset();
            RAMSelectionManager.Instance.Reset();
            LibraryElementInstanceManager.Instance.Reset();
            Objects.InterfaceManager.Instance.Reset();
            Objects.Library.Instance.Reset();

            foreach (Interfaces.IResetable reset in ObjectsToReset)
            {
                if (reset != null)
                {
                    reset.Reset();
                }
            }

            // add a default net list container
            AddNetlistContainer addDefaultNetlistContainerCommand = new AddNetlistContainer();
            addDefaultNetlistContainerCommand.NetlistContainerName = NetlistContainerManager.DefaultNetlistContainerName;
            CommandExecuter.Instance.Execute(addDefaultNetlistContainerCommand);

            // call family specific hook
            CommandExecuter.Instance.Execute(new LoadFPGAFamilyScript());
        }

        public override void Undo()
        {
        }

        public static List<Interfaces.IResetable> ObjectsToReset = new List<Interfaces.IResetable>();
    }
}

