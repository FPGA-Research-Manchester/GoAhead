using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Insert missing inpin and outpins statements", Wrapper = false, Publish = true)]
    class DerivePinsFromPips : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            NetlistContainer netlistContainer = GetNetlistContainer();
            
            foreach(XDLNet n in netlistContainer.Nets)
            {
                foreach (XDLPip pip in n.Pips)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                    if (!IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                    {
                        continue;
                    }
                    foreach (Slice s in t.Slices)
                    {
                        bool inport = s.PortMapping.IsSliceInPort(new Port(pip.To)); 
                        bool outport = s.PortMapping.IsSliceOutPort(new Port(pip.To));                         

                        if((inport | outport) && pip.To.Contains('_'))
                        {
                            NetPin pin = null;
                            if(inport)
                            {
                                pin = new NetInpin();
                            }
                            else
                            {
                                pin = new NetOutpin();
                            }

                            string[] atoms = pip.To.Split('_');
                            pin.SlicePort = atoms[1];

                            if (netlistContainer.Instances.Any(i => i.SliceName.Equals(s.SliceName)))
                            {
                                // there should be only one instance on the slice
                                XDLInstance inst = (XDLInstance) netlistContainer.Instances.First(i => i.SliceName.Equals(s.SliceName));
                                pin.InstanceName = inst.Name;
                            }
                            else
                            {
                                pin.InstanceName = s.SliceName;
                            }

                            bool pinExistsAlready = n.NetPins.FirstOrDefault(np => np.InstanceName.Equals(pin.InstanceName) && np.SlicePort.Equals(pin.SlicePort)) != null;
                            if (!pinExistsAlready)
                            {
                                n.Add(pin);
                            }
                        }
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
