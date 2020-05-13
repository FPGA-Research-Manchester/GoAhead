using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using GoAhead.Objects;

namespace GoAhead.Commands
{    
    public partial class CommandStringParser
    {
        public CommandStringParser(byte[] charBuffer, int length)
        {
            m_byteBuffer = charBuffer;
            m_length = length;
        }

        public CommandStringParser(string cmdString)
        {
            string command = cmdString;
            if (command.StartsWith("\"") && command.EndsWith("\""))
            {
                command = command.Remove(0, 1);
                command = command.Remove(command.Length - 1, 1);
            }
            ASCIIEncoding enc = new ASCIIEncoding();
            m_byteBuffer = enc.GetBytes(command);
            m_length = command.Length;
        }

        public CommandStringParser(FileInfo fi)
        {
            m_byteBuffer = File.ReadAllBytes(fi.FullName);
            m_length = m_byteBuffer.Length;
            m_state.ParsedFile = fi.FullName;
        }

        public IEnumerable<string> Parse()
        {
            m_topology = new CommandStringTopology();

            bool pendingQuote = false;
            bool pendingApostroph = false;
            int charIndex = 0;

            bool scanForCommandTag = true;
            bool parseCommandTag = false;
            bool scanForArgument = false;
            bool parseArgumentPart = false;
            bool scanForEndOfValue = false;
            bool commentAtEndOfFile = false;
            int from = 0;
            int lastValueFrom = 0;
            int lastCommandTagFrom = 0;

            while (charIndex < m_length)
            {
                char c = (char)m_byteBuffer[charIndex++];

                if (c == '\n')
                {
                    m_state.LineNumber++;
                }

                // consume unqouted(!) comments
                while (c == '#' && !(pendingQuote || pendingApostroph) && charIndex < m_byteBuffer.Length)
                {
                    int commentStart = charIndex - 1;
                    while (true)
                    {
                        c = (char)m_byteBuffer[charIndex++];
                        // comment an end of ile
                        if (charIndex >= m_length-1)
                        {
                            m_topology.Add(CommandStringTopology.TopologyType.Comment, commentStart, charIndex - 2);
                            commentAtEndOfFile = true;
                            break;
                        }
                        if (c == '\n')
                        {
                            // store comment 
                            m_topology.Add(CommandStringTopology.TopologyType.Comment, commentStart, charIndex - 2);
                            m_state.LineNumber++;
                            break;
                        }
                    }
                    // comment at end of file
                    if (charIndex >= m_byteBuffer.Length)
                    {
                        break;
                    }
                    else
                    {
                        // consume next char after comment
                        c = (char)m_byteBuffer[charIndex++];
                        from = charIndex;
                    }
                }

                if (c == '"')
                {
                    pendingQuote = !pendingQuote;
                }
                if (c == '\'')
                {
                    pendingApostroph = !pendingApostroph;
                }                               

                if (scanForCommandTag)
                {
                    if (char.IsLetter(c))
                    {
                        scanForCommandTag = false;
                        parseCommandTag = true;
                        from = charIndex-1;
                    }
                }
                else if (parseCommandTag)
                {
                    if (c == ';')
                    {
                        m_topology.Add(CommandStringTopology.TopologyType.CommandTag, from, charIndex - 1);
                        m_topology.Add(CommandStringTopology.TopologyType.CompleteCommand, from, charIndex);
                        lastCommandTagFrom = from;
                    }
                    else if (!char.IsLetter(c) && c != '\n')
                    {
                        m_topology.Add(CommandStringTopology.TopologyType.CommandTag, from, charIndex - 1);
                        lastCommandTagFrom = from;
                        parseCommandTag = false;
                        scanForArgument = true;
                    }
                }
                else if (scanForArgument)
                {
                    if (c == ';')
                    {
                        // the last argument was complete, while skipping blank (scanning for the next argument) we encounter a ";",  e.g., 
                        // If Condition=%RowCount%=5 Then="Set Variable=LeftHalfOnly Value=True;" Else="NOP;"\s\s\s";"

                        m_topology.Add(CommandStringTopology.TopologyType.CompleteCommand, lastCommandTagFrom, charIndex);
                    }
                    if (char.IsLetter(c))
                    {
                        from = charIndex - 1;
                        scanForArgument = false;
                        parseArgumentPart = true;
                    }
                }
                else if(parseArgumentPart)
                {
                    string debug = new string(cmdBuffer, 0, cmdBufferIndex);
                    if (c == '=' && !(pendingQuote || pendingApostroph) && !scanForEndOfValue)
                    {
                        m_topology.Add(CommandStringTopology.TopologyType.ArgumentNames, from, charIndex-1);
                        lastValueFrom = charIndex;
                        scanForEndOfValue = true;
                    }
                    else if (scanForEndOfValue && !(pendingQuote || pendingApostroph) && (c == ' ' || c == '\r' || c == '\n'))
                    {
                        m_topology.Add(CommandStringTopology.TopologyType.ArgumentValues, lastValueFrom, charIndex - 1);
                        scanForEndOfValue = false;
                        scanForArgument = true;
                    }

                    if (c == ';' && !(pendingQuote || pendingApostroph))
                    {
                        //this.m_topology.Add(CommandStringTopology.TopologyType.ArgumentNames, from, charIndex - 1);
                        m_topology.Add(CommandStringTopology.TopologyType.CompleteCommand, lastCommandTagFrom, charIndex);
                        // flags are reset upon yield return
                    }
                }

                // no semicolon found
                if (cmdBufferIndex >= cmdBuffer.Length)
                {
                    throw new ArgumentException("The current command is too long (> 8192 chars):" + new string(cmdBuffer, 0, 128) + "... " + m_state.ToString());
                }
                
                cmdBuffer[cmdBufferIndex++] = c;

                if (c == ';' && !(pendingQuote || pendingApostroph) && !commentAtEndOfFile)
                {
                    // do not return stand alone semicolons
                    if (cmdBuffer[0] != ';')
                    {
                        string command = new string(cmdBuffer, 0, cmdBufferIndex);
                        command = Regex.Replace(command, @"^\s*", "");
                        m_state.LastParserCommand = command;
                        yield return command;
                    }

                    // clear buffer for next run, otherwise, we detect unparsable commands at the end of file
                    for (int i = 0; i < cmdBufferIndex; i++)
                    {
                        cmdBuffer[i] = ' ';
                    }

                    cmdBufferIndex = 0;
                    scanForCommandTag = true;
                    parseCommandTag = false;
                    scanForArgument = false;
                    scanForEndOfValue = false;
                    parseArgumentPart = false;
                    from = charIndex;
                }
            }
            
            // the line may contain multiple commands whereby the last one is not terminated with a ;
            if (cmdBufferIndex > 0 && !commentAtEndOfFile)
            {
                bool lineFeedsOnly = true;
                for(int i=0;i<cmdBufferIndex;i++)
                {
                    if (cmdBuffer[i] != '\n' && cmdBuffer[i] != '\r' && cmdBuffer[i] != '\t' && cmdBuffer[i] != ' ')
                    {
                        lineFeedsOnly = false;
                        break;
                    }
                }

                if (!lineFeedsOnly)
                {
                    throw new ArgumentException("Detected an incomplete command at the end. Missing semicolon or quotes? " + m_state.ToString());
                }
                //yield return new String(cmdBuffer, 0, cmdBufferIndex);
            }
        }

