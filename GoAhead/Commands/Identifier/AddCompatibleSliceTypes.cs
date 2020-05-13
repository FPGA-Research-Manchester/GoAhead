using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description="Add a pair of compatible slice types for the given Family. The slice type RequiredSliceType may be placed at TargetSliceType")]
    class AddCompatibleSliceTypes : Command
    {
        protected override void DoCommandAction()
        {
            Objects.SliceCompare.Instance.Add(FamilyRegexp, RequiredSliceType, PossibleTargetSliceType);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "e.g. Spartan6")]
        public string FamilyRegexp = "Spartan6";

        [Parameter(Comment = "Required slice type (from to be placed macro)")]
        public string RequiredSliceType = "SLICEX";

        [Parameter(Comment = "A possible target slice type (the slice type where an instance shall be relocated to)")]
        public string PossibleTargetSliceType = "SLICEX";
    }
}
