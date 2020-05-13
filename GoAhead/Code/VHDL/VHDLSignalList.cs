using System;
using System.Collections.Generic;
using GoAhead.Objects;

namespace GoAhead.Code.VHDL
{
    public abstract class VHDLSignalList
    {
        public void Add(string signalName, int width, PortMapper.MappingKind mappingKind)
        {
            if (m_signals.ContainsKey(signalName))
            {
                throw new ArgumentException("Entity signal " + signalName + " exists already");
            }

            m_signals[signalName] = width;
            m_mappings[signalName] = mappingKind;
        }

        public void Add(string signalName, int width)
        {
            if (m_signals.ContainsKey(signalName))
            {
                throw new ArgumentException("Entity signal " + signalName + " exists already");
            }

            m_signals[signalName] = width;
        }

        public bool HasSignal(string signalName)
        {
            return m_signals.ContainsKey(signalName);
        }

        public bool HasMapping(string signalName)
        {
            return m_mappings.ContainsKey(signalName);
        }

        public PortMapper.MappingKind GetMapping(string signalName)
        {
            return m_mappings[signalName];
        }

        public int GetSignalWidth(string signalName)
        {
            if (!m_signals.ContainsKey(signalName))
            {
                throw new ArgumentException("Entity signal " + signalName + " does not exist");
            }

            return m_signals[signalName];
        }

        public void SetSignalWidth(string signalName, int width)
        {
            if (!m_signals.ContainsKey(signalName))
            {
                throw new ArgumentException("Entity signal " + signalName + " does not exist");
            }

            m_signals[signalName] = width;
        }

        public IEnumerable<Tuple<string, int>> GetSignals()
        {
            foreach (KeyValuePair<string, int> s in m_signals)
            {
                yield return new Tuple<string, int>(s.Key, s.Value);
            }
        }

        protected Dictionary<string, int> m_signals = new Dictionary<string, int>();
        protected Dictionary<string, PortMapper.MappingKind> m_mappings = new Dictionary<string, PortMapper.MappingKind>();
    }
}