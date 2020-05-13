using System;
using System.Text;

namespace GoAhead.Code
{
    [Serializable]
    public abstract class NetlistElement
    {
        virtual public void AddCode(string line)
        {
            m_code.Append(line);
        }

        virtual public void AddCode(char c)
        {
            m_code.Append(c);
        }

        public string GetCode()
        {
            return m_code.ToString();
        }

        public override string ToString()
        {
            return m_code.ToString();
        }

        public virtual void Clear()
        {
            m_code.Clear();
        }

        public void SetCode(string code)
        {
            Clear();
            AddCode(code);
        }

        protected StringBuilder m_code = new StringBuilder();
    }
}