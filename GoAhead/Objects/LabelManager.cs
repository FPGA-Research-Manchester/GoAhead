using System;
using System.Collections.Generic;

namespace GoAhead.Objects
{
    public class LabelManager
    {
        private LabelManager()
        {
        }

        public static LabelManager Instance = new LabelManager();

        public bool Contains(string labelName)
        {
            return m_labels.ContainsKey(labelName);
        }

        public int GetCommandListIndex(string labelName)
        {
            return m_labels[labelName];
        }

        /// <summary>
        /// Set a named label pointing into the command executer command list index
        /// </summary>
        /// <param name="labelName"></param>
        /// <param name="commandListIndex"></param>
        public void SetLabel(string labelName, int commandListIndex)
        {
            m_labels[labelName] = commandListIndex;
        }

        private Dictionary<string, int> m_labels = new Dictionary<string, int>();
    }
}