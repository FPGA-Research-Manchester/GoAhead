﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;

namespace GoAhead.Commands.XDLManipulation
{
    [CommandDescription(Description="Fuse all nets in a netlist container", Wrapper=true, Publish=true)]
    class SimplifyIdentifier : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = GetNetlistContainer();

            int instIdentifierWidth = (int) Math.Ceiling(Math.Log((double)nlc.InstanceCount, (double)2.0d));
            int netIdentifierWidth = (int)Math.Ceiling(Math.Log((double)nlc.NetCount, (double)2.0d));

            Dictionary<string, string> instanceReplacements = SimplifyInstanceIdentifier(nlc);
            
            // restart with nets
            m_identifier = 0;
            SimplifyNetIdentifier(nlc, instanceReplacements);
        }

        private void SimplifyNetIdentifier(NetlistContainer nlc, Dictionary<string, string> instanceReplacements)
        {
            Dictionary<string, string> netReplacements = new Dictionary<string, string>();
            int done = 0;
            foreach (XDLNet net in nlc.Nets)
            {
                ProgressInfo.Progress = 80 + (int)((double)done++ / (double)nlc.NetCount * 10);                

                if (!string.IsNullOrEmpty(net.Config))
                {
                    string newConfig = net.Config;
                    foreach (string identifier in instanceReplacements.Keys)
                    {
                        if (net.Config.Contains(identifier))
                        {
                            newConfig = newConfig.Replace(identifier, instanceReplacements[identifier]);
                        }
                    }

                    net.Config = newConfig;
                }

                string newIdentifier = "i" + GetNextIdentfier();

                netReplacements.Add(net.Name, newIdentifier);
                // update header (if any) ...
                if (net.Header != null)
                {
                    net.Header = net.Header.Replace(net.Name, newIdentifier);
                }
                // ... and name
                net.Name = newIdentifier;

                foreach (NetPin pin in net.NetPins)
                {
                    pin.InstanceName = instanceReplacements[pin.InstanceName];
                }
            }
        }

        private Dictionary<string, string> SimplifyInstanceIdentifier(NetlistContainer nlc)
        {
            Dictionary<string, string> instanceReplacements = new Dictionary<string, string>();

            Regex belprop = new Regex("_BEL_PROP::(A|B|C|D)(5|6)LUT$");
            Regex whiteSpace = new Regex(@"\s");

            int done = 0;
            foreach (XDLInstance inst in nlc.Instances)
            {
                ProgressInfo.Progress = (int) ((double)done++ / (double)nlc.InstanceCount * 90);

                string newIdentifier = "n" + GetNextIdentfier();
                instanceReplacements.Add(inst.Name, newIdentifier);
                string code = inst.ToString();

                if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.CLB))
                {
                    string fixedCode = "";

                    bool insideIdentifier = false;
                    string buffer = "";
                    for (int i = 0; i < code.Length; i++)
                    {
                        // discard any _INST_PROP code
                        if (fixedCode.EndsWith("_INST_PROP"))
                        {
                            fixedCode = fixedCode.Replace("_INST_PROP", "");
                            fixedCode += "\";";
                            break;
                        }

                        // C6LUT:st/ddr_top/u_memc_ui_top/u_mem_intfc/mc0/rank_mach0/rank_common0/maintenance_request.maint_arb0/Mmux_dbl_last_master_ns<1\:0>21:#LUT:O6=(((~A4*A5)+(A4*A1))+A2)
                        bool dashFollows = i < code.Length - 1 ? code[i + 1] == '#' : false;
                        bool whiteSpaceFollows = i < code.Length - 1 ? whiteSpace.IsMatch(code[i + 1].ToString()) : false;
                        bool colonFollows = i < code.Length - 1 ? code[i + 1] == ':' : false;

                        bool config = fixedCode.EndsWith("#LUT") || fixedCode.EndsWith("#FF");

                        bool startOfIdentifier = code[i] == ':' && (fixedCode.EndsWith("LUT") || fixedCode.EndsWith("FF")) && !colonFollows && !dashFollows && !config && !belprop.IsMatch(fixedCode);
                        bool endOfIdentifier = code[i] == ':' && (dashFollows || whiteSpaceFollows) && insideIdentifier && !config;
                        if (startOfIdentifier || endOfIdentifier)
                        {
                            if (insideIdentifier)
                            {
                                // B5FF:st/gen4.udsg4/ddr_user_mem/U0/xst_fifo_generator/gaxis_fifo.gaxisf.axisf/grf.rf/gntv_or_sync_fifo.gl0.wr/wpntr/gcc0.gc0.count_3:
                                // B5FF::
                                if (buffer.Equals("PK_PACKTHRU") || buffer.Equals("BEL") || buffer.Equals("ASYNC_REG") || buffer.Equals("PK_REPLICATED_FROM") || whiteSpaceFollows)
                                {
                                    fixedCode += buffer;
                                }
                                else
                                {
                                    // remove content between ::
                                }
                                fixedCode += code[i];
                                insideIdentifier = false;
                                continue;
                            }
                            else
                            {
                                insideIdentifier = true;
                                fixedCode += code[i];
                                buffer = "";
                                continue;
                            }
                        }
                        if (!insideIdentifier)
                        {
                            fixedCode += code[i];
                        }
                        else
                        {
                            buffer += code[i];
                        }
                    }
                    inst.Clear();
                    fixedCode = fixedCode.Replace("\"" + inst.Name +"\"", "\"" + newIdentifier + "\"");
                    inst.AddCode(fixedCode);
                }
                else
                {
                    inst.Clear();
                    string fixedCode = code.Replace("\"" + inst.Name + "\"", "\"" + newIdentifier + "\"");
                    inst.AddCode(fixedCode);
                }

            }

            return instanceReplacements;
        }

        private string GetNextIdentfier()
        {
            string result = m_identifier.ToString("X");
            m_identifier++;
            return result;
        }

        private int m_identifier = 0;

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
