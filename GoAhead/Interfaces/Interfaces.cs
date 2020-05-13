using System;
using System.Collections.Generic;

namespace GoAhead.Interfaces
{
    public interface IResetable
    {
        void Reset();
    }

    public class Subject
    {
        public void Add(IObserver observer)
        {
            m_observer.Add(observer);
        }

        public void Remove(IObserver observer)
        {
            m_observer.Remove(observer);
        }

        protected void Notfiy(object value)
        {
            foreach (IObserver obs in m_observer)
            {
                obs.Notify(value);
            }
        }

        private List<IObserver> m_observer = new List<IObserver>();
    }

    public interface IObserver
    {
        void Notify(object obj);
    }


}