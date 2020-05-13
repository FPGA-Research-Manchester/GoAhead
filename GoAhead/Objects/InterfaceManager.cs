using System;
using System.Collections.Generic;
using System.ComponentModel;
using GoAhead.Commands.InterfaceManager;

namespace GoAhead.Objects
{
    /// <summary>
    /// Singelton
    /// </summary>
    public class InterfaceManager
    {
        private InterfaceManager()
        {
            m_signals.RaiseListChangedEvents = true;
            m_signals.AllowNew = true;
            m_signals.AllowRemove = true;
            m_signals.AllowEdit = true;
            m_signals.AllowRemove = true;

            m_signals.ListChanged += new ListChangedEventHandler(SignalListChanged);
        }

        public static InterfaceManager Instance = new InterfaceManager();

        private void SignalListChanged(object sender, ListChangedEventArgs e)
        {
            // this hook is called AFTER midification of this.m_signals
            // here only create Command to keep Command trace uptodate

            if (GenerateCommandsForChanges)
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        {
                            AddSignal addCmd = new AddSignal(m_signals[e.NewIndex]);
                            // no executtion, only update trace
                            addCmd.Execute = false;
                            Commands.CommandExecuter.Instance.Execute(addCmd);
                            break;
                        }
                    case ListChangedType.ItemChanged:
                        {
                            ModifiySignal modCmd = new ModifiySignal(m_signals[e.NewIndex]);
                            modCmd.CurrentSignalName = m_signalNamesPriorToModification[e.NewIndex];
                            // no executtion, only update trace
                            modCmd.Execute = false;
                            Commands.CommandExecuter.Instance.Execute(modCmd);
                            break;
                        }
                    case ListChangedType.ItemDeleted:
                        {
                            DeleteSignal delCmd = new DeleteSignal();
                            delCmd.SignalName = m_signalNamesPriorToModification[e.NewIndex];
                            // no execution, only update trace
                            delCmd.Execute = false;
                            Commands.CommandExecuter.Instance.Execute(delCmd);

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            CopySignalList();
        }

        public void Reset()
        {
            m_signals.Clear();
            m_signalNamesPriorToModification.Clear();
        }

        public void Remove(string signalName)
        {
            foreach (Signal s in GetAllSignals())
            {
                if (s.SignalName == signalName)
                {
                    m_signals.Remove(s);
                    break;
                }
            }
            // state changed -> capture current signal names
            CopySignalList();
        }

        public Signal GetSignal(string signalName)
        {
            foreach (Signal s in m_signals)
            {
                if (s.SignalName.Equals(signalName))
                {
                    return s;
                }
            }

            throw new ArgumentException("No signal found named " + signalName);
        }

        public int GetWireCount(string mode)
        {
            int wireCount = 0;
            foreach (Signal s in Instance.GetAllSignals())
            {
                if (s.SignalMode.Equals(mode))
                {
                    wireCount += 1;
                }
            }
            return wireCount;
        }

        public void Add(Signal newSignal)
        {
            /*
               // check for existance
               foreach(Signal signal in this.m_signals)
               {
                   if (signal.SignalName.ToString().Equals(newSignal.ToString()))
                   {
                       throw new ArgumentException("A Signal named " + newSignal.SignalName + " already exists");
                   }
               }
               */
            // add
            m_signals.Add(newSignal);

            // state changed -> capture current signal names
            CopySignalList();
        }

        public IEnumerable<Signal> GetAllSignals()
        {
            foreach (Signal s in m_signals)
            {
                yield return s;
            }
        }

        public IEnumerable<Signal> GetAllSignals(Predicate<Signal> p)
        {
            foreach (Signal s in m_signals)
            {
                if (p(s))
                {
                    yield return s;
                }
            }
        }

        public List<Signal> GetFlatSignalList(Predicate<Signal> p)
        {
            List<Signal> result = new List<Signal>();
            foreach (Signal s in GetAllSignals(p))
            {
                Signal flatSignal = new Signal(s.SignalName, s.SignalMode, s.SignalDirection, s.PartialRegion, s.Column);
                result.Add(flatSignal);
            }
            return result;
        }

        public BindingList<Signal> GetAllSignalsForPartialArea(string partialAreaName)
        {
            BindingList<Signal> result = new BindingList<Signal>();
            foreach (Signal s in Signals)
            {
                if (s.PartialRegion.Equals(partialAreaName))
                {
                    result.Add(s);
                }
            }
            return result;
        }

        public BindingList<Signal> Signals
        {
            get { return m_signals; }
        }

        public BindingList<Signal> GetSignals()
        {
            return m_signals;
        }

        public BindingList<Signal> GetSignals(Predicate<Signal> p)
        {
            BindingList<Signal> result = new BindingList<Signal>();
            foreach (Signal signal in GetAllSignals(s => p(s)))
            {
                result.Add(signal);
            }
            return result;
        }

        private void CopySignalList()
        {
            m_signalNamesPriorToModification.Clear();
            foreach (Signal s in m_signals)
            {
                m_signalNamesPriorToModification.Add(s.SignalName);
            }
        }

        public string GetNewSignalName()
        {
            string signalName = "signal_0";
            int count = 1;
            while (Has(signalName))
            {
                signalName = "signal_" + count;
                count++;
            }

            return signalName;
        }

        public bool Has(string signalName)
        {
            foreach (Signal s in m_signals)
            {
                if (s.SignalName.Equals(signalName))
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> LoadCommands
        {
            get { return m_loadCommands; }
            set { m_loadCommands = value; }
        }

        public bool GenerateCommandsForChanges
        {
            get { return m_generateCommandsForChanges; }
            set { m_generateCommandsForChanges = value; }
        }

        private bool m_generateCommandsForChanges = false;
        private List<string> m_loadCommands = new List<string>();
        private BindingList<Signal> m_signals = new BindingList<Signal>();
        private List<string> m_signalNamesPriorToModification = new List<string>();
    }
}