using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLPort : NetlistPort
    {
        public TCLPort()
        {
            ExternalName = "";
        }

        public TCLProperties Properties = new TCLProperties();
    }
}