        public CommandStringTopology Topology
        {
            get { return m_topology; }
        }

        public bool ParseCommand(string commandString, bool setFields,  out Command command, out string errorDescription)
        {
            command = new NOP();
            errorDescription = "";

            // resolve aliases
            if (AliasManager.Instance.HasAlias(commandString))
            {
                ExecuteAlias execAliasCmd = new ExecuteAlias();
                execAliasCmd.AliasName = commandString;
                execAliasCmd.Commands = AliasManager.Instance.GetCommand(commandString);
                command = execAliasCmd;
                return true;
            }

            // skip comments
            if (string.IsNullOrEmpty(commandString) || m_commentsRegexp.IsMatch(commandString))
            {
                errorDescription = "Comment or empty string";
                return false;
            }

            // preserve original command string version
            string orgCmd = commandString;
             
            // resolve envirnonement variable (if any) for windows style env var %MY_VAR%
            ResolveWindowsStypeVariables(ref commandString);

            // resolve envirnonement variable (if any) for unix style env var ${MY_VAR}
            MatchCollection matches = m_envVarUnixRegexp.Matches(commandString);
            foreach (Match match in matches)
            {
                string varName = match.Groups[0].Value;
                // remove ${}
                string varValue = Environment.GetEnvironmentVariable(Regex.Replace(varName, @"{|}|\$", ""));
                if (varValue != null)
                {
                    // regep does not work with { ??
                    // command = Regex.Replace(command, varName, varValue);
                    commandString = commandString.Replace(varName, varValue);
                }
                else
                {
                    errorDescription = "Could not resolve environment variable " + varName;
                    return false;
                }
            }

            bool valid = SplitCommand(commandString, out string cmdTag, out string argumentPart);

            if (!valid)
            {
                errorDescription = "Found an invalid command string: " + commandString;
                return false;
            }

            // argumentPart
            List<string> arguments = new List<string>();

            if (!string.IsNullOrEmpty(argumentPart))
            {
                foreach (NameValuePair nameValuePair in GetNameValuePairs(argumentPart))
                {
                    string parameter = nameValuePair.Name + "=" + nameValuePair.Value;
                    //parameter = Regex.Replace(parameter, "^\"*", "");
                    //parameter = Regex.Replace(parameter, "^'", "");
                    parameter = Regex.Replace(parameter, @"^\s+", "");
                    parameter = Regex.Replace(parameter, @"\s+$", "");           

                    arguments.Add(parameter);
                }
            }

            // resolve aliases with arguments
            if (AliasManager.Instance.HasAlias(cmdTag))
            {
                ExecuteAlias execAliasCmd = new ExecuteAlias(arguments);
                execAliasCmd.AliasName = commandString;
                execAliasCmd.Commands = AliasManager.Instance.GetCommand(cmdTag);
                command = execAliasCmd;
                return true;
            }

            int hits = GetAllCommandTypes().Count(t => t.Name.Equals(cmdTag));            
            if (hits == 0)
            {
                string candidateNames = GetSimiliarCommandNames(cmdTag);
                errorDescription = 
                    "Can not handle unknown command name " + cmdTag +
                    (candidateNames.Length > 0 ? ". Did you mean " + candidateNames + "?" : "") + Environment.NewLine + 
                    m_state.ToString();
                return false;
            }
            if(hits > 1)
            {
                errorDescription = "Can not handle duplicate command name " + cmdTag + ".";
                return false;
            }

            Type type = GetAllCommandTypes().FirstOrDefault(t => t.Name.Equals(cmdTag));
            
            command = (Command)Activator.CreateInstance(type);
            // store the unresolved command string along with each command (goto label)
            command.OriginalCommandString = orgCmd;

            return SetParamters(command, setFields, arguments, ref errorDescription);
        }

