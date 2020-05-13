using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GoAhead.Objects;

namespace GoAhead.Commands.InterfaceManager
{
    [CommandDescription(Description = "Save an interface to file", Wrapper = false)]
    class SaveInterface : Command
    {
        public SaveInterface()
        {
        }

       protected override void DoCommandAction()
        {
            //Opens a file and serializes  into it in binary format.
            Stream stream = File.Open(FileName, FileMode.Create);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, Objects.InterfaceManager.Instance.Signals);
                stream.Flush();
            }
            catch (Exception error)
            {
                throw new ArgumentException("Could not serialize Interface: " + error.Message);
            }
            stream.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to save the interface in")]
        public string FileName;
    }
}
