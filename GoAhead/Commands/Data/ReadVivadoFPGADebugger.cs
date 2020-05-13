using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Using this before ReadVivadoFPGA sets it up to be ran in debug mode.", Wrapper = true)]
    class ReadVivadoFPGADebugger_Setup : Command
    {
        [Parameter(Comment = "Setting this to a valid file path activates the debug mode.")]
        public string DebugFileOutput = "";

        [Parameter(Comment = "The format of the output, possible values are 'Complete' and 'Compact'")]
        public string Format = "Complete";

        [Parameter(Comment = "If true, the debugger will ignore wires starting and ending at the same pip.")]
        public bool IgnoreIdenticalLocationWires = true;

        public override void Undo()
        {
        }

        protected override void DoCommandAction()
        {
            ReadVivadoFPGADebugger.FilePath = DebugFileOutput;
            if (Enum.TryParse(Format, out ReadVivadoFPGADebugger.OutputMode mode))
            {
                ReadVivadoFPGADebugger.Mode = mode;
            }
            else
            {
                throw new Exception("Invalid 'Format' parameter value.");
            }
            ReadVivadoFPGADebugger.IgnoreIdenticalLocationWires = IgnoreIdenticalLocationWires;
        }
    }

    [CommandDescription(Description = "Adds a regular expression for wires to be included during debugging ReadVivadoFPGA.", Wrapper = true)]
    class ReadVivadoFPGADebugger_AddWireInclude : ReadVivadoFPGADebugger_AddWireRegex
    {
        protected override ReadVivadoFPGADebugger.RegexType WireRegexType => ReadVivadoFPGADebugger.RegexType.Include;
    }

    [CommandDescription(Description = "Adds a regular expression for wires to be ignored during debugging ReadVivadoFPGA.", Wrapper = true)]
    class ReadVivadoFPGADebugger_AddWireIgnore : ReadVivadoFPGADebugger_AddWireRegex
    {
        protected override ReadVivadoFPGADebugger.RegexType WireRegexType => ReadVivadoFPGADebugger.RegexType.Ignore;
    }

    [CommandDescription(Description = "Deletes all current debugger settings, disabling the debugger.", Wrapper = true)]
    class ReadVivadoFPGADebugger_Reset : Command
    {
        public override void Undo()
        {
        }

        protected override void DoCommandAction()
        {
            ReadVivadoFPGADebugger.DisableAndReset();
        }
    }

    [CommandDescription(Description = "Deletes all current wire includes/ignores settings.", Wrapper = true)]
    class ReadVivadoFPGADebugger_DeleteWireRegexes : Command
    {
        public override void Undo()
        {
        }

        protected override void DoCommandAction()
        {
            ReadVivadoFPGADebugger.ResetWireRegexes();
        }
    }

    [CommandDescription(Description = "", Wrapper = true)]
    abstract class ReadVivadoFPGADebugger_AddWireRegex : Command
    {
        [Parameter(Comment = "The start tile of the wire.")]
        public string WireRegex_StartTile = "";

        [Parameter(Comment = "The start port of the wire.")]
        public string WireRegex_StartPort = "";

        [Parameter(Comment = "The end tile of the wire.")]
        public string WireRegex_EndTile = "";

        [Parameter(Comment = "The end port of the wire.")]
        public string WireRegex_EndPort = "";

        public override void Undo()
        {
        }

        protected override void DoCommandAction()
        {
            ReadVivadoFPGADebugger.AddWireRegex(
                WireRegexType,
                WireRegex_StartTile,
                WireRegex_StartPort,
                WireRegex_EndTile,
                WireRegex_EndPort);
        }

        protected abstract ReadVivadoFPGADebugger.RegexType WireRegexType { get; }
    }

    static class ReadVivadoFPGADebugger
    {
        public enum OutputMode
        {
            Complete,
            Compact
        }

        public enum RegexType
        {
            Include,
            Ignore
        }

        private struct WireRegex
        {
            public Regex startTile;
            public Regex startPort;
            public Regex endTile;
            public Regex endPort;
        }

        public static string FilePath = "";
        private static StreamWriter File = null;

        public static bool Debugging => !string.IsNullOrEmpty(FilePath);

        public static bool IgnoreIdenticalLocationWires = true;

        private static List<WireRegex> Wire_Includes;
        private static List<WireRegex> Wire_Ignores;
        private static ref List<WireRegex> GetWireListForRegexType(RegexType regexType)
        {
            if (regexType == RegexType.Include)
                return ref Wire_Includes;
            if (regexType == RegexType.Ignore)
                return ref Wire_Ignores;

            throw new Exception();
        }

        public static void AddWireRegex(RegexType regexType, string st, string sp, string et, string ep)
        {
            if (string.IsNullOrEmpty(st) ||
                string.IsNullOrEmpty(sp) ||
                string.IsNullOrEmpty(et) ||
                string.IsNullOrEmpty(ep))
                return;

            ref List<WireRegex> wireRegexList = ref GetWireListForRegexType(regexType);
            if (wireRegexList == null) wireRegexList = new List<WireRegex>();

            wireRegexList.Add(new WireRegex
            {
                startTile = new Regex(st),
                startPort = new Regex(sp),
                endTile = new Regex(et),
                endPort = new Regex(ep)
            });
        }

        public static List<string[]> GetWireRegexStrings(RegexType regexType)
        {
            List<WireRegex> list = null;

            if (regexType == RegexType.Include)
            {
                list = Wire_Includes;
            }
            else if (regexType == RegexType.Ignore)
            {
                list = Wire_Ignores;
            }

            if (list == null) return null;

            return list.Select(w => new string[]
                {
                    w.startTile.ToString(),
                    w.startPort.ToString(),
                    w.endTile.ToString(),
                    w.endPort.ToString()
                }).ToList();
        }

        /// <summary>
        /// If the debugger is set-up, the provided wire will be matched against the includes and ignores regexes.
        /// If the wire is matching, it will be written to the debug output file.
        /// </summary>
        /// <param name="st">Wire start tile</param>
        /// <param name="sp">Wire start port</param>
        /// <param name="et">Wire end tile</param>
        /// <param name="ep">Wire end port</param>
        public static void DebugWire(string st, string sp, string et, string ep)
        {
            if (!Debugging)
                return;

            if (IgnoreIdenticalLocationWires && st == et && sp == ep)
                return;

            if (!WireMatches(RegexType.Include, st, sp, et, ep) || WireMatches(RegexType.Ignore, st, sp, et, ep))
                return;

            WriteToFile(st, sp, et, ep);
        }

        /// <summary>
        /// Returns true if the provided wire matches any regex in the corresponding regexType list.
        /// </summary>
        /// <param name="regexType"></param>
        /// <param name="st"></param>
        /// <param name="sp"></param>
        /// <param name="et"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        private static bool WireMatches(RegexType regexType, string st, string sp, string et, string ep)
        {
            List<WireRegex> wireRegexList = GetWireListForRegexType(regexType);
            if (wireRegexList == null) return false;

            foreach (WireRegex wr in wireRegexList)
            {
                if (wr.startTile != null &&
                    wr.startPort != null &&
                    wr.endTile != null &&
                    wr.endPort != null &&
                    wr.startTile.IsMatch(st) &&
                    wr.startPort.IsMatch(sp) &&
                    wr.endTile.IsMatch(et) &&
                    wr.endPort.IsMatch(ep))
                {
                    return true;
                }
            }

            return false;
        }

        public static OutputMode Mode = OutputMode.Complete;
        private static string processingTile = "";
        private static void WriteToFile(string st, string sp, string et, string ep)
        {
            if (!Debugging)
                return;

            // Try to open a file
            try
            {
                if (File == null)
                {
                    File = new StreamWriter(FilePath);
                    processingTile = "";
                }
            }
            catch
            {
                Console.WriteLine("Failed opening the required file: " + FilePath);
                Console.WriteLine("Debug mode aborted. Please assign a new valid file path and try again.");
                FilePath = "";
                return;
            }

            // Write output
            if (Mode == OutputMode.Complete)
            {
                File.WriteLine(st + "." + sp + "->" + et + "." + ep);
            }
            else if (Mode == OutputMode.Compact)
            {
                if (st != processingTile)
                {
                    if (processingTile != "") File.WriteLine();
                    processingTile = st;
                    File.Write(processingTile + " ");
                }
                File.Write(sp + "->" + et + "." + ep + " ");
            }
        }

        public static void CloseStream()
        {
            if (File != null)
            {
                File.Close();
                File = null;
                Console.WriteLine("ReadVivadoFPGA Debugging finished successfully.");
            }
        }

        public static void DisableAndReset()
        {
            FilePath = "";
            File = null;
            IgnoreIdenticalLocationWires = true;
            ResetWireRegexes();

            Mode = OutputMode.Complete;
            processingTile = "";
        }

        public static void ResetWireRegexes()
        {
            Wire_Includes = null;
            Wire_Ignores = null;
        }
    }
}
