using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    [CommandDescription(Description = "An abstract base class for commands that print VHDL wrappers", Wrapper = false)]
    abstract class PrintVHDLWrapperCommand : CommandWithFileOutput
    {
        protected sealed override void DoCommandAction()
        {
            VHDLFile vhdlFile = new VHDLFile(this.EntityName);

            this.InstantiationFilter = Regex.Replace(this.InstantiationFilter, "\\\"", "");

            foreach (LibElemInst inst in Objects.LibraryElementInstanceManager.Instance.GetAllInstantiations().Where(i => Regex.IsMatch(i.InstanceName, this.InstantiationFilter)))
            {
                LibraryElement libElement = Objects.Library.Instance.GetElement(inst.LibraryElementName);

                // add each component once
                if (!vhdlFile.HasComponent(libElement.PrimitiveName))
                {
                    vhdlFile.Add(new VHDLComponent(libElement));
                }

                vhdlFile.Add(new VHDLInstantiation(vhdlFile, inst, libElement, this));
            }

            // call base class implementation
            this.PrintVHDLCode(vhdlFile);
        }

        protected abstract void PrintVHDLCode(VHDLFile vhdlFile);

        [Parameter(Comment = "Only consider those library lelement instantiations whose name matches this filter")]
        public String InstantiationFilter = ".*";

        [Parameter(Comment = "The entity name of the VHDL wrapper")]
        public String EntityName = "PartialSubsystem";
    }
}
