using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.Commands.GUI
{
    [CommandDescription(Description = "Clear all user defined static menu entries", Wrapper = false)]
    class ClearStaticUserMenu : ClearUserMenuCommand
    {
    }
}
