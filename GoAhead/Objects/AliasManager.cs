using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.Commands;

namespace GoAhead.Objects
{
    public class AliasManager
    {
        private AliasManager()
        {
        }

        public static AliasManager Instance = new AliasManager();

        public void AddAlias(string aliasName, string commands)
        {
            bool nameConflict = CommandStringParser.GetAllCommandTypes().Where(t => t.Name.Equals(aliasName)).Any();
            if (nameConflict)
            {
                throw new ArgumentException(aliasName + " is an invalid alias name as it overwrites an existing command name");
            }

            m_aliases[aliasName] = commands;
        }

        public string GetCommand(string aliasName)
        {
            string aliasNameWithoutSemicolon = aliasName.EndsWith(";") ? aliasName.Substring(0, aliasName.Length - 1) : aliasName;
            return m_aliases[aliasNameWithoutSemicolon];
        }

        public bool HasAlias(string aliasName)
        {
            string aliasNameWithoutSemicolon = aliasName.EndsWith(";") ? aliasName.Substring(0, aliasName.Length - 1) : aliasName;
            return m_aliases.ContainsKey(aliasNameWithoutSemicolon);
        }

        private Dictionary<string, string> m_aliases = new Dictionary<string, string>();
    }
}