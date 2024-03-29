using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Data
{
	class GenerateXDL : Command
	{
        public GenerateXDL()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XDLfileName">name of the xdl file to save the macro in</param>
        /// <param name="m_macroNames"></param>
        public GenerateXDL(string XDLfileName, List<string> macroNames, bool includePorts, bool includeDummyNets, bool includeDesignStatement, bool includeModuleHeader, bool includeModuleFooter)
		{
			FileName = XDLfileName;
			NetlistContainerNames = macroNames;
            IncludePorts = includePorts;
            IncludeDummyNets = includeDummyNets;
            IncludeDesignStatement = includeDesignStatement;
            IncludeModuleHeader = includeModuleHeader;
            IncludeModuleFooter = includeModuleFooter;
		}

        protected override void DoCommandAction() 
		{
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE);

            List<XDLContainer> netlistContainer = new List<XDLContainer>();
            foreach (string netlistContainerName in NetlistContainerNames)
            {
                netlistContainer.Add((XDLContainer)NetlistContainerManager.Instance.Get(netlistContainerName));
            }

            using (StreamWriter sw = new StreamWriter(FileName, false))
            {
                XDLFile file = new XDLFile(
                    IncludePorts, 
                    IncludeDummyNets, 
                    netlistContainer, 
                    IncludeDesignStatement, 
                    IncludeModuleHeader, 
                    IncludeModuleFooter, 
                    DesignName, 
                    SortInstancesBySliceName);

                file.WriteXDLCodeToFile(sw);

                sw.Close();
            }
		}

		public override void Undo()
		{
			throw new ArgumentException("The method or operation is not implemented.");
		}

        [Parameter(Comment = "The name of the file to save the XDL code in")]
        public string FileName = "";

        [Parameter(Comment = "A list of macro names (XDL-Containter) for which XDL code will be generated")]
        public List<string> NetlistContainerNames = new List<string>();

        [Parameter(Comment = "Whether to include XDL ports or not")]
        public bool IncludePorts = false;

        [Parameter(Comment = "Whether to include a dummy net for each XDL port or not")]
        public bool IncludeDummyNets = false;

        [Parameter(Comment = "Whether to include a design header liek e.g. design __XILINX_NMC_MACRO xc6slx16csg324-3")]
        public bool IncludeDesignStatement = false;
                
        [Parameter(Comment ="Whether to a include a module header like e.g. module my_instance, cfg _SYSTEM_MACRO::FALSE")]
        public bool IncludeModuleHeader = true;

        [Parameter(Comment = "Whether to include a module footer like e.g. endmodule my_instance;")]
        public bool IncludeModuleFooter = false;

        [Parameter(Comment = "The Design name to insert in the design statement")]
        public string DesignName = "__XILINX_NMC_MACRO";     
                        
        [Parameter(Comment = "Whether to sort the generated instances by the slice name (e.g. SLICE_X3Y4) (we do not(!) by the instance names")]
        public bool SortInstancesBySliceName = false;
        
	}
}


