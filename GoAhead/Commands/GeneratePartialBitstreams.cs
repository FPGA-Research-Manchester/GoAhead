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
            String batchFile = this.TargetDirectory + "\\gen_bitstreams.bat";

            if (!Directory.Exists(this.TargetDirectory))
            {
                Directory.CreateDirectory(this.TargetDirectory);
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

            if (this.Positions.All(s => String.IsNullOrEmpty(s)))
            {
                String moduleName = Path.GetFileNameWithoutExtension(this.Module);
                if (!Objects.Library.Instance.Contains(moduleName))
                {
                    AddBinaryLibraryElement addCmd = new AddBinaryLibraryElement();
                    addCmd.FileName = this.Module;
                    CommandExecuter.Instance.Execute(addCmd);
                }               
                LibraryElement libElement = Objects.Library.Instance.GetElement(moduleName);
                StringBuilder errorList = null;
                foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
                {
                    bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(tile, libElement, out errorList);
                    if (placementOk)
                    {
                        this.PlaceModuleAndAddBatchFileEntry(tile, batchFile);
                        //this.OutputManager.WriteOutput("Placing " + moduleName + " to " + tile.Location);
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.Positions.Count; i++)
                {
                    String[] atoms = this.Positions[i].Split('X', 'Y');
                    int x = Int32.Parse(atoms[1]);
                    int y = Int32.Parse(atoms[2]);
                     Tile anchor = FPGA.FPGA.Instance.GetAllTiles().Where(t =>
                        IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB) &&
                        t.LocationX == x &&
                        t.LocationY == y).FirstOrDefault();

                     if (anchor == null)
                     {
                         throw new ArgumentException("Could not find an anchor for " + this.Positions[i]);
                     }

                    this.PlaceModuleAndAddBatchFileEntry(anchor, batchFile);
                }
            }
        }

        private String GetFileName(int x, int y, String extension)
        {
            String target = this.TargetDirectory + "\\" + Path.GetFileNameWithoutExtension(this.Module) + "_X" + x.ToString() + "Y" + y.ToString() + "." + extension;
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
        private void PlaceModuleAndAddBatchFileEntry(Tile anchor, String batchFile)
        {        
            String xdlFile = this.GetFileName(anchor.LocationX, anchor.LocationY, "xdl");
            String ncdFile = this.GetFileName(anchor.LocationX, anchor.LocationY, "ncd");
            String bitFile = this.GetFileName(anchor.LocationX, anchor.LocationY, "bit");
            
            String libraryElement = Path.GetFileNameWithoutExtension(this.Module);

            // read in module
            if (!Objects.Library.Instance.Contains(libraryElement))
            {
                AddBinaryLibraryElement addCmd = new AddBinaryLibraryElement();
                addCmd.FileName = this.Module;
                CommandExecuter.Instance.Execute(addCmd);
            }

            // place module
            PlaceModule placeCmd = new PlaceModule();
            placeCmd.AnchorLocation = anchor.Location;
            placeCmd.AutoClearModuleSlot = true;
            placeCmd.InstanceName = "inst_" + libraryElement;
            placeCmd.LibraryElementName = libraryElement;
            placeCmd.NetlistContainerName = this.NetlistContainerName;
            placeCmd.Mute = this.Mute;
            CommandExecuter.Instance.Execute(placeCmd);
            
            // save as netlist
            SaveAsBlocker saveCmd = new SaveAsBlocker();
            saveCmd.FileName = xdlFile;
            saveCmd.NetlistContainerNames.Add(this.NetlistContainerName);
            CommandExecuter.Instance.Execute(saveCmd);

            TextWriter tw = new StreamWriter(batchFile, true);
            tw.WriteLine("xdl -xdl2ncd " + xdlFile + " " + ncdFile + " -nodrc");
            tw.WriteLine("bitgen " + ncdFile + " -d -w -g ActiveReconfig:Yes -g CRC:Disable -g binary:Yes -r " + this.EmptyBitfile);
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
        public String Module = "";

        [Parameter(Comment = "A liszt of X;Y tuples ")]
        public List<String> Positions = new List<String>();

        [Parameter(Comment = "The bitstream used for differential bitstream generation (first.bit)")]
        public String EmptyBitfile = "";

        [Parameter(Comment = "Where to store the partial bitfiles")]
        public String TargetDirectory = "";
    }


}
