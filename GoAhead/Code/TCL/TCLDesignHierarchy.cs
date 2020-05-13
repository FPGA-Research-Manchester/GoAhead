using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GoAhead.Code.TCL
{
    class TCLDesignHierarchy : NetlistElement
    {
        [DefaultValue("undefined bel type"), DataMember]
        public string Name { get; set; }

        public TCLProperties Properties = new TCLProperties();
    }
}
