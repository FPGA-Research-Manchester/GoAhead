using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.InterfaceManager
{
    [CommandDescription(Description = "Load an interface from file", Wrapper = true)]
    class LoadInterface : Command
    {
        protected override void DoCommandAction()
        {
		    Stream stream = null;
            //Opens a file and deserializes a new interface from it.
		    try
		    {
                stream = File.OpenRead(FileName);

                BinaryFormatter formatter = new BinaryFormatter();
                BindingList<Signal> loadedSignals = (BindingList<Signal>)formatter.Deserialize(stream);

                // assign item by item to fire change event to grid vieew
                foreach (Signal s in loadedSignals)
                {
                    Objects.InterfaceManager.Instance.Add(s);
                }
                Objects.InterfaceManager.Instance.LoadCommands.Add(ToString());
            }
		    catch (Exception error)
		    {
			    throw new ArgumentException("Could not deserialize interface: " + error.Message);
		    }
		    finally
		    {
			    stream.Close();
		    }
        }

        public override void  Undo()
        {
 	        throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to save the interface in")]
        public string FileName;
    }
}
