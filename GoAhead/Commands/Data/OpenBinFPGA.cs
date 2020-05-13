using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Read a serialized binary FPGA description", Wrapper = true)]
	public class OpenBinFPGA : Command
	{
        protected override void DoCommandAction()
		{
            // reset PRIOR to reading otherwise high lighter in gui might crash
            CommandExecuter.Instance.Execute(new Reset());

            GZipStream gz = null;

            bool decompress = false;
            //Opens a file and deserialize it into FPGA.FPGA.Instance

            StreamDecorator stream = new StreamDecorator(FileName, this);
			
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                FPGA.FPGA.Instance = (FPGA.FPGA)formatter.Deserialize(stream);
            }
            catch(Exception)
            {
                // upon error try to decompress
                stream.Position = 0;
                decompress = true;
                gz = new GZipStream(stream, CompressionMode.Decompress);
                FPGA.FPGA.Instance = (FPGA.FPGA)formatter.Deserialize(gz);                    
            }
            finally
            {
                if (!decompress)
                {
                    stream.Close();
                }
                else
                {
                    gz.Close();
                    stream.Close();
                }
            }

            FPGA.FPGA.Instance.DoPostSerializationTasks();

            CommandExecuter.Instance.Execute(new Reset());
            CommandExecuter.Instance.Execute(new GC());
            // no LoadFPGAFamilyScript here! LoadFPGAFamilyScript is called through Reset

            // remember for other stuff how we read in this FPGA
            Blackboard.Instance.LastLoadCommandForFPGA = ToString();

            // familiy related warnings
            if (FPGA.FPGA.Instance.Family == FPGATypes.FPGAFamily.Spartan6 && FPGA.FPGA.Instance.GetAllTiles().Count(t => t.HasNonstopoverBlockedPorts) == 0)
            {
                OutputManager.WriteWarning("There are no tiles with bidirectional wires excluded from blocking");
                OutputManager.WriteWarning("Consider using ExcludePipsToBidirectionalWiresFromBlocking for avoiding RUGs");
            }

            // Reset tcl context
            TclAPI.ResetContext();
		}

        /*private void OnLoaded()
        {
            // Block all ports on clb switch matrix arcs with stopovers
            if (WireHelper.GetIncludeFlag(WireHelper.IncludeFlag.BlockStopoverPorts))
            {
                WireHelper wireHelper = new WireHelper();

                foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLBRegex)))
                {
                    foreach(var arc in tile.SwitchMatrix.GetAllArcs())
                    {
                        if (wireHelper.IsBELInPip(tile, arc.Item1.Name))
                        {
                            tile.BlockPort(arc.Item1, Tile.BlockReason.Stopover);
                        }
                        if (wireHelper.IsBELOutPip(tile, arc.Item2.Name))
                        {
                            tile.BlockPort(arc.Item2, Tile.BlockReason.Stopover);
                        }
                    }
                }
            }
        }*/

		public override void Undo()
		{
			throw new ArgumentException("The method or operation is not implemented.");
		}
        
        [Parameter(Comment = "The name of the file to read the FPGA from")]
		public string FileName = "";
	}

    public class StreamDecorator : Stream
    {
        public StreamDecorator(string fileName, Command cmd)
        {
            m_stream = File.OpenRead(fileName);
            m_cmd = cmd;
        }

        public override void Close()
        {
            base.Close();
            m_stream.Close();
        }

        public override bool CanRead
        {
            get { return m_stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return m_stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_stream.CanWrite; }
        }

        public override void Flush()
        {
            m_stream.Flush();
        }

        public override long Length
        {
            get { return m_stream.Length; }
        }

        public override long Position
        {
            get
            {
                return m_stream.Position;
            }
            set
            {
                m_stream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (m_cmd != null)
            {
                int currentProgress = (int)((double)Position / (double)Length * 100);
                if (m_cmd.ProgressInfo.Progress != currentProgress)
                {
                    m_cmd.ProgressInfo.Progress = currentProgress;
                }
            }
            return m_stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            m_stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_stream.Write(buffer, offset, count);
        }

        private readonly Command m_cmd;
        private readonly Stream m_stream;
    }
}


