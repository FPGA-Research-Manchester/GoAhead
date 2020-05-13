using System;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    public class SelectionManager
    {
        private SelectionManager()
        {
        }

        public static SelectionManager Instance = new SelectionManager();

        public Tile Anchor
        {
            get { return m_anchor; }
            set { m_anchor = value; }
        }

        public string XAnchorName
        {
            get { return m_xAnchorName; }
            set { m_xAnchorName = value; }
        }

        public string YAnchorName
        {
            get { return m_yAnchorName; }
            set { m_yAnchorName = value; }
        }

        private Tile m_anchor = null;
        private string m_xAnchorName = "";
        private string m_yAnchorName = "";
    }
}