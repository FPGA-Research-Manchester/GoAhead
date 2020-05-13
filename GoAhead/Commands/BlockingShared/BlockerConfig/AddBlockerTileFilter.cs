using System;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    public class AddBlockerTileFilter : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            Objects.BlockerSettings.Instance.AddTileFilter(FamilyRegexp, Regexp);
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The regular expression which identifies all ports not to block")]
        public string Regexp = "";
    }
}