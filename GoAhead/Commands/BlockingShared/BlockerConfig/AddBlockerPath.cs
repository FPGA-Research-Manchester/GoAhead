using System;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    [CommandDescription(Description = "Configure the blocker to create paths via all ports that match HopRegexp using driver and sink as given", Wrapper = false, Publish = true)]
    public class AddBlockerPath : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            Objects.BlockerSettings.Instance.AddBlockerPath(this.FamilyRegexp, this.DriverRegexp, this.HopRegexp, this.SinkRegexp);
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "Where to start the path that")]
        public String DriverRegexp = "";

        [Parameter(Comment = "The port Where to start the path that")]
        public String HopRegexp = "";

        [Parameter(Comment = "Where to end the path")]
        public String SinkRegexp = "";
    }
}