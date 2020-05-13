using System;
using GoAhead.Commands;

namespace GoAhead.Objects
{
    public class CommandShell
    {
        /// <summary>
        /// Start the shell and run until exit command
        /// </summary>
        public void Run()
        {
            Console.WriteLine("GoAhead shell mode. Enter your command:");
            Console.Write(">");

            string cmdString = "";
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey();

                if (info.Key.Equals(ConsoleKey.Enter))
                {
                    // clear command for command trace
                    Console.WriteLine("");
                    // remove trailing ;
                    cmdString = cmdString.Replace(";", "");
                    Command cmd;
                    string errorDescr;
                    CommandStringParser parser = new CommandStringParser(cmdString);
                    bool valid = parser.ParseCommand(cmdString, true, out cmd, out errorDescr);

                    if (valid)
                    {
                        CommandExecuter.Instance.Execute(cmd);
                        CommandStackManager.Instance.Execute();

                        // new line printed by CommandExecuter already
                        Console.Write(">");
                        cmdString = "";
                    }
                    else
                    {
                        Console.WriteLine(errorDescr);
                        Console.Write(">");
                        cmdString = "";
                    }
                }
                else if (info.Key.Equals(ConsoleKey.Backspace))
                {
                    if (cmdString.Length > 0)
                    {
                        cmdString = cmdString.Substring(0, cmdString.Length - 1);
                    }
                }
                else if (info.Key.Equals(ConsoleKey.Escape))
                {
                    Console.WriteLine("");
                    Console.Write(">");
                    cmdString = "";
                }
                else
                {
                    cmdString += info.KeyChar;

                    //Console.Write(info.KeyChar);
                }
            }
        }
    }
}