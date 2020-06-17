using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Save one or more blockers to file. This command shall only be used for macros (XDL-Containter) that were created with BlockSelection", Wrapper = true, Publish = true)]
    class SaveAsBlocker : Command
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE, FPGA.FPGATypes.BackendType.Vivado);

            // work on default netlist if no netlist container names are specifed
            List<String> netlistContainerNames = new List<String>();
            if (this.NetlistContainerNames.Count > 0)
            {
                netlistContainerNames.AddRange(this.NetlistContainerNames);
            }
            else
            {
                netlistContainerNames.Add(NetlistContainerManager.DefaultNetlistContainerName);
            }

            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGA.FPGATypes.BackendType.ISE:
                    GenerateXDL genCmd = new GenerateXDL();
                    genCmd.DesignName = "blocker";
                    genCmd.FileName = this.FileName;
                    genCmd.IncludeDesignStatement = true;
                    genCmd.IncludeDummyNets = true;
                    genCmd.IncludeModuleFooter = false;
                    genCmd.IncludeModuleHeader = false;
                    genCmd.IncludePorts = false;
                    genCmd.NetlistContainerNames = netlistContainerNames;
                    genCmd.SortInstancesBySliceName = false;

                    CommandExecuter.Instance.Execute(genCmd);
                    break;
                case FPGA.FPGATypes.BackendType.Vivado:
                    if (netlistContainerNames.Count != 1)
                    {
                        throw new ArgumentNullException(this.GetType() + " for backend " + FPGA.FPGATypes.BackendType.Vivado + " only supports one netlist container for code generation");
                    }

                    GenerateTCL saveCmd = new GenerateTCL();
                    saveCmd.FileName = this.FileName;
                    saveCmd.NetlistContainerName = netlistContainerNames[0];
                    saveCmd.IncludeLinkDesignCommand = false;
                    CommandExecuter.Instance.Execute(saveCmd);
                    break;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "A list of net list container for which XDL code will be generated")]
        public List<String> NetlistContainerNames = new List<String>();

        [Parameter(Comment = "The filename to write the blocker netlist to")]
        public String FileName = "blocker.xdl";
    }
}
