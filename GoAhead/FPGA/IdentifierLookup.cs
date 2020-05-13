using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GoAhead.FPGA
{
    [Serializable]
    public class IdentifierLookup : ISerializable
    {
        public IdentifierLookup()
        {
        }

        private void AddIdentifier(string identifier)
        {
            if (!m_identifier.ContainsKey(identifier))
            {
                uint key = (uint )m_identifier.Count;
                string internedIdentifier = string.Intern(identifier);
                m_identifier.Add(internedIdentifier, key);
                m_keys.Add(key, internedIdentifier);
            }

            // test only
            uint idKey = GetKey(identifier);
            if (!GetIdentifier(idKey).Equals(identifier))
            {
                throw new ArgumentException("Hash collision for " + identifier);
            }
        }

        public string GetIdentifier(uint key)
        {
            if (!m_keys.ContainsKey(key))
            {
                return "";
            }

            return m_keys[key];
        }

        public uint GetKey(string identifier)
        {
            if (!m_identifier.ContainsKey(identifier))
            {
                AddIdentifier(identifier);
            }

            uint key = m_identifier[identifier];
            if (key > 0xFFFFF)
            {
                Console.WriteLine("key overflow with " + identifier);
            }
            return key;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<uint, string> keyIdentifierTupel in m_keys.OrderBy(t => t.Key))
            {
                buffer.Append(keyIdentifierTupel.Value + ";");
            }

            info.AddValue("identifierlist", buffer.ToString());
        }

        private IdentifierLookup(SerializationInfo info, StreamingContext context)
        {
            m_identifier.Clear();
            m_keys.Clear();

            string[] atoms = info.GetString("identifierlist").Split(';');
            for (uint i = 0; i < atoms.Length; i++)
            {
                if (!m_identifier.ContainsKey(atoms[i]))
                {
                    m_identifier.Add(atoms[i], i);
                    m_keys.Add(i, atoms[i]);
                }
                else
                {
                    // nothing to do
                }
            }
        }

        /// <summary>
        /// Number of known identifiers
        /// </summary>
        public int Count
        {
            get { return m_identifier.Count; }
        }

        private Dictionary<string, uint> m_identifier = new Dictionary<string, uint>();
        private Dictionary<uint, string> m_keys = new Dictionary<uint, string>();
    }
}