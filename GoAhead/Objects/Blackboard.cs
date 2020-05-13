using System;
using System.Collections.Generic;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    public class Blackboard
    {
        private Blackboard()
        {
        }

        public static Blackboard Instance = new Blackboard();

        public void ClearToolTipInfo(Tile t)
        {
            if (m_toolTipInfo.ContainsKey(t))
            {
                m_toolTipInfo[t] = "";
            }
        }

        public void AddToolTipInfo(Tile t, string info)
        {
            if (!m_toolTipInfo.ContainsKey(t))
            {
                m_toolTipInfo.Add(t, info);
            }
            else
            {
                m_toolTipInfo[t] += info;
            }
        }

        public bool HasToolTipInfo(Tile t)
        {
            return m_toolTipInfo.ContainsKey(t);
        }

        public string GetToolTipInfo(Tile t)
        {
            return m_toolTipInfo[t];
        }

        public string LastLoadCommandForFPGA = "";

        private Dictionary<Tile, string> m_toolTipInfo = new Dictionary<Tile, string>();
    }
}