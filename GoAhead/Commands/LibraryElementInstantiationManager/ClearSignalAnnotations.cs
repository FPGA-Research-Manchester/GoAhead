using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;

namespace GoAhead.Commands.LibraryElementInstantiationManager
{
    [CommandDescription(Description = "Clear all existing signal annotations of all library element instantiations", Wrapper = false)]
    class ClearSignalAnnotations : Command
    {
        protected override void DoCommandAction()
        {
            foreach (LibElemInst inst in LibraryElementInstanceManager.Instance.GetAllInstantiations().Where(inst => Regex.IsMatch(inst.InstanceName, InstantiationFilter)))
            {
                inst.PortMapper.Clear();
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Only consider those library element instantiations whose name matches this filter")]
        public string InstantiationFilter = ".*";
    }
}
