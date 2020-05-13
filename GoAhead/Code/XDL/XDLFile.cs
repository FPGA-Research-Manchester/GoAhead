using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.XDL
{
    public class XDLFile
    {
        public XDLFile(bool exportPortDeclarations, bool exportDummyNets, List<XDLContainer> netlistContainerNames, bool includeDesignStatement, bool includeModuleHeader, bool includeModuleFooter, string designName, bool sortInstancesBySliceName)
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            m_exportPortDeclarations = exportPortDeclarations;
            m_exportDummyNets = exportDummyNets;
            m_netlistContainer = netlistContainerNames;

            m_includeDesignStatement = includeDesignStatement;
            m_includeModuleHeader = includeModuleHeader;
            m_includeModuleFooter = includeModuleFooter;

            m_designName = designName;
        }

        public void WriteXDLCodeToFile(StreamWriter sw)
        {
            if (m_includeDesignStatement)
            {
                // design "__XILINX_NMC_MACRO" 2v500fg256-5;
                sw.WriteLine("design \"" + m_designName + "\" " + FPGA.FPGA.Instance.DeviceName + ";");

                if (m_includeModuleHeader)
                {
                    // set the anchor to the op left slice of all macros
                    string anchor;
                    bool anchorFound = XDLContainer.GetAnchor(m_netlistContainer, out anchor);

                    // module "BtnBar" "SLICE_X60Y148" , cfg "_SYSTEM_MACRO::FALSE" ;
                    sw.WriteLine("module \"" + GetModuleName() + "\"" + (anchorFound ? " \"" + anchor + "\"" : "") + ", cfg \"_SYSTEM_MACRO::FALSE\";");
                    sw.WriteLine("");
                }
            }
            else if (m_netlistContainer.Count > 0)
            {
                // print the first header
                string designConfig = m_netlistContainer[0].GetDesignConfig();
                if (designConfig.Length > 0)
                {
                    sw.WriteLine(designConfig);
                }
                else
                {
                    // generate default header
                    sw.WriteLine("design \"" + m_designName + "\" " + FPGA.FPGA.Instance.DeviceName + " v3.2 ,");
                    sw.WriteLine("cfg \"");
                    sw.WriteLine("\t" + "_DESIGN_PROP:P3_PLACE_OPTIONS:EFFORT_LEVEL:high");
                    sw.WriteLine("\t" + "_DESIGN_PROP::P3_PLACED:");
                    sw.WriteLine("\t" + "_DESIGN_PROP::P3_PLACE_OPTIONS:");
                    sw.WriteLine("\t" + "_DESIGN_PROP::PK_NGMTIMESTAMP:1397048215\";");
                }
            }

            foreach (XDLContainer n in m_netlistContainer)
            {
                foreach (XDLModule mod in n.Modules)
                {
                    sw.WriteLine(mod.ToString());
                }
            }

            if (m_exportPortDeclarations)
            {
                foreach (XDLContainer n in m_netlistContainer)
                {
                    foreach (XDLMacroPort port in n.MacroPorts)
                    {
                        sw.WriteLine(GetCode(port));
                    }
                    sw.WriteLine(n.GetNetPortsBlocks());
                }
            }

            sw.WriteLine("");
            int instCount = 0;
            foreach (XDLContainer n in m_netlistContainer)
            {
                foreach (Slice s in n.GetAllSlicesTemplates().OrderBy(s => s.SliceName))
                {
                    sw.WriteLine(GetCode(s));
                    instCount++;
                }

                foreach (XDLInstance inst in n.Instances.OrderBy(i => i.InstanceIndex))
                {
                    sw.WriteLine(inst.ToString());
                    instCount++;
                }
            }

            sw.WriteLine("");
            /*
            foreach (NetlistContainer n in this.m_netlistContainer)
            {
                foreach (Tile t in n.GetAllRAMTemplates())
                {
                    sw.WriteLine(this.GetCode(t));
                }
            }*/

            int netCount = 0;
            if (m_exportDummyNets)
            {
                sw.WriteLine("");
                foreach (XDLContainer n in m_netlistContainer)
                {
                    foreach (XDLMacroPort p in n.MacroPorts)
                    {
                        sw.WriteLine(GetDummyNetCode(p));
                        netCount++;
                    }

                    // e.g. dummy blocker nets form template
                    n.WriteNetCodeBlocks(sw);
                }
            }

            sw.WriteLine("");
            foreach (XDLContainer nlc in m_netlistContainer)
            {
                foreach (XDLNet n in nlc.Nets)
                {
                    // blocker nets become very huge ...
                    n.WriteToFile(sw);
                    netCount++;
                }
            }
            sw.WriteLine("");

            if (m_includeModuleFooter)
            {
                sw.WriteLine("endmodule \"" + GetModuleName() + "\";");
            }

            sw.WriteLine("# =======================================================");
            sw.WriteLine("# SUMMARY");
            sw.WriteLine("# Number of Module Defs: 0");
            sw.WriteLine("# Number of Module Insts: 0");
            sw.WriteLine("# Number of Primitive Insts: " + instCount);
            sw.WriteLine("# Number of Nets: " + netCount);
            sw.WriteLine("# =======================================================");
        }

        /// <summary>
        /// net "$NET_8" , outpin "sink" D , ;
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private string GetDummyNetCode(XDLMacroPort port)
        {
            string direction = port.Slice.PortMapping.IsSliceInPort(port.Port) ? "inpin" : "outpin";
            string result = "net" + GetDummyNetName(port) + "\", " + direction + " \"" + port.SliceName + "\" " + port.DummyNetPortName + ",;";
            return result;
        }

        public static string GetDummyNetName(XDLMacroPort port)
        {
            return " \"dummy_GOA_net_" + port.Slice + "_" + port.Port;
        }

        private string GetModuleName()
        {
            string result = "";
            foreach (NetlistContainer n in m_netlistContainer)
            {
                result += n.Name;
            }
            return result;
        }

        private string GetCode(XDLMacroPort port)
        {
            // port "$extpin_2" "SLICE_X57Y77" "B2";
            string portName = Regex.Replace(port.Port.ToString(), "^(L|M)_", "");
            return "port \"" + port.PortName + "\" \"" + port.SliceName + "\" \"" + portName + "\";";
        }

        public static string GetCode(Slice slice)
        {
            // open config body of the resulting slice config
            StringBuilder buffer = new StringBuilder();
            string instanceName = slice.SliceName;
            buffer.AppendLine("inst \"" + instanceName + "\" \"" + slice.SliceType + "\", placed " + slice.ContainingTile.Location + " " + instanceName + ",");

            string part = "A";
            buffer.AppendLine("    cfg \"");
            buffer.Append("\t");
            foreach (string setting in slice.GetAllAttributeValues())
            {
                string firstChar = setting.Substring(0, 1);
                // line break for ABCD, no line break for rest (RS) but for P
                if (!firstChar.Equals(part) && !Regex.IsMatch(firstChar, "(R|S)"))
                {
                    part = firstChar;
                    buffer.Append(Environment.NewLine + "\t" + setting);
                }
                else
                {
                    buffer.Append(setting);
                }
                buffer.Append(" ");
            }

            buffer.AppendLine(Environment.NewLine + "\"");
            buffer.AppendLine(";");

            return buffer.ToString();
        }

        private readonly bool m_exportPortDeclarations;
        private readonly bool m_exportDummyNets;
        private readonly List<XDLContainer> m_netlistContainer;

        private readonly bool m_includeDesignStatement;
        private readonly bool m_includeModuleHeader;
        private readonly bool m_includeModuleFooter;
        private readonly string m_designName;
    }
}