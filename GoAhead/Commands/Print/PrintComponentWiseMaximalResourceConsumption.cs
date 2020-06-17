using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Print the resources in the given (file) netlists", Wrapper = false, Publish = true)]
    class PrintComponentWiseMaximalResourceConsumption : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            int slicesMax = 0;
            int dspMax = 0;
            int bramMax = 0;

            foreach (String fileName in this.Files.Where(s => !String.IsNullOrEmpty(s)))
            {
                int slices = 0;
                int dsp = 0;
                int bram = 0;

                // read file
                DesignParser parser = DesignParser.CreateDesignParser(fileName);
                // into design
                NetlistContainer inContainer = new NetlistContainer();
                parser.ParseDesign(inContainer, this);

                foreach (Instance inst in inContainer.Instances)
                {
                    if (Objects.IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.CLB)) 
                    { 
                        slices++; 
                    }
                    if (Objects.IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.DSP)) 
                    { 
                        dsp++; 
                    }
                    if (Objects.IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.BRAM)) 
                    { 
                        bram++; 
                    }
                }

                if (slices > slicesMax) { slicesMax = slices; }
                if (dsp > dspMax) { dspMax = dsp; }
                if (bram > bramMax) { bramMax = bram; }
                // TODO chains erkennen
            }

            // two slices per CLB
            this.OutputManager.WriteOutput("CLBs: " + slicesMax / 2 + " DSPs: " + dspMax + " BRAMs: " + bramMax);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = "The netlists to read in")]
        public List<String> Files = new List<String>();
    }
}
