using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    class PrintUnionInterface : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            VHDLFile union = new VHDLFile("union_interface");

            foreach (string vhdlFile in Modules)
            {
                VHDLParser p = new VHDLParser(vhdlFile);
                foreach (VHDLParserEntity entity in p.GetEntities())
                {
                    foreach (HDLEntitySignal signal in entity.InterfaceSignals)
                    {
                        int width = signal.MSB - signal.LSB + 1;
                        if (!union.Entity.HasSignal(signal.SignalName))
                        {
                            union.Entity.Add(signal.SignalName, width, Objects.PortMapper.MappingKind.External);
                            union.Entity.SetDirection(signal.SignalName, FPGA.FPGATypes.GetPortDirectionFromString(signal.SignalDirection));
                        }
                        else
                        {
                            union.Entity.SetSignalWidth(signal.SignalName, Math.Max(width, union.Entity.GetSignalWidth(signal.SignalName)));
                        }
                    }
                }
            }

            OutputManager.WriteOutput("entity union_interface is port (");
            OutputManager.WriteOutput(union.Entity.ToString());
            OutputManager.WriteOutput("end union_interface;");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "A list of files with VHDL Entities")]
        public List<string> Modules = new List<string>();
    }
}