        public bool SetParamters(Command command,bool setFields, List<string> arguments, ref string errorDescription)
        {
            // map each name=value to a paramter field
            for (int i = 0; i < arguments.Count; i++)
            {
                string[] atoms = arguments[i].Split(new char[] { '=' }, 2);
                if (atoms.Length != 2)
                {
                    errorDescription = "Bad command line found : " + arguments[i];
                    return false;
                }
                string name = atoms[0];
                string value = atoms[1];

                FieldInfo fi = command.GetType().GetFields().FirstOrDefault(f => f.GetCustomAttributes(true).Count(attr => attr is Parameter) == 1 && f.Name.Equals(name));
                if (fi == null)
                {
                    string candidateNames = GetSimiliarArgumentNames(command.GetType(), name);
                    errorDescription =
                        "Could not map argument " + arguments[i] + " to command " + command.GetType().Name +
                        ". Missing semicolon?" + (!string.IsNullOrEmpty(candidateNames) ? " Did you mean " + candidateNames + "?" : "") + Environment.NewLine +
                        m_state.ToString();
                    return false;
                }

                // break here to include above checks (Bad command line found, unmapable argument)
                if (!setFields)
                {
                    break;
                }

                value = VariableManager.Instance.Resolve(value);

                //NetName=quote(name[3]) -> name[3]
                if (m_quoteRegexp.IsMatch(value))
                {
                    value = value.Substring(6);
                    value = value.Substring(0, value.Length - 1);
                }
                else
                {
                    ResolveEmbeddedArithmeticExpressions(ref value);
                }


                bool validParamter = SetParameterByName(command, fi, name, value);
                if (!validParamter)
                {
                    errorDescription = "Can not handle type " + fi.FieldHandle.ToString() + " of type " + fi.FieldType.Name;
                    return false;
                }
            }
            return true;
        }

