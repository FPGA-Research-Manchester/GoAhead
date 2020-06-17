using System;
using System.IO;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.Library;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.XDLManipulation
{
    [CommandDescription(Description = "Read in an XDL netlist and cut off everthing inside the current selection and store the result as an XDL netlist and optionally as binary library element", Wrapper = true, Publish = true)]
    class ExtractModule : Command
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            if (FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                this.OutputManager.WriteOutput("Warning: No tiles selected");
            }

            // read file
            DesignParser parser = DesignParser.CreateDesignParser(this.XDLInFile);
            // into design
            NetlistContainer inContainer = new XDLContainer();
            parser.ParseDesign(inContainer, this);

            // store selected parts in outDesign
            XDLContainer outContainer = (XDLContainer)inContainer.GetSelectedDesignElements();

            // write design to file
            StreamWriter sw = new StreamWriter(this.XDLOutFile, false);
            outContainer.WriteCodeToFile(sw);
            sw.Close();

            if (!String.IsNullOrEmpty(this.BinaryNetlist))
            {
                SaveXDLLibraryElementAsBinaryLibraryElement saveCmd = new SaveXDLLibraryElementAsBinaryLibraryElement();
                saveCmd.FileName = this.BinaryNetlist;
                saveCmd.XDLFileName = this.XDLOutFile;
                CommandExecuter.Instance.Execute(saveCmd);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to read in")]
        public String XDLInFile = "in.xdl";

        [Parameter(Comment = "The optional (may be left empty) name of the file to write the new XDL design to")]
        public String XDLOutFile = "out.xdl";

        [Parameter(Comment = "The optional (may be left empty) name of the file to save a binary netlist in (this filename without extension will be come the module name)")]
        public String BinaryNetlist = "module.binNetlist";
    }
}
