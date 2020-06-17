using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.Commands.Variables;

namespace GoAhead.Commands.Misc.FPL2013
{
    class GetSourcesAndSinks : Command
    {
        protected override void DoCommandAction()
        {
            FileStream fs = File.OpenRead(this.OverlayScript);
            byte[] byteBuffer = new byte[fs.Length];
            int length = Convert.ToInt32(fs.Length);
            fs.Read(byteBuffer, 0, length);
            fs.Close();

            CommandExecuter.Instance.MuteCommandTrace = true;

            CommandStringParser parser = new CommandStringParser(byteBuffer, length);

            TextWriter sl = new StreamWriter(this.SearchLoad, false);
            TextWriter fence = new StreamWriter(this.Fence, false);

            foreach (String cmdString in parser.Parse())
            {
                Command resolvedCommand = null;
                String errorDescr;

                bool valid = parser.ParseCommand(cmdString, true, out resolvedCommand, out errorDescr);

                if (resolvedCommand is Set)
                {
                    CommandExecuter.Instance.Execute(resolvedCommand);

                    Set setCmd = (Set) resolvedCommand;
                    String value = setCmd.Value;
                    if(Objects.IdentifierManager.Instance.IsMatch(value, Objects.IdentifierManager.RegexTypes.CLB))
                    {
                        AddToSelectionLoc addCmd = new AddToSelectionLoc();
                        addCmd.Location = value;
                        fence.WriteLine(addCmd.ToString());
                    }

                }
                else if (resolvedCommand is PathSearchOnFPGA)
                {
                    CommandExecuter.Instance.MuteCommandTrace = false;
                    PathSearchOnFPGA searchCmd = (PathSearchOnFPGA)resolvedCommand;
                    //String from 
                    //this.OutputManager.WriteOutput(searchCmd.StartLocation + "." + searchCmd.StartPort + "-" + searchCmd.TargetLocation + "." + searchCmd.TargetPort);
                    sl.WriteLine(searchCmd.StartLocation + "." + searchCmd.StartPort + "-" + searchCmd.TargetLocation + "." + searchCmd.TargetPort);
                }
                else
                {

                }
            }

            sl.Close();
            fence.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the script to run")]
        public String OverlayScript = "script.goa";

        [Parameter(Comment = "")]
        public String SearchLoad = "out.txt";

        [Parameter(Comment = "")]
        public String Fence = "out.txt";
    }
}
