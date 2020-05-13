using System;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    public class AddBlockerOrder : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            Objects.BlockerSettings.Instance.AddBlockerOrder(FamilyRegexp, DriverRegexp, SinkRegexp, ConnectAll, EndPip, VivadoPipConnector);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The regexp that identifies ports to use as drivers. '.*' matches all pips")]
        public string DriverRegexp = ".*";

        [Parameter(Comment = "The regexp that identifies ports to use as sinks. '.*' matches all pips")]
        public string SinkRegexp = ".*";

        [Parameter(Comment = "The special Vivado pip connector symbol, e.g., <1> for connecting long lines to begin ports")]
        public string VivadoPipConnector = "";
        

        [Parameter(Comment = "Wheter to connect all directly reachable other pips (true) or only one (false)")]
        public bool ConnectAll = true;

        [Parameter(Comment = "Wheter to connect all directly reachable other pips (true) or only one (false)")]
        public bool EndPip = false;
    }
}