        private bool SetParameterByName(Command command, FieldInfo fi, string name, string value)
        {
            // the following type comparisons 
            if (fi.FieldType == typeof(string))
            {
                fi.SetValue(command, value);
                return true;
            }
            else if (fi.FieldType == typeof(int))
            {
                ExpressionParser ep = new ExpressionParser();
                int evaluationResult = 0;
                bool validExpression = ep.Evaluate(value, out evaluationResult);
                fi.SetValue(command, evaluationResult);
                return true;
            }
            else if (fi.FieldType == typeof(bool))
            {
                fi.SetValue(command, bool.Parse(value));
                return true;
            }
            else if (fi.FieldType == typeof(List<string>))
            {
                List<string> list = new List<string>();
                list.AddRange(value.Split(','));
                fi.SetValue(command, list);
                return true;
            }
            else if (fi.FieldType == typeof(List<int>))
            {
                List<int> list = new List<int>();
                foreach (string intAsString in value.Split(','))
                {
                    list.Add(int.Parse(intAsString));
                }
                fi.SetValue(command, list);
                return true;
            }
            else if (fi.FieldType == typeof(List<bool>))
            {
                List<bool> list = new List<bool>();
                foreach (string intAsString in value.Split(','))
                {
                    list.Add(bool.Parse(intAsString));
                }
                fi.SetValue(command, list);
                return true;
            }
            else
            {                    
                return false;
            }

        }

        private void ResolveEmbeddedArithmeticExpressions(ref string argumentValue)
        {
            MatchCollection matches = m_embeddedArithmeticExpression.Matches(argumentValue);
            
            foreach (Match match in matches)
            {
                string arithmeticExpression = match.Groups[0].Value;
                // remove brackets
                arithmeticExpression = arithmeticExpression.Replace("[", "");
                arithmeticExpression = arithmeticExpression.Replace("]", "");

                ExpressionParser ep = new ExpressionParser();
                int evaluationResult = 0;
                bool validExpression = ep.Evaluate(arithmeticExpression, out evaluationResult);
                if (validExpression)
                {
                    argumentValue = argumentValue.Replace(match.Groups[0].Value, evaluationResult.ToString());
                }
            }
        }
    
        private void ResolveWindowsStypeVariables(ref string commandString)
        {
            MatchCollection matches = m_envVarWindowsStyleRegexp.Matches(commandString);
            foreach (Match match in matches)
            {
                string varName = match.Groups[0].Value;
                // remove %%
                string varValue = Environment.GetEnvironmentVariable(Regex.Replace(varName, "%", ""));
                if (varValue != null)
                {
                    commandString = Regex.Replace(commandString, varName, varValue);
                }
                // else :could not resolve environment variable, maybe the % % encloses a to be resolved define
            }
        }

        private string GetSimiliarCommandNames(string mispelledName)
        {
            string candidateNames = "";
            int candidatesCount = 0;
            foreach (Type type in GetAllCommandTypes())
            {                
                foreach (string subString in mispelledName.Substrings(5))
                {
                    if (type.Name.Contains(subString))
                    {
                        candidateNames += (string.IsNullOrEmpty(candidateNames) ? "" : " or ") + type.Name;
                        candidatesCount++;
                        break;
                    };
                }

                if (candidatesCount > 3)
                {
                    break;

                }
            }
            return candidateNames;
        }

        private string GetSimiliarArgumentNames(Type commandType, string mispelledName)
        {
            string candidateNames = "";
            foreach (FieldInfo candidate in commandType.GetFields().Where(f => f.GetCustomAttributes(true).Count(attr => attr is Parameter) == 1))
            {                
                foreach (string subString in mispelledName.Substrings(4))
                {
                    if (candidate.Name.Contains(subString))
                    {
                        candidateNames += (string.IsNullOrEmpty(candidateNames) ? "" : " or ") + candidate.Name;
                        break;
                    };
                }
            }
            return candidateNames;
        }

        /// <summary>
        /// Split a command string into command tag and argument part
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cmdTag"></param>
        /// <param name="argumentPart"></param>
        /// <returns></returns>
        public bool SplitCommand(string command, out string cmdTag, out string argumentPart)
        {
            cmdTag = "";
            argumentPart = "";

            string trimmedCommand = Regex.Replace(command, @"^\s+", "");
            trimmedCommand = Regex.Replace(trimmedCommand, @"\s+$", "");

            // split command into arguments
            string[] cmdAtoms = Regex.Split(trimmedCommand, @"\s");

            // skip empty lines
            if (cmdAtoms.Length == 0)
            {
                return false;
            }

            // the command itself
            cmdTag = cmdAtoms[0];

            // syntax errror: semicolon after True
            // If Condition=%LeftHalfOnly%=True; Then="NOP;"  Else="NOP;";
            if (cmdTag.Contains("="))
            {
                return false;
            }

            cmdTag = Regex.Replace(cmdTag, @";$", "");

            // from command extract 'name1=value1 name2="val ue2'
            argumentPart = Regex.Replace(trimmedCommand, "^" + cmdTag, "", RegexOptions.IgnoreCase);
            argumentPart = Regex.Replace(argumentPart, @"^\s+", "");
            argumentPart = Regex.Replace(argumentPart, @"\s+$", "");
            argumentPart = Regex.Replace(argumentPart, @";$", "");

            return true;
        }

