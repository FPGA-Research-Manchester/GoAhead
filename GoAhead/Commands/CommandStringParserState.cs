using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    public class CommandStringParserState
    {   
        public String LastParserCommand
        {
            get { return this.m_lastParsedCommand; }
            set { this.m_lastParsedCommand = value; }
        }

        public String ParsedFile
        {
            get { return this.m_parsedFile; }
            set { this.m_parsedFile = value; }
        }

        public int LineNumber
        {
            get { return this.m_lineNumber; }
            set { this.m_lineNumber = value; }
        }

        public override string ToString()
        {
            return
                (!String.IsNullOrEmpty(this.ParsedFile) ? "Parsing file " + this.ParsedFile + Environment.NewLine : "") +
                ("Last successfully parsed command is " + this.LastParserCommand + Environment.NewLine) +
                ("The parser was working around line " + this.LineNumber);
        }

        private String m_parsedFile = "";
        private String m_lastParsedCommand = ""; 
        private int m_lineNumber = 0;   
    }
}
