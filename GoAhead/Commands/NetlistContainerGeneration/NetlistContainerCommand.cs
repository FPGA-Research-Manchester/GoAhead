using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    public abstract class NetlistContainerCommand : Command
    {
        protected NetlistContainer GetNetlistContainer()        
        {
            string netlistContainerName = string.IsNullOrEmpty(NetlistContainerName) ? NetlistContainerManager.DefaultNetlistContainerName : NetlistContainerName;   

            return NetlistContainerManager.Instance.Get(netlistContainerName);
        }

        [Parameter(Comment = "The netlist container this command works on. Leave this parameter empty to work on the default netlist")]
        public string NetlistContainerName = NetlistContainerManager.DefaultNetlistContainerName;
    }

    public abstract class NetlistContainerCommandWithFileOutput : CommandWithFileOutput
    {
        protected NetlistContainer GetNetlistContainer()
        {
            string netlistContainerName = string.IsNullOrEmpty(NetlistContainerName) ? NetlistContainerManager.DefaultNetlistContainerName : NetlistContainerName;

            return NetlistContainerManager.Instance.Get(netlistContainerName);
        }

        [Parameter(Comment = "The netlist container this command works on. Leave this parameter empty to work on the default netlist")]
        public string NetlistContainerName = NetlistContainerManager.DefaultNetlistContainerName;
    }
}
