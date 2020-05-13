using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GoAhead.Commands.Debug;

namespace GoAhead.Objects
{
    public class NetlistContainerManager
    {
        private NetlistContainerManager()
        {
            PrintGoAheadInternals.ObjectsToPrint.Add(this);
        }

        public void Add(NetlistContainer netlistContainer)
        {
            if (Contains(netlistContainer.Name))
            {
                throw new ArgumentException(this + ": A netlist container with the name " + netlistContainer.Name + " has already been added");
            }
            m_netlistContainer.Add(netlistContainer);
        }

        public string GetName()
        {
            int index = 0;
            while (Contains("temp_" + index))
            {
                index++;
            }
            return "temp_" + index;
        }

        public bool Contains(string netlistContainerName)
        {
            NetlistContainer probe = m_netlistContainer.FirstOrDefault(macro => macro.Name.Equals(netlistContainerName));

            return probe != null;
        }

        public NetlistContainer Get(string netlistContainerName)
        {
            NetlistContainer netlistContainer = m_netlistContainer.FirstOrDefault(m => m.Name.Equals(netlistContainerName));
            if (netlistContainer == null)
            {
                throw new ArgumentException("A netlist container with the name " + netlistContainerName + " has not been added");
            }

            return netlistContainer;
        }

        public IEnumerable<NetlistContainer> NetlistContainer
        {
            get { return m_netlistContainer; }
        }

        public void Reset()
        {
            m_netlistContainer.Clear();
        }

        public BindingList<NetlistContainer> NetlistContainerBindingList
        {
            get { return m_netlistContainer; }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            if (m_netlistContainer.Count == 0)
            {
                result.AppendLine("No netlist container added");
            }
            else
            {
                result.AppendLine("Added netlist container are");
                foreach (NetlistContainer m in NetlistContainer)
                {
                    PrintStatistics printCmd = new PrintStatistics();
                    printCmd.NetlistContainerName = m.Name;
                    result.AppendLine(m.Name + " (use " + printCmd.ToString() + " for more information)");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// The class MacroManager is a Singelton
        /// </summary>
        public static NetlistContainerManager Instance = new NetlistContainerManager();

        public const string DefaultNetlistContainerName = "default_netlist_container";

        private BindingList<NetlistContainer> m_netlistContainer = new BindingList<NetlistContainer>();
    }
}