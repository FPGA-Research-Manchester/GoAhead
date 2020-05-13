using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Commands;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.XDL
{
    public class XDLDesignParser : DesignParser
    {
        public XDLDesignParser(string fileName)
        {
            m_fileName = fileName;
        }

        public override void ParseDesign(NetlistContainer nlc, Command caller)
        {
            XDLContainer xdlContainer = (XDLContainer)nlc;

            bool readNet = false;
            bool readDesign = false;
            bool readModule = false;
            bool readInstance = false;
            XDLNet net = null;
            XDLInstance inst = null;
            XDLModule module = null;
            char[] buffer = new char[1];
            bool pendingQuote = false;

            StringBuilder designConfig = new StringBuilder();
            StringBuilder lineBuffer = new StringBuilder();

            Regex anchorMatch = new Regex("^\".*\"$", RegexOptions.Compiled);
            Regex whiteSpaceMatch = new Regex(@"\s+", RegexOptions.Compiled);

            FileInfo fi = new FileInfo(m_fileName);
            int length = (int)fi.Length;
            char c = ' ';

            char[] modReferenceBuffer = new char[2048];
            int modReferenceBufferIndex = 0;

            char[] keyWordBuffer = new char[2048];
            int keyWordBufferIndex = 0;

            char[] locationBuffer = new char[2048];
            char[] fromBuffer = new char[32];
            char[] toBuffer = new char[32];

            StreamReader sr = new StreamReader(m_fileName);

            int charIndex = 0;
            while (charIndex < length)
            {
                // get next char
                c = (char)sr.Read(); charIndex++;

                // measure progress
                if (caller != null)
                {
                    caller.ProgressInfo.Progress = (int)((double)charIndex / (double)length * 100);
                }

                // skip comments
                while (c == '#' && !readNet && !readInstance && !readDesign && !readModule)
                {
                    while (true)
                    {
                        c = (char)sr.Read();
                        charIndex++;
                        if (c == '\n')
                        {
                            break;
                        }
                    }
                    c = (char)sr.Read(); charIndex++;
                }

                if (readNet)
                {
                    ReadNet(net, c, sr, ref charIndex, length, caller);
                    readNet = false;
                    if (readModule)
                    {
                        module.Add(net);
                    }
                    else
                    {
                        xdlContainer.Add(net);
                    }
                    lineBuffer.Clear();
                }
                else if (readInstance)
                {
                    ReadInstance(inst, c, sr, ref charIndex);
                    readInstance = false;

                    if (readModule)
                    {
                        module.Add(inst);
                    }
                    else
                    {
                        xdlContainer.Add(inst);
                    }
                    lineBuffer.Clear();
                }
                else
                {
                    keyWordBufferIndex = 0;

                    switch (c)
                    {
                        case 'n':
                            // read everything until first comma outside quotes
                            // net "Inst_PE/Mmult_OPA[31]_OPB[31]_MuLt_18_OUT_OPA,OPB<10>_x_OPA,OPB<62>_mand1_FRB"
                            pendingQuote = false;
                            keyWordBuffer[keyWordBufferIndex++] = c;
                            while (true)
                            {
                                //c = (char)byteBuffer[charIndex++];
                                c = (char)sr.Read(); charIndex++;
                                keyWordBuffer[keyWordBufferIndex++] = c;

                                if (c == '"')
                                {
                                    pendingQuote = !pendingQuote;
                                }
                                if (c == ',' && !pendingQuote)
                                {
                                    break;
                                }
                            }
                            readNet = true;
                            break;

                        case 'c':
                            // read everything until first ; outside quotes
                            // may appear anywhere and will be ignore (Jo)
                            //cfg "
                            //# _DESIGN_PROP:P3_PLACE_OPTIONS:EFFORT_LEVEL:high
                            //# _DESIGN_PROP::P3_PLACED:
                            //# _DESIGN_PROP::P3_PLACE_OPTIONS:
                            //# _DESIGN_PROP::PK_NGMTIMESTAMP:1397048215";
                            pendingQuote = false;
                            keyWordBuffer[keyWordBufferIndex++] = c;
                            while (true)
                            {
                                c = (char)sr.Read(); charIndex++;
                                keyWordBuffer[keyWordBufferIndex++] = c;
                                if (c == '"')
                                {
                                    pendingQuote = !pendingQuote;
                                }
                                if (c == ';' && !pendingQuote)
                                {
                                    break;
                                }
                            }
                            break;

                        case 'i':
                            // read two commas outside qoutes
                            // inst "Inst_PE/Mmult_OPA[31]_OPB[31]_MuLt_18_OUT_OPA,OPB<21>_x_OPA,OPB<62>_mand1_FRB" "SLICEL",placed CLBLM_X18Y191 SLICE_X27Y191  ,
                            pendingQuote = false;
                            int commas = 0;
                            keyWordBuffer[keyWordBufferIndex++] = c;
                            while (commas != 2)
                            {
                                //c = (char)byteBuffer[charIndex++];
                                c = (char)sr.Read();
                                charIndex++;
                                keyWordBuffer[keyWordBufferIndex++] = c;

                                if (c == '"')
                                {
                                    pendingQuote = !pendingQuote;
                                }
                                if (c == ',' && !pendingQuote)
                                {
                                    commas++;
                                }
                            }
                            readInstance = true;
                            break;

                        case 'd':
                            designConfig.Append(c);
                            while (true)
                            {
                                c = (char)sr.Read(); charIndex++;
                                designConfig.Append(c);
                                if (c == ';')
                                {
                                    break;
                                }
                            }
                            string[] atoms = whiteSpaceMatch.Split(designConfig.ToString());
                            // do not modify name here
                            if (xdlContainer.DesignName == null)
                            {
                                xdlContainer.DesignName = Regex.Replace(atoms[1], "\"", "");
                                xdlContainer.Family = (atoms.Length > 3 ? Regex.Replace(atoms[2], "\"", "") : "unknown");
                                xdlContainer.AddDesignConfig(designConfig.ToString());
                            }
                            else
                            {
                                caller.OutputManager.WriteWarning("Ignoring the device config as there is already a device name " + xdlContainer.DesignName + " present");
                            }
                            if (!FPGA.FPGA.Instance.DeviceName.ToString().Equals(xdlContainer.Family) && !xdlContainer.Family.Equals("unknown"))
                            {
                                // TODO wenn das obige if GARANTIERT "tut", dann kann man auch eine ArgumentException werden
                                caller.OutputManager.WriteWarning("The currenlty loaded device is " + FPGA.FPGA.Instance.DeviceName + ". However, the device specified in the currently parsed in netlist is " + xdlContainer.Family);
                            }

                            break;

                        case 'm':
                            module = new XDLModule();

                            readModule = true;
                            string moduleHeader = "";
                            moduleHeader += c;
                            while (true)
                            {
                                if (charIndex >= length)
                                {
                                    throw new ArgumentException("Unexpected end of file while reading module. Missing endmodule statement?");
                                }

                                c = (char)sr.Read(); charIndex++;
                                moduleHeader += c;
                                if (c == ';')
                                {
                                    break;
                                }
                            }

                            moduleHeader = Regex.Replace(moduleHeader, @"^\s*", "");
                            string[] moduleAtoms = whiteSpaceMatch.Split(moduleHeader);
                            // extract module
                            module.Name = Regex.Replace(moduleAtoms[1], "\"", "");
                            module.Name = Regex.Replace(module.Name, ",", "");
                            // names is used as a key
                            xdlContainer.Add(module);

                            // extract explicit anchor
                            string anchor = moduleAtoms[2];
                            module.ExplicitAnchorFound = anchorMatch.IsMatch(anchor);
                            if (module.ExplicitAnchorFound)
                            {
                                module.Anchor = Regex.Replace(anchor, "\"", "");
                            }

                            break;

                        case 'p':
                            //port "H0" "SliceInstance" "A6";
                            ReadPort(module, c, sr, ref charIndex);
                            break;

                        case 'e': // endmodule
                            while (true)
                            {
                                c = (char)sr.Read(); charIndex++;
                                if (c == ';')
                                {
                                    break;
                                }
                            }
                            readModule = false;
                            break;
                    }

                    string s = new string(keyWordBuffer, 0, keyWordBufferIndex);

                    if (readNet)
                    {
                        net = new XDLNet();

                        net.Header = s;
                        List<string> atoms = SplitLine(s);
                        for (int i = 0; i < atoms.Count; i++)
                        {
                            if (atoms[i].Equals("net"))
                            {
                                net.Name = atoms[i + 1];
                                net.Name = net.Name.Replace("\"", "");
                                if (atoms.Count > i + 2)
                                {
                                    net.HeaderExtension = atoms[i + 2];
                                }
                                break;
                            }
                        }
                        lineBuffer.Clear();
                    }
                    else if (readInstance)
                    {
                        inst = new XDLInstance();
                        List<string> atoms = SplitLine(s);
                        inst.AddCode(s);
                        for (int i = 0; i < atoms.Count; i++)
                        {
                            if (atoms[i].Equals("inst"))
                            {
                                //inst.Name = Regex.Replace(atoms[i + 1], "\"", "");
                                inst.Name = atoms[i + 1];
                                inst.Name = inst.Name.Replace("\"", "");
                                //inst.SliceType = Regex.Replace(atoms[i + 2], "\"", "");
                                inst.SliceType = atoms[i + 2];
                                inst.SliceType = inst.SliceType.Replace("\"", "");
                            }
                            else if (atoms[i].Equals("placed"))
                            {
                                string[] locationAtoms = atoms[i + 1].Split('X', 'Y');
                                inst.LocationX = int.Parse(locationAtoms[locationAtoms.Length - 2]);
                                inst.LocationY = int.Parse(locationAtoms[locationAtoms.Length - 1]);
                                inst.Location = atoms[i + 1];
                                inst.SliceName = atoms[i + 2];

                                break;
                            }
                        }

                        c = (char)sr.Read(); charIndex++;
                        if (c != 'm') // should be \r
                        {
                            inst.AddCode(c);
                        }
                        else
                        {
                            modReferenceBufferIndex = 0;
                            // consume module reference (will not become part of the module)
                            modReferenceBuffer[modReferenceBufferIndex++] = c;
                            while (true)
                            {
                                c = (char)sr.Read(); charIndex++;
                                modReferenceBuffer[modReferenceBufferIndex++] = c;
                                if (c == ',')
                                {
                                    break;
                                }
                            }
                            inst.ModuleReference = new string(modReferenceBuffer, 0, modReferenceBufferIndex);
                        }
                    }
                }
            }
            sr.Close();
        }

        private static List<string> SplitLine(string s)
        {
            try
            {
                List<string> result = new List<string>();

                bool pendingQuote = false;

                int index = 0;
                string buffer = "";
                while (index < s.Length)
                {
                    char current = s[index++];
                    if (current == '"')
                    {
                        pendingQuote = !pendingQuote;
                    }
                    if ((current == ' ' || current == ',') && !pendingQuote)
                    {
                        if (!string.IsNullOrWhiteSpace(buffer))
                        {
                            result.Add(buffer);
                        }
                        buffer = "";
                    }
                    else
                    {
                        buffer += current;
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private char[] m_pipBuffer = new char[2048];

        private void ReadNet(XDLNet net, char c, StreamReader sr, ref int charIndex, int length, Command cmd)
        {
            m_pipBuffer[0] = c;
            int pipBufferIndex = 1;
            bool pendingQuotes = false;
            while (c != ';')
            {
                // net "R_I" , cfg " _BELSIG:PAD,PAD,R_I:R_I",
                c = (char)sr.Read(); charIndex++;
                if (c == '"')
                {
                    pendingQuotes = !pendingQuotes;
                }
                if (pipBufferIndex >= m_pipBuffer.Length)
                {
                    throw new ArgumentException("Found malformed pip " + new string(m_pipBuffer, 0, 32));
                }
                m_pipBuffer[pipBufferIndex++] = c;
                //lineBuffer.Append(c);
                if (c == ',' && !pendingQuotes)
                {
                    ProcessNextNetLine(m_pipBuffer, pipBufferIndex, net);
                    pipBufferIndex = 0;
                    if (cmd != null)
                    {
                        cmd.ProgressInfo.Progress = (int)((double)charIndex / (double)length * 100);
                    }
                }
            }
        }

        private void ReadInstance(XDLInstance inst, char c, StreamReader sr, ref int charIndex)
        {
            inst.AddCode(c);
            while (c != ';')
            {
                c = (char)sr.Read(); charIndex++;
                inst.AddCode(c);
            }
        }

        private void ReadPort(XDLModule module, char c, StreamReader sr, ref int charIndex)
        {
            string portCode = "";
            portCode += c;
            while (true)
            {
                c = (char)sr.Read(); charIndex++;
                if (c == ';')
                {
                    break;
                }
                portCode += c;
            }

            XDLPort p = ExtractPort(portCode);
            module.Add(p);
        }

        private void ProcessNextNetLine(char[] buffer, int size, XDLNet net)
        {
            string[] atoms = null;
            try
            {
                atoms = m_splitNetLineWhiteSpaceOnly.Split(new string(buffer, 0, size));
            }
            catch
            {
                Console.WriteLine(size);
                Console.WriteLine(buffer);
                Console.WriteLine(net);
            }
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i].Equals("pip"))
                {
                    string location = atoms[i + 1];
                    string from = atoms[i + 2];
                    string op = atoms[i + 3];
                    string to = atoms[i + 4].Replace(",", "");
                    XDLPip pip = new XDLPip(location, from, op, to);
                    net.Add(pip);
                    return;
                }
                else if (atoms[i].Equals("inpin"))
                {
                    NetInpin inpin = new NetInpin();
                    inpin.InstanceName = atoms[i + 1];
                    inpin.SlicePort = atoms[i + 2];
                    net.Add(Trim(inpin));
                    return;
                }
                else if (atoms[i].Equals("outpin"))
                {
                    NetOutpin outpin = new NetOutpin();
                    outpin.InstanceName = atoms[i + 1];
                    outpin.SlicePort = atoms[i + 2];
                    net.Add(Trim(outpin));
                    return;
                }
                else if (atoms[i].Equals("cfg"))
                {
                    //net.Config = "cfg ";
                    for (int j = 0; j < size; j++)
                    {
                        net.Config += buffer[j];
                    }
                    return;
                }
            }
        }

        private XDLPort ExtractPort(string portCode)
        {
            // extract port
            string[] atoms = Regex.Split(portCode, @"\s+");

            XDLPort port = new XDLPort();
            port.ExternalName = Regex.Replace(atoms[1], "\"", "");
            port.InstanceName = Regex.Replace(atoms[2], "\"", "");
            port.SlicePort = Regex.Replace(atoms[3], "\"", "");
            port.SlicePort = Regex.Replace(port.SlicePort, ";", "");
            port.Direction = FPGATypes.GetDirection(new Port(port.SlicePort));

            return port;
        }

        public static XDLInstance ExtractInstance(string instanceCode)
        {
            instanceCode = Regex.Replace(instanceCode, @"^\s*", "");

            List<string> atoms = SplitLine(instanceCode);

            // extract instance
            XDLInstance inst = new XDLInstance();
            inst.Name = Regex.Replace(atoms[1], "\"", "");
            inst.SliceType = Regex.Replace(atoms[2], "\"", "");

            for (int i = 0; i < atoms.Count; i++)
            {
                if (atoms[i].Equals("placed"))
                {
                    int index = i + 1;
                    string[] locationAtoms = atoms[index].Split('X', 'Y');
                    inst.LocationX = int.Parse(locationAtoms[locationAtoms.Length - 2]);
                    inst.LocationY = int.Parse(locationAtoms[locationAtoms.Length - 1]);
                    inst.Location = atoms[index];

                    inst.SliceName = atoms[index + 1];
                    break;
                }
            }
            // fields TileKey can only be set if the target FPGA is loaded,
            // they will be set on first get

            inst.AddCode(instanceCode);

            return inst;
        }

        private NetPin Trim(NetPin np)
        {
            np.InstanceName = np.InstanceName.Replace("\"", "");
            np.SlicePort = np.SlicePort.Replace("\"", "");
            np.SlicePort = np.SlicePort.Replace(",", "");
            return np;
        }

        private static Regex m_splitNetLineWhiteSpaceOnly = new Regex(@"\s+", RegexOptions.Compiled);
    }
}