
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Selection
{
    /// <summary>
    /// A common base class for all commmands that add tiles to the current selecition
    /// </summary>
    [Serializable]
    public abstract class AddToSelectionCommand : Command
    {
    }
}
