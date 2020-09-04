using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Commands.Debug;
using GoAhead.FPGA;
using GoAhead.Objects;


namespace GoAhead.Commands.Misc
{

    class PrintTileInformationCSV : CommandWithFileOutput
    {
        public PrintTileInformationCSV()
        {
            
        }

        protected override void DoCommandAction()
        {

            TextWriter write = new StreamWriter(filePath, true);

            // click done out of FPGA range
            if (!FPGA.FPGA.Instance.Contains(x, y))
            {
                throw new ArgumentException("Tile not found");
            }

            //Get tile from X and Y coordinates.
            m_tile = FPGA.FPGA.Instance.GetTile(x, y);

            StringBuilder buffer = new StringBuilder();
  
            string header1 = "switch matrix: in, out \n ";
            string header2 = "\nwires: localpip, localpipisdriver, piponothertile, xincr, yincr, target \n";
            string header3 = "\nLUT Routing: lutinput, in, out, lutoutput\n";


            buffer.Append(PrintGeneralInfo());
            buffer.Append(header1);
            buffer.Append(PrintSwitchMatrixInfo());
            buffer.Append(header2);
            buffer.Append(PrintWireInfo());
            buffer.Append(header3);
            buffer.Append(PrintLUTRoutingInfo());

            //Output in csv format.
            write.Write(buffer.ToString());
            write.Close();

            //File.WriteAllText(filePath, buffer.ToString());

        }
        private string PrintGeneralInfo()
        {
            StringBuilder output = new StringBuilder();

            PrintTileInfo printTileInfoCommand = new PrintTileInfo();
            printTileInfoCommand.Location = m_tile.Location;
            printTileInfoCommand.Do();

            output.Append("\nTile: "+ printTileInfoCommand.OutputManager.GetOutput()+"\n");

            return output.ToString();
        }

        private string PrintLUTRoutingInfo()
        {
            StringBuilder output = new StringBuilder();

            // LUT routing requires wire lists
            if (FPGA.FPGA.Instance.WireListCount == 0)
            {
                return string.Empty;
            }
            if (IdentifierManager.Instance.IsMatch(m_tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                Regex filter1 = null;
                Regex filter2 = null;
                Regex filter3 = null;
                Regex filter4 = null;
                bool filter1Valid = false;
                bool filter2Valid = false;
                bool filter3Valid = false;
                bool filter4Valid = false;
                GetFilter( m_lut_filterIn , out filter1, out filter1Valid);
                GetFilter(m_lut_filterInput, out filter2, out filter2Valid);
                GetFilter(m_lut_filterOut, out filter3, out filter3Valid);
                GetFilter(m_lut_filterOutput, out filter4, out filter4Valid);

                if (!filter1Valid || !filter2Valid || !filter3Valid || !filter4Valid)
                {
                    return "Invalid regular expression for filters.";
                }

                foreach (LUTRoutingInfo info in FPGATypes.GetLUTRouting(m_tile))
                {
                    string port1 = info.Port1 != null ? info.Port1.Name : "";
                    string port2 = info.Port2 != null ? info.Port2.Name : "";
                    string port3 = info.Port3 != null ? info.Port3.Name : "";
                    string port4 = info.Port4 != null ? info.Port4.Name : "";

                    if (filter1.IsMatch(port1) && filter2.IsMatch(port2) && filter3.IsMatch(port3) && filter4.IsMatch(port4))
                    {
                        output.Append(string.Format("{0},{1},{2},{3}\n", port1, port2, port3, port4));
                    }
                }
            }

            return output.ToString();

        }

        private string PrintWireInfo()
        {
            StringBuilder output = new StringBuilder();

            if (!FPGA.FPGA.Instance.ContainsWireList(m_tile.WireListHashCode))
            {
                return string.Empty;
            }
         
            foreach (Wire wire in m_tile.WireList)
            {
                Tile target = Navigator.GetDestinationByWire(m_tile, wire);

                output.Append(string.Format("{0},{1},{2},{3},{4},{5}\n", wire.LocalPip, wire.LocalPipIsDriver, wire.PipOnOtherTile, wire.XIncr, wire.YIncr, target));
                
            }



            return output.ToString();
        }

        private string PrintSwitchMatrixInfo()
        {
            StringBuilder output = new StringBuilder();

            if (!FPGA.FPGA.Instance.ContainsSwitchMatrix(m_tile.SwitchMatrixHashCode))
            {
                return string.Empty;
            }

            GetFilter(m_sm_filterIn, out Regex inFilter, out bool inFilterValid);
            GetFilter(m_sm_filterOut, out Regex outFilter, out bool outFilterValid);

            if (!inFilterValid || !outFilterValid)
            {
                return "Invalid regular expression for filters.";
            }

            foreach (Tuple<Port, Port> arc in m_tile.SwitchMatrix.GetAllArcs().Where(a => inFilter.IsMatch(a.Item1.Name) && outFilter.IsMatch(a.Item2.Name)))
            {
                string toAppend = "";

                // Default entries
                List<object> entries = new List<object>
                {
                    arc.Item1.Name,
                    arc.Item2.Name
                };

                // If time data is present
                List<float> times = m_tile.GetTimeData(arc.Item1, arc.Item2);
                if (times != null)
                {
                    int index = -1;

                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute1);
                    if (index > -1) entries.Add(times[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute2);
                    if (index > -1) entries.Add(times[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute3);
                    if (index > -1) entries.Add(times[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute4);
                    if (index > -1) entries.Add(times[index]);
                }

                int i = 0;

                foreach (object entry in entries)
                {
                    if (i % 2 == 0)
                        toAppend += entry.ToString() + ", ";
                    else
                        toAppend += entry.ToString();
                    i++;
                }

                output.Append(toAppend + '\n');
            }
                return output.ToString();
        }

        private void GetFilter(string regexp, out Regex filter, out bool valid)
        {
            filter = null;
            valid = false;

            if (string.IsNullOrEmpty(regexp))
            {
                filter = new Regex("", RegexOptions.Compiled);
                valid = true;
            }
            else
            {
                valid = true;
                try
                {
                    filter = new Regex(regexp, RegexOptions.Compiled);
                }
                catch (Exception)
                {
                    valid = false;
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private Tile m_tile;
        [Parameter(Comment = "The X coordinate of the desired tile.")]
        public int x = 0;
        [Parameter(Comment = "The Y coordinate of the desired tile.")]
        public int y = 0;
        [Parameter(Comment = "Switch Matrix filter In.")]
        public string m_sm_filterIn = ".*";
        [Parameter(Comment = "Switch Matrix filter Out.")]
        public string m_sm_filterOut = ".*";
        [Parameter(Comment = "LUT Routing filter Input.")]
        public string m_lut_filterInput = ".*";
        [Parameter(Comment = "LUT Routing filter In.")]
        public string m_lut_filterIn = ".*";
        [Parameter(Comment = "LUT Routing filter Out.")]
        public string m_lut_filterOut = ".*";
        [Parameter(Comment = "LUT Routing filter Input.")]
        public string m_lut_filterOutput = ".*";
        [Parameter(Comment = "Output file path.")]
        public string filePath = @"..\tile_information.txt";

    }
}


