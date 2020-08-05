using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllWirelists : CommandWithFileOutput
    {
        // each wirelist is broken down into miniwirelists based on the start port

        Dictionary<int, List<int>> miniWirelistMappings = new Dictionary<int, List<int>>();
        Dictionary<int, WireList> miniWirelists = new Dictionary<int, WireList>();

        // each port is substituted with a hashcode (number) to reduce the space required to store the model. 
        // This is different from the "LocalPipKey", etc, because keys for these ports only start at around 11000, leading to unnecessary extra chars
        Dictionary<string, int> portMappings = new Dictionary<string, int>();

        protected override void DoCommandAction()
        {
            foreach (WireList wl in Wirelists.Values)
            {
                // for each wirelist, divide into north, east, south, west and any other wires.
                WireList northWires = new WireList();
                WireList eastWires = new WireList();
                WireList southWires = new WireList();
                WireList westWires = new WireList();
                WireList leftoverWires = new WireList();

                foreach (PrimitiveWire w in wl)
                {
                    // fill up the port names dictionary
                    if (!portMappings.ContainsKey(w.LocalPip))
                        portMappings.Add(w.LocalPip, portMappings.Count);

                    if (!portMappings.ContainsKey(w.PipOnOtherTile))
                        portMappings.Add(w.PipOnOtherTile, portMappings.Count);

                    string localPipStartingWith = w.LocalPip.Substring(0, 2);
                    switch(localPipStartingWith)
                    {
                        case "NN": 
                            {
                                northWires.Add(w);
                                break;
                            }
                        case "EE":
                            {
                                eastWires.Add(w);
                                break;
                            }
                        case "SS":
                            {
                                southWires.Add(w);
                                break;
                            }
                        case "WW":
                            {
                                westWires.Add(w);
                                break;
                            }
                        default:
                            {
                                leftoverWires.Add(w);
                                break;
                            }
                    }
                }

                addToMiniWirelistMappings(northWires, wl.Key);
                addToMiniWirelistMappings(eastWires, wl.Key);
                addToMiniWirelistMappings(southWires, wl.Key);
                addToMiniWirelistMappings(westWires, wl.Key);
                addToMiniWirelistMappings(leftoverWires, wl.Key);
            }

            foreach (WireList wl in Wirelists.Values)
            {
                OutputManager.WriteOutput(wl.Key + ",(" + string.Join(",", miniWirelistMappings[wl.Key]) + ")");
            }

            PrintWirelists printMiniWirelists = new PrintWirelists();
            printMiniWirelists.FileName = Path.Combine(FileName.Substring(0, FileName.LastIndexOf(Path.DirectorySeparatorChar.ToString())), "miniWirelists.ag");
            printMiniWirelists.MiniWirelists = miniWirelists;
            printMiniWirelists.PortMappings = portMappings;
            CommandExecuter.Instance.Execute(printMiniWirelists);

            PrintPortNames printPortNames = new PrintPortNames();
            printPortNames.FileName = Path.Combine(FileName.Substring(0, FileName.LastIndexOf(Path.DirectorySeparatorChar.ToString())), "portNames.ag");
            printPortNames.PortMappings = portMappings;
            CommandExecuter.Instance.Execute(printPortNames);


        }

        private void addToMiniWirelistMappings (WireList miniWireList, int mainWireListKey)
        {
            if (!miniWirelistMappings.ContainsKey(mainWireListKey))
                miniWirelistMappings.Add(mainWireListKey, new List<int>());

            if (miniWireList.Count > 0)
            {
                PrintArchitectureGraph.StoreWirelist(miniWireList, miniWirelists);

                if (!miniWirelistMappings[mainWireListKey].Contains(miniWireList.Key))
                    miniWirelistMappings[mainWireListKey].Add(miniWireList.Key);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Interconnect Wirelists")]
        public Dictionary<int, WireList> Wirelists = null;
    }
}
