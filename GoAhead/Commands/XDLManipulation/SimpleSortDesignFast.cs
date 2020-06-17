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
            String bufferFileName = "";
            String instBufferFileName = Path.GetTempFileName();
            String netBufferFileName = Path.GetTempFileName();

            if (this.XDLInFile.Equals(this.XDLOutFile))
            {
                bufferFileName = Path.GetTempFileName();
            }
            else
            {
                bufferFileName = this.XDLOutFile;
            }

            TextWriter result = new StreamWriter(bufferFileName, false);
            TextWriter instBuffer = new StreamWriter(instBufferFileName, false);
            TextWriter netBuffer = new StreamWriter(netBufferFileName, false);
            StreamReader inFile = new StreamReader(this.XDLInFile);

            bool scanForKeyWord = true;
            bool readDesign = false;
            bool readModule = false;
            bool readInst = false;
            bool readNet = false;
            bool readConfig = false;

            StringBuilder blockBuffer = new StringBuilder();
            String keyWordBuffer = "";

            foreach(String c in this.Read(inFile))
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
            String buffer = "";
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
            if (this.XDLInFile.Equals(this.XDLOutFile))
            {
                File.Copy(bufferFileName, this.XDLOutFile, true);
                File.Delete(bufferFileName);
            }
        }

        private IEnumerable<String> Read(StreamReader sr)
        {
            String line = "";
            while((line = sr.ReadLine()) != null)
            {
                if (!Regex.IsMatch(line, @"^\s*#") && line.Length > 0)
                {
                    for(int i=0;i<line.Length;i++)
                    {
                        yield return new String(line[i], 1);
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
        public String XDLInFile = "no file given";

        [Parameter(Comment = "The resulting sorted XDL-File")]
        public String XDLOutFile = "no file given";


    }
}
