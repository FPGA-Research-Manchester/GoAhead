using System;
using System.IO;

namespace GoAhead.Code.XDL.ResourceDescription
{
    /// <summary>
    /// StreamReaderWithUndo allows to read lines like a StreamReader but allows also to undo the last consumption
    /// Decorates a StreamReader
    /// </summary>
    public class XDLStreamReaderWithUndo
    {
        public XDLStreamReaderWithUndo(string fileName)
        {
            m_streamReader = new StreamReader(fileName);
        }

        public void Close()
        {
            m_streamReader.Close();
        }

        public string ReadLine()
        {
            if (m_readBufferedLine)
            {
                m_readBufferedLine = false;
                return m_lastLineRead;
            }
            else
            {
                //Console.WriteLine(m_linesRead + " " + line);
                //buffer current line
                m_lastLineRead = m_streamReader.ReadLine();
                return m_lastLineRead;
            }
        }

        /// <summary>
        /// Calling ReadLine the next time will returns the last line read
        /// </summary>
        public void UndoLastRead()
        {
            if (m_readBufferedLine)
            {
                throw new ArgumentException("Only the last consumption can be undone");
            }

            m_readBufferedLine = true;
        }

        #region Member

        private StreamReader m_streamReader;

        /// <summary>
        /// buffer the last line read
        /// </summary>
        private string m_lastLineRead = "";

        /// <summary>
        /// if the next call of ReadLine returns lastLineRead ot the next line from the streamReader
        /// </summary>
        private bool m_readBufferedLine = false;

        #endregion Member
    }
}