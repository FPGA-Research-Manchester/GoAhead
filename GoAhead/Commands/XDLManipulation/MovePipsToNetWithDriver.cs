using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.XDLManipulation
{
    class MovePipsToNetWithDriver : Command
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            // read file
            DesignParser parser = DesignParser.CreateDesignParser(this.XDLInFile);
            // into design
            XDLContainer container = new XDLContainer();
            parser.ParseDesign(container, this);

            XDLNet netWithOutPin = (XDLNet) container.Nets.FirstOrDefault(n => n.OutpinCount == 1 && String.IsNullOrEmpty(((XDLNet)n).HeaderExtension));

            if(netWithOutPin == null)
            {
                throw new ArgumentException("No net with outpin found");
            }

            List<String> namesOfNetsWithoutOutpin = new List<String>();
            foreach (Net net in container.Nets.Where(n => n.OutpinCount == 0))
            {
                namesOfNetsWithoutOutpin.Add(net.Name);
            }

            foreach (String netName in namesOfNetsWithoutOutpin)
            {
                XDLNet net = (XDLNet) container.Nets.FirstOrDefault(n => n.Name.Equals(netName));
                if (net == null)
                {
                    throw new ArgumentException("Net " + netName + " not found");
                }
                foreach (XDLPip pip in net.Pips)
                {
                    netWithOutPin.Add(pip);
                }
                net.ClearPips();
            }

            System.IO.TextWriter tw = new System.IO.StreamWriter(this.XDLOutFile, false);
            tw.WriteLine(container.GetDesignConfig().ToString());

            foreach (XDLModule mod in container.Modules)
            {
                tw.WriteLine(mod.ToString());
            }

            foreach (XDLPort p in container.Ports)
            {
                tw.WriteLine(p.ToString());
            }

            foreach (XDLInstance inst in container.Instances)
            {
                tw.WriteLine(inst.ToString());
            }

            foreach (XDLNet net in container.Nets)
            {
                tw.WriteLine(net.ToString());
            }
            tw.Close();

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to read in")]
        public String XDLInFile = "in.xdl";

        [Parameter(Comment = "The name of the file to write the new design to")]
        public String XDLOutFile = "out.xdl";
    }
}
