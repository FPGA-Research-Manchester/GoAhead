using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GoAhead.Commands;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Create an XDl resource descriptions of all FPGAs, parse them in and serialize them", Wrapper = false)]
    class PartGenAll : Command
    {
        public static IEnumerable<String> GetAllPackages()
        {
            if (PartGenAll.m_packages.Count == 0)
            {
                if (FPGA.FPGA.Instance.BackendType == FPGA.FPGATypes.BackendType.ISE)
                {
                    PreparePackagesXDL();
                }
                else if (FPGA.FPGA.Instance.BackendType == FPGA.FPGATypes.BackendType.Vivado)
                {
                    PreparePackagesVivado();
                }
            }

            foreach (String package in PartGenAll.m_packages)
            {
                yield return package;
            }
        }

        private static void PreparePackagesVivado()
        {
            String tempTCLScript = Path.ChangeExtension(Path.GetTempFileName(), "tcl");
            StreamWriter sw = new StreamWriter(tempTCLScript);
            sw.WriteLine("set parts [get_parts]");
            sw.WriteLine("set devices {}");
            sw.WriteLine("foreach part $parts {");
            sw.WriteLine("	set idx [string first - $part]");
            sw.WriteLine("	set device [string range $part 0 $idx-1]");
            sw.WriteLine("  if {[lsearch $devices $device] < 0} {");
            sw.WriteLine("	    lappend devices $device");
            sw.WriteLine("  }");
            sw.WriteLine("}");
            sw.WriteLine("puts $devices");
            sw.Close();

            //vivado -notrace -mode batch -source c:\temp\run.tcl -tclargs spbrg
            ProcessStartInfo devDevInfo = new ProcessStartInfo(@"vivado.bat", " -mode batch -source " + tempTCLScript);
            devDevInfo.UseShellExecute = false;
            devDevInfo.ErrorDialog = true;
            devDevInfo.RedirectStandardOutput = true;
            devDevInfo.CreateNoWindow = false;
            Process p = System.Diagnostics.Process.Start(devDevInfo);

            StreamReader output = p.StandardOutput;
            
            while (true)
            {
                String partLine = output.ReadLine();

                // no more lines
                if (partLine == null)
                {
                    break;
                }
            }
        }

        private static void PreparePackagesXDL()
        {
            //Start partgen exe... 
            ProcessStartInfo partgenInfo = new ProcessStartInfo(@"partgen", " -i");
            partgenInfo.UseShellExecute = false;
            partgenInfo.ErrorDialog = true;
            partgenInfo.RedirectStandardOutput = true;
            partgenInfo.CreateNoWindow = false;
            Process partgen = System.Diagnostics.Process.Start(partgenInfo);

            StreamReader partGenOutput = partgen.StandardOutput;

            String currentDevice = "";
            bool package = false;
            while (true)
            {
                String partLine = partGenOutput.ReadLine();

                // no more lines
                if (partLine == null)
                {
                    break;
                }
                if (Regex.IsMatch(partLine, "SPEEDS"))
                {
                    String[] atoms = Regex.Split(partLine, @"\s");
                    currentDevice = atoms[0];
                    package = true;
                }
                else if (package)
                {
                    String packageName = Regex.Replace(partLine, @"\s", "");
                    // xc6slx16cpg196	-3    -2 > xc6slx16cpg196
                    while (Regex.IsMatch(packageName, @"\-\d$"))
                    {
                        packageName = packageName.Remove(packageName.Length - 2, 2);
                    }
                    PartGenAll.m_packages.Add(currentDevice + "-" + packageName);
                }
            }
        }

        protected override void DoCommandAction()
        {
            List<StringTripel> files = new List<StringTripel>();

            foreach (String package in PartGenAll.GetAllPackages().Where(p => Regex.IsMatch(p, this.FPGAFilter)))
            {
                String[] atoms = Regex.Split(package, "\t");
                String deviceName = atoms[0];
                String xdlFileName = this.StorePath + Path.DirectorySeparatorChar + package + ".xdl";
                String binFPGAFileName = this.StorePath + Path.DirectorySeparatorChar + package + ".binFPGA";

                /*
                StringTripel tripel = new StringTripel();
                tripel.xdlFileName = xdlFileName;
                tripel.binFPGAFileName = binFPGAFileName;
                tripel.decivceName = deviceName;
                files.Add(tripel);
                 * */

                if (!this.PrintCommandsOnly)
                {
                    this.GenerateXDL(xdlFileName, binFPGAFileName, deviceName);
                }
            }

            /*
            if (!this.PrintCommandsOnly)
            {
                // generate XDL in parallel 
                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = this.MaxDegreeOfParallelism;
                Parallel.ForEach(files, parallelOptions, tripel => { this.GenerateXDL(tripel.xdlFileName, tripel.binFPGAFileName, tripel.decivceName); });
            }*/

            // read and serialize must be sequentiell
            foreach (StringTripel tripel in files)
            {
                if (File.Exists(tripel.binFPGAFileName) && this.KeepExisting_binFPGAS)
                {
                    // TODO check if tripel.binFPGAFileName maybe deserialzed, if not ReadXDL!
                    continue;
                }

                // parse in ...
                ReadXDL read = new ReadXDL();
                read.FileName = tripel.xdlFileName;
                read.PrintProgress = true;
                read.ExcludePipsToBidirectionalWiresFromBlocking = this.ExcludePipsToBidirectionalWiresFromBlocking;
                // ... and serialize
                SaveBinFPGA save = new SaveBinFPGA();
                save.FileName = tripel.binFPGAFileName;
                save.Compress = this.Compress;

                if (this.PrintCommandsOnly)
                {
                    this.OutputManager.WriteOutput(read.ToString());
                    this.OutputManager.WriteOutput(save.ToString());
                }
                else
                {
                    CommandExecuter.Instance.Execute(read);
                    CommandExecuter.Instance.Execute(save);

                    // remove?
                    if (!this.KeepXDLFiles)
                    {
                        File.Delete(tripel.xdlFileName);
                    }
                }
            }
        }
        
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private static List<String> m_packages = new List<String>();

        public class StringTripel
        {
            public String xdlFileName = "";
            public String binFPGAFileName = "";
        }

        private void GenerateXDL(String xdlFileName, String binFPGAFileName, String deviceName)
        {
            // check if the XDL is complete, i.e. has a summary at the end
            if (File.Exists(xdlFileName))
            {
                using (FileStream fs = File.OpenRead(xdlFileName))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        sr.BaseStream.Position = fs.Length - 100;
                        String tail = sr.ReadToEnd();
                        if (tail.Contains("(summary"))
                        {
                            return;
                        }
                    }
                }
            }

            // skip this device if xdl and binFpga already exist
            if (File.Exists(binFPGAFileName) && this.KeepExisting_binFPGAS)
            {
                return;
            }
            
            ProcessStartInfo xdlInfo = new ProcessStartInfo(@"xdl", " -report -pips " + (this.AllCons ? "-all_conns " : "") + deviceName + " " + xdlFileName);
            Process xdlGen = Process.Start(xdlInfo);
            // wait for end of xdl generation
            xdlGen.WaitForExit();
            

            // check for success
            if (!File.Exists(xdlFileName))
            {
                this.OutputManager.WriteOutput("Could not generate XDL File " + xdlFileName);
                return;
            }
        }        
                
        [Parameter(Comment = "Only generate XDL and binFPGA for those FPGA families that match this filter")]
        public String FPGAFilter = "^xc[3-7]";
                
        [Parameter(Comment = "Where to store the XDL files and the binFPGA files")]
        public String StorePath = "";

        [Parameter(Comment = "Wheter to keep (TRUE) the intermediate XDL files or not (FALSE)")]
        public bool KeepXDLFiles = true;

        [Parameter(Comment = "Whether to keep before serialized binFPGAs files (TRUE) or not (FALSE)")]
        public bool KeepExisting_binFPGAS = false;

        [Parameter(Comment = "Whether to used the -all_conns sitch for xdl generation")]
        public bool AllCons = true;

        [Parameter(Comment = "Whether to skip the XDL generation (e.g. for only reading existing XDLs and saving binFPGAs)")]
        public bool SkipXDLGeneration = false;

        [Parameter(Comment = "Whether compress the resultung binFPGAs")]
        public bool Compress = false;

        [Parameter(Comment = "Only print the ReadXDL and SaveBinFPGAs command without reading or generating anything")]
        public bool PrintCommandsOnly = false;

        //[Parameter(Comment = "Maximum Degree of Parallelism for XDL Generation. Serialization must run sequentiell")]
        //public int MaxDegreeOfParallelism = 2;

        [Parameter(Comment = "Whether run a ExcludePipsToBidirectionalWiresFromBlocking command after reading (forwared to ReadXDL)")]
        public bool ExcludePipsToBidirectionalWiresFromBlocking = true;
    }
}
