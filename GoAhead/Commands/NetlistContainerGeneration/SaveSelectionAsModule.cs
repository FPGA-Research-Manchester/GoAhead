using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Code.TCL;
using GoAhead.Commands.Library;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Save the currently selected area as a module (*.xdl or *.viv_rpt and binary)", Wrapper = true, Publish = true)]
    class SaveSelectionAsModule : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE, FPGA.FPGATypes.BackendType.Vivado);

            if (FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                OutputManager.WriteOutput("Warning: No tiles selected");
            }

            NetlistContainer netlistContainer = (NetlistContainer)GetNetlistContainer();

            // store selected parts in outDesign
            NetlistContainer outContainer = netlistContainer.GetSelectedDesignElements();

            // write design to file
            StreamWriter sw = new StreamWriter(OutFile, false);
            outContainer.WriteCodeToFile(sw);
            sw.Close();

            if (FPGA.FPGA.Instance.BackendType == FPGA.FPGATypes.BackendType.ISE)
            {
                if (!string.IsNullOrEmpty(BinaryNetlist))
                {
                    SaveXDLLibraryElementAsBinaryLibraryElement saveCmd = new SaveXDLLibraryElementAsBinaryLibraryElement();
                    saveCmd.FileName = BinaryNetlist;
                    saveCmd.XDLFileName = OutFile;
                    CommandExecuter.Instance.Execute(saveCmd);
                }
            }
            else if (FPGA.FPGA.Instance.BackendType == FPGA.FPGATypes.BackendType.Vivado)
            {
                if (!string.IsNullOrEmpty(BinaryNetlist))
                {
                    // TODO
                    //SaveXDLLibraryElementAsBinaryLibraryElement fuer SetResourceShapeInfo dann weiter relokieren
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The optional (may be left empty) name of the file to write the new design to (*.xdl or *.viv_rpt")]
        public string OutFile = "out.xdl";

        [Parameter(Comment = "The optional (may be left empty) name of the file to save a binary netlist in (this filename without extension will become the module name)")]
        public string BinaryNetlist = "module.binNetlist";
    }
}
