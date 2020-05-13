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
        public static IEnumerable<string> GetAllPackages()
        {
            if (m_packages.Count == 0)
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

            foreach (string package in m_packages)
            {
                yield return package;
            }
        }

        private static void PreparePackagesVivado()
        {
            string tempTCLScript = Path.ChangeExtension(Path.GetTempFileName(), "tcl");
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
            Process p = Process.Start(devDevInfo);

            StreamReader output = p.StandardOutput;
            
            while (true)
            {
                string partLine = output.ReadLine();

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
            Process partgen = Process.Start(partgenInfo);

            StreamReader partGenOutput = partgen.StandardOutput;

            string currentDevice = "";
            bool package = false;
            while (true)
            {
                string partLine = partGenOutput.ReadLine();

                // no more lines
                if (partLine == null)
                {
                    break;
                }
                if (Regex.IsMatch(partLine, "SPEEDS"))
                {
                    string[] atoms = Regex.Split(partLine, @"\s");
                    currentDevice = atoms[0];
                    package = true;
                }
                else if (package)
                {
                    string packageName = Regex.Replace(partLine, @"\s", "");
                    // xc6slx16cpg196	-3    -2 > xc6slx16cpg196
                    while (Regex.IsMatch(packageName, @"\-\d$"))
                    {
                        packageName = packageName.Remove(packageName.Length - 2, 2);
                    }
                    m_packages.Add(currentDevice + "-" + packageName);
                }
            }
        }

        protected override void DoCommandAction()
        {
            List<StringTripel> files = new List<StringTripel>();

            foreach (string package in GetAllPackages().Where(p => Regex.IsMatch(p, FPGAFilter)))
            {
                string[] atoms = Regex.Split(package, "\t");
                string deviceName = atoms[0];
                string xdlFileName = StorePath + Path.DirectorySeparatorChar + package + ".xdl";
                string binFPGAFileName = StorePath + Path.DirectorySeparatorChar + package + ".binFPGA";

                /*
                StringTripel tripel = new StringTripel();
                tripel.xdlFileName = xdlFileName;
                tripel.binFPGAFileName = binFPGAFileName;
                tripel.decivceName = deviceName;
                files.Add(tripel);
                 * */

                if (!PrintCommandsOnly)
                {
                    GenerateXDL(xdlFileName, binFPGAFileName, deviceName);
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
                if (File.Exists(tripel.binFPGAFileName) && KeepExisting_binFPGAS)
                {
                    // TODO check if tripel.binFPGAFileName maybe deserialzed, if not ReadXDL!
                    continue;
                }

                // parse in ...
                ReadXDL read = new ReadXDL();
                read.FileName = tripel.xdlFileName;
                read.PrintProgress = true;
                read.ExcludePipsToBidirectionalWiresFromBlocking = ExcludePipsToBidirectionalWiresFromBlocking;
                // ... and serialize
                SaveBinFPGA save = new SaveBinFPGA();
                save.FileName = tripel.binFPGAFileName;
                save.Compress = Compress;

                if (PrintCommandsOnly)
                {
                    OutputManager.WriteOutput(read.ToString());
                    OutputManager.WriteOutput(save.ToString());
                }
                else
                {
                    CommandExecuter.Instance.Execute(read);
                    CommandExecuter.Instance.Execute(save);

                    // remove?
                    if (!KeepXDLFiles)
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

        private static List<string> m_packages = new List<string>();

        public class StringTripel
        {
            public string xdlFileName = "";
            public string binFPGAFileName = "";
        }

        private void GenerateXDL(string xdlFileName, string binFPGAFileName, string deviceName)
        {
            // check if the XDL is complete, i.e. has a summary at the end
            if (File.Exists(xdlFileName))
            {
                using (FileStream fs = File.OpenRead(xdlFileName))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        sr.BaseStream.Position = fs.Length - 100;
                        string tail = sr.ReadToEnd();
                        if (tail.Contains("(summary"))
                        {
                            return;
                        }
                    }
                }
            }

            // skip this device if xdl and binFpga already exist
            if (File.Exists(binFPGAFileName) && KeepExisting_binFPGAS)
            {
                return;
            }
            
            ProcessStartInfo xdlInfo = new ProcessStartInfo(@"xdl", " -report -pips " + (AllCons ? "-all_conns " : "") + deviceName + " " + xdlFileName);
            Process xdlGen = Process.Start(xdlInfo);
            // wait for end of xdl generation
            xdlGen.WaitForExit();
            

            // check for success
            if (!File.Exists(xdlFileName))
            {
                OutputManager.WriteOutput("Could not generate XDL File " + xdlFileName);
                return;
            }
        }        
                
        [Parameter(Comment = "Only generate XDL and binFPGA for those FPGA families that match this filter")]
        public string FPGAFilter = "^xc[3-7]";
                
        [Parameter(Comment = "Where to store the XDL files and the binFPGA files")]
        public string StorePath = "";

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
