using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands.Library;
using GoAhead.Commands.Selection;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Read in an XDL library element (AddXDLLibraryElement) and save it in a binary format (SaveLibraryElement). The element will not be added to the macro library.", Wrapper = true, Publish = true)]
    class SaveXDLLibraryElementAsBinaryLibraryElement : Command
    {
        protected override void DoCommandAction()
        {
            String libElementName = Path.GetFileNameWithoutExtension(this.XDLFileName);

            AddXDLLibraryElement addCmd = new AddXDLLibraryElement();
            addCmd.FileName = this.XDLFileName;
            CommandExecuter.Instance.Execute(addCmd);
                        
            SetResourceShapeInfo setCmd = new SetResourceShapeInfo();
            setCmd.LibraryElementName = libElementName;
            CommandExecuter.Instance.Execute(setCmd);

            SaveLibraryElement saveCmd = new SaveLibraryElement();
            saveCmd.FileName = this.FileName;
            saveCmd.LibraryElementName = libElementName;
            CommandExecuter.Instance.Execute(saveCmd);

            RemoveLibraryElement removeCmd = new RemoveLibraryElement();
            removeCmd.LibraryElementName = libElementName;
            CommandExecuter.Instance.Execute(removeCmd);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The XDL netlist to read in")]
        public String XDLFileName = "design.xdl";

        [Parameter(Comment = "The name of the file to save the library element in")]
        public String FileName = "libelement.binNetlist";
    }
}
