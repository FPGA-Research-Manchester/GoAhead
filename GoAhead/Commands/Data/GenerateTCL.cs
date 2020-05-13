using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Code;
using GoAhead.Code.TCL;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Save a netlist as TCL script for Vivado", Wrapper = false)]
    class GenerateTCL : NetlistContainerCommandWithFileOutput
    {
        public GenerateTCL()
        {
        }

        /// <summary>
        /// Internal use only, SW is passed as an argument
        /// </summary>
        /// <param name="sw"></param>
        public GenerateTCL(StreamWriter sw)
        {
            m_sw = sw;
        }

        protected override void DoCommandAction()
        {
            // Vivado only
            FPGATypes.AssertBackendType(FPGATypes.BackendType.Vivado);

            TCLContainer nlc = (TCLContainer)GetNetlistContainer();

            bool closeStream = false;
            if (m_sw == null)
            {
                // do not close external stream
                closeStream = true;
                m_sw = new StreamWriter(FileName, false);
            }

            WriteHeader(nlc, m_sw);

            if (IncludeLinkDesignCommand)
            {
                //this.m_sw.WriteLine("link_design -name empty_netlist -part " + FPGA.FPGA.Instance.DeviceName);
            }

            // eingelesen aus Netzliste (also von Vivado erstellt!) genrieren (brauchen wir erstmal nicht, nur fuer eigene instanzen ggf neu Hierstufen ienziehen, siehe create_cell)
            //this.WriteHierarchyCells(nlc);

            WriteInstances(nlc);

            //this.WritePins(nlc);

            ////this.WritePorts(nlc);

            WriteNets(nlc);
            

            m_sw.WriteLine("");
            m_sw.WriteLine("# end of file");

            if (closeStream)
            {
                m_sw.Close();
            }
        }

        private void WritePins(TCLContainer nlc)
        {
            m_sw.WriteLine("#########################################################################");
            m_sw.WriteLine("# pins");
            m_sw.WriteLine("#########################################################################");
            m_sw.WriteLine("");
            int pinCount = 0;
            foreach (TCLPin pin in nlc.Pins)
            {
                m_sw.WriteLine("# pin " + pinCount++);
                string name = pin.Instance.Name + "/" + pin.Properties.GetValue("REF_PIN_NAME");
                m_sw.WriteLine("create_pin -direction " + pin.Properties.GetValue("DIRECTION") + " " + name);
                m_sw.WriteLine("connect_net -net [get_nets " + pin.Net.Name + "] -objects [get_pins " + name + "]"); 
            }
        }

        private void WriteNets(TCLContainer nlc)
        {
            m_sw.WriteLine("#########################################################################");
            m_sw.WriteLine("# nets");
            m_sw.WriteLine("#########################################################################");
            m_sw.WriteLine("");
            int netCount = 0;
            foreach (TCLNet net in nlc.Nets)
            {
                if (net.NodeNet)
                {
                    net.UnflattenNet();
                }

                m_sw.WriteLine("# net " + netCount++);
                if ((netCount % 1000) == 0)
                {
                    m_sw.WriteLine("puts " + netCount);
                }
                m_sw.WriteLine("remove_net -quiet " + net.Name);
                m_sw.WriteLine("create_net " + net.Name);

                foreach (NetPin np in net.NetPins)
                {
                    string direction = np is NetOutpin ? "OUT" : "IN";
                    m_sw.WriteLine("create_pin -direction " + direction + " " + np.InstanceName + "/" + np.SlicePort);
                }
                foreach (NetPin np in net.NetPins)
                {
                    m_sw.WriteLine("connect_net -net " + net.Name + " -objects " + "[get_pins " + np.InstanceName + "/" + np.SlicePort + "]");
                }

                // insert the ROUTE property
                m_sw.WriteLine(net.GetTCLRouting());

                // we can not set "empty" values
                foreach (TCLProperty prop in net.Properties.Where(p => !string.IsNullOrEmpty(p.Value) && !p.ReadOnly))
                {
                    string value = prop.Value.Contains(" ") ? ("\"" + prop.Value + "\"") : prop.Value;
                    m_sw.WriteLine("set_property " + prop.Name + " " + value + " [get_nets " + net.Name + "]");
                }
                //this.m_sw.WriteLine("set_property IS_ROUTE_FIXED TRUE [get_nets " + net.Name + "]");
                m_sw.WriteLine(net.FooterComment);
            }
        }

        private void WriteHierarchyCells(TCLContainer nlc)
        {
            foreach (TCLDesignHierarchy hier in nlc.Hierarchies)
            {
                m_sw.WriteLine("create_cell -reference " + hier.Properties.GetValue("REF_NAME") + " -black_box " + hier.Name);
            }
        }

        private void WriteInstances(TCLContainer nlc)
        {
            int instanceCount = 0;
            // GDN currnently only blockers re supported, overwrok twhen placing netlist!!!
            foreach (TCLInstance inst in nlc.Instances.Where(i => ((TCLInstance)i).BELType.Equals("GND")))
            {
                m_sw.WriteLine("# instance " + instanceCount++);

                if (!string.IsNullOrEmpty(inst.BELType) && inst.OmitPlaceCommand)
                {
                    // Used by blocker
                    m_sw.WriteLine("create_cell -reference " + inst.BELType + " " + inst.Name);
                }
                else                
                {
                    m_sw.WriteLine("create_cell -reference " + inst.Properties.GetValue("REF_NAME") + " " + inst.Name);
                    string belName = inst.Properties.GetValue("BEL");
                    belName = belName.Remove(0, belName.IndexOf('.') + 1);

                    //this.m_sw.WriteLine("place_cell " + inst.Name + " " + inst.SliceName + "/" + belName);

                    bool wrapCatch =
                        inst.Properties.GetValue("REF_NAME").Equals("IBUF") ||
                        inst.Properties.GetValue("REF_NAME").Equals("OBUFT");

                    // we can not set "empty" values, e.g., "set_property ASYNC_REG  [get_sites IOB_X0Y1]"    
                    foreach (TCLProperty prop in inst.Properties.Where(p => !string.IsNullOrEmpty(p.Value) && !p.ReadOnly && !ExcludedProperties.Contains(p.Name)))
                    {
                        string prefix = wrapCatch ? "catch {" : "";
                        string suffix = wrapCatch ? "}" : "";
                        string value = prop.Value.Contains(" ") ? ("\"" + prop.Value + "\"") : prop.Value;
                        m_sw.WriteLine(prefix + "set_property " + prop.Name + " " + value + " [get_cells " + inst.Name + "]"  + suffix);
                    }
                }
                m_sw.WriteLine("");
            }
        }

        private void WriteHeader(TCLContainer nlc, StreamWriter sw)
        {
            sw.WriteLine("#########################################################################");
            sw.WriteLine("# this TCL script is generated by GoAhead, the script assembles a netlist");
            sw.WriteLine("#########################################################################");
            sw.WriteLine("#");
            sw.WriteLine("# current device is " + FPGA.FPGA.Instance.DeviceName);
            sw.WriteLine("#  * " + nlc.InstanceCount + " primitives");
            sw.WriteLine("#  * " + nlc.NetCount + " nets");
            sw.WriteLine("#  * " + nlc.Pins.Count() + " pins");
            sw.WriteLine("#");
            sw.WriteLine("#########################################################################");
            sw.WriteLine("# instances");
            sw.WriteLine("#########################################################################");
            sw.WriteLine("");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, bool> m_hierarchy = new Dictionary<string, bool>();

        private StreamWriter m_sw = null;

        [Parameter(Comment = "Whether to emit link_desing")]
        public bool IncludeLinkDesignCommand = false;
        
        [Parameter(Comment = "A list of properties which will not be emiitedd")]
        public List<string> ExcludedProperties = new List<string>();
    }
}
