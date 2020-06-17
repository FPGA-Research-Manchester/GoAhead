using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.InterfaceManager
{
    class DeleteSignal : Command
    {
        public DeleteSignal()
        {
        }

       protected override void DoCommandAction()
        {
            Objects.InterfaceManager.Instance.Remove(this.SignalName);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of signal to delete")]
        public String SignalName = "s";


    }
}
