using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description="Saved a library element into a binary format")]
    class SaveLibraryElement : Command
    {
        protected override void DoCommandAction()
        {            
            Stream stream = null;

            LibraryElement libElement = Objects.Library.Instance.GetElement(this.LibraryElementName);

            try
            {
                stream = File.Open(this.FileName, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, libElement);
            }
            catch (Exception error)
            {
                throw new ArgumentException("Could not serialize library element: " + error.Message);
            }
            finally
            {
                stream.Close();
            }

            // update the restore command for the library element
            AddBinaryLibraryElement addElCmd = new AddBinaryLibraryElement();
            addElCmd.FileName = this.FileName;
            libElement.LoadCommand = addElCmd.ToString();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the library element to save")]
        public String LibraryElementName = "libelement";

        [Parameter(Comment = "The name of the file to save the library element in")]
        public String FileName = "libelement.binNetlist";
        
    }
}
