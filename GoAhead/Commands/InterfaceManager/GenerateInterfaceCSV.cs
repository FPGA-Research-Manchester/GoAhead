using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.InterfaceManager
{
    class GenerateInterfaceCSV : CommandWithFileOutput
    {
       protected override void DoCommandAction()
        {
            if (this.PartialAreaNames.Count != this.InputMSB.Count ||
                this.PartialAreaNames.Count != this.InputLSB.Count || 
                this.PartialAreaNames.Count != this.OutputMSB.Count ||
                this.PartialAreaNames.Count != this.OutputLSB.Count ||
                this.PartialAreaNames.Count != this.StreamMSB.Count ||
                this.PartialAreaNames.Count != this.streamLSB.Count || 
                this.PartialAreaNames.Count != this.Columns.Count ||
                this.PartialAreaNames.Count != this.InputSignalNames.Count ||
                this.PartialAreaNames.Count != this.OuputSignalNames.Count ||
                this.PartialAreaNames.Count != this.StreamSignalNames.Count)
            {
                throw new ArgumentException("Size of PartialAreaNames, InputMSBIndex, InputLSBIndex, OutputMSBIndex, OutputLSBIndex, StreamMSB, StreamLSB, Columns, InputSignalNames, OuputSignalNames, and StreamSignalNames must match");
            }

            for(int i=0;i<this.PartialAreaNames.Count;i++)
            {
                //StaticToPartial(),in,East,pr1,0;				StaticToPartial(),in,East,pr1,1;					StaticFromPartial(),out,East,pr1,0;					StaticFromPartial(),out,East,pr1,1;

                // -1 zero based index
                int inputMSB = this.InputMSB[i];
                int inputLSB = this.InputLSB[i];
                int outputMSB = this.OutputMSB[i];
                int outputLSB = this.OutputLSB[i];
                int streamMSB = this.StreamMSB[i];
                int streamLSB = this.streamLSB[i];
                int columns = this.Columns[i];
                String inputSignalName = this.InputSignalNames[i];
                String outputSignalName = this.OuputSignalNames[i];
                String streamSignalName = this.StreamSignalNames[i];
                String pr = this.PartialAreaNames[i];

                this.OutputManager.WriteOutput("#########################");
                this.OutputManager.WriteOutput("# interface for " + pr + " " + this.Direction + ": " + ((inputMSB-inputLSB)+1) + " inputs, " + ((outputMSB-outputLSB)+1) + " outputs, and " + ((streamMSB-streamLSB)+1) + " streaming port using " + columns + " interleaved columns");
                this.OutputManager.WriteOutput("#########################");

                int lineCounter = 0;
                while (inputMSB >= inputLSB || outputMSB >= outputLSB || streamMSB >= streamLSB)
                {
                    String currentLine = "";
                    // input
                    for (int c = 0; c < columns; c++)
                    {
                        if (inputMSB >= inputLSB)
                        {
                            String signalDef = inputSignalName + "(" + inputMSB-- + "),in," + this.Direction + "," + pr + "," + c + ";";
                            currentLine += signalDef;
                        }
                        else
                        {
                            String openInput = "open,in," + this.Direction + "," + pr + "," + c + ";";
                            currentLine += openInput;
                        }                        
                    }

                    // output
                    for (int c = 0; c < columns; c++)
                    {
                        if (outputMSB >= outputLSB)
                        {
                            String signalDef = outputSignalName + "(" + outputMSB-- + "),out," + this.Direction + "," + pr + "," + c + ";";
                            currentLine += signalDef;
                        }
                        else
                        {
                            String openOutput = "open,out," + this.Direction + "," + pr + "," + c + ";";
                            currentLine += openOutput;
                        }
                    }

                    // stream
                    for (int c = 0; c < columns; c++)
                    {
                        if (streamMSB >= streamLSB)
                        {
                            String signalDef = streamSignalName + "(" + streamMSB-- + "),stream," + this.Direction + "," + pr + "," + c + ";";
                            currentLine += signalDef;
                        }
                        else
                        {
                            String openStream = "open,stream," + this.Direction + "," + pr + "," + c + ";";
                            currentLine += openStream;
                        }
                    }

                    currentLine = Regex.Replace(currentLine, ";", ";" + "\t");

                    this.OutputManager.WriteOutput(currentLine);
                    lineCounter++;
                    if(lineCounter % 4 == 0)
                    {
                        this.OutputManager.WriteOutput("");
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        
        [Parameter(Comment = "The names of the partial areas, e.g. pr0,pr1")]
        public List<String> PartialAreaNames = new List<String>(){"pr0","pr1"};

        [Parameter(Comment = "The number of interleaved columns for each area")]
        public List<int> Columns = new List<int>(){2,2};

        [Parameter(Comment = "The indeces of the MSBs, e.g. 63,31")]
        public List<int> InputMSB = new List<int>(){63,63};

        [Parameter(Comment = "The indeces of the LSBs, e.g. 0,0")]
        public List<int> InputLSB = new List<int>(){0,0};

        [Parameter(Comment = "The indeces of the LSBs, e.g. 63,31")]
        public List<int> OutputMSB = new List<int>(){63,63};

        [Parameter(Comment = "The indeces of the LSBs, e.g. 0,0")]
        public List<int> OutputLSB = new List<int>(){0,0};

        [Parameter(Comment = "The indeces of the LSBs, e.g. 63,31")]
        public List<int> StreamMSB = new List<int>(){63,63};

        [Parameter(Comment = "The indeces of the LSBs, e.g. 0,0")]
        public List<int> streamLSB = new List<int>(){0,0};

        [Parameter(Comment = "The name of the input signals (from the partial region point of view), e.g. pr0_StaticToPartial,pr1_StaticToPartial")]
        public List<String> InputSignalNames = new List<String>(){"pr0_StaticToPartial", "pr1_StaticToPartial"};

        [Parameter(Comment = "The name of the output signals (from the partial region point of view), e.g. pr0_StaticFromPartial,pr1_StaticFromPartial")]
        public List<String> OuputSignalNames = new List<String>(){"pr0_StaticFromPartial", "pr1_StaticFromPartial"};

        [Parameter(Comment = "The name of the streaming signals, e.g. pr0_stream,pr1_stream")]
        public List<String> StreamSignalNames = new List<String>(){"pr0_stream", "pr1_stream"};

        [Parameter(Comment = "Where to place the interface (East, West, South, North)")]
        public String Direction = "East";
    }
}
