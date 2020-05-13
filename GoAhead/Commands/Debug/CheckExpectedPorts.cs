﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Check the expected placement generated by PrintExpectedPorts of ports after the mapping phase against the actual port placement found in the XDL netlist ", Wrapper = false)]
    class CheckExpectedPorts : NetlistContainerCommandWithFileOutput
    {
        class Expectation
        {
            public string Direction;
            public string InstanceNamePart;
            public string SliceName;
            public string ExpectedPortName;
            public int ExpectedIndex;

            public bool InstanceChecked = false;
            public bool PortNameChecked = false;
            public bool NetNameChecked = false;
            public bool IndexChecked = false;

            public override string ToString()
            {
                return InstanceNamePart + "." + ExpectedPortName 
                    + " InstanceName: " + (InstanceChecked ? "1" : "0") 
                    + " PortName: " + (PortNameChecked ? "1" : "0")
                    + " NetName:" + (NetNameChecked ? "1" : "0")
                    + " Index:" + (IndexChecked ? "1" : "0");
            }
        }

        protected override void DoCommandAction()
        {
            List<Expectation> expectations = new List<Expectation>();

            string line = "";
            TextReader tr = new StreamReader(ExpectFile);
            while ((line = tr.ReadLine()) != null)
            {
                if (!line.StartsWith("expect"))
                {
                    continue;
                }

                string[] atoms = line.Split(' ');

                Expectation expectation = new Expectation();
                expectation.Direction = atoms[1];
                expectation.InstanceNamePart = atoms[2];
                expectation.SliceName = atoms[3];
                expectation.ExpectedPortName = atoms[4];
                expectation.ExpectedIndex = int.Parse(atoms[6]);
                expectations.Add(expectation);
            }            
            tr.Close();
            
            NetlistContainer netlist = GetNetlistContainer();

            foreach (XDLInstance inst in netlist.Instances)
            {
                foreach(Expectation expectation in expectations.Where(e => e.SliceName.Equals(inst.SliceName)))
                {
                    expectation.InstanceChecked = true;
                }
            }

            // check whether the ports reside on the expected nets
            Regex netNameMatch = new Regex(NetnameRegexp, RegexOptions.Compiled);

            // check index (works for module)
            foreach (Expectation expectation in expectations.Where(e => e.ExpectedIndex > 0))
            {
                // inst might be null for connection macros when checking the fused module
                XDLInstance inst = (XDLInstance) netlist.Instances.FirstOrDefault(i => i.SliceName.Equals(expectation.SliceName));

                foreach (XDLNet net in netlist.Nets)
                {
                    foreach (NetPin pin in net.NetPins.Where(np => np.GetDirection().Equals(expectation.Direction)))
                    {
                        bool portMatch = pin.SlicePort.Equals(expectation.ExpectedPortName) && (inst == null ? true : pin.InstanceName.Equals(inst.Name));
                        bool netMatch =
                            net.Name.Contains("<" + expectation.ExpectedIndex + ">") ||
                            net.Name.Contains("(" + expectation.ExpectedIndex + ")") ||
                            net.Name.Contains("[" + expectation.ExpectedIndex + "]");
                        // some ports may be connected through the net "GLOBAL_LOGIC0"
                        bool globalLogicNet = net.Name.StartsWith("GLOBAL_LOGIC");
                        if (portMatch && netMatch)
                        {
                            expectation.IndexChecked = true;
                            break;
                        }
                        else if (portMatch && globalLogicNet)
                        {
                            OutputManager.WriteWarning("Index of expectation " + expectation + " machted using GLOBAL_LOGIC net " + net.Name);
                            expectation.IndexChecked = true;
                            break;
                        }
                    }
                    if (expectation.IndexChecked)
                    {
                        break;
                    }
                }
            }

            // check what?
            foreach (Expectation expectation in expectations)
            {
                XDLInstance inst = (XDLInstance)netlist.Instances.FirstOrDefault(i => i.SliceName.Equals(expectation.SliceName));
                if (inst == null)
                {
                    continue;  
                }
                else 
                {
                    foreach (XDLNet net in netlist.Nets)
                    {
                        foreach (NetPin pin in net.NetPins)
                        {
                            if (pin.InstanceName.Equals(inst.Name) && pin.SlicePort.Equals(expectation.ExpectedPortName))
                            {
                                expectation.PortNameChecked = true;
                                expectation.NetNameChecked = netNameMatch.IsMatch(net.Name);
                                break;
                            }
                        }
                        if(expectation.PortNameChecked)
                        {
                            break;
                        }
                    }
                }
            }
            
            List<Expectation> missing = new List<Expectation>();
            missing.AddRange(expectations.Where(e => !e.InstanceChecked || !e.PortNameChecked || !e.NetNameChecked || (e.ExpectedIndex > 0 && !e.IndexChecked)));
            foreach (Expectation expectation in missing)
            {
                OutputManager.WriteOutput("Could not satisfy expectation " + expectation);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The expected placement generated by PrintExpectedPorts")]
        public string ExpectFile = "expected_ports.txt";

        [Parameter(Comment = @"The ports are excepted to be routed only via whose names matches this regular expression. If you don't care about the netname use .*. If you expect a name use e.g. sink2source<\d+>")]
        public string NetnameRegexp = ".*";
    }
}