using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using GoAhead.Commands;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Generate HTML help file based on the example files in GOAHEAD_HOME/Help", Wrapper = false, Publish = false)]
    class PrintCommandHelp : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            StringBuilder htmlBuffer = new StringBuilder();
            htmlBuffer.AppendLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\"");
            htmlBuffer.AppendLine("\"http://www.w3.org/TR/html4/strict.dtd\">");
            htmlBuffer.AppendLine("<html>");
            htmlBuffer.AppendLine("<head>");
            htmlBuffer.AppendLine("<title>GoAhead Command Reference</title>");
            htmlBuffer.AppendLine("</head>");
            htmlBuffer.AppendLine("<body>");

            htmlBuffer.AppendLine("<frameset cols=\"200,*\">");

            htmlBuffer.AppendLine("<a name=\"top\" href=\"#Top\"></a>");

            htmlBuffer.AppendLine("<h1>GoAhead Command Reference</h1>");

            foreach (Type type in CommandStringParser.GetAllCommandTypes().OrderBy(t => t.Name))
            {
                Command command = (Command)Activator.CreateInstance(type);
                if (!command.PublishCommand)
                {
                    continue;
                }
                htmlBuffer.AppendLine("<a href=#" + type.Name + ">" + type.Name + "</a>");
                //htmlBuffer.AppendLine("<font size=5><a name=" + type.Name + " href=#" + type.Name + ">" + type.Name + "</a></font>");
            }


            string topLink = "<a href=\"#top\">Top</a>";

            foreach (Type type in CommandStringParser.GetAllCommandTypes().OrderBy(t => t.Name))
            {               
                Command command = (Command)Activator.CreateInstance(type);
                if (!command.PublishCommand)
                {
                    continue;
                }

                // new table
                htmlBuffer.AppendLine("<table border=1 width=90% style='table-layout:fixed'>");
                htmlBuffer.AppendLine("<col width=25%>");
                htmlBuffer.AppendLine("<col width=10%>");
                htmlBuffer.AppendLine("<col width=65%>");

                // header
                htmlBuffer.AppendLine("<tr>");
                htmlBuffer.AppendLine("\t<td colspan=3><b><font size=5>" + type.Name + "</font></b></td>");
                htmlBuffer.AppendLine("\t<a name=\"" + type.Name + "\" href=\"#" + type.Name + "\"></a>");
                htmlBuffer.AppendLine("</tr>");

                // synopsis
                htmlBuffer.AppendLine("<tr>");
                htmlBuffer.AppendLine("\t<tr><td colspan=3><b> Synopsis </font></b></td></tr>");
                if (!string.IsNullOrEmpty(command.GetCommandDescription()))
                {
                    htmlBuffer.AppendLine("\t<td colspan=3>" + command.GetCommandDescription() + "</td>");
                }
                else
                {
                    htmlBuffer.AppendLine("\t<td colspan=3> no command description available</td>");
                }
                htmlBuffer.AppendLine("</tr>");

                // get example form GOAHEAD_HOME
                string helpFile = command.GetHelpFilePath();
                htmlBuffer.AppendLine("<tr>");
                htmlBuffer.AppendLine("\t<td colspan=3><b> Example </font></b></td>");
                htmlBuffer.AppendLine("</tr>");
                if (File.Exists(helpFile))
                {
                    FileInfo fi = new FileInfo(helpFile);
                    if (fi.Length > 0)
                    {
                        string help = GetHelpFromFileAndInsertSyntaxHighlighting(helpFile);    
                        htmlBuffer.AppendLine("\t<td colspan=3>");
                        // help contains alignement tabs already
                        htmlBuffer.AppendLine(help);
                        htmlBuffer.AppendLine("\t</td>");
                    }
                    else
                    {
                        htmlBuffer.AppendLine("\t<td colspan=3> example file " + helpFile + " is empty </td>");
                    }
                }
                else
                {
                    htmlBuffer.AppendLine("\t<td colspan=3> example file " + helpFile + " not found </td>");
                }
                htmlBuffer.AppendLine("</td>");
                htmlBuffer.AppendLine("</tr>");

                // insert additional note (if the note file is present)
                string noteFile = command.GetNoteFilePath();
                if (File.Exists(noteFile))
                {
                    htmlBuffer.AppendLine("<tr>");
                    htmlBuffer.AppendLine("\t<td colspan=3><b> Notes </font></b></td>");
                    htmlBuffer.AppendLine("</tr>");
                    FileInfo fi = new FileInfo(noteFile);
                    if (fi.Length > 0)
                    {
                        string help = InsertHTLMBlanks(File.ReadAllText(noteFile));
                        htmlBuffer.AppendLine("\t<td colspan=3>");
                        htmlBuffer.AppendLine("\t\t" + help);
                        htmlBuffer.AppendLine("\t</td>");
                    }
                    else
                    {
                        htmlBuffer.AppendLine("\t<td colspan=3> note file " + helpFile + " is empty </td>");
                    }
                    htmlBuffer.AppendLine("</td>");
                    htmlBuffer.AppendLine("</tr>");
                }
                
                // parameter
                bool firstParameter = true;            
                foreach (FieldInfo fi in type.GetFields())
                {
                    // find ParamterField attr
                    foreach (object obj in fi.GetCustomAttributes(true).Where(o => o is Parameter))
                    {
                        Parameter param = (Parameter)obj;
                        if (param.PrintParameter)
                        {
                            // print header upon first command (some command do not have paramters)
                            if (firstParameter)
                            {
                                firstParameter = false;
                                //htmlBuffer.AppendLine("<tr><td colspan=3><b> Parameter </font></b></td></tr>");
                                htmlBuffer.AppendLine("<tr>");
                                htmlBuffer.AppendLine("\t<td><b>Parameter Name</b></td>");
                                htmlBuffer.AppendLine("\t<td><b>Type</b></td>");
                                htmlBuffer.AppendLine("\t<td><b>Description</b></td>");
                                htmlBuffer.AppendLine("</tr>");
                            }

                            htmlBuffer.AppendLine("<tr>");
                            htmlBuffer.AppendLine("\t<td>" + fi.Name + "</td>");
                            htmlBuffer.AppendLine("\t<td>" + fi.FieldType.Name + "</td>");
                            htmlBuffer.AppendLine("\t<td>" + param.Comment + "</td>");
                            htmlBuffer.AppendLine("</tr>");
                        }
                    }
                }

                htmlBuffer.AppendLine("<tr><td colspan=3>" + topLink + "</td></tr>");
                
                htmlBuffer.AppendLine("<br>");
            }

            htmlBuffer.AppendLine("</frameset>");
            htmlBuffer.AppendLine("</body>");
            htmlBuffer.AppendLine("</html>");
            OutputManager.WriteOutput(htmlBuffer.ToString());
        }

        private string GetHelpFromFileAndInsertSyntaxHighlighting(string helpFile)
        {
            StreamReader streamReader = new StreamReader(helpFile);
            string content = streamReader.ReadToEnd();
            streamReader.Close();
            
            CommandStringParser parser = new CommandStringParser(content);
            foreach (string cmdStr in parser.Parse())
            {
                Command cmd;
                string errorDescr;
                bool valid = parser.ParseCommand(cmdStr, true, out cmd, out errorDescr);
            }            

            List<Tuple<CommandStringTopology.TopologyType, Tuple<int, int>>> borders = parser.Topology.GetBorders().OrderBy(t => t.Item2.Item1).ToList();
            int offset = 0;
            foreach (Tuple<CommandStringTopology.TopologyType, Tuple<int, int>> t in borders)
            {
                int start = t.Item2.Item1;
                int end = t.Item2.Item2;
                string color = "";

                //<FONT COLOR="#cc6600">sample text</FONT>
                switch (t.Item1)
                {
                    case CommandStringTopology.TopologyType.CommandTag:
                        color = "#ff0000"; // red
                        break;
                    case CommandStringTopology.TopologyType.Comment:
                        color = "#00ff00"; // green
                        break;
                    case CommandStringTopology.TopologyType.ArgumentNames:
                        color = "#0000ff"; // blue
                        break;
                    default:
                        continue;
                }

                string openTag = "<font color =" + color + ">";
                string closeTag = "</font>";
                content = content.Insert(start + offset, openTag);
                offset += openTag.Length;
                content = content.Insert(end + offset, closeTag);                                
                offset += closeTag.Length;

                /*
                StreamWriter tw = new StreamWriter(@"c:\temp\debug.txt", append);
                append = true;
                tw.WriteLine(t.Item1);
                tw.WriteLine(content);
                tw.WriteLine("------------------");
                tw.Close();
                */
            }

            // insert tabs ...
            content = "\t" + content;
            // ... HTML blanks
            content = InsertHTLMBlanks(content);
            // ... and HTML line breaks
            content = content.Replace(Environment.NewLine, "<br>" + Environment.NewLine + "\t");

            return content;
        }

        private string InsertHTLMBlanks(string content)
        {
            string nbsp = "";
            bool insideBraces = false;
            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];
                if (c == '<')
                {
                    insideBraces = true;
                    nbsp += c;
                }
                else if (c == '>')
                {
                    insideBraces = false;
                    nbsp += c;
                }
                else if (!insideBraces && c == ' ')
                {
                    nbsp += "&nbsp";
                }
                else
                {
                    nbsp += c;
                }
            }
            return nbsp;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
