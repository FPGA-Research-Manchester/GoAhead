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
            LibraryElement libElement = Objects.Library.Instance.GetElement(LibraryElementName);

            Tile anchor = FPGA.FPGA.Instance.GetTile(AnchorLocation);

            if (libElement.ResourceShape.Anchor.AnchorSliceNumber >= anchor.Slices.Count)
            {
                throw new ArgumentException("Too few slices on tile " + anchor.Location + ". Expecting " + libElement.ResourceShape.Anchor.AnchorSliceNumber + " but found " + anchor.Slices.Count + " slice.");
            }

            if (IdentifierManager.Instance.IsMatch(anchor.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                anchor = FPGATypes.GetCLTile(anchor).FirstOrDefault();
            }

            if (AutoClearModuleSlot)
            {
                //this.FastAutoClearModuleSlotBeforeInstantiation(libElement, Enumerable.Repeat(anchor, 1));
                AutoClearModuleSlotBeforeInstantiation(libElement, Enumerable.Repeat(anchor, 1));
            }

            LibElemInst instantiation = new LibElemInst();
            instantiation.AnchorLocation = AnchorLocation;
            instantiation.InstanceName = Hierarchy + InstanceName;
            instantiation.LibraryElementName = LibraryElementName;
            instantiation.SliceNumber = libElement.ResourceShape.Anchor.AnchorSliceNumber;
            instantiation.SliceName = anchor.Slices[(int)libElement.ResourceShape.Anchor.AnchorSliceNumber].SliceName;

            LibraryElementInstanceManager.Instance.Add(instantiation);

            // mark source as blocked
            ExcludeInstantiationSourcesFromBlocking markSrc = new ExcludeInstantiationSourcesFromBlocking();
            markSrc.AnchorLocation = AnchorLocation;
            markSrc.LibraryElementName = LibraryElementName;
            CommandExecuter.Instance.Execute(markSrc);

            SaveLibraryElementInstantiation saveCmd = new SaveLibraryElementInstantiation();
            saveCmd.AddDesignConfig = false;
            saveCmd.InsertPrefix = true;
            saveCmd.InstanceName = InstanceName;
            saveCmd.NetlistContainerName = NetlistContainerName;
            CommandExecuter.Instance.Execute(saveCmd);

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

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The location string of the anchor")]
        public string AnchorLocation = "CLB_X4Y3";
    }
}
