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
            NetlistContainer netlistContainer = GetNetlistContainer();
            Slice anchor = FPGA.FPGA.Instance.GetSlice(SliceName);
            LibraryElement libElement = Objects.Library.Instance.GetElement(LibraryElementName);

            if (anchor == null)
            {
                throw new ArgumentException("Can not find Slice " + SliceName);
            }

            if (AutoClearModuleSlot)
            {
                AutoClearModuleSlotBeforeInstantiation(libElement, Enumerable.Repeat(anchor.ContainingTile, 1));
            }

            LibElemInst instantiation = new LibElemInst();
            instantiation.AnchorLocation = anchor.ContainingTile.Location;
            instantiation.InstanceName = Hierarchy + InstanceName;
            instantiation.LibraryElementName = LibraryElementName;
            instantiation.SliceNumber = anchor.ContainingTile.GetSliceNumberByName(SliceName);
            instantiation.SliceName = SliceName;

            LibraryElementInstanceManager.Instance.Add(instantiation);

            // mark source as blocked
            ExcludeInstantiationSourcesFromBlocking markSrc = new ExcludeInstantiationSourcesFromBlocking();
            markSrc.AnchorLocation = anchor.ContainingTile.Location;
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

        [Parameter(Comment = "The slice name")]
        public string SliceName = "SLICEM_X34Y34";
    }
}
