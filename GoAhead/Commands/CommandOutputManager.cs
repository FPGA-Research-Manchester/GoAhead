using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands
{
    [Serializable]
    public class CommandOutputManager
    {
        #region Write
        public void WriteOutput(String msg)
        {
            if (msg.EndsWith(System.Environment.NewLine))
            {
                this.m_output.Append(msg);
                this.m_outputEndsWithNewLine = true;
            }
            else
            {
                this.m_output.AppendLine(msg);
                this.m_outputEndsWithNewLine = false;
            }
        }
        public void WriteWarning(String msg)
        {
            String msgWithDash = Regex.IsMatch(msg, @"\s*#") ? msg : "# " + msg;

            if (msg.EndsWith(System.Environment.NewLine))
            {
                this.m_warnings.Append(msgWithDash);
                this.m_warningEndsWithNewLine = true;
            }
            else
            {
                this.m_warnings.AppendLine(msgWithDash);
                this.m_warningEndsWithNewLine = false;
            }
        }
        public void WriteUCFOutput(String msg)
        {
            if (msg.EndsWith(System.Environment.NewLine))
            {
                this.m_UCFTrace.Append(msg);
                this.m_UCFTraceEndsWithNewLine = true;
            }
            else
            {
                this.m_UCFTrace.AppendLine(msg);
                this.m_UCFTraceEndsWithNewLine = false;
            }
        }
        public void WriteTCLOutput(String msg)
        {
            if (msg.EndsWith(System.Environment.NewLine))
            {
                this.m_TCLTrace.Append(msg);
                this.m_TCLTraceEndsWithNewLine = true;
            }
            else
            {
                this.m_TCLTrace.AppendLine(msg);
                this.m_TCLTraceEndsWithNewLine = false;
            }
        }
        public void WriteVHDLOutput(String msg)
        {
            if (msg.EndsWith(System.Environment.NewLine))
            {
                this.m_VHDLTrace.Append(msg);
                this.m_VHDLTraceEndsWithNewLine = true;
            }
            else
            {
                this.m_VHDLTrace.AppendLine(msg);
                this.m_VHDLTraceEndsWithNewLine = false;
            }
        }
        public void WriteWrapperOutput(String msg)
        {
            if (msg.EndsWith(System.Environment.NewLine))
            {
                this.m_wrapperTrace.Append(msg);
                this.m_wrapperTraceEndsWithNewLine = true;
            }
            else
            {
                this.m_wrapperTrace.AppendLine(msg);
                this.m_wrapperTraceEndsWithNewLine = false;
            }
        }
        #endregion Write

        #region Get
        public String GetOutput()
        {
            return this.m_output.ToString();
        }
        public String GetWarnings()
        {
            return this.m_warnings.ToString();
        }
        public String GetVHDLOuput()
        {
            return this.m_VHDLTrace.ToString();
        }
        public String GetUCFOuput()
        {
            return this.m_UCFTrace.ToString();
        }
        public String GetTCLOuput()
        {
            return this.m_TCLTrace.ToString();
        }
        public String GetWrapperOuput()
        {
            return this.m_wrapperTrace.ToString();
        }

        public bool OutputEndsWithNewLine
        {
            get { return this.m_outputEndsWithNewLine; }
        }
        public bool WarningsEndsWithNewLine
        {
            get { return this.m_warningEndsWithNewLine; }
        }
        public bool UCFTraceEndsWithNewLine
        {
            get { return this.m_UCFTraceEndsWithNewLine; }
        }
        public bool TCLTraceEndsWithNewLine
        {
            get { return this.m_TCLTraceEndsWithNewLine; }
        }
        public bool VHDLTraceEndsWithNewLine
        {
            get { return this.m_VHDLTraceEndsWithNewLine; }
        }
        public bool WrapperTraceEndsWithNewLine
        {
            get { return this.m_wrapperTraceEndsWithNewLine; }
        }
        #endregion Write

        #region Has
        public bool HasOutput
        {
            get { return this.m_output.Length > 0; }
        }
        public bool HasWarnings
        {
            get { return this.m_warnings.Length > 0; }
        }
        public bool HasVHDLOutput
        {
            get { return this.m_VHDLTrace.Length > 0; }
        }
        public bool HasUCFOutput
        {
            get { return this.m_UCFTrace.Length > 0; }
        }
        public bool HasTCLOutput
        {
            get { return this.m_TCLTrace.Length > 0; }
        }
        public bool HasWrapperOutput
        {
            get { return this.m_wrapperTrace.Length > 0; }
        }
        #endregion Write

        public void Start()
        {
            this.m_output.Clear();
            this.m_warnings.Clear();
            this.m_UCFTrace.Clear();
            this.m_TCLTrace.Clear();
            this.m_VHDLTrace.Clear();
            this.m_wrapperTrace.Clear();
            this.m_outputEndsWithNewLine = false;
            this.m_warningEndsWithNewLine = false;
            this.m_UCFTraceEndsWithNewLine = false;
            this.m_TCLTraceEndsWithNewLine = false;
            this.m_VHDLTraceEndsWithNewLine = false;
            this.m_wrapperTraceEndsWithNewLine = false;
        }

        public void Stop()
        {

        }

        private StringBuilder m_output = new StringBuilder();
        private StringBuilder m_warnings = new StringBuilder();
        private StringBuilder m_UCFTrace = new StringBuilder();
        private StringBuilder m_TCLTrace = new StringBuilder();
        private StringBuilder m_VHDLTrace = new StringBuilder();
        private StringBuilder m_wrapperTrace = new StringBuilder();

        private bool m_outputEndsWithNewLine = false;
        private bool m_warningEndsWithNewLine = false;
        private bool m_UCFTraceEndsWithNewLine = false;
        private bool m_TCLTraceEndsWithNewLine = false;
        private bool m_VHDLTraceEndsWithNewLine = false;
        private bool m_wrapperTraceEndsWithNewLine = false;
    }

}
