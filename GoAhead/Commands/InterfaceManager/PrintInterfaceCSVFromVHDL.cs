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
            if(!File.Exists(VHDlFile))
            {
                throw new ArgumentException(VHDlFile + " does not exits");
            }

            //open,in,East,pr0,0;

            VHDLParser parser = new VHDLParser(VHDlFile);

            foreach (VHDLParserEntity entity in parser.GetEntities())
            {                
                Dictionary<FPGA.FPGATypes.Direction, StringBuilder> interfaces = new Dictionary<FPGA.FPGATypes.Direction,StringBuilder>();

                foreach (HDLEntitySignal signal in entity.InterfaceSignals.Where(s => s.MetaData != null))
                {
                    if(!interfaces.ContainsKey(signal.MetaData.Direction))
                    {
                        interfaces.Add(signal.MetaData.Direction, new StringBuilder());
                    }
                    foreach (string csvString in GetCSVString(entity, signal, signal.MetaData.Direction))
                    {
                        interfaces[signal.MetaData.Direction].AppendLine(csvString);
                    }
                }
                foreach (HDLEntitySignal signal in entity.InterfaceSignals.Where(s => s.MetaData == null))
                {
                }

                OutputManager.WriteOutput("############################################");
                OutputManager.WriteOutput("# interface derived from entity " + entity.EntityName);
                OutputManager.WriteOutput("############################################");
                foreach (KeyValuePair<FPGA.FPGATypes.Direction, StringBuilder> tupel in interfaces)
                {
                    OutputManager.WriteOutput("# " + tupel.Key);
                    OutputManager.WriteOutput(tupel.Value.ToString());
                }
                OutputManager.WriteOutput("############################################");
                OutputManager.WriteOutput("# end of interface for entity " + entity.EntityName);
                OutputManager.WriteOutput("############################################");
            }

            
        }

        private IEnumerable<string> GetCSVString(VHDLParserEntity entity, HDLEntitySignal signal, FPGA.FPGATypes.Direction direction)
        {
            int to = Math.Max(signal.MSB, signal.LSB);
            int from = Math.Min(signal.MSB, signal.LSB);
            for (int index = to; index >= from; index--)
            {
                // no nam
                string line = signal.SignalName + "(" + index + ")," + signal.SignalDirection + "," + direction+ "," + entity.EntityName + "," + signal.MetaData.Column;
                yield return line;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        
        [Parameter(Comment = "The VHDL file with the entity to read")]
        public string VHDlFile = "entity.vhd";
    }
}
