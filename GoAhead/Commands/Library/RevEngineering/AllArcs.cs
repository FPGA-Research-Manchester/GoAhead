using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.Commands.MacroGeneration;
using GoAhead.FPGA;

namespace GoAhead.Commands.RevEngineering
{
    class AllArcs : Command
    {
        public AllArcs()
        {
        }

        public override void Do()
        {
            // fetch all clbs
            Queue<Tile> allCLBs = new Queue<Tile>();
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles())
            {
                if (Regex.IsMatch(tile.Location, FPGA.Types.GetCLBRegex()))
                {
                    allCLBs.Enqueue(tile);
                }   
            }

            // get interconnect tile
            SortedDictionary<Port, bool> allPorts = new SortedDictionary<Port, bool>();
            
            // get all port filter
            String portFilter = "";
            String srcPortFilter = "";
            if (FPGA.FPGA.Instance.Family.Equals(Types.FPGAFamily.Spartan6))
            {
                portFilter = "(N|E|S|W)(E|L|N|R|W|S)(1|2|4)B[0-3]";
                srcPortFilter = "LOGICOUT";
            }
            else if (FPGA.FPGA.Instance.Family.Equals(Types.FPGAFamily.Kintex7))
            {
                portFilter = "(N|E|S|W)(E|L|N|R|W|S)(1|2|4)BEG[0-3]";
                srcPortFilter = "LOGIC_OUT";
            }
            else
            {
                throw new ArgumentException("Current FPGA familiy not implemented in AllArcs");
            }

            // look for a "typical" interconnect tile from which we store all ports
            foreach (Tile t in allCLBs)
            {
                Tile interConnectTile = GetInterconnectTile(allCLBs.Peek());

                // for all ports
                foreach (Port p in interConnectTile.SwitchMatrix.GetAllPorts())
                {
                    if (Regex.IsMatch(p.ToString(), portFilter) && !allPorts.ContainsKey(p))
                    {
                        allPorts.Add(p, false);
                    }
                }
                // stop after the first CLB after adding some ports
                if (allPorts.Count > 0)
                {
                    break;
                }
            }

            // all nets have the same source
            foreach (Port drivenPorts in allPorts.Keys)
            {
                Tile currentCLB = allCLBs.Dequeue();
                Tile currentInt = this.GetInterconnectTile(currentCLB);

                foreach(Port source in currentInt.SwitchMatrix.GetAllDrivers())
                {
                    if (currentInt.SwitchMatrix.Contains(source, drivenPorts) && Regex.IsMatch(source.ToString(), srcPortFilter))
                    {
                        Net next = new Net(drivenPorts.ToString());

                        //CLEXL_LOGICOUT0
                        String clbPort = this.GetCLBPort(currentCLB, source);

                        // get lut port
                        Port lutOutPort = null;
                        foreach (Port p in currentCLB.SwitchMatrix.GetAllDrivers())
                        {
                            if(currentCLB.SwitchMatrix.Contains(p, new Port(clbPort)))
                            {
                                lutOutPort = p;
                                break;
                            }
                        }

                        if(lutOutPort==null)
                        {
                        }

                        next.Add(currentCLB, lutOutPort, new Port(clbPort));
                        next.Add(currentInt, source, drivenPorts);
                        Objects.MacroManager.Instance.CurrentMacro.Add(next, false);

                        CommandExecuter.Instance.Execute(new SetFocus(currentCLB.Location));
                        CommandExecuter.Instance.Execute(new AddSlice(0));
                        CommandExecuter.Instance.Execute(new AddSlice(1));

                        break;
                    }
                }
            }
        }

        private String GetCLBPort(Tile currentCLB, Port source)
        {
            if (FPGA.FPGA.Instance.Family.Equals(Types.FPGAFamily.Spartan6))
            {
                String clbPrefix = currentCLB.Location.Split('_')[0];
                String clbPort = clbPrefix + "_" + source;
                return clbPort;
            }
            else if (FPGA.FPGA.Instance.Family.Equals(Types.FPGAFamily.Kintex7))
            {
                // pip CLBLM_L_X12Y50 CLBLM_L_D -> CLBLM_LOGIC_OUTS11 , 
                String clbPrefix = Regex.Split(currentCLB.Location, "_(L|R)")[0];
                String clbPort = clbPrefix + "_" + source;
                clbPort = clbPrefix + "_" + Regex.Replace(source.ToString(), "S_L", "S");
                return clbPort;
            }
            else
            {
                throw new ArgumentException("Current FPGA familiy not implemented in AllArcs");
            }            
           
        }

        private Tile GetInterconnectTile(Tile clb)
        {
            Tile interConnectTile = null;
            if (FPGA.FPGA.Instance.Family.Equals(Types.FPGAFamily.Spartan6))
            {
                interConnectTile = FPGA.FPGA.Instance.GetTile(Regex.Replace(clb.Location, "CLEX[L|M]", "INT"));
            }
            else if (FPGA.FPGA.Instance.Family.Equals(Types.FPGAFamily.Kintex7))
            {
                String loc = Regex.Replace(clb.Location, "CLBL[L|M]", "INT");
                interConnectTile = FPGA.FPGA.Instance.GetTile(loc);
            }
            else
            {
                throw new ArgumentException("Current FPGA familiy not implemented in AllArcs");
            }
            return interConnectTile;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
