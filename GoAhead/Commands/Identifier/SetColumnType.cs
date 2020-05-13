using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Define column type for the report function in command PrintColumnTypes", Wrapper = false, Publish = true)]
    public class SetColumnTypeNames : Command
    {
        protected override void DoCommandAction()
        {
            Objects.ColumnTypeNameManager.Instance.AddTypeNameByResource(ColumnTypeName, Resources);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of colum type")]
        public string ColumnTypeName = "L";

        [Parameter(Comment = "The resource string that we define a name for")]
        public string Resources = "SLICEL,SLICEL";
    }
}
