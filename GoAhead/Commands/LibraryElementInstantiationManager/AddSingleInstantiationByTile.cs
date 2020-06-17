using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.XDLManipulation;
using GoAhead.FPGA;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.LibraryElementInstantiation
{
    [CommandDescription(Description = "Add a library element and mark all resources used by the library element as blocked.", Wrapper = true, Publish = true)]
    class AddSingleInstantiationByTile : AddInstantiationCommand
    {
        protected override void DoCommandAction()
        {
            LibraryElement libElement = Objects.Library.Instance.GetElement(this.LibraryElementName);

            Tile anchor = FPGA.FPGA.Instance.GetTile(this.AnchorLocation);

            if (libElement.ResourceShape.Anchor.AnchorSliceNumber >= anchor.Slices.Count)
            {
                throw new ArgumentException("Too few slices on tile " + anchor.Location + ". Expecting " + libElement.ResourceShape.Anchor.AnchorSliceNumber + " but found " + anchor.Slices.Count + " slice.");
            }

            if (Objects.IdentifierManager.Instance.IsMatch(anchor.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                anchor = FPGATypes.GetCLTile(anchor).FirstOrDefault();
            }

            if (this.AutoClearModuleSlot)
            {
                //this.FastAutoClearModuleSlotBeforeInstantiation(libElement, Enumerable.Repeat(anchor, 1));
                this.AutoClearModuleSlotBeforeInstantiation(libElement, Enumerable.Repeat(anchor, 1));
            }

            LibElemInst instantiation = new LibElemInst();
            instantiation.AnchorLocation = this.AnchorLocation;
            instantiation.InstanceName = this.Hierarchy + this.InstanceName;
            instantiation.LibraryElementName = this.LibraryElementName;
            instantiation.SliceNumber = libElement.ResourceShape.Anchor.AnchorSliceNumber;
            instantiation.SliceName = anchor.Slices[(int)libElement.ResourceShape.Anchor.AnchorSliceNumber].SliceName;

            Objects.LibraryElementInstanceManager.Instance.Add(instantiation);

            // mark source as blocked
            ExcludeInstantiationSourcesFromBlocking markSrc = new ExcludeInstantiationSourcesFromBlocking();
            markSrc.AnchorLocation = this.AnchorLocation;
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

        [Parameter(Comment = "The location string of the anchor")]
        public String AnchorLocation = "CLB_X4Y3";
    }
}
