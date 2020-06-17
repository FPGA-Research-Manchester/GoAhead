using System;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    public class AddBlockerOrder : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            Objects.BlockerSettings.Instance.AddBlockerOrder(this.FamilyRegexp, this.DriverRegexp, this.SinkRegexp, this.ConnectAll, this.EndPip, this.VivadoPipConnector);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The regexp that identifies ports to use as drivers. '.*' matches all pips")]
        public String DriverRegexp = ".*";

        [Parameter(Comment = "The regexp that identifies ports to use as sinks. '.*' matches all pips")]
        public String SinkRegexp = ".*";

        [Parameter(Comment = "The special Vivado pip connector symbol, e.g., <1> for connecting long lines to begin ports")]
        public String VivadoPipConnector = "";
        

        [Parameter(Comment = "Wheter to connect all directly reachable other pips (true) or only one (false)")]
        public bool ConnectAll = true;

        [Parameter(Comment = "Wheter to connect all directly reachable other pips (true) or only one (false)")]
        public bool EndPip = false;
    }
}