using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Commands
{
    class System : Command
    {
        public override void Do()
        {
            throw new NotImplementedException();
        }
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        private readonly String m_command;
        private readonly String m_args;
    }
}
