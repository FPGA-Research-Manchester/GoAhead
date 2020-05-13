using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.Commands.GUI
{
    [CommandDescription(Description = "Add user defined menu entry to he GUI", Wrapper = false)]
    class AddUserMenu : AddUserElement
    {
        protected override void DoCommandAction()
        {
            if (m_menu == null)
            {
                // prevent tracing this command twice
                UpdateCommandTrace = false;
                // postpone execution until GUI comes up
                ShowGUI.GUICommands.Add(this);
            }
            else
            {
                m_menu.Visible = true;

                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Visible = true;
                menuItem.Name = Name;
                menuItem.ToolTipText = string.IsNullOrEmpty(ToolTip) ? (Name + ": " + Command) : ToolTip;
                menuItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                menuItem.Text = Name;
                menuItem.Click += new System.EventHandler(UserDefinedAction);
                menuItem.Tag = Static;
                m_menu.DropDownItems.Add(menuItem);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public ToolStripMenuItem ToolStrip
        {
            get { return m_menu; }
            set { m_menu = value; }
        }

        private ToolStripMenuItem m_menu = null;
    }
}
