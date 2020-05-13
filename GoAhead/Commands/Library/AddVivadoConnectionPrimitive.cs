using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    struct BELInfo
    {
        public string belName;
        public bool makeInputsConstant;
        public string inputPortPrefix;
        public string outputPort;

        public BELType type;
        public bool traverseBackwards;
    }

    [CommandDescription(Description = "Add a Vivado connection primitive.", Wrapper = false, Publish = true)]
    class AddVivadoConnectionPrimitive : Command
    {
        [Parameter(Comment = "The name of ths connection primitive")]
        public string Name = "VivadoConnectionPrimitive";

        [Parameter(Comment = "The type of BEl that is instantiated (currently only LUT6 is supported/tested")]
        public string BELType = "LUT6";

        [Parameter(Comment = "The list of BELs incorporated in this connection primitive, used for conected wires (currently only the default is supported")]
        public List<string> BELs = new List<string> { "A6LUT", "B6LUT", "C6LUT", "D6LUT" };

        [Parameter(Comment = "We need to provide the prefix for the input bel input ports (namely LUT input ports)")]
        public List<string> InputBELinputPortPrefix = new List<string> { "_L_A", "_L_B", "_L_C", "_L_D" };

        [Parameter(Comment = "Which BEL out of BELs shall be used for connecting the four wires (e.g. EE2END0..3). See command PrintLUTRouting for finding a suitable LUT")]
        public string InputBEL = "C6LUT";

        [Parameter(Comment = "Which ports in the interconnect tile shall be blocked to allow stop over routing, e.g. WW2END3 -> FAN_ALT3 -> FAN_BOUNCE3 -> IMUX_L21 -> CLBLM_IMUX21 -> CLBLM_L_C4. These port will be excluded from blocking as they are needed by this connection primitive ")]
        public List<string> StopOverPorts = new List<string> { "WW2END3", "FAN_ALT3", "FAN_BOUNCE3", "IMUX_L21" };

        [Parameter(Comment = "The name of the BEl (currently only 6LUT) output ports")]
        public List<string> BELOutputPorts = new List<string> { "A", "B", "C", "D" };

        [Parameter(Comment = "The slice number where to place the BELs")]
        public int SliceNumber = 0;

        protected override void DoCommandAction()
        {
            CheckParameters();

            LibraryElement mainElement = GetMainElement();
            
            for (int i = 0; i < BELs.Count; i++)
            {
                string bel = BELs[i];                
                string prefix = InputBELinputPortPrefix[i];
                string outputPort = BELOutputPorts[i];
                LibraryElement elem = GetOtherElement(bel, !bel.Equals(InputBEL), prefix, outputPort);

                mainElement.Add(elem);
                Objects.Library.Instance.Add(elem);
            }
        }

        private void CheckParameters()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Name must not be empty");
            }
            if (BELs.Count == 0)
            {
                throw new ArgumentException("No BELs provided");
            }
            if (!string.IsNullOrEmpty(InputBEL))
            {
                if (BELs.IndexOf(InputBEL) < 0)
                {
                    throw new ArgumentException("InputBEL must be included in BELs");
                }
            }
            if(BELs.Any(s => Regex.IsMatch(s, @"[A-D]LUT6$^")))
            {
                throw new ArgumentException("InputBEL must be included in A..DLUT6");
            }
            if (BELs.Count != InputBELinputPortPrefix.Count)
            {
                throw new ArgumentException("All list arguments must be of the same size (1)");
            }
            /*
            if (this.BELs.Count != this.InputBELPorts.Count)
            {
                throw new ArgumentException("All list arguments must be of the same size (2)");
            }
             * */
            if (BELs.Count != BELOutputPorts.Count)
            {
                throw new ArgumentException("All list arguments must be of the same size (3)");
            }
        }

        private LibraryElement GetMainElement()
        {
            if (Objects.Library.Instance.Contains(Name)) return Objects.Library.Instance.GetElement(Name);

            LibraryElement newElement = new LibraryElement
            {
                Name = Name,
                VivadoConnectionPrimitive = true,
                Containter = new NetlistContainer()
            };
            Objects.Library.Instance.Add(newElement);

            return newElement;
        }

        private LibraryElement GetOtherElement(string belName, bool makeInputsConstant, string inputPortPrefix, string outputPort)
        {
            BELInfo info = new BELInfo
            {
                belName = belName,
                makeInputsConstant = makeInputsConstant,
                inputPortPrefix = inputPortPrefix,
                outputPort = outputPort
            };

            BELType? type = VivadoBELManager.Instance.GetBELType(BELType);
            if (type != null)
            {
                info.traverseBackwards = false;
                info.type = (BELType)type;
                // if (type == LUT6)  type.Value.inputsConstantValue = makeInputsConstant

                return GetBEL(info);
            }

            throw new ArgumentException("Unsupported BEL Type: " + BELType);
        }

        private LibraryElement GetBEL(BELInfo info)
        {
            LibraryElement element = new LibraryElement
            {
                SliceNumber = SliceNumber,
                Name = info.belName,
                PrimitiveName = BELType,
                BEL = info.belName,
                LoadCommand = ToString(),
                Containter = new XDLModule(),
                VHDLGenericMap = info.type.VHDLGenericMap,
                VivadoConnectionPrimitive = true
            };

            // Outputs
            foreach (string output in info.type.outputNames)
                element.Containter.Add(MakeXDLPort(output, FPGATypes.PortDirection.Out));

            // Inputs
            foreach (string input in info.type.inputNames)
                element.Containter.Add(MakeXDLPort(input, FPGATypes.PortDirection.In, info.type.inputsConstantValue));

            // Get first encountered clb ?!
            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            Tile interconnect = FPGATypes.GetInterconnectTile(clb);

            if (info.traverseBackwards)
                TraverseBackwards(element, clb, interconnect, info);

            foreach (string stopOverPortName in StopOverPorts)
                element.AddPortToBlock(interconnect, new Port(stopOverPortName));


            return element;
        }

        private void TraverseBackwards(LibraryElement element, Tile clb, Tile interconnect, BELInfo info)
        {
            List<string> lutPortNames = new List<string>();

            // see LUTRouting tab
            for (int i = 1; i <= info.type.inputNames.Count; i++)
            {
                lutPortNames.Add(info.inputPortPrefix + i);
            }

            foreach (string s in lutPortNames)
            {
                // travers backwards into INT
                foreach (Tuple<Port, Port> t in clb.SwitchMatrix.GetAllArcs().Where(a => a.Item2.Name.EndsWith(s)))
                {
                    element.AddPortToBlock(clb, t.Item1);
                    element.AddPortToBlock(clb, t.Item2);
                    if (interconnect.WireList == null)
                    {
                        OutputManager.WriteWarning("No wire list found on " + interconnect.Location);
                    }
                    else
                    {
                        foreach (Wire w in interconnect.WireList.Where(w => w.PipOnOtherTile.Equals(t.Item1.Name)))
                        {
                            element.AddPortToBlock(interconnect, new FPGA.Port(w.LocalPip));
                        }
                    }
                }
            }

            // we always need to exclude the port from the LUT output from blocking
            // assuming name
            foreach (Tuple<Port, Port> t in clb.SwitchMatrix.GetAllArcs().Where(a => a.Item1.Name.EndsWith(info.outputPort)))
            {
                // no realy neccessary
                element.AddPortToBlock(clb, t.Item1);
                element.AddPortToBlock(clb, t.Item2);
                foreach (Wire w in clb.WireList.Where(w => w.LocalPip.Equals(t.Item2.Name)))
                {
                    element.AddPortToBlock(interconnect, new Port(w.PipOnOtherTile));
                }
            }
        }

        private XDLPort MakeXDLPort(string name, FPGATypes.PortDirection dir, bool makeInputsConstant = false)
        {
            return new XDLPort
            {
                Direction = dir,
                ExternalName = name,
                InstanceName = "unknown",
                SlicePort = "unknown",
                ConstantValuePort = makeInputsConstant
            };
        }

        public override void Undo()
        {
        }

    }
}
