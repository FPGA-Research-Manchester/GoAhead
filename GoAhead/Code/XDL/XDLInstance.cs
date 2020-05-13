using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GoAhead.Code.XDL
{
    [Serializable]
    public class XDLInstance : Instance
    {
        public override string ToString()
        {
            // TODO XDL needs slice configuration??
            return m_code.ToString();
        }

        [DefaultValue("undefined module reference"), DataMember]
        public string ModuleReference { get; set; }
    }
}