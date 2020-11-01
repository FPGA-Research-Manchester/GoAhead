using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintWirelists : CommandWithFileOutput
    {
        List<string> timings = new List<string>();

        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Wirelist hashcode,Local Pip,Target Pip,LocalPipIsDriver,RelativeX,RelativeY,Primitive Number");
            StringBuilder buffer = new StringBuilder();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (WireList wl in MiniWirelists.Values)
            {
                foreach (PrimitiveWire w in wl)
                {
                    buffer.AppendLine(wl.Key + "," + string.Join(",", PortMappings[w.LocalPip], PortMappings[w.PipOnOtherTile], w.LocalPipIsDriver, w.XIncr, w.YIncr, w.primitiveNumber));
                }
            }
            watch.Stop();
            timings.Add("Time taken to create miniWirelists long string = " + watch.ElapsedMilliseconds);

            watch = System.Diagnostics.Stopwatch.StartNew();
            OutputManager.WriteOutput(buffer.ToString());
            watch.Stop();
            timings.Add("Time taken to dump miniWirelists long string to file = " + watch.ElapsedMilliseconds);

            System.IO.File.WriteAllLines(@"C:\Users\prabh\OneDrive\Desktop\timings\MiniWirelists.txt", timings);
        }
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Mini Wirelists")]
        public Dictionary<int, WireList> MiniWirelists = null;

        [Parameter(Comment = "Port Mappings")]
        public Dictionary<string, int> PortMappings = null;
    }
}
