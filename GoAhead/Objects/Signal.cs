using System;
using System.ComponentModel;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    [Serializable]
    public class Signal : INotifyPropertyChanged
    {
        public Signal()
        {
            m_signalName = InterfaceManager.Instance.GetNewSignalName();
            m_signalMode = "in";
            m_signalDirection = FPGATypes.InterfaceDirection.East;
        }

        /// <summary>
        /// Parse SignalEntry members from atoms which must contain 7 entries
        /// </summary>
        /// <param name="atoms"></param>
        public Signal(string signalName, string signalMode, FPGATypes.InterfaceDirection signalDirection, string partialRegion, int column)
        {
            m_signalName = signalName;
            m_signalMode = signalMode;
            m_signalDirection = signalDirection;
            m_partialRegion = partialRegion;
            m_column = column;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string SignalName
        {
            get { return m_signalName; }
            set { m_signalName = value; NotifyPropertyChanged("SignalName"); }
        }

        public string SignalNameWithoutBraces
        {
            get
            {
                // Video_Bar(31) -> VideoBar.
                // Assume only one brace
                string[] braces = { "<", "{", "[", "(" };
                foreach (string brace in braces)
                {
                    if (SignalName.Contains(brace))
                    {
                        int index = SignalName.IndexOf(brace);
                        if (index > 0)
                        {
                            return SignalName.Substring(0, index);
                        };
                    };
                }
                return SignalName;
            }
        }

        /// <summary>
        /// in or out
        /// </summary>
        public string SignalMode
        {
            get { return m_signalMode; }
            set { m_signalMode = value; NotifyPropertyChanged("SignalMode"); }
        }

        /// <summary>
        /// East, West, South or North
        /// </summary>
        public FPGATypes.InterfaceDirection SignalDirection
        {
            get { return m_signalDirection; }
            set { m_signalDirection = value; NotifyPropertyChanged("SignalDirection"); }
        }

        public string PartialRegion
        {
            get { return m_partialRegion; }
            set { m_partialRegion = value; NotifyPropertyChanged("PartialRegion"); }
        }

        public int Column
        {
            get { return m_column; }
            set { m_column = value; NotifyPropertyChanged("Column"); }
        }

        public override string ToString()
        {
            return
                "SignalName: " + SignalName + " " +
                "SignalMode: " + SignalMode + " " +
                "SignalDirection: " + SignalDirection + " " +
                "PartialRegion: " + m_partialRegion + " " +
                "Column:" + m_column;
        }

        /// <summary>
        /// Return a comms separated value representation of this signal
        /// </summary>
        /// <returns></returns>
        public string ToCSV()
        {
            return SignalName + "," + SignalMode + "," + SignalDirection + "," + PartialRegion + "," + Column;
        }

        /// <summary>
        /// addr_in
        /// </summary>
        private string m_signalName = "default signal name";

        /// <summary>
        /// in, out, streaming
        /// </summary>
        private string m_signalMode = "in or out";

        /// <summary>
        /// East, West, ...
        /// </summary>
        private FPGA.FPGATypes.InterfaceDirection m_signalDirection = FPGATypes.InterfaceDirection.East;

        /// <summary>
        /// an optional tag for this signal
        /// </summary>
        private string m_partialRegion = "";

        /// <summary>
        /// For interleaving (currently we support 1 and 2)
        /// </summary>
        private int m_column = 1;
    }
}