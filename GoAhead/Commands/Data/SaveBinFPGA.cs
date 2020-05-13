using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GoAhead.FPGA;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Serialize the currently loaded FPGA into a binary description", Wrapper = false)]
	class SaveBinFPGA : Command
	{
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("The current FPGA has " + FPGA.FPGA.Instance.GetAllTiles().Count(t => t.HasNonstopoverBlockedPorts) + " tiles with blocked ports");

			//Opens a file and serializes m_fpga into it in binary format.
			Stream stream = File.Open(FileName, FileMode.Create);
            GZipStream gz = null;
            if (Compress)
            {
                gz = new GZipStream(stream, CompressionMode.Compress);
            };

            //FPGA.FPGA.Instance.WriteWireListToGoAheadHomeCache();
            FPGA.FPGA.Instance.DoPreSerializationTasks();

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (Compress)
                {
                    formatter.Serialize(gz, FPGA.FPGA.Instance);
                    gz.Flush();
                }
                else
                {
                    formatter.Serialize(stream, FPGA.FPGA.Instance);
                }
            }
            catch (Exception error)
            {
                throw new ArgumentException("Could not serialize FPGA: " + error.Message);
            }
            if (Compress)
            {
                gz.Close();
            }
            stream.Close();

            FPGA.FPGA.Instance.DoPostSerializationTasks();
            
            // remember for other stuff how we read in this FPGA
            OpenBinFPGA loadCmd = new OpenBinFPGA();
            loadCmd.FileName = FileName;

            Objects.Blackboard.Instance.LastLoadCommandForFPGA = loadCmd.ToString();
		}

		public override void Undo()
		{
			throw new ArgumentException("The method or operation is not implemented.");
		}

        [Parameter(Comment = "The name of the file to save the FPGA in")]
		public string FileName = "";

        [Parameter(Comment = "Wheter to compress the resultiung file or not")]
        public bool Compress = false;
	}
}


