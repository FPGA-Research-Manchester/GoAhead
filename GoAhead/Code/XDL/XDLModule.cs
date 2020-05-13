using System;
using System.Text;

namespace GoAhead.Code.XDL
{
    [Serializable]
    public class XDLModule : XDLContainer
    {
        public void AddCode(string line)
        {
            m_xdlCode.Append(line);
        }

        public void AddCode(char c)
        {
            m_xdlCode.Append(c);
        }

        public void Clear()
        {
            m_xdlCode.Clear();
        }

        public void SetCode(string xdlCode)
        {
            Clear();
            AddCode(xdlCode);
        }

        public override string ToString()
        {
            return m_xdlCode.ToString();
        }

        protected StringBuilder m_xdlCode = new StringBuilder();
    }
}