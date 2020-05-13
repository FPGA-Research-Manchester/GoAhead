using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description="Delete an element from the library", Wrapper=false, Publish=true)]
    class RemoveLibraryElement : Command
    {
        protected override void DoCommandAction()
        {
            Objects.Library.Instance.Remove(LibraryElementName);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the element to remove, e.g BM_S6_L4_R4_double")]
        public string LibraryElementName = "BM_S6_L4_R4_double";     
    }
}
