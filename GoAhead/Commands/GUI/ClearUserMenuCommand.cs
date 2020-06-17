using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.Commands.GUI
{
    abstract class ClearUserMenuCommand : GUICommand
    {
        protected override void DoCommandAction()
        {
            if (this.m_menu == null)
            {
                // prevent tracing this command twice
                this.UpdateCommandTrace = false;
                // postpone execution until GUI comes up
                ShowGUI.GUICommands.Add(this);
            }
            else
            {                
                this.Delete();
                if (this.m_menu.DropDownItems.Count == 0)
                {
                    this.m_menu.Visible = false;
                }
            }
        }

        private void Delete()
        {
            List<ToolStripMenuItem> itemsToRemove = new List<ToolStripMenuItem>();
            foreach (ToolStripMenuItem item in this.m_menu.DropDownItems)
            {
                String tag = item.Tag.ToString();
                if (this is ClearStaticUserMenu)
                {
                    if (tag.Equals("True"))
                    {
                        itemsToRemove.Add(item);
                    }
                }
                else if (this is ClearDynamicUserMenu)
                {
                    if (tag.Equals("False"))
                    {
                        itemsToRemove.Add(item);
                    }
                }
            }

            foreach (ToolStripMenuItem item in itemsToRemove)
            {
                this.m_menu.DropDownItems.Remove(item);
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

        protected ToolStripMenuItem m_menu = null;
    }
}
