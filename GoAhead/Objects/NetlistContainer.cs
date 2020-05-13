using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoAhead.Code;
using GoAhead.FPGA;
using GoAhead.Commands.Data;

namespace GoAhead.Objects
{
    [Serializable]
    public class NetlistContainer
    {
        public NetlistContainer()
        {
            Name = NetlistContainerManager.Instance.GetName();
        }

        public NetlistContainer(string netlistContainerName)
        {
            Name = netlistContainerName;
        }

        public void Add(Slice slice)
        {
            // overwrite
            if (!m_slices.ContainsKey(slice))
            {
                m_slices.Add(slice, slice.SliceName);
            }
            else
            {
                m_slices[slice] = slice.SliceName;
            }
        }

        public void Add(Instance instance)
        {
            if (m_instances.ContainsKey(instance.Name))
            {
                throw new ArgumentException("An instance named " + instance.Name + " has already been added");
            }

            instance.InstanceIndex = m_instances.Count;

            // add instance
            m_instances.Add(instance.Name, instance);

            // add location->instance mapping
            if (!m_locationInstanceMapping.ContainsKey(instance.Location))
            {
                m_locationInstanceMapping.Add(instance.Location, new List<Instance>());
            }
            m_locationInstanceMapping[instance.Location].Add(instance);
            
            // add slice->instance mapping
            if (!m_sliceInstanceMapping.ContainsKey(instance.SliceName))
            {
                m_sliceInstanceMapping.Add(instance.SliceName, new List<Instance>());
            }
            m_sliceInstanceMapping[instance.SliceName].Add(instance);
        }

        public void Add(Net net)
        {
            if (m_nets.ContainsKey(net.Name))
            {
                return;
                throw new ArgumentException("A net named " + net.Name + " has already been added");
            }
            m_nets.Add(net.Name, net);
            m_lastNetAdded = net;
        }

        public void Add(NetlistPort port)
        {
            if (m_ports.Contains(port))
            {
                return;
                throw new ArgumentException("A port named " + port.ExternalName + " has already been added");
            }

            m_ports.Add(port);
        }

        public IEnumerable<Slice> GetAllSlicesTemplates()
        {
            foreach (KeyValuePair<Slice, string> next in m_slices)
            {
                yield return next.Key;
            }
        }

        public Net GetNet(string netName)
        {
            if (!m_nets.ContainsKey(netName))
            {
                throw new ArgumentException("A net with the name " + netName + " has not been added");
            }
            else
            {
                return m_nets[netName];
            }
        }

        public virtual Net GetAnyNet()
        {
            if (m_nets.Count == 0)
            {
                throw new ArgumentException("GetAnyNet: No nets found in netlist container " + Name);
            }
            else
            {
                return m_nets.Values.First();
            }
        }

        public Slice GetTopLeftSlice()
        {
            int topLeftX = int.MaxValue;
            int topLeftY = int.MaxValue;

            Slice topLeftSlice = null;

            // the anchor must reside on an instantiated slice within a CLB
            foreach (Instance inst in Instances.Where(i => IdentifierManager.Instance.IsMatch(i.Location, IdentifierManager.RegexTypes.CLB)))
            {
                Tile t = FPGA.FPGA.Instance.GetTile(inst.Location);
                Slice s = t.GetSliceByName(inst.SliceName);
                //Slice s = FPGA.FPGA.Instance.GetSlice(inst.SliceName);
                if (s.ContainingTile.TileKey.X < topLeftX)
                {
                    topLeftX = s.ContainingTile.TileKey.X;
                    topLeftY = s.ContainingTile.TileKey.Y;
                    //used this slice as an anchor
                    topLeftSlice = s;
                }
                else if (s.ContainingTile.TileKey.X == topLeftX && s.ContainingTile.TileKey.Y < topLeftY)
                {
                    topLeftX = s.ContainingTile.TileKey.X;
                    topLeftY = s.ContainingTile.TileKey.Y;
                    //used this slice as an anchor
                    topLeftSlice = s;
                }
            }
            return topLeftSlice;
        }

        public Instance GetInstanceByName(string instanceName)
        {
            if (!m_instances.ContainsKey(instanceName))
            {
                //throw new ArgumentException("Instance " + instanceName + " not known in " + this.ToString());
                return null;
            }
            else
            {
                return m_instances[instanceName];
            }
        }

        public bool HasMappedInstances(string location)
        {
            return m_locationInstanceMapping.ContainsKey(location);
        }

        public IEnumerable<NetlistPort> Ports
        {
            get { return m_ports; }
        }

        public IEnumerable<Net> Nets
        {
            get { return m_nets.Values; }
        }

        public Net LastNetAdded
        {
            get { return m_lastNetAdded; }
        }

        public void RemoveNet(string netName)
        {
            if (!m_nets.ContainsKey(netName))
            {
                throw new ArgumentException("Net " + netName + " not found");
            }
            UnblockResourcesAfterRemoval(netName);

            m_nets.Remove(netName);
        }

        protected virtual void UnblockResourcesAfterRemoval(string netName)
        {
            // TODO TCL
        }

        public virtual NetlistContainer GetSelectedDesignElements()
        {
            throw new NotImplementedException();
        }

        public virtual void WriteCodeToFile(StreamWriter sw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all instances
        /// </summary>
        public IEnumerable<Instance> Instances
        {
            get { return m_instances.Values; }
        }

        public int InstanceCount
        {
            get { return m_instances.Count; }
        }

        public int NetCount
        {
            get { return m_nets.Count; }
        }

        public void Remove(Predicate<Net> p)
        {
            List<string> keysToDelete = new List<string>();
            foreach (KeyValuePair<string, Net> tupel in m_nets.Where(t => p(t.Value)))
            {
                keysToDelete.Add(tupel.Key);
                UnblockResourcesAfterRemoval(tupel.Key);
            }
            foreach (string s in keysToDelete)
            {
                m_nets.Remove(s);
            }
        }

        public void Remove(Predicate<Instance> p)
        {
            List<string> keysToDelete = new List<string>();
            foreach (KeyValuePair<string, Instance> tupel in m_instances.Where(t => p(t.Value)))
            {
                Tile t = FPGA.FPGA.Instance.GetTile(tupel.Value.Location);
                if (t.HasSlice(tupel.Value.SliceName))
                {
                    Slice s = t.GetSliceByName(tupel.Value.SliceName);
                    s.Usage = FPGATypes.SliceUsage.Free;
                }
                keysToDelete.Add(tupel.Key);
            }
            foreach (string s in keysToDelete)
            {
                m_instances.Remove(s);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// How to identify this netlist container (e.g. default_netlist_container)
        /// </summary>
        public string Name { get; set; }

        protected Dictionary<string, List<Instance>> m_locationInstanceMapping = new Dictionary<string, List<Instance>>();
        protected Dictionary<string, List<Instance>> m_sliceInstanceMapping = new Dictionary<string, List<Instance>>();
        protected Dictionary<string, Instance> m_instances = new Dictionary<string, Instance>();
        protected Dictionary<string, Net> m_nets = new Dictionary<string, Net>();
        protected Dictionary<Slice, string> m_slices = new Dictionary<Slice, string>();
        protected Net m_lastNetAdded = null;
        private List<NetlistPort> m_ports = new List<NetlistPort>();

    }
}