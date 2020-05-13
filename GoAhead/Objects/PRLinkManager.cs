using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    class PRLinkManager : IEnumerable<PRLink>
    {
        private PRLinkManager()
        {
        }

        public void Add(PRLink prLink)
        {
            m_prLinks.Add(prLink);
        }        

        public static PRLinkManager Instance = new PRLinkManager();
              
        public IEnumerator<PRLink> GetEnumerator()
        {
            return m_prLinks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<PRLink> m_prLinks = new List<PRLink>();
    }
    public class PRLink
    {
        public PRLink(Tile t, string netName)
        {
            Tile = t;
            Ports = new List<Port>();
            NetName = netName;
        }

        public void Add(Port port)
        {
            Ports.Add(port);
        }

        public readonly Tile Tile;
        public readonly List<Port> Ports;
        public readonly string NetName;
    }
    
}
