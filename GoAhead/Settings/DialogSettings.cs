using System;
using System.Collections.Generic;

namespace GoAhead.Settings
{
    [Serializable]
    public class DialogSettings
    {
        public bool HasSetting(string caller)
        {
            return m_settings.ContainsKey(caller);
        }

        public string GetSetting(string caller)
        {
            return m_settings[caller];
        }

        public void AddOrUpdateSetting(string caller, string path)
        {
            m_settings[caller] = path;
        }

        private Dictionary<string, string> m_settings = new Dictionary<string, string>();
    }
}