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
        public void WriteOutput(string msg)
        {
            if (msg.EndsWith(Environment.NewLine))
            {
                m_output.Append(msg);
                m_outputEndsWithNewLine = true;
            }
            else
            {
                m_output.AppendLine(msg);
                m_outputEndsWithNewLine = false;
            }
        }
        public void WriteWarning(string msg)
        {
            string msgWithDash = Regex.IsMatch(msg, @"\s*#") ? msg : "# " + msg;

            if (msg.EndsWith(Environment.NewLine))
            {
                m_warnings.Append(msgWithDash);
                m_warningEndsWithNewLine = true;
            }
            else
            {
                m_warnings.AppendLine(msgWithDash);
                m_warningEndsWithNewLine = false;
            }
        }
        public void WriteUCFOutput(string msg)
        {
            if (msg.EndsWith(Environment.NewLine))
            {
                m_UCFTrace.Append(msg);
                m_UCFTraceEndsWithNewLine = true;
            }
            else
            {
                m_UCFTrace.AppendLine(msg);
                m_UCFTraceEndsWithNewLine = false;
            }
        }
        public void WriteTCLOutput(string msg)
        {
            if (msg.EndsWith(Environment.NewLine))
            {
                m_TCLTrace.Append(msg);
                m_TCLTraceEndsWithNewLine = true;
            }
            else
            {
                m_TCLTrace.AppendLine(msg);
                m_TCLTraceEndsWithNewLine = false;
            }
        }
        public void WriteVHDLOutput(string msg)
        {
            if (msg.EndsWith(Environment.NewLine))
            {
                m_VHDLTrace.Append(msg);
                m_VHDLTraceEndsWithNewLine = true;
            }
            else
            {
                m_VHDLTrace.AppendLine(msg);
                m_VHDLTraceEndsWithNewLine = false;
            }
        }
        public void WriteWrapperOutput(string msg)
        {
            if (msg.EndsWith(Environment.NewLine))
            {
                m_wrapperTrace.Append(msg);
                m_wrapperTraceEndsWithNewLine = true;
            }
            else
            {
                m_wrapperTrace.AppendLine(msg);
                m_wrapperTraceEndsWithNewLine = false;
            }
        }
        #endregion Write

        #region Get
        public string GetOutput()
        {
            return m_output.ToString();
        }
        public string GetWarnings()
        {
            return m_warnings.ToString();
        }
        public string GetVHDLOuput()
        {
            return m_VHDLTrace.ToString();
        }
        public string GetUCFOuput()
        {
            return m_UCFTrace.ToString();
        }
        public string GetTCLOuput()
        {
            return m_TCLTrace.ToString();
        }
        public string GetWrapperOuput()
        {
            return m_wrapperTrace.ToString();
        }

        public bool OutputEndsWithNewLine
        {
            get { return m_outputEndsWithNewLine; }
        }
        public bool WarningsEndsWithNewLine
        {
            get { return m_warningEndsWithNewLine; }
        }
        public bool UCFTraceEndsWithNewLine
        {
            get { return m_UCFTraceEndsWithNewLine; }
        }
        public bool TCLTraceEndsWithNewLine
        {
            get { return m_TCLTraceEndsWithNewLine; }
        }
        public bool VHDLTraceEndsWithNewLine
        {
            get { return m_VHDLTraceEndsWithNewLine; }
        }
        public bool WrapperTraceEndsWithNewLine
        {
            get { return m_wrapperTraceEndsWithNewLine; }
        }
        #endregion Write

        #region Has
        public bool HasOutput
        {
            get { return m_output.Length > 0; }
        }
        public bool HasWarnings
        {
            get { return m_warnings.Length > 0; }
        }
        public bool HasVHDLOutput
        {
            get { return m_VHDLTrace.Length > 0; }
        }
        public bool HasUCFOutput
        {
            get { return m_UCFTrace.Length > 0; }
        }
        public bool HasTCLOutput
        {
            get { return m_TCLTrace.Length > 0; }
        }
        public bool HasWrapperOutput
        {
            get { return m_wrapperTrace.Length > 0; }
        }
        #endregion Write

        public void Start()
        {
            m_output.Clear();
            m_warnings.Clear();
            m_UCFTrace.Clear();
            m_TCLTrace.Clear();
            m_VHDLTrace.Clear();
            m_wrapperTrace.Clear();
            m_outputEndsWithNewLine = false;
            m_warningEndsWithNewLine = false;
            m_UCFTraceEndsWithNewLine = false;
            m_TCLTraceEndsWithNewLine = false;
            m_VHDLTraceEndsWithNewLine = false;
            m_wrapperTraceEndsWithNewLine = false;
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
