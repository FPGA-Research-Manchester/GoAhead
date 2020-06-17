using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "", Wrapper = true)]
	public class ParseTimingData : Command
	{
        [Parameter(Comment = "The name of the file to read the data from")]
        public string FileName = "";

        [Parameter(Comment = "CSV Column indices of: [tile, start pip, end pip, att1, att2, att3, att4]")]
        public List<int> CSVIndices = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };

        [Parameter(Comment = "Ignore lines in the CSV starting with this")]
        public string IgnoreString = "#"; 

        protected override void DoCommandAction()
		{
            // Clean before parsing
            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles()) t.TimeData = null;

            int lineCounter = -1;
            string problem = "";

            Tile.TimeModelAttributeIndices = new Dictionary<Tile.TimeAttributes, int>();
            int counter = 0;
            for(int i=3; i<=6; i++)
            {
                if(CSVIndices[i] >= 0)
                {
                    Tile.TimeModelAttributeIndices.Add(
                        (Tile.TimeAttributes)Enum.Parse(typeof(Tile.TimeAttributes), (i - 3).ToString()), counter);
                    counter++;
                }
            }
            FPGA.FPGA.Instance.AddTimeModel();

            using (var reader = new StreamReader(FileName))
            {
                Tile tile = null;
                string currentTile = "";

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lineCounter++;
                    if (line.StartsWith(IgnoreString)) continue;

                    string[] values = line.Split(',');
                    for(int i=0; i<values.Length; i++)
                    {
                        values[i] = values[i].Trim('\"');
                    }

                    // Parsing
                    try
                    {
                        problem = "tile";
                        string tileVal = values[CSVIndices[0]];
                        if(tileVal != currentTile)
                        {
                            tile = FPGA.FPGA.Instance.GetTile(tileVal) ?? throw new ArgumentException();
                            currentTile = tileVal;
                        }

                        List<float> attributeValues = new List<float>();
                        for (int i = 3; i <= 6; i++)
                        {
                            problem = "attribute " + (i+1).ToString();
                            if (CSVIndices[i] >= 0)
                            {
                                float val = float.Parse(values[CSVIndices[i]]);
                                attributeValues.Add(val);
                            }
                        }

                        problem = "ports";
                        tile.AddTimeData(values[CSVIndices[1]], values[CSVIndices[2]], attributeValues.ToArray());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error parsing line: " + lineCounter);
                        Console.WriteLine("\"" + line + "\"");
                        Console.WriteLine("Problem: " + problem);
                        return;
                    }
                }
            }
        }

		public override void Undo()
		{
			throw new ArgumentException("The method or operation is not implemented.");
		}
	}
}


