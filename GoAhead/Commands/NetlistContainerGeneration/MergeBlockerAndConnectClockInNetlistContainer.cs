using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;


namespace GoAhead.Commands.NetlistContainerGeneration
{        
    [CommandDescription(Description="Merge clock pins from the given blocker file", Wrapper=true, Publish=true)]
    class MergeBlockerAndConnectClockInNetlistContainer : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            NetlistContainer nlc = this.GetNetlistContainer();

            foreach (XDLNet bufgNet in nlc.Nets.Where(n => ((XDLNet)n).NetPins.Count(np => np is NetOutpin && np.InstanceName.Contains(this.BUFGInstanceName)) > 0))
            {
                this.OutputManager.WriteOutput("Merging blocker into net " + bufgNet.Name);

                foreach (String blockerFileName in this.XDLBlockerFiles)
                {
                    NetlistContainer blocker = this.ReadBlocker(blockerFileName);
                    foreach (XDLNet blockerNet in blocker.Nets)
                    {
                        foreach (NetPin inpin in blockerNet.NetPins.Where(np => np is NetInpin && np.SlicePort.Contains("CLK")))
                        {
                            // add inpin to BUFG net
                            bufgNet.Add(inpin);
                        }
                    }
                }
            }
        }

        private NetlistContainer ReadBlocker(String blockerFileName)
        {
            NetlistContainer blocker = new NetlistContainer("blocker");
            // read file
            XDLDesignParser parser = new XDLDesignParser(blockerFileName);

            // into design            
            try
            {
                parser.ParseDesign(blocker, this);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error during parsing the design " + blockerFileName + ": " + e.Message + ". Are you trying to open the design on the correct device?");
            }

            return blocker;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the BUFG instance (e.g. instBUFG100)")]
        public String BUFGInstanceName = "NULL";

        [Parameter(Comment = "A list of blockers")]
        public List<String> XDLBlockerFiles = new List<String>();
    }
}
