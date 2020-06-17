using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Save the library to file", Wrapper = false, Publish=true)]
    class SaveLibrary : Command
    {
        protected override void DoCommandAction()
        {
            //Opens a file and serializes  into it in binary format.
            Stream stream = File.Open(this.FileName, FileMode.Create);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, Objects.Library.Instance);
                stream.Flush();
            }
            catch (Exception error)
            {
                throw new ArgumentException("Could not serialize NMC library: " + error.Message);
            }
            stream.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to save the library in")]
        public String FileName = "macroLib.binLibrary";
    }
}
