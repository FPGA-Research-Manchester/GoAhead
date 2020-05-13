using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands
{
    class AllArcsInBram : Command
    {
        public AllArcsInBram()
        {
        }

        public override void Do()
        {
            // fetch all clbs
            Queue<Tile> allINTs = new Queue<Tile>();
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles())
            {
                if (Regex.IsMatch(tile.Location, "^INT_BRAM_"))
                {
                    allINTs.Enqueue(tile);
                }
            }

            // get all ports
            SortedDictionary<Port, bool> allPorts = new SortedDictionary<Port, bool>();
            Tile interConnectTile = allINTs.Peek();

            // for all ports
            foreach (Port p in interConnectTile.SwitchMatrix.GetAllPorts())
            {
                if (Regex.IsMatch(p.ToString(), "(N|E|S|W)(E|L|N|R|W|S)(1|2|4)B[0-3]") && !allPorts.ContainsKey(p))
                {
                    allPorts.Add(p, false);
                }
                if (Regex.IsMatch(p.ToString(), @"^LOGICIN\d+") && !allPorts.ContainsKey(p))
                {
                    //allPorts.Add(p, false);
                }
            }

            // all nets have the same source
            foreach (Port drivenPorts in allPorts.Keys)
            {
                Tile currentInt = allINTs.Dequeue();

                String interfaceLocation = Regex.Replace(currentInt.Location, "BRAM", "INTERFACE");
                if (!FPGA.FPGA.Instance.Contains(interfaceLocation))
                {
                    continue;
                }

                Tile currentCLB  = FPGA.FPGA.Instance.GetTile(interfaceLocation);

                foreach (Port source in currentInt.SwitchMatrix.GetAllDrivers())
                {
                    if (currentInt.SwitchMatrix.Contains(source, drivenPorts) && Regex.IsMatch(source.ToString(), "LOGICOUT"))
                    {
                        Net next = new Net(drivenPorts.ToString());

                        //CLEXL_LOGICOUT0
                        String clbPrefix = currentCLB.Location.Split('_')[0];
                        String clbPort = clbPrefix + "_INTERFACE_" + source;

                        // get lut port
                        Port lutOutPort = null;
                        foreach (Port p in currentCLB.SwitchMatrix.GetAllDrivers())
                        {
                            if (currentCLB.SwitchMatrix.Contains(p, new Port(clbPort)))
                            {
                                lutOutPort = p;
                                break;
                            }
                        }

                        next.Add(currentCLB, lutOutPort, new Port(clbPort));
                        next.Add(currentInt, source, drivenPorts);
                        Objects.MacroManager.Instance.CurrentMacro.Add(next, false);

                        //CommandExecuter.Instance.Execute(new SetFocus(currentCLB.Location));
                        //CommandExecuter.Instance.Execute(new AddSlice(0));
                        //CommandExecuter.Instance.Execute(new AddSlice(1));

                        break;
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
