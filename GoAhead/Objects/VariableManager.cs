using System;
using System.Collections.Generic;

namespace GoAhead.Objects
{
    public class VariableManager : Interfaces.Subject
    {
        private VariableManager()
        {
        }

        public static VariableManager Instance = new VariableManager();

        public void Set(string variable, string value)
        {
            m_variables[variable] = value;

            // notify observers about change
            Notfiy(null);
        }

        public void Unset(string variable)
        {
            if (!IsSet(variable))
            {
                throw new ArgumentException("A variable named " + variable + " has not been set");
            }
            m_variables.Remove(variable);

            // notify observers about change
            Notfiy(null);
        }

        public IEnumerable<string> GetAllVariableNames()
        {
            return m_variables.Keys;
        }

        public IEnumerable<string> GetAllVariableValues()
        {
            return m_variables.Values;
        }

        public string GetValue(string variable)
        {
            return m_variables[variable];
        }

        public bool IsSet(string variable)
        {
            return m_variables.ContainsKey(variable);
        }

        /// <summary>
        /// translate value to a before set value and return the translation. if no variable is found, value is returned
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Resolve(string value)
        {
            string resolvedValue = value;
            foreach (KeyValuePair<string, string> entry in m_variables)
            {
                resolvedValue = resolvedValue.Replace("%" + entry.Key + "%", entry.Value);
                //resolvedValue = Regex.Replace(resolvedValue, "%" + entry.Key + "%", entry.Value);
            }

            return resolvedValue;
        }

        private Dictionary<string, string> m_variables = new Dictionary<string, string>();
    }
}