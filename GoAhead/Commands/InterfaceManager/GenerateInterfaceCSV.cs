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
            if (PartialAreaNames.Count != InputMSB.Count ||
                PartialAreaNames.Count != InputLSB.Count || 
                PartialAreaNames.Count != OutputMSB.Count ||
                PartialAreaNames.Count != OutputLSB.Count ||
                PartialAreaNames.Count != StreamMSB.Count ||
                PartialAreaNames.Count != this.streamLSB.Count || 
                PartialAreaNames.Count != Columns.Count ||
                PartialAreaNames.Count != InputSignalNames.Count ||
                PartialAreaNames.Count != OuputSignalNames.Count ||
                PartialAreaNames.Count != StreamSignalNames.Count)
            {
                throw new ArgumentException("Size of PartialAreaNames, InputMSBIndex, InputLSBIndex, OutputMSBIndex, OutputLSBIndex, StreamMSB, StreamLSB, Columns, InputSignalNames, OuputSignalNames, and StreamSignalNames must match");
            }

            for(int i=0;i<PartialAreaNames.Count;i++)
            {
                //StaticToPartial(),in,East,pr1,0;				StaticToPartial(),in,East,pr1,1;					StaticFromPartial(),out,East,pr1,0;					StaticFromPartial(),out,East,pr1,1;

                // -1 zero based index
                int inputMSB = InputMSB[i];
                int inputLSB = InputLSB[i];
                int outputMSB = OutputMSB[i];
                int outputLSB = OutputLSB[i];
                int streamMSB = StreamMSB[i];
                int streamLSB = this.streamLSB[i];
                int columns = Columns[i];
                string inputSignalName = InputSignalNames[i];
                string outputSignalName = OuputSignalNames[i];
                string streamSignalName = StreamSignalNames[i];
                string pr = PartialAreaNames[i];

                OutputManager.WriteOutput("#########################");
                OutputManager.WriteOutput("# interface for " + pr + " " + Direction + ": " + ((inputMSB-inputLSB)+1) + " inputs, " + ((outputMSB-outputLSB)+1) + " outputs, and " + ((streamMSB-streamLSB)+1) + " streaming port using " + columns + " interleaved columns");
                OutputManager.WriteOutput("#########################");

                int lineCounter = 0;
                while (inputMSB >= inputLSB || outputMSB >= outputLSB || streamMSB >= streamLSB)
                {
                    string currentLine = "";
                    // input
                    for (int c = 0; c < columns; c++)
                    {
                        if (inputMSB >= inputLSB)
                        {
                            string signalDef = inputSignalName + "(" + inputMSB-- + "),in," + Direction + "," + pr + "," + c + ";";
                            currentLine += signalDef;
                        }
                        else
                        {
                            string openInput = "open,in," + Direction + "," + pr + "," + c + ";";
                            currentLine += openInput;
                        }                        
                    }

                    // output
                    for (int c = 0; c < columns; c++)
                    {
                        if (outputMSB >= outputLSB)
                        {
                            string signalDef = outputSignalName + "(" + outputMSB-- + "),out," + Direction + "," + pr + "," + c + ";";
                            currentLine += signalDef;
                        }
                        else
                        {
                            string openOutput = "open,out," + Direction + "," + pr + "," + c + ";";
                            currentLine += openOutput;
                        }
                    }

                    // stream
                    for (int c = 0; c < columns; c++)
                    {
                        if (streamMSB >= streamLSB)
                        {
                            string signalDef = streamSignalName + "(" + streamMSB-- + "),stream," + Direction + "," + pr + "," + c + ";";
                            currentLine += signalDef;
                        }
                        else
                        {
                            string openStream = "open,stream," + Direction + "," + pr + "," + c + ";";
                            currentLine += openStream;
                        }
                    }

                    currentLine = Regex.Replace(currentLine, ";", ";" + "\t");

                    OutputManager.WriteOutput(currentLine);
                    lineCounter++;
                    if(lineCounter % 4 == 0)
                    {
                        OutputManager.WriteOutput("");
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        
        [Parameter(Comment = "The names of the partial areas, e.g. pr0,pr1")]
        public List<string> PartialAreaNames = new List<string>(){"pr0","pr1"};

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
        public List<string> InputSignalNames = new List<string>(){"pr0_StaticToPartial", "pr1_StaticToPartial"};

        [Parameter(Comment = "The name of the output signals (from the partial region point of view), e.g. pr0_StaticFromPartial,pr1_StaticFromPartial")]
        public List<string> OuputSignalNames = new List<string>(){"pr0_StaticFromPartial", "pr1_StaticFromPartial"};

        [Parameter(Comment = "The name of the streaming signals, e.g. pr0_stream,pr1_stream")]
        public List<string> StreamSignalNames = new List<string>(){"pr0_stream", "pr1_stream"};

        [Parameter(Comment = "Where to place the interface (East, West, South, North)")]
        public string Direction = "East";
    }
}
