using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Commands
{
    public abstract class CommandHook
    {
        public abstract void CommandTrace(Command cmd);
        public abstract void PreRun(Command cmd);
        public abstract void Error(Command cmd, Exception error);
        public abstract void PostRun(Command cmd);
        public abstract void ParseError(string cmd, string error);
        public abstract void ProgressUpdate(Command cmd);
    }
}
