using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using GoAhead.Code.XDL;
using GoAhead.Code.XDL.ResourceDescription;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Read an XDL resource file", Wrapper=true)]
	class ReadXDL : Command
	{
        protected override void DoCommandAction()
		{
            // reset PRIOR to reading to reset high lighter 
            CommandExecuter.Instance.Execute(new Reset());

			// create reader & open file
			XDLStreamReaderWithUndo sr = new XDLStreamReaderWithUndo(FileName);

            FPGA.FPGA.Instance.Reset();

            // XDL is only available with ISE
            FPGA.FPGA.Instance.BackendType = FPGA.FPGATypes.BackendType.ISE;

            XDLTileParser tp = new XDLTileParser();

            try
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    // add space not to match tile_summary or tiles
                    if (line.Contains("(tile "))
                    {
                        tp.ParseTile(line, sr);

                        if (PrintProgress)
                        {
                            ProgressInfo.Progress = (int)((double)FPGA.FPGA.Instance.TileCount / (double)FPGA.FPGA.Instance.NumberOfExpectedTiles * (ReadWireStatements ? 20 : 100));                            
                        }
                    }
                    //skip commens
                    else if (line.StartsWith("#"))
                    {
                        continue;
                    }
                    else if (line.StartsWith("(xdl_resource_report"))
                    {
                        XDLResourceReportParser.Parse(line);
                    }
                    else if (line.StartsWith("(tiles"))
                    {
                        XDLDeviceShapeParser.Parse(line);
                    }
                }
            }
            finally
            {
                sr.Close();
            }
            
            // read wires in second run
            if (ReadWireStatements)
            {
                ReadWireStatements rw = new ReadWireStatements();
                rw.ProgressStart = 20;
                rw.ProgressShare = 30;
                rw.FileName = FileName;
                rw.HandleUnresolvedWires = false;
                rw.PrintProgress = PrintProgress;
                rw.Profile = Profile;
                CommandExecuter.Instance.Execute(rw);

                // in third run
                rw = new ReadWireStatements();
                rw.ProgressStart = 50;
                rw.ProgressShare = 30;
                rw.FileName = FileName;
                rw.HandleUnresolvedWires = true;
                rw.PrintProgress = PrintProgress;
                rw.Profile = Profile;
                CommandExecuter.Instance.Execute(rw);
            }

            if (ExcludePipsToBidirectionalWiresFromBlocking)
            {
                ExcludePipsToBidirectionalWiresFromBlocking exclCmd = new ExcludePipsToBidirectionalWiresFromBlocking();
                exclCmd.Profile = Profile;
                exclCmd.PrintProgress = PrintProgress;
                exclCmd.ProgressStart = 80;
                exclCmd.ProgressShare = 20;
                exclCmd.FileName = "";
                CommandExecuter.Instance.Execute(exclCmd);
            }

            CommandExecuter.Instance.Execute(new Reset());

            // no LoadFPGAFamilyScript here! LoadFPGAFamilyScript is called through Reset
            
            // remember for other stuff how we read in this FPGA
            Objects.Blackboard.Instance.LastLoadCommandForFPGA = ToString();
		}

		public override void Undo()
		{
			throw new ArgumentException("The method or operation is not implemented.");
		}

        [Parameter(Comment = "The XDL file to read")]
        public string FileName = "xc6slx16.xdl";

        [Parameter(Comment = "Whether to read in wire statements")]
        public bool ReadWireStatements = true;

        [Parameter(Comment = "Whether run a ExcludePipsToBidirectionalWiresFromBlocking command after reading")]
        public bool ExcludePipsToBidirectionalWiresFromBlocking = true;


	}
}


