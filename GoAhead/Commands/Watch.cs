using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    public class WatchManager
    {
        private WatchManager()
        {
        }

        public void Add(Watch w)
        {
            m_watches.Add(w);
        }

        public string GetResults()
        {
            StringBuilder result = new StringBuilder();
            foreach (Watch watch in m_watches.Where(w => w.TotalDuration > 0).OrderBy(w => w.TotalDuration))
            {
                result.Append(watch.GetResults());
            }
            return result.ToString();
        }

        public static WatchManager Instance = new WatchManager();

        private List<Watch> m_watches = new List<Watch>();
    }

    [Serializable]
    public class Watch
    {
        public Watch()
        {
            WatchManager.Instance.Add(this);
        }

        public void Clear()
        {
            m_calls.Clear();
            m_durations.Clear();
            m_starts.Clear();
        }

        public void Start(string who)
        {
            if (m_starts.ContainsKey(who))
            {
                Console.WriteLine(who + " exists already");
                return;
            }
            if (!m_calls.ContainsKey(who))
            {
                m_calls.Add(who, 0);
            }
            m_calls[who]++;

            m_starts.Add(who, DateTime.Now);
        }

        public void Stop(string who)
        {
            if (!m_durations.ContainsKey(who))
            {
                m_durations.Add(who, new TimeSpan(0));
            }

            if (!m_starts.ContainsKey(who))
            {
                Console.WriteLine(who + " not started");
                return;
            }

            m_durations[who] += DateTime.Now - m_starts[who];

            if (!m_starts.ContainsKey(who))
            {
                Console.WriteLine(who + " does not exists already");
            }
            m_starts.Remove(who);
        }

        public string GetResults()
        {
            StringBuilder result = new StringBuilder();
            int scale = 64;
            foreach (KeyValuePair<string, TimeSpan> tupel in m_durations.OrderBy(t => t.Value))
            {
                string currentResult = "# ";
                currentResult += tupel.Key;
                while (currentResult.Length <= 1 * scale)
                {
                    currentResult += ' ';
                }

                currentResult += " spent " + tupel.Value;
                while (currentResult.Length <= 2 * scale)
                {
                    currentResult += ' ';
                }

                currentResult += " in " + m_calls[tupel.Key] + " call(s).";

                result.AppendLine(currentResult);
            }
            return result.ToString();
        }

        public long TotalDuration
        {
            get { return m_durations.Sum(t => t.Value.Ticks); }
        }

        public void Reset()
        {
            m_calls.Clear();
            m_starts.Clear();
            m_durations.Clear();
        }

        private Dictionary<string, int> m_calls = new Dictionary<string, int>();
        private Dictionary<string, DateTime> m_starts = new Dictionary<string, DateTime>();
        private Dictionary<string, TimeSpan> m_durations = new Dictionary<string, TimeSpan>();
    }
}
