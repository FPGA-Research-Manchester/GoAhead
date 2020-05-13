using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    public class LibElemInst
    {
        public LibElemInst()
        {
            LibraryElementName = "";
            InstanceName = "";
            AnchorLocation = "";
            SliceName = "";
            SliceNumber = -1;
            m_portMapper = new PortMapper();
        }

        public string LibraryElementName { get; set; }
        public string InstanceName { get; set; }
        public string AnchorLocation { get; set; }
        public string SliceName { get; set; }
        public int SliceNumber { get; set; }

        public PortMapper PortMapper
        {
            get { return m_portMapper; }
        }

        public override string ToString()
        {
            return
                "InstanceName " + InstanceName + Environment.NewLine +
                "LibraryElementName: " + LibraryElementName + Environment.NewLine +
                "AnchorLocation " + AnchorLocation + Environment.NewLine +
                "SliceName " + SliceName + Environment.NewLine +
                "SliceNumber " + SliceNumber;
        }

        public LibraryElement GetLibraryElement()
        {
            return Library.Instance.GetElement(LibraryElementName);
        }

        public IEnumerable<Tuple<Tile, Slice>> GetAllUsedSlices()
        {
            LibraryElement libElement = Library.Instance.GetElement(LibraryElementName);

            Tile anchor = FPGA.FPGA.Instance.GetTile(AnchorLocation);

            foreach (Tuple<Instance, Tile> instanceTileTupel in libElement.GetInstanceTiles(anchor, libElement))
            {
                // placement of the current instance;
                Tile targetTile = instanceTileTupel.Item2;

                yield return new Tuple<Tile, Slice>(targetTile, targetTile.Slices[(int)instanceTileTupel.Item1.SliceNumber]);
            }
        }

        private readonly PortMapper m_portMapper;
    }

    public class PortMapper
    {
        public enum MappingKind { NoVector = 0, Internal = 1, External = 2 }

        public void AddMapping(string portName, string mappedSignal, MappingKind kind, int index)
        {
            m_kindMapping[portName] = kind;
            m_signalMapping[portName] = mappedSignal;
            m_signalIndex[portName] = index;
        }

        public int GetIndex(string portName)
        {
            if (!m_signalIndex.ContainsKey(portName))
            {
                throw new ArgumentException("No index found for signal " + portName);
            }
            else
            {
                return m_signalIndex[portName];
            }
        }

        public bool HasIndex(string portName)
        {
            return m_signalIndex.ContainsKey(portName);
        }

        public IEnumerable<Tuple<string, string, MappingKind>> GetMappings()
        {
            foreach (KeyValuePair<string, MappingKind> tuple in m_kindMapping)
            {
                Tuple<string, string, MappingKind> next = new Tuple<string, string, MappingKind>(tuple.Key, m_signalMapping[tuple.Key], tuple.Value);
                yield return next;
            }
        }

        public MappingKind GetMapping(string portName)
        {
            return m_kindMapping[portName];
        }

        public string GetSignalName(string portName)
        {
            return m_signalMapping[portName];
        }

        public bool HasKindMapping(string portName)
        {
            return m_kindMapping.ContainsKey(portName);
        }

        public bool HasSignalMapping(string portName)
        {
            return m_signalMapping.ContainsKey(portName);
        }

        public void Clear()
        {
            m_kindMapping.Clear();
            m_signalMapping.Clear();
            m_signalIndex.Clear();
        }

        // alles per Instantiation Filter
        private Dictionary<string, MappingKind> m_kindMapping = new Dictionary<string, MappingKind>();

        private Dictionary<string, string> m_signalMapping = new Dictionary<string, string>();
        private Dictionary<string, int> m_signalIndex = new Dictionary<string, int>();
    }

    public class LibraryElementInstanceManager : Interfaces.IResetable
    {
        private LibraryElementInstanceManager()
        {
            Commands.Debug.PrintGoAheadInternals.ObjectsToPrint.Add(this);
        }

        public void Add(LibElemInst instantiation)
        {
            if (m_instanceNames.ContainsKey(instantiation.InstanceName))
            {
                throw new ArgumentException("Instance name " + instantiation.InstanceName + " already used");
            }

            m_instantiations.Add(instantiation);
            m_instanceNames.Add(instantiation.InstanceName, true);
        }

        public void Remove(string instanceName)
        {
            if (!m_instanceNames.ContainsKey(instanceName))
            {
                throw new ArgumentException("Instance name " + instanceName + " already used");
            }

            // there should be only one element
            LibElemInst instToDelete = m_instantiations.FirstOrDefault(element => element.InstanceName == instanceName);

            m_instantiations.Remove(instToDelete);
            m_instanceNames.Remove(instanceName);
        }

        public void Reset()
        {
            m_instanceNames.Clear();
            m_instantiations.Clear();
        }

        public bool HasInstanceName(string instanceName)
        {
            return m_instanceNames.ContainsKey(instanceName);
        }

        public string ProposeInstanceName(string prefix)
        {
            int suffix = m_instanceNames.Keys.Count(str => Regex.IsMatch(str, prefix));

            return prefix + "_" + suffix;
        }

        public IEnumerable<LibElemInst> GetAllInstantiations()
        {
            foreach (LibElemInst instantiation in m_instantiations)
            {
                yield return instantiation;
            }
        }

        public LibElemInst GetInstantiation(int index)
        {
            return m_instantiations[index];
        }

        public LibElemInst GetInstantiation(Tile t)
        {
            foreach (LibElemInst instantiation in m_instantiations)
            {
                if (instantiation.AnchorLocation.Equals(t.Location))
                {
                    return instantiation;
                }
            }
            return null;
        }

        public LibElemInst GetInstantiation(string instanceName)
        {
            if (!HasInstanceName(instanceName))
            {
                throw new ArgumentException("Instance name " + instanceName + " not found");
            }

            // InstanceName is unique
            return m_instantiations.First(macro => macro.InstanceName.Equals(instanceName));
        }

        public BindingList<LibElemInst> Instances
        {
            get { return m_instantiations; }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (m_instanceNames.Count == 0)
            {
                result.AppendLine("No instances added");
            }
            else
            {
                result.AppendLine("Added instances are");
                foreach (LibElemInst m in GetAllInstantiations())
                {
                    result.AppendLine(m.ToString());
                }
            }
            return result.ToString();
        }

        public static LibraryElementInstanceManager Instance = new LibraryElementInstanceManager();

        private Dictionary<string, bool> m_instanceNames = new Dictionary<string, bool>();
        private BindingList<LibElemInst> m_instantiations = new BindingList<LibElemInst>();
    }
}