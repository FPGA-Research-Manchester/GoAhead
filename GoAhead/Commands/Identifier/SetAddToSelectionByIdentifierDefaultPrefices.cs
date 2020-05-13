using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Identifier
{
    class SetAddToSelectionByIdentifierDefaultPrefices : Command
    {
        protected override void DoCommandAction()
        {
            Objects.IdentifierPrefixManager.Instance.Prefices = DefaultIdentifierPrefices;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The possible identifier prefices used in AddToSelectionByIdentifier (you may however overload this setting when using AddToSelectionByIdentifier)")]
        public List<string> DefaultIdentifierPrefices = new List<string>();
    }
}
