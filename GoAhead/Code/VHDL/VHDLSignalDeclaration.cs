using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Code.VHDL
{
    public class VHDLSignalDeclaration : VHDLSignalList
    {
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (KeyValuePair<string, int> tupel in m_signals)
            {
                string decl = "signal " + tupel.Key + " : std_logic_vector(" + (tupel.Value - 1) + " downto 0) := (others => '1');";
                buffer.AppendLine(decl);
            }

            foreach (KeyValuePair<string, int> tupel in m_signals)
            {
                string attr = "attribute s of " + tupel.Key + " : signal is \"true\";";
                buffer.AppendLine(attr);
            }

            foreach (KeyValuePair<string, int> tupel in m_signals)
            {
                string attr = "attribute keep of " + tupel.Key + " : signal is \"true\";";
                buffer.AppendLine(attr);
            }

            return buffer.ToString();
        }
    }
}