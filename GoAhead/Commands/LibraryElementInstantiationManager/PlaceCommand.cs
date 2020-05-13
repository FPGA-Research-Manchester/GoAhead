using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.LibraryElementInstantiation
{
    [CommandDescription(Description = "A common base class for place commands (module and connection macro)")]
    abstract class PlaceCommand : AddInstantiationCommand
    {
        protected override sealed void DoCommandAction()
        {
            if (FPGA.FPGA.Instance.Contains(AnchorLocation))
            {
                Tile anchor = FPGA.FPGA.Instance.GetTile(AnchorLocation);
                AddSingleInstantiationByTile addCmd = new AddSingleInstantiationByTile();
                addCmd.AnchorLocation = AnchorLocation;
                SetBaseClassParametersAndExecuteCommand(addCmd);
            }
            else if (FPGA.FPGA.Instance.ContainsSlice(AnchorLocation))
            {
                Slice s = FPGA.FPGA.Instance.GetSlice(AnchorLocation);
                AddSingleInstantiationBySlice addCmd = new AddSingleInstantiationBySlice();
                addCmd.SliceName = AnchorLocation;
                SetBaseClassParametersAndExecuteCommand(addCmd);
            }
            else
            {
                throw new ArgumentException("Expecting either a tile or a slice identifier (CLB_X4Y3 or SLICE_X465). Found " + AnchorLocation);
            }
                        
            if (GetAutoClearModuleSlotValue())
            {
                FuseNets fuseCmd = new FuseNets();
                fuseCmd.NetlistContainerName = NetlistContainerName;
                CommandExecuter.Instance.Execute(fuseCmd);
            }
        }
        
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected void SetBaseClassParametersAndExecuteCommand(AddInstantiationCommand addCmd)
        {
            addCmd.AutoClearModuleSlot = GetAutoClearModuleSlotValue();
            addCmd.InstanceName = InstanceName;
            addCmd.LibraryElementName = LibraryElementName;
            addCmd.NetlistContainerName = NetlistContainerName;
            addCmd.PrintProgress = PrintProgress;
            addCmd.Profile = Profile;
            addCmd.Mute = Mute;
            CommandExecuter.Instance.Execute(addCmd);
        }

        [Parameter(Comment = "The location string of the anchor. Either a tile or a slice identifier (CLB_X4Y3 or SLICE_X465)")]
        public string AnchorLocation = "CLB_X4Y3";

        protected abstract bool GetAutoClearModuleSlotValue();
    }

    [CommandDescription(Description = "Place a connection macro (clears the module slot prior to placement)", Wrapper = true, Publish = true)]
    class PlaceConnectionPrimitve : PlaceCommand
    {
        protected override bool GetAutoClearModuleSlotValue()
        {
            return false;
        }

    }

    [CommandDescription(Description = "Place a module (clears the module slot prior to placement)", Wrapper=true, Publish=true)]
    class PlaceModule : PlaceCommand
    {
        protected override bool GetAutoClearModuleSlotValue()
        {
            return true;
        }
    }
}
