using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.TCL
{
    class CommandsList
    {
        private static readonly string commandsStr =
            "after errorInfo load re_syntax tcl_startOfNextWord append eval lrange read tcl_startOfPreviousWord apply exec lrepeat refchan tcl_traceCompile argc exit lreplace regexp tcl_traceExec argv expr lreverse registry tcl_version argv0 fblocked lsearch regsub tcl_wordBreakAfter array fconfigure lset rename tcl_wordBreakBefore auto_execok fcopy lsort return tcl_wordchars auto_import file mathfunc safe tcltest auto_load fileevent mathop scan tell auto_mkindex filename memory seek throw auto_path flush msgcat self time auto_qualify for my set tm auto_reset foreach namespace socket trace bgerror format next source transchan binary gets nextto split try break glob oo::class string unknown catch global oo::copy subst unload cd history oo::define switch unset chan http oo::objdefine tailcall update clock if oo::object Tcl uplevel close incr open tcl::prefix upvar concat info package tcl_endOfWord variable continue interp parray tcl_findLibrary vwait coroutine join pid tcl_interactive while dde lappend pkg::create tcl_library yield dict lassign pkg_mkIndex tcl_nonwordchars yieldto encoding lindex platform tcl_patchLevel zlib env linsert platform::shell tcl_pkgPath eof list proc tcl_platform error llength puts tcl_precision errorCode lmap pwd tcl_rcFileName";

        private static List<string> commands = null;
        public static List<string> Commands
        {
            get
            {
                if(commands == null)
                {
                    commands = commandsStr.Split(' ').ToList();
                    commands.Sort();
                }
                return commands;
            }
        }

        public static List<string> ExtraCommands { get; private set; }

        public static void RegisterExtraCommand(string name)
        {
            if (ExtraCommands == null) ExtraCommands = new List<string>();
            ExtraCommands.Add(name);
        }
    }
}
