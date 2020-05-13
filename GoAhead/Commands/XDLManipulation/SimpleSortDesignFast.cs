using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.XDLManipulation
{
    class SimpleSortDesignFast : Command
    {
        protected override void DoCommandAction()
        {
            string bufferFileName = "";
            string instBufferFileName = Path.GetTempFileName();
            string netBufferFileName = Path.GetTempFileName();

            if (XDLInFile.Equals(XDLOutFile))
            {
                bufferFileName = Path.GetTempFileName();
            }
            else
            {
                bufferFileName = XDLOutFile;
            }

            TextWriter result = new StreamWriter(bufferFileName, false);
            TextWriter instBuffer = new StreamWriter(instBufferFileName, false);
            TextWriter netBuffer = new StreamWriter(netBufferFileName, false);
            StreamReader inFile = new StreamReader(XDLInFile);

            bool scanForKeyWord = true;
            bool readDesign = false;
            bool readModule = false;
            bool readInst = false;
            bool readNet = false;
            bool readConfig = false;

            StringBuilder blockBuffer = new StringBuilder();
            string keyWordBuffer = "";

            foreach(string c in Read(inFile))
            {
                blockBuffer.Append(c);

                if (scanForKeyWord)
                {
                    keyWordBuffer += c;
                    if (Regex.IsMatch(keyWordBuffer, @"^\s*design"))
                    {
                        readDesign = true;
                        scanForKeyWord = false;
                    }
                    else if (Regex.IsMatch(keyWordBuffer, @"^\s*module"))
                    {
                        readModule = true;
                        scanForKeyWord = false;
                    }
                    else if (Regex.IsMatch(keyWordBuffer, @"^\s*inst"))
                    {
                        readInst = true;
                        scanForKeyWord = false;
                    }
                    else if (Regex.IsMatch(keyWordBuffer, @"^\s*net"))
                    {
                        readNet = true;
                        scanForKeyWord = false;
                    }
                    else if (Regex.IsMatch(keyWordBuffer, @"^\s*cfg"))
                    {
                        readConfig = true;
                        scanForKeyWord = false;
                    }
                }
                                
                if (readDesign)
                {
                    if (c.Equals(";"))
                    {
                        result.Write(blockBuffer.ToString());
                        readDesign = false;
                        blockBuffer.Clear();
                        scanForKeyWord = true;
                        keyWordBuffer = "";
                    }
                }
                else if (readModule)
                {
                    if (Regex.IsMatch(blockBuffer.ToString(), "endmodule.*;"))
                    {
                        result.Write(blockBuffer.ToString());
                        readModule = false;
                        blockBuffer.Clear();
                        scanForKeyWord = true;
                        keyWordBuffer = "";
                    }
                }
                else if (readInst)
                {
                    if (c.Equals(";"))
                    {
                        instBuffer.Write(blockBuffer.ToString());
                        readInst = false;
                        blockBuffer.Clear();
                        scanForKeyWord = true;
                        keyWordBuffer = "";
                    }
                }
                else if (readNet)
                {
                    if (c.Equals(";"))
                    {
                        netBuffer.Write(blockBuffer.ToString());
                        readNet = false;
                        blockBuffer.Clear();
                        scanForKeyWord = true;
                        keyWordBuffer = "";
                    }
                }
                else if (readConfig)
                {
                    if (c.Equals(";"))
                    {
                        readConfig = false;
                        blockBuffer.Clear();
                        scanForKeyWord = true;
                        keyWordBuffer = "";
                    }
                }
            }

            instBuffer.Close();
            netBuffer.Close();
            inFile.Close();

            // append inst and net buffer to result
            TextReader rd = new StreamReader(instBufferFileName);
            string buffer = "";
            while ((buffer = rd.ReadLine()) != null)
            {
                result.WriteLine(buffer);
            }
            rd.Close();
            rd = new StreamReader(netBufferFileName);
            buffer = "";
            while ((buffer = rd.ReadLine()) != null)
            {
                result.WriteLine(buffer);
            }
            rd.Close(); 

            result.Close();

            // overwrite XDLOutfile with temp file and delete temp file
            if (XDLInFile.Equals(XDLOutFile))
            {
                File.Copy(bufferFileName, XDLOutFile, true);
                File.Delete(bufferFileName);
            }
        }

        private IEnumerable<string> Read(StreamReader sr)
        {
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                if (!Regex.IsMatch(line, @"^\s*#") && line.Length > 0)
                {
                    for(int i=0;i<line.Length;i++)
                    {
                        yield return new string(line[i], 1);
                    }
                    yield return Environment.NewLine;
                }
                //Console.WriteLine(line);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The unsorted XDL-File to read in")]
        public string XDLInFile = "no file given";

        [Parameter(Comment = "The resulting sorted XDL-File")]
        public string XDLOutFile = "no file given";


    }
}
