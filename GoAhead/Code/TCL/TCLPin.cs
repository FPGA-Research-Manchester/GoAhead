using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLPin : NetlistElement
    {
        public TCLPin()
        {
            Name = "";
        }

        public TCLPin(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public TCLInstance Instance { get; set; }
        public TCLNet Net { get; set; }
        public TCLProperties Properties = new TCLProperties();

    }
}
