using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Read in a blocker and check that the blocker resources are not used by the currently loaded design (read in before via ReadDesign).", Wrapper = false, Publish = true)]
    class CheckDesignAgainstBlocker : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            XDLContainer design = (XDLContainer) GetNetlistContainer();
            XDLContainer blocker = new XDLContainer();

            // read file
            DesignParser parser = DesignParser.CreateDesignParser(Blocker);

            // into design            
            parser.ParseDesign(blocker, this);

            /*
             * only the blocker net is deleted, the blocker instance remains part of the static design
            foreach (XDLInstance inst in blocker.Instances)
            {
                if (design.HasInstanceBySliceName(inst.SliceName))
                {
                    this.OutputManager.WriteOutput("Resource conflict on instance " + inst.Name);
                }
            }*/
            
            foreach (XDLNet net in blocker.Nets)
            {
                foreach (XDLPip pip in net.Pips)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                    if (t.IsPortBlocked(pip.From))
                    {
                        OutputManager.WriteOutput("In net " + net.Name + ": resource conflict on FROM port " + pip);
                    }
                    if (t.IsPortBlocked(pip.To))
                    {
                        OutputManager.WriteOutput("In net " + net.Name + ": resource conflict on TO port " + pip);
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The blocker")]
        public string Blocker = "blocker.xdl";
    }
}
