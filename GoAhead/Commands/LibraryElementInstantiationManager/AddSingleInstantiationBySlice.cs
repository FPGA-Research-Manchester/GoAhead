using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.XDLManipulation;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.LibraryElementInstantiation
{
    [CommandDescription(Description = "Instantiate a library element and mark all resources used by the library element as blocked.", Wrapper = true, Publish = true)]
    class AddSingleInstantiationBySlice : AddInstantiationCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer netlistContainer = this.GetNetlistContainer();
            Slice anchor = FPGA.FPGA.Instance.GetSlice(this.SliceName);
            LibraryElement libElement = Objects.Library.Instance.GetElement(this.LibraryElementName);

            if (anchor == null)
            {
                throw new ArgumentException("Can not find Slice " + this.SliceName);
            }

            if (this.AutoClearModuleSlot)
            {
                this.AutoClearModuleSlotBeforeInstantiation(libElement, Enumerable.Repeat(anchor.ContainingTile, 1));
            }

            LibElemInst instantiation = new LibElemInst();
            instantiation.AnchorLocation = anchor.ContainingTile.Location;
            instantiation.InstanceName = this.Hierarchy + this.InstanceName;
            instantiation.LibraryElementName = this.LibraryElementName;
            instantiation.SliceNumber = anchor.ContainingTile.GetSliceNumberByName(this.SliceName);
            instantiation.SliceName = this.SliceName;

            Objects.LibraryElementInstanceManager.Instance.Add(instantiation);

            // mark source as blocked
            ExcludeInstantiationSourcesFromBlocking markSrc = new ExcludeInstantiationSourcesFromBlocking();
            markSrc.AnchorLocation = anchor.ContainingTile.Location;
            markSrc.LibraryElementName = this.LibraryElementName;

            CommandExecuter.Instance.Execute(markSrc);

            SaveLibraryElementInstantiation saveCmd = new SaveLibraryElementInstantiation();
            saveCmd.AddDesignConfig = false;
            saveCmd.InsertPrefix = true;
            saveCmd.InstanceName = this.InstanceName;
            saveCmd.NetlistContainerName = this.NetlistContainerName;
            CommandExecuter.Instance.Execute(saveCmd);

            if (this.AutoFuse)
            {
                FuseNets fuseCmd = new FuseNets();
                fuseCmd.NetlistContainerName = this.NetlistContainerName;
                fuseCmd.Mute = this.Mute;
                fuseCmd.Profile = this.Profile;
                fuseCmd.PrintProgress = this.PrintProgress;
                CommandExecuter.Instance.Execute(fuseCmd);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The slice name")]
        public String SliceName = "SLICEM_X34Y34";
    }
}
