using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Load a before saved macro library from file", Wrapper = false)]
    class LoadLibrary : Command
    {
        protected override void DoCommandAction()
        {
            Stream stream = null;
            //Opens a file and deserializes a new interface from it.
            try
            {
                stream = File.OpenRead(FileName);

                BinaryFormatter formatter = new BinaryFormatter();

                Objects.Library.Instance = (Objects.Library)formatter.Deserialize(stream);
            }
            catch (Exception error)
            {
                throw new ArgumentException("Could not deserialize library: " + error.Message);
            }
            finally
            {
                stream.Close();
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to save the library in")]
        public string FileName = "macroLib.binLibrary";
    }
}
