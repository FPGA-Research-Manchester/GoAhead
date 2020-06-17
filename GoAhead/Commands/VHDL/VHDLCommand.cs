using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.VHDL
{
    abstract class VHDLCommand : CommandWithFileOutput
    {
        protected void GetSignalList(String partialAreaName, bool invert, 
            out Dictionary<String, List<int>> signalWidths,
            out Dictionary<String, String> directions,
            out List<Tuple<String, List<int>>> interfaces,
            out List<String> ifSignals,
            out List<String> signalsForInterface,
            out List<String> signalsDeclarationsForMappingAndKeep)
        {
            signalWidths = new Dictionary<String, List<int>>();
            directions = new Dictionary<String, String>();
            interfaces = new List<Tuple<String, List<int>>>();
            ifSignals = new List<String>();
            signalsForInterface = new List<String>();
            signalsDeclarationsForMappingAndKeep = new List<String>();
            foreach (Signal signal in this.GetSignalForEntity(partialAreaName))
            {
                String[] atoms = Regex.Split(signal.SignalName, @"\(|\)");
                String name = atoms[0];
                int index = Int32.Parse(atoms[1]);

                if (!signalWidths.ContainsKey(name))
                {
                    signalWidths.Add(name, new List<int>());
                    String dir = signal.SignalMode;
                    if (invert)
                    {
                        dir = dir.Equals("in") ? "out" : "in";
                    }
                    directions.Add(name, dir);
                }
                signalWidths[name].Add(index);

            }

            // transform hash to sorted list
            foreach (String name in signalWidths.Keys.OrderBy(s => s))
            {
                interfaces.Add(new Tuple<String, List<int>>(name, signalWidths[name]));
            }

            // ensure that every value between min and max is found
            foreach (KeyValuePair<String, List<int>> tupel in signalWidths)
            {
                int min = tupel.Value.Min();
                int max = tupel.Value.Max();

                if ((max - min) + 1 != tupel.Value.Count)
                {
                    throw new ArgumentException("Some indeces are missing for signal " + tupel.Key + ". Max=" + max + " and Min=" + min + ". However " + tupel.Value.Count + " indeces are expected");
                }

                for (int i = min; i <= max; i++)
                {
                    if (!tupel.Value.Contains(i))
                    {
                        throw new ArgumentException("Index " + i + " is missing for signal " + tupel.Key);
                    }
                }
            }

            for (int i = 0; i < interfaces.Count; i++)
            {
                Tuple<String, List<int>> tupel = interfaces[i];
                int min = tupel.Item2.Min();
                int max = tupel.Item2.Max();

                String ifSignal = '\t' + tupel.Item1 + " : " + directions[tupel.Item1] + " std_logic_vector(" + max + " downto " + min + ")";
                if (i < interfaces.Count - 1)
                {
                    ifSignal += ";";
                }
                else
                {
                    ifSignal += ");";
                }
                ifSignals.Add(ifSignal);

                // collect line for component decl
                signalsForInterface.Add(ifSignal);

                String sigDecl = tupel.Item1 + " : std_logic_vector(" + max + " downto " + min + ");";
                signalsDeclarationsForMappingAndKeep.Add(sigDecl);
            }
        }


        /// <summary>
        /// for simle input and outpit signals return all those
        /// for each stream port video(31) return video_in(31) and video_out(31)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Signal> GetSignalForEntity(String partialAreaName)
        {
            foreach (Signal s in Objects.InterfaceManager.Instance.GetAllSignals(s => s.PartialRegion.Equals(partialAreaName) && !s.SignalName.Equals("open")).OrderBy(s => s.Column))
            {
                if (s.SignalMode.Equals("stream"))
                {
                    String inName = Regex.Replace(s.SignalName, @"\(", "_in(");
                    String outName = Regex.Replace(s.SignalName, @"\(", "_out(");
                    yield return new Signal(inName, "in", s.SignalDirection, s.PartialRegion, s.Column);
                    yield return new Signal(outName, "out", s.SignalDirection, s.PartialRegion, s.Column);
                }
                else
                {
                    yield return s;
                }
            }
        }
    }

    class PortMappingHandler
    {
        public static Dictionary<String, String> GetPortMapping(String portMappingString)
        {
            Dictionary<String, String> portMapping = new Dictionary<String, String>();

            if (portMappingString.Length == 0)
            {
                return portMapping;
            }
            String[] mappings = Regex.Split(portMappingString, ",");

            foreach (String mapping in mappings)
            {
                String[] atoms = Regex.Split(mapping, ":");
                try
                {
                    portMapping.Add(atoms[0], atoms[1]);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Can not handle mapping " + portMappingString + " " + mapping);
                }
            }

            return portMapping;
        }

        public static bool HasMapping(XDLPort port, Dictionary<String, String> portMapping, out String portMappingKey)
        {
            portMappingKey = "";
            foreach (String key in portMapping.Keys)
            {
                //if (port.ExternalName.Equals(key))
                if (Regex.IsMatch(port.ExternalName, key))
                {
                    portMappingKey = key;
                    return true;
                }
            }
            return false;
        }
    }
}
