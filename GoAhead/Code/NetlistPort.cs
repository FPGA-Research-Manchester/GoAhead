using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoAhead.FPGA;

namespace GoAhead.Code
{
    [Serializable]
    public abstract class NetlistPort : NetlistElement
    {
        [DefaultValue("undefined direction"), DataMember]
        public FPGATypes.PortDirection Direction { get; set; }

        [DefaultValue("undefined name"), DataMember]
        public string ExternalName { get; set; }
    }
}
