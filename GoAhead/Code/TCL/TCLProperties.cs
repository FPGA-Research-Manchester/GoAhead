using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLProperty
    {
        public TCLProperty(string name, string value, bool readOnly)
        {
            Name = name;
            Value = value;
            ReadOnly = readOnly;
        }

        public readonly string Name;
        public readonly string Value;
        public readonly bool ReadOnly;

         
        public override string ToString()
        {
            return Name + "=" + Value;
        }
    }


    [Serializable]
    public class TCLProperties : IEnumerable<TCLProperty>
    {
        public TCLProperties()
        {
        }

        /// <summary>
        /// Copy constuctor
        /// </summary>
        /// <param name="other"></param>
        public TCLProperties(TCLProperties other)
        {
            foreach (TCLProperty p in other)
            {
                SetProperty(p.Name, p.Value, p.ReadOnly);
            }
        }

        public void SetProperty(string name, string value, bool readOnly)
        {
            if (m_properties.ContainsKey(name))
            {
                TCLProperty prop = new TCLProperty(name, value, readOnly);
                m_properties[name] = prop;
            }
            else
            {
                TCLProperty prop = new TCLProperty(name, value, readOnly);
                m_properties.Add(name, prop);
            }
        }

        public bool HasProperty(string name)
        {
            return m_properties.ContainsKey(name);
        }

        public string GetValue(string name)
        {
            if (!m_properties.ContainsKey(name))
            {
                throw new ArgumentException("The property " + name + " was not found");
            }
            else
            {
                return m_properties[name].Value;
            }
        }

        public IEnumerator<TCLProperty> GetEnumerator()
        {
            return m_properties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



        private Dictionary<string, TCLProperty> m_properties = new Dictionary<string, TCLProperty>();
    }
}
