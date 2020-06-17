using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.InterfaceManager
{
    [CommandDescription(Description="Reads in an VHDL entiry declaration, extracts its interface and store the interface as a CSV", Wrapper=false, Publish=true)]
    class PrintInterfaceCSVFromVHDL : CommandWithFileOutput
    {
       protected override void DoCommandAction()
        {
            if(!File.Exists(this.VHDlFile))
            {
                throw new ArgumentException(this.VHDlFile + " does not exits");
            }

            //open,in,East,pr0,0;

            VHDLParser parser = new VHDLParser(this.VHDlFile);

            foreach (VHDLParserEntity entity in parser.GetEntities())
            {                
                Dictionary<FPGA.FPGATypes.Direction, StringBuilder> interfaces = new Dictionary<FPGA.FPGATypes.Direction,StringBuilder>();

                foreach (HDLEntitySignal signal in entity.InterfaceSignals.Where(s => s.MetaData != null))
                {
                    if(!interfaces.ContainsKey(signal.MetaData.Direction))
                    {
                        interfaces.Add(signal.MetaData.Direction, new StringBuilder());
                    }
                    foreach (String csvString in this.GetCSVString(entity, signal, signal.MetaData.Direction))
                    {
                        interfaces[signal.MetaData.Direction].AppendLine(csvString);
                    }
                }
                foreach (HDLEntitySignal signal in entity.InterfaceSignals.Where(s => s.MetaData == null))
                {
                }

                this.OutputManager.WriteOutput("############################################");
                this.OutputManager.WriteOutput("# interface derived from entity " + entity.EntityName);
                this.OutputManager.WriteOutput("############################################");
                foreach (KeyValuePair<FPGA.FPGATypes.Direction, StringBuilder> tupel in interfaces)
                {
                    this.OutputManager.WriteOutput("# " + tupel.Key);
                    this.OutputManager.WriteOutput(tupel.Value.ToString());
                }
                this.OutputManager.WriteOutput("############################################");
                this.OutputManager.WriteOutput("# end of interface for entity " + entity.EntityName);
                this.OutputManager.WriteOutput("############################################");
            }

            
        }

        private IEnumerable<String> GetCSVString(VHDLParserEntity entity, HDLEntitySignal signal, FPGA.FPGATypes.Direction direction)
        {
            int to = Math.Max(signal.MSB, signal.LSB);
            int from = Math.Min(signal.MSB, signal.LSB);
            for (int index = to; index >= from; index--)
            {
                // no nam
                String line = signal.SignalName + "(" + index + ")," + signal.SignalDirection + "," + direction+ "," + entity.EntityName + "," + signal.MetaData.Column;
                yield return line;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        
        [Parameter(Comment = "The VHDL file with the entity to read")]
        public String VHDlFile = "entity.vhd";
    }
}
