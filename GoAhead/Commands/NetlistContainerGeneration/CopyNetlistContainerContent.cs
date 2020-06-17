using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Copy primitives and nets between netlist containers")]
    class CopyNetlistContainerContent: Command
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE, FPGA.FPGATypes.BackendType.Vivado);

            NetlistContainer target = Objects.NetlistContainerManager.Instance.Get(this.Target);

            foreach (NetlistContainer other in Objects.NetlistContainerManager.Instance.NetlistContainer.Where(o => !o.Name.Equals(this.Target)))
            {
                foreach (Instance inst in other.Instances.Where(i => FPGA.TileSelectionManager.Instance.IsSelected(i.TileKey)))
                {
                    target.Add(inst);
                }

                switch (FPGA.FPGA.Instance.BackendType)
                {
                    case GoAhead.FPGA.FPGATypes.BackendType.ISE:
                        CopyXDLNets(target, (XDLContainer)other);
                        break;
                    case GoAhead.FPGA.FPGATypes.BackendType.Vivado:
                        CopyVivadoNets(target, other);
                        break;                   
                }               

            }
        }

        private void CopyXDLNets(NetlistContainer target, XDLContainer other)
        {
            foreach (XDLNet net in other.Nets)
            {
                XDLNet copy = (XDLNet)Net.CreateNet(this.Target + "_" + net.Name);
                foreach (NetPin pin in net.NetPins.Where(p => FPGA.TileSelectionManager.Instance.IsSelected(other.GetInstance(p).TileKey)))
                {
                    copy.Add(pin);
                }
                foreach (XDLPip pip in net.Pips.Where(p => FPGA.TileSelectionManager.Instance.IsSelected(p.Location)))
                {
                    copy.Add(pip);
                }
                if (copy.NetPinCount + copy.PipCount > 0)
                {
                    target.Add(copy);
                }
            }
        }

        private void CopyVivadoNets(NetlistContainer target, NetlistContainer other)
        {
            foreach (Net net in other.Nets)
            {
                Net copy = Net.CreateNet(this.Target + "_" + net.Name);
                /*
                foreach (NetPin pin in net.NetPins.Where(p => FPGA.TileSelectionManager.Instance.IsSelected(other.GetInstance(p).TileKey)))
                {
                    copy.Add(pin);
                }
                foreach (XDLPip pip in net.Pips.Where(p => FPGA.TileSelectionManager.Instance.IsSelected(p.Location)))
                {
                    copy.Add(pip);
                }
                if (copy.NetPinCount + copy.PipCount > 0)
                {
                    target.Add(copy);
                }*/
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the target netlist container")]
        public String Target = "nlc";
    }
}
