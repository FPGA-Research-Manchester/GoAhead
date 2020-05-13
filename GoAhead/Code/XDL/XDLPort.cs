using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using GoAhead.FPGA;

namespace GoAhead.Code.XDL
{
    [Serializable]
    public class XDLPort : NetlistPort
    {
        public override string ToString()
        {
            //  port "LI0" "center" "D1";
            string p = "port \"" + ExternalName + "\" \"" + InstanceName + "\" \"" + SlicePort + "\";";
            return p;
        }

        /// <summary>
        /// The name of instance (slice) in which the ports resides
        /// </summary>
        [DefaultValue("undefined slice"), DataMember]
        public string InstanceName { get; set; }

        [DefaultValue("undefined slice port"), DataMember]
        public string SlicePort { get; set; }

        /// <summary>
        /// Ports marked as ConstantValuePort can be assigned no any HDL signals (see commad AnnotateSignalNames)
        /// </summary>
        [DefaultValue(false), DataMember]
        public bool ConstantValuePort { get; set; }
    }
}