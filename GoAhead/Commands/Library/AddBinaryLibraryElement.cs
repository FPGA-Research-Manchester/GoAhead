using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Add a module or hard macro to the library of placeable elements", Wrapper = true, Publish = true)]
    class AddBinaryLibraryElement : Command
    {
        protected override void DoCommandAction()
        {
            //Opens a file and serializes m_fpga into it in binary format.
            Stream stream = null;

            String elementName = Path.GetFileNameWithoutExtension(this.FileName);

            try
            {
                stream = File.OpenRead(this.FileName);

                BinaryFormatter formatter = new BinaryFormatter();

                this.m_loadedElement = (Objects.LibraryElement)formatter.Deserialize(stream);
                
                // store how the element was added                
                this.m_loadedElement.LoadCommand = this.ToString();

                // rename library element accorindg to file name
                this.m_loadedElement.Name = elementName;

                Objects.Library.Instance.Add(this.m_loadedElement);

                try
                {
                    String str = this.m_loadedElement.ToString();
                }
                catch (NullReferenceException)
                {
                    this.OutputManager.WriteWarning("The binary libray element " + this.FileName + " seems to be out of date. Regenerate it by reading in the XDL version with the command AddXDLLibraryElement");
                }

            }
            catch (Exception error)
            {
                throw new ArgumentException("Could not open library element: " + error.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public override void Undo()
        {
            if (this.m_loadedElement != null)
            {
                Objects.Library.Instance.Remove(this.m_loadedElement.Name);
            }
        }

        private Objects.LibraryElement m_loadedElement = null;

        [Parameter(Comment = "The name of the file to load the library element from")]
        public String FileName = "macro.binNetlist";
    }
}
