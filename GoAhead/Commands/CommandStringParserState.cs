using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    public class CommandStringParserState
    {   
        public string LastParserCommand
        {
            get { return m_lastParsedCommand; }
            set { m_lastParsedCommand = value; }
        }

        public string ParsedFile
        {
            get { return m_parsedFile; }
            set { m_parsedFile = value; }
        }

        public int LineNumber
        {
            get { return m_lineNumber; }
            set { m_lineNumber = value; }
        }

        public override string ToString()
        {
            return
                (!string.IsNullOrEmpty(ParsedFile) ? "Parsing file " + ParsedFile + Environment.NewLine : "") +
                ("Last successfully parsed command is " + LastParserCommand + Environment.NewLine) +
                ("The parser was working around line " + LineNumber);
        }

        private string m_parsedFile = "";
        private string m_lastParsedCommand = ""; 
        private int m_lineNumber = 0;   
    }
}
