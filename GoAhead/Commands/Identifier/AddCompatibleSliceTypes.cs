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
            Objects.SliceCompare.Instance.Add(this.FamilyRegexp, this.RequiredSliceType, this.PossibleTargetSliceType);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "e.g. Spartan6")]
        public String FamilyRegexp = "Spartan6";

        [Parameter(Comment = "Required slice type (from to be placed macro)")]
        public String RequiredSliceType = "SLICEX";

        [Parameter(Comment = "A possible target slice type (the slice type where an instance shall be relocated to)")]
        public String PossibleTargetSliceType = "SLICEX";
    }
}
