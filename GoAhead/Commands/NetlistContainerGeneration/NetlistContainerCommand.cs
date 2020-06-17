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
            String netlistContainerName = String.IsNullOrEmpty(this.NetlistContainerName) ? NetlistContainerManager.DefaultNetlistContainerName : this.NetlistContainerName;   

            return Objects.NetlistContainerManager.Instance.Get(netlistContainerName);
        }

        [Parameter(Comment = "The netlist container this command works on. Leave this parameter empty to work on the default netlist")]
        public String NetlistContainerName = NetlistContainerManager.DefaultNetlistContainerName;
    }

    public abstract class NetlistContainerCommandWithFileOutput : CommandWithFileOutput
    {
        protected NetlistContainer GetNetlistContainer()
        {
            String netlistContainerName = String.IsNullOrEmpty(this.NetlistContainerName) ? NetlistContainerManager.DefaultNetlistContainerName : this.NetlistContainerName;

            return Objects.NetlistContainerManager.Instance.Get(netlistContainerName);
        }

        [Parameter(Comment = "The netlist container this command works on. Leave this parameter empty to work on the default netlist")]
        public String NetlistContainerName = NetlistContainerManager.DefaultNetlistContainerName;
    }
}
