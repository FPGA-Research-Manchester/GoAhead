using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.Objects;

namespace GoAhead.Code.VHDL
{
    public class VHDLComponent
    {
        public VHDLComponent(LibraryElement libraryElement)
        {
            m_libraryElement = libraryElement;
        }

        public override string ToString()
        {
            if (m_libraryElement.SubElements.Count() > 0)
            {
                return "Composed element, no code available";
            }
            List<string> buffer = new List<string>();
            buffer.Add("component " + m_libraryElement.PrimitiveName + " is Port (");

            if (m_libraryElement.Containter != null)
            {
                foreach (NetlistPort port in m_libraryElement.Containter.Ports)
                {
                    buffer.Add("\t" + port.ExternalName + " : " + port.Direction.ToString().ToLower() + " std_logic;");
                }
            }

            buffer[buffer.Count - 1] = Regex.Replace(buffer[buffer.Count - 1], ";", "");
            buffer.Add(");");
            buffer.Add("end component " + m_libraryElement.PrimitiveName + ";");

            StringBuilder all = new StringBuilder();
            foreach (string s in buffer)
            {
                all.AppendLine(s);
            }
            return all.ToString();
        }

        public string Name
        {
            get { return m_libraryElement.PrimitiveName; }
        }

        private readonly LibraryElement m_libraryElement;
    }
}