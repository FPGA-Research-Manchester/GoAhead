using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.Spartan6
{
    class FixS6XDLBug : Command
    {
       protected override void DoCommandAction()
        {
            StreamReader reader = new StreamReader(this.XDLInFile);
            String line = "";
            bool update = false;
            bool outbufFound = false;
            List<String> unmodifiedBuffer = new List<string>();
            List<String> modifiedBuffer = new List<string>();

            String bufferFileName = "";
            if (this.XDLInFile.Equals(this.XDLOutFile))
            {
                bufferFileName = Path.GetTempFileName();
            }
            else
            {
                bufferFileName = this.XDLOutFile;
            }
            TextWriter result = new StreamWriter(bufferFileName, false);

            while ((line = reader.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, "^inst.*IOB"))
                {
                    update = true;
                    outbufFound = false;
                    unmodifiedBuffer.Add(line);
                    modifiedBuffer.Add(line);
                }
                else if (Regex.IsMatch(line, ";"))
                {
                    update = false;
                    unmodifiedBuffer.Add(line);
                    modifiedBuffer.Add(line);
                    if (!outbufFound)
                    {
                        for (int i = 0; i < modifiedBuffer.Count; i++) { result.WriteLine(modifiedBuffer[i]); }
                    }
                    else
                    {
                        for (int i = 0; i < unmodifiedBuffer.Count; i++) { result.WriteLine(unmodifiedBuffer[i]); }
                    }
                    unmodifiedBuffer.Clear();
                    modifiedBuffer.Clear();
                }
                else if (update)
                {
                    unmodifiedBuffer.Add(line);

                    if (Regex.IsMatch(line, "OUTBUF:"))
                    {
                        outbufFound = true;
                    }
                    if (Regex.IsMatch(line, "PRE_EMPHASIS::#OFF"))
                    {
                        line = Regex.Replace(line, "PRE_EMPHASIS::#OFF", "");
                    }
                    modifiedBuffer.Add(line);
                }
                else
                {
                    result.WriteLine(line);
                }
            }
            reader.Close();
            result.Close();

            // overwrite XDLOutfile with temp file and delete temp file
            if (this.XDLInFile.Equals(this.XDLOutFile))
            {
                File.Copy(bufferFileName, this.XDLOutFile, true);
                File.Delete(bufferFileName);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The XDL-File to read in")]
        public String XDLInFile = "in.xdl";

        [Parameter(Comment = "The XDL-File to write the result to (overwrittten or created)")]
        public String XDLOutFile = "out.xdl";
    }
}
