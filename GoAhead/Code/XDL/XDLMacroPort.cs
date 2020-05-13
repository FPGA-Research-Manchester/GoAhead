using System;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Code.XDL
{
    public class XDLMacroPort
    {
        /// <summary>
        /// port "$extpin" "SLICE_X57Y77" "B6";
        /// </summary>
        /// <param name="portName">$extpin</param>
        /// <param name="port">SLICE_X57Y77</param>
        /// <param name="where">B6</param>
        public XDLMacroPort(string portName, Port port, Slice where)
        {
            m_portName = portName;
            m_port = port;
            m_where = where;
            m_sliceName = "";
            //if removing L_ or M_ does not work, promote m_dummyNetPortName to the addport command
            if (Regex.IsMatch(port.ToString(), "^(L|M)_"))
            {
                m_dummyNetPortName = port.ToString().Substring(2, port.ToString().Length - 2);
            }
            else if (Regex.IsMatch(port.ToString(), "^CLB[L|M][L|M]_([L|M])+_"))
            {
                // virtex 6
                m_dummyNetPortName = Regex.Replace(port.ToString(), "^CLB[L|M][L|M]_([L|M])+_", "");
            }
            else
            {
                m_dummyNetPortName = port.ToString();
                // throw new NotImplementedException("Removing L_ or M_ failed in XDLPort");
            }
        }

        /// <summary>
        /// Overwrite the instance name on which this port resides
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="port"></param>
        /// <param name="where"></param>
        /// <param name="sliceName"></param>
        public XDLMacroPort(string portName, Port port, Slice where, string sliceName)
        {
            m_portName = portName;
            m_port = port;
            m_where = where;
            m_sliceName = sliceName;
            m_dummyNetPortName = port.ToString();
        }

        /// <summary>
        /// might be changed to add indeces
        /// </summary>
        public string PortName
        {
            get { return m_portName; }
            set { m_portName = value; }
        }

        public Slice Slice
        {
            get { return m_where; }
        }

        public string SliceName
        {
            get { return string.IsNullOrEmpty(m_sliceName) ? Slice.ToString() : m_sliceName; }
        }

        public Port Port
        {
            get { return m_port; }
        }

        public string DummyNetPortName
        {
            get { return m_dummyNetPortName; }
        }

        private string m_portName;
        private readonly Port m_port;
        private readonly Slice m_where;
        private readonly string m_sliceName;
        private readonly string m_dummyNetPortName;
    }
}