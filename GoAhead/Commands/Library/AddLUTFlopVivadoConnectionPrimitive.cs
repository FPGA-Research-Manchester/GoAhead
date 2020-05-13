using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Add a Vivado connection primitive.", Wrapper = false, Publish = true)]
    class AddLUTFlopVivadoConnectionPrimitive : Command
    {
        protected override void DoCommandAction()
        {
            CheckParameter();

            
            LibraryElement main;
            if(Objects.Library.Instance.Contains(Name))
            {
                main = Objects.Library.Instance.GetElement(Name);
            }
            else
            {
                main = new LibraryElement();
                main.Name = Name;
                main.VivadoConnectionPrimitive = true;
                main.Containter = new NetlistContainer();
                Objects.Library.Instance.Add(main);
            }
            for (int i = 0; i < BELs.Count; i++)
            {
                string bel = BELs[i];                
                string prefix = InputBELinputPortPrefix[i];
                string outputPort = BELOutputPorts[i];
                string initValue = InitValues[i];
                LibraryElement elem = GetLibraryElement(bel, false, prefix, outputPort, initValue);

                main.Add(elem);
                Objects.Library.Instance.Add(elem);
            }

            /*
            LibElem a = this.GetLUT6LibraryElement("A", true);
            LibElem b = this.GetLUT6LibraryElement("B", true);
            LibElem c = this.GetLUT6LibraryElement("C", false);
            LibElem d = this.GetLUT6LibraryElement("D", true);
            
            main.Add(a);
            main.Add(b);
            main.Add(c);
            main.Add(d);

            Objects.Library.Instance.Add(main);
            Objects.Library.Instance.Add(a);
            Objects.Library.Instance.Add(b);
            Objects.Library.Instance.Add(c);
            Objects.Library.Instance.Add(d);
             * */
        }

        private void CheckParameter()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Name must not be empty");
            }
            if(BELs.Any(s => Regex.IsMatch(s, @"[A-D]LUT6$^")))
            {
                throw new ArgumentException("InputBEL must be included in A..DLUT6");
            }
            if (BELs.Count != InputBELinputPortPrefix.Count)
            {
                throw new ArgumentException("All list arguments must be of the same size (1)");
            }            
            if (BELs.Count != InitValues.Count)
            {
                throw new ArgumentException("All list arguments must be of the same size (2)");
            }            
            if (BELs.Count != BELOutputPorts.Count)
            {
                throw new ArgumentException("All list arguments must be of the same size (3)");
            }
        }

        private LibraryElement GetLibraryElement(string belName, bool makeInputsConstant, string inputPortPrefix, string outputPort, string initValue)
        {
            if (BELType.Equals("LUT6"))
            {
                return GetLUT(belName, makeInputsConstant, inputPortPrefix, outputPort, initValue);
            }
            else if (BELType.Equals("FDRE"))
            {
                return GetFF(belName, makeInputsConstant, inputPortPrefix, outputPort);
            }
            else
            {
                throw new ArgumentException("Unsupportd BEL " + BELType);
            }
        }

        private LibraryElement GetLUT(string belName, bool makeInputsConstant, string inputPortPrefix, string outputPort, string initValue)
        {
            int lutSize = 6;
            LibraryElement el = new LibraryElement();
            el.SliceNumber = SliceNumber;
            el.Name = belName;
            el.PrimitiveName = BELType;
            el.BEL = belName;
            el.LoadCommand = ToString();
            el.Containter = new NetlistContainer();
            el.VHDLGenericMap = "generic map ( INIT => X\"" + initValue + "\" )";
            el.Containter = new XDLModule();
            el.VivadoConnectionPrimitive = true;

            // one output per LUT
            XDLPort outPort = new XDLPort();
            outPort.Direction = FPGATypes.PortDirection.Out;
            outPort.ExternalName = "O";
            outPort.InstanceName = "unknown";
            outPort.SlicePort = "unknown";
            el.Containter.Add(outPort);

            // six inputs I=..I5
            for (int i = 0; i < lutSize; i++)
            {
                AddXDLPort(el, "I", i, FPGATypes.PortDirection.In, makeInputsConstant);
            }

            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            Tile interconnect = FPGATypes.GetInterconnectTile(clb);

            List<string> lutPortNames = new List<string>();
            // see LUTRouting tab
            for (int i = 1; i <= lutSize; i++)
            {
                lutPortNames.Add(inputPortPrefix + i);
            }

            foreach (string stopOverPortName in StopOverPorts)
            {
                el.AddPortToBlock(interconnect, new Port(stopOverPortName));
            }

            return el;
        }

        private LibraryElement GetFF(string belName, bool makeInputsConstant, string inputPortPrefix, string outputPort)
        {
            LibraryElement el = new LibraryElement();
            el.SliceNumber = SliceNumber;
            el.Name = belName;
            el.PrimitiveName = BELType;
            el.BEL = belName;
            el.LoadCommand = ToString();
            el.Containter = new NetlistContainer();
            el.VHDLGenericMap = "generic map ( INIT => '0' )";
            el.Containter = new XDLModule();
            el.VivadoConnectionPrimitive = true;

            // Q output
            XDLPort q = new XDLPort();
            q.Direction = FPGATypes.PortDirection.Out;
            q.ExternalName = "Q";
            q.InstanceName = "unknown";
            q.SlicePort = "unknown";
            el.Containter.Add(q);

            List<string> inputs = new List<string>();
            inputs.Add("D");
            inputs.Add("C");
            inputs.Add("CE");
            inputs.Add("R");

            foreach (string i in inputs)
            {
                XDLPort p = new XDLPort();
                p.Direction = FPGATypes.PortDirection.In;
                p.ExternalName = i;
                p.InstanceName = "unknown";
                p.SlicePort = "unknown";
                p.ConstantValuePort = false;
                el.Containter.Add(p);
            };

            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            Tile interconnect = FPGATypes.GetInterconnectTile(clb);

            foreach (string stopOverPortName in StopOverPorts)
            {
                el.AddPortToBlock(interconnect, new Port(stopOverPortName));
            }
            
            return el;
        }

        private void AddXDLPort(LibraryElement el, string prefix, int index, FPGATypes.PortDirection dir, bool makeInputsConstant)
        {
            XDLPort p = new XDLPort();
            p.Direction = dir;
            p.ExternalName = prefix + index;
            p.InstanceName = "unknown";
            p.SlicePort = "unknown";
            p.ConstantValuePort = makeInputsConstant;
            el.Containter.Add(p);
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The name of ths connection primitive")]
        public string Name = "VivadoConnectionPrimitive";

        [Parameter(Comment = "The type of BEl that is instantiated (currently only LUT6 is supported/tested")]
        public string BELType = "LUT6";

        [Parameter(Comment = "The list of BELs incorporated in this connection primitive, used for conected wires (currently only the default is supported")]
        public List<string> BELs = new List<string> { "A6LUT", "B6LUT", "C6LUT", "D6LUT" };

        [Parameter(Comment = "We need to provide the prefix for the input bel input ports (namely LUT input ports)")]
        public List<string> InputBELinputPortPrefix = new List<string> { "_L_A", "_L_B", "_L_C", "_L_D" };

        [Parameter(Comment = "Which ports in the interconnect tile shall be blocked to allow stop over routing, e.g. WW2END3 -> FAN_ALT3 -> FAN_BOUNCE3 -> IMUX_L21 -> CLBLM_IMUX21 -> CLBLM_L_C4. These port will be excluded from blocking as they are needed by this connection primitive ")]
        public List<string> StopOverPorts = new List<string> { "WW2END3", "FAN_ALT3", "FAN_BOUNCE3", "IMUX_L21" };

        [Parameter(Comment = "Init values for BELs, i.e. generic value for generic map ( INIT => X\"ABCDABCDABCDABCD\" ) ")]
        public List<string> InitValues = new List<string> { "FFFF0000FFFF0000", "CCCCCCCCCCCCCCCC", "FF00FF00FF00FF00", "F0F0F0F0F0F0F0F0" };
        
        [Parameter(Comment = "")]
        public List<string> BELInputPorts = new List<string> { "A", "B", "C", "D" };

        [Parameter(Comment = "The name of the BEl (currently only 6LUT) output ports")]
        public List<string> BELOutputPorts = new List<string> { "A", "B", "C", "D" };

        [Parameter(Comment = "The slice number where to place the BELs")]
        public int SliceNumber = 0;

    }
}