        public IEnumerable<NameValuePair> GetNameValuePairs(string argumentPart)
        {
            int index = 0;
            string str = argumentPart;

            bool pendingQuote = false;
            bool pendingApostroph = false;

            while (index < str.Length && str[index] != ';' && !(pendingQuote || pendingApostroph))
            {
                NameValuePair current = new NameValuePair();

                while (index < str.Length && IsSeparator(str[index]))
                {
                    index++;
                }

                if (index >= str.Length)
                {
                    break;
                }

                current.NameFrom = index;
                while (index < str.Length && !IsSeparator(str[index]) && str[index] != '=')
                {
                    current.Name += str[index++];
                }
                current.NameTo = index++;
                current.ValueFrom = index;


                while (index < str.Length && ((!IsSeparator(str[index]) && str[index] != ';') || (pendingQuote || pendingApostroph)))
                {
                    if (str[index] == '"')
                    {
                        pendingQuote = !pendingQuote;
                    }
                    if (str[index] == '\'')
                    {
                        pendingApostroph = !pendingApostroph;
                    }
                    current.Value += str[index++];
                }
                current.ValueTo = ++index;

                current.Trim();
                yield return current;
            }
        }

        private bool IsSeparator(char c)
        {
            return c == ' ' || c == '\n' || c == ';' || c == '\t' || c == '\r';
        }

        public static IEnumerable<Type> GetAllCommandTypes()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type type in asm.GetTypes())
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(Command)))
                {
                    yield return type;
                }
            }
        }

        public CommandStringParserState State
        {
            get { return m_state; }
        }

        private static Regex m_commentsRegexp = new Regex(@"^\s*#", RegexOptions.Compiled);
        private static Regex m_quoteRegexp = new Regex(@"^quote\(.*\)$", RegexOptions.Compiled);
        private static Regex m_envVarWindowsStyleRegexp = new Regex("%[^%]+?%", RegexOptions.Compiled);
        private static Regex m_envVarUnixRegexp = new Regex(@"\${[^}]+?}", RegexOptions.Compiled);
        /// <summary>
        /// everything between [ and ]
        /// </summary>
        private static Regex m_embeddedArithmeticExpression = new Regex(@"\[[^\]]+?\]", RegexOptions.Compiled);

        private CommandStringTopology m_topology = null;
        //private readonly String m_command = "";
        private readonly int m_length;
        private readonly byte[] m_byteBuffer;
        private char[] cmdBuffer = new char[128*1024];
        private int cmdBufferIndex = 0;

        private CommandStringParserState m_state = new CommandStringParserState();
    }

    public class NameValuePair
    {
        public string Name = "";
        public string Value = "";
        public int NameFrom = 0;
        public int NameTo = 0;
        public int ValueFrom = 0;
        public int ValueTo = 0;

        public void Trim()
        {
            Trim(ref Name);
            Trim(ref Value);
        }

        private void Trim(ref string str)
        {
            if (str.Length == 0)
            {
                return;
            }
            if (str.StartsWith("\"") && str.EndsWith("\""))
            {
                str = str.Remove(0, 1);
                if (str.Length == 0)
                {
                    return;
                }

                str = str.Remove(str.Length - 1, 1);
            }
            if (str.StartsWith("'") && str.EndsWith("'"))
            {
                str = str.Remove(0, 1);
                str = str.Remove(str.Length - 1, 1);
            }
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Return all substring of the given minimal length
        /// </summary>
        /// <param name="s"></param>
        /// <param name="minimalLength"></param>
        /// <returns></returns>
        public static IEnumerable<string> Substrings(this string s, int minimalLength = 1)
        {
            var subStrings =
                        from start in Enumerable.Range(0, s.Length)
                        from length in Enumerable.Range(1, s.Length - start)
                        where length >= minimalLength
                        select s.Substring(start, length);
            return subStrings;
        }
    }
}
