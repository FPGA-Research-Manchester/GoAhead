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
            if (FPGA.FPGA.Instance.Contains(this.AnchorLocation))
            {
                Tile anchor = FPGA.FPGA.Instance.GetTile(this.AnchorLocation);
                AddSingleInstantiationByTile addCmd = new AddSingleInstantiationByTile();
                addCmd.AnchorLocation = this.AnchorLocation;
                this.SetBaseClassParametersAndExecuteCommand(addCmd);
            }
            else if (FPGA.FPGA.Instance.ContainsSlice(this.AnchorLocation))
            {
                Slice s = FPGA.FPGA.Instance.GetSlice(this.AnchorLocation);
                AddSingleInstantiationBySlice addCmd = new AddSingleInstantiationBySlice();
                addCmd.SliceName = this.AnchorLocation;
                this.SetBaseClassParametersAndExecuteCommand(addCmd);
            }
            else
            {
                throw new ArgumentException("Expecting either a tile or a slice identifier (CLB_X4Y3 or SLICE_X465). Found " + this.AnchorLocation);
            }
                        
            if (this.GetAutoClearModuleSlotValue())
            {
                FuseNets fuseCmd = new FuseNets();
                fuseCmd.NetlistContainerName = this.NetlistContainerName;
                CommandExecuter.Instance.Execute(fuseCmd);
            }
        }
        
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected void SetBaseClassParametersAndExecuteCommand(AddInstantiationCommand addCmd)
        {
            addCmd.AutoClearModuleSlot = this.GetAutoClearModuleSlotValue();
            addCmd.InstanceName = this.InstanceName;
            addCmd.LibraryElementName = this.LibraryElementName;
            addCmd.NetlistContainerName = this.NetlistContainerName;
            addCmd.PrintProgress = this.PrintProgress;
            addCmd.Profile = this.Profile;
            addCmd.Mute = this.Mute;
            CommandExecuter.Instance.Execute(addCmd);
        }

        [Parameter(Comment = "The location string of the anchor. Either a tile or a slice identifier (CLB_X4Y3 or SLICE_X465)")]
        public String AnchorLocation = "CLB_X4Y3";

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
