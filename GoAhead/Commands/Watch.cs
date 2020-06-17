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
            this.m_watches.Add(w);
        }

        public String GetResults()
        {
            StringBuilder result = new StringBuilder();
            foreach (Watch watch in this.m_watches.Where(w => w.TotalDuration > 0).OrderBy(w => w.TotalDuration))
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
            this.m_calls.Clear();
            this.m_durations.Clear();
            this.m_starts.Clear();
        }

        public void Start(String who)
        {
            if (this.m_starts.ContainsKey(who))
            {
                Console.WriteLine(who + " exists already");
                return;
            }
            if (!this.m_calls.ContainsKey(who))
            {
                this.m_calls.Add(who, 0);
            }
            this.m_calls[who]++;

            this.m_starts.Add(who, DateTime.Now);
        }

        public void Stop(String who)
        {
            if (!this.m_durations.ContainsKey(who))
            {
                this.m_durations.Add(who, new TimeSpan(0));
            }

            if (!this.m_starts.ContainsKey(who))
            {
                Console.WriteLine(who + " not started");
                return;
            }

            this.m_durations[who] += DateTime.Now - this.m_starts[who];

            if (!this.m_starts.ContainsKey(who))
            {
                Console.WriteLine(who + " does not exists already");
            }
            this.m_starts.Remove(who);
        }

        public String GetResults()
        {
            StringBuilder result = new StringBuilder();
            int scale = 64;
            foreach (KeyValuePair<String, TimeSpan> tupel in this.m_durations.OrderBy(t => t.Value))
            {
                String currentResult = "# ";
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

                currentResult += " in " + this.m_calls[tupel.Key] + " call(s).";

                result.AppendLine(currentResult);
            }
            return result.ToString();
        }

        public long TotalDuration
        {
            get { return this.m_durations.Sum(t => t.Value.Ticks); }
        }

        public void Reset()
        {
            this.m_calls.Clear();
            this.m_starts.Clear();
            this.m_durations.Clear();
        }

        private Dictionary<String, int> m_calls = new Dictionary<String, int>();
        private Dictionary<String, DateTime> m_starts = new Dictionary<String, DateTime>();
        private Dictionary<String, TimeSpan> m_durations = new Dictionary<String, TimeSpan>();
    }
}
