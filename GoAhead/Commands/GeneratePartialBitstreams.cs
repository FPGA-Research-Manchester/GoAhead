using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;
using GoAhead.Commands.Data;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.LibraryElementInstantiation;
using GoAhead.Commands.Library;

namespace GoAhead.Commands
{
    class GeneratePartialBitstreams : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            string batchFile = TargetDirectory + "\\gen_bitstreams.bat";

            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }

            if (File.Exists(batchFile))
            {
                File.Delete(batchFile);
            }

            TextWriter tw = new StreamWriter(batchFile, true);
            tw.WriteLine("rm *.ncd > NUL");
            tw.WriteLine("rm *.bgn > NUL");
            tw.WriteLine("rm *.xwbt > NUL");
            tw.WriteLine("rm *.bit > NUL");
            tw.WriteLine("rm *.bin > NUL");
            tw.Close();

            if (Positions.All(s => string.IsNullOrEmpty(s)))
            {
                string moduleName = Path.GetFileNameWithoutExtension(Module);
                if (!Objects.Library.Instance.Contains(moduleName))
                {
                    AddBinaryLibraryElement addCmd = new AddBinaryLibraryElement();
                    addCmd.FileName = Module;
                    CommandExecuter.Instance.Execute(addCmd);
                }               
                LibraryElement libElement = Objects.Library.Instance.GetElement(moduleName);
                StringBuilder errorList = null;
                foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
                {
                    bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(tile, libElement, out errorList);
                    if (placementOk)
                    {
                        PlaceModuleAndAddBatchFileEntry(tile, batchFile);
                        //this.OutputManager.WriteOutput("Placing " + moduleName + " to " + tile.Location);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Positions.Count; i++)
                {
                    string[] atoms = Positions[i].Split('X', 'Y');
                    int x = int.Parse(atoms[1]);
                    int y = int.Parse(atoms[2]);
                     Tile anchor = FPGA.FPGA.Instance.GetAllTiles().Where(t =>
                        IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB) &&
                        t.LocationX == x &&
                        t.LocationY == y).FirstOrDefault();

                     if (anchor == null)
                     {
                         throw new ArgumentException("Could not find an anchor for " + Positions[i]);
                     }

                    PlaceModuleAndAddBatchFileEntry(anchor, batchFile);
                }
            }
        }

        private string GetFileName(int x, int y, string extension)
        {
            string target = TargetDirectory + "\\" + Path.GetFileNameWithoutExtension(Module) + "_X" + x.ToString() + "Y" + y.ToString() + "." + extension;
            return target;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Place the module at the given positions and return the resulting partial bitstream
        /// </summary>
        /// <param name="libraryElementname"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private void PlaceModuleAndAddBatchFileEntry(Tile anchor, string batchFile)
        {
            string xdlFile = GetFileName(anchor.LocationX, anchor.LocationY, "xdl");
            string ncdFile = GetFileName(anchor.LocationX, anchor.LocationY, "ncd");
            string bitFile = GetFileName(anchor.LocationX, anchor.LocationY, "bit");

            string libraryElement = Path.GetFileNameWithoutExtension(Module);

            // read in module
            if (!Objects.Library.Instance.Contains(libraryElement))
            {
                AddBinaryLibraryElement addCmd = new AddBinaryLibraryElement();
                addCmd.FileName = Module;
                CommandExecuter.Instance.Execute(addCmd);
            }

            // place module
            PlaceModule placeCmd = new PlaceModule();
            placeCmd.AnchorLocation = anchor.Location;
            placeCmd.AutoClearModuleSlot = true;
            placeCmd.InstanceName = "inst_" + libraryElement;
            placeCmd.LibraryElementName = libraryElement;
            placeCmd.NetlistContainerName = NetlistContainerName;
            placeCmd.Mute = Mute;
            CommandExecuter.Instance.Execute(placeCmd);
            
            // save as netlist
            SaveAsBlocker saveCmd = new SaveAsBlocker();
            saveCmd.FileName = xdlFile;
            saveCmd.NetlistContainerNames.Add(NetlistContainerName);
            CommandExecuter.Instance.Execute(saveCmd);

            TextWriter tw = new StreamWriter(batchFile, true);
            tw.WriteLine("xdl -xdl2ncd " + xdlFile + " " + ncdFile + " -nodrc");
            tw.WriteLine("bitgen " + ncdFile + " -d -w -g ActiveReconfig:Yes -g CRC:Disable -g binary:Yes -r " + EmptyBitfile);
            tw.Close();
            /*
            // do it
            Process xdl2ncd = new Process();
            xdl2ncd.StartInfo.FileName = "xdl";
            xdl2ncd.StartInfo.Arguments = "-xdl2ncd " + xdlFile + " " + ncdFile + " -nodrc";
            xdl2ncd.Start();
            xdl2ncd.WaitForExit();
            
            Process bitgen = new Process();
            bitgen.StartInfo.FileName = "bitgen";
            bitgen.StartInfo.Arguments = ncdFile + " -d -w -g ActiveReconfig:Yes -g CRC:Disable -g binary:Yes -r " + this.EmptyBitfile;
            bitgen.Start();
            bitgen.WaitForExit();
            */
            // clean up
            Reset resetCmd = new Reset();
            CommandExecuter.Instance.Execute(resetCmd);
        }

        [Parameter(Comment = "The binary module")]
        public string Module = "";

        [Parameter(Comment = "A liszt of X;Y tuples ")]
        public List<string> Positions = new List<string>();

        [Parameter(Comment = "The bitstream used for differential bitstream generation (first.bit)")]
        public string EmptyBitfile = "";

        [Parameter(Comment = "Where to store the partial bitfiles")]
        public string TargetDirectory = "";
    }


}
