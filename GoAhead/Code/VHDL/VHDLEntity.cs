using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.VHDL
{
    public class VHDLEntity : VHDLSignalList
    {
        public VHDLEntity()
        {
        }

        public void SetDirection(string signalName, FPGATypes.PortDirection direction)
        {
            m_directions[signalName] = direction;
        }

        public FPGATypes.PortDirection GetDirection(string signalName)
        {
            if (!m_directions.ContainsKey(signalName))
            {
                throw new ArgumentException("Entity signal " + signalName + " does not exist");
            }

            return m_directions[signalName];
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            // we need signalWidths as a list ...
            List<KeyValuePair<string, int>> interfaceSignals = new List<KeyValuePair<string, int>>();
            foreach (KeyValuePair<string, int> tupel in m_signals)
            {
                interfaceSignals.Add(tupel);
            }
            for (int i = 0; i < interfaceSignals.Count; i++)
            {
                string signalName = interfaceSignals[i].Key;
                string line = "";

                PortMapper.MappingKind mappingKind = m_mappings.ContainsKey(signalName) ? m_mappings[signalName] : PortMapper.MappingKind.External;

                if (mappingKind == PortMapper.MappingKind.NoVector)
                {
                    line = "\t" + signalName + " : " + m_directions[signalName].ToString().ToLower() + " std_logic";
                }
                else
                {
                    line = "\t" + signalName + " : " + m_directions[signalName].ToString().ToLower() + " std_logic_vector(" + (interfaceSignals[i].Value - 1) + " downto 0)";
                }
                // ... to find the last index
                if (i < interfaceSignals.Count - 1)
                {
                    line += ";";
                }
                else
                {
                    line += ");";
                }
                buffer.AppendLine(line);
            }

            return buffer.ToString();
        }

        private Dictionary<string, FPGATypes.PortDirection> m_directions = new Dictionary<string, FPGATypes.PortDirection>();
    }
}