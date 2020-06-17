using System;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Create an alias for which upon call the semicolon seperated commands are executed", Wrapper = false)]
    public class AddAlias : Command
    {
        protected override void DoCommandAction()
        {
            Objects.AliasManager.Instance.AddAlias(this.AliasName, this.Commands);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of alias")]
        public String AliasName = "select_an_area";

        [Parameter(Comment = "A semicolon seperated list of GoAhead commands that will be executed if the alias name is used as a command.")]
        public String Commands = "ClearSelection;AddToSelectionXY UpperLeftX=138 UpperLeftY=85 LowerRightX=145 LowerRightY=125;ExpandSelection;";
    }
}