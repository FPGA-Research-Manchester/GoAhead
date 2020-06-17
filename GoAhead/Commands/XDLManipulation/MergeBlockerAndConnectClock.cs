using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.XDLManipulation
{
    [CommandDescription(Description="", Wrapper=true)]
    class MergeBlockerAndConnectClock : Command
    {
        protected override void DoCommandAction()
        {
            String bufferFileName = this.XDLInFile.Equals(this.XDLOutFile) ? Path.GetTempFileName() : this.XDLOutFile;
            
            TextWriter resultingBlocker = new StreamWriter(bufferFileName, false); 

            StreamReader xdlInFile = new StreamReader(this.XDLInFile);
            String currentLine = "";
            String lastLine1 = "";
            String lastLine2 = "";

            while ((currentLine = xdlInFile.ReadLine()) != null)
            {
                // filter out dummy clock nets
                if (this.BUFGInstanceName.Equals("NULL"))
                {
                    resultingBlocker.WriteLine(currentLine);
                }
                else
                {
                    // cut out clock nets
                    if (Regex.IsMatch(currentLine, "dummy_GOA_net.*CLK"))
                    {
                        
                    }
                    else
                    {
                        resultingBlocker.WriteLine(currentLine);
                    }
                }
                
                // search for outpin "BUFG_Instance_name" O, might be spread over multiple lines 
                // net "Inst_system/sys_clk_s" ,
  		        // outpin "Inst_system/clock_generator_0/clock_generator_0/Using_DCM0.DCM0_INST/Using_BUFG_for_CLKDV.CLKDV_BUFG_INST"
  		        // O ,

                lastLine1 = lastLine2;
  		        lastLine2 = currentLine;

                // remove line feed
                lastLine2 = Regex.Replace(lastLine2, "\n", "");

  		        // scan last four line for a match
		        String multiLine = lastLine1 + lastLine2;
		        bool multiLineHit = Regex.IsMatch(multiLine, "outpin.*" + this.BUFGInstanceName + ".*O");
		
		        // scan the current line for a match
		        bool singleLineHit = Regex.IsMatch(currentLine, "outpin.*" + this.BUFGInstanceName + ".*O");

                if (multiLineHit || singleLineHit)
                {
                    multiLine = "";
                    lastLine1 = "";
                    lastLine2 = "";

                    foreach (String blockerFileName in this.XDLBlockerFiles)
                    {
                        StreamReader blockerFile = new StreamReader(blockerFileName);

                        String currentBlockerLine = "";
                        while ((currentBlockerLine = blockerFile.ReadLine()) != null)
                        {
                            if (Regex.IsMatch(currentBlockerLine, "inpin.*CLK"))
                            {
                                // convert
								// net "dummy_ModuleBlocker_clock0<302>",	inpin "SLICE_X82Y126" CLK,;
                                // net "dummy_GOA_netMACCSITE2_X18Y24CLK",                inpin "MACCSITE2_X18Y24" CLK,; 
								// to
								// inpin "SLICE_X82Y126" CLK,
								String[] atoms = Regex.Split(currentBlockerLine, ",");// ($net, $inpin) = split(/,/, $_ ,2);
                                String net = atoms[0];
                                String inpin = atoms[1] + ",";
                                        
								// # cut last ';'
                                inpin = Regex.Replace(inpin, ";", ""); // $inpin =~ tr/;/ /; 
                                resultingBlocker.WriteLine(inpin);
                            }
                        }

                        blockerFile.Close();
                    }
                }
            }

            // add blocker xdl code without header, footer and without dummy clock nets (keep instance and net "...BlockSelection" net)
            foreach (String blockerFileName in this.XDLBlockerFiles)
            {
                StreamReader blockerFile = new StreamReader(blockerFileName);

                String currentBlockerLine = "";
                while ((currentBlockerLine = blockerFile.ReadLine()) != null)
                {
                    if (Regex.IsMatch(currentBlockerLine, "(design )|(module )|(endmodule)"))
                    {
                        continue;
                    }

                    if (!this.BUFGInstanceName.Equals("NULL") && Regex.IsMatch(currentBlockerLine, "(dummy_goa_net.*clock)|(dummy_GOA_net.*CLK)"))
                    {
                        continue;
                    }

                    resultingBlocker.WriteLine(currentBlockerLine);
                }

                blockerFile.Close();
            }

            xdlInFile.Close();
            resultingBlocker.Close();

            // overwrite XDLOutfile with temp file and delete temp file
            if (this.XDLInFile.Equals(this.XDLOutFile))
            {
                File.Copy(bufferFileName, this.XDLOutFile, true);
                File.Copy(bufferFileName, this.XDLOutFile + "before_sort", true);
                File.Delete(bufferFileName);
            }

            SimpleSortDesignFast simpleSortCmd = new SimpleSortDesignFast();
            simpleSortCmd.XDLInFile = this.XDLOutFile;
            simpleSortCmd.XDLOutFile = this.XDLOutFile;
            
            Commands.CommandExecuter.Instance.Execute(simpleSortCmd);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The XDL file to read")]
        public String XDLInFile = "";

        [Parameter(Comment = "The XDL file to write the result to ")]
        public String XDLOutFile = "";

        [Parameter(Comment = "The name of the BUFG instance (e.g. instBUFG100)")]
        public String BUFGInstanceName = "NULL";

        [Parameter(Comment = "A list of blockers")]
        public List<String> XDLBlockerFiles = new List<String>();
    }
}
