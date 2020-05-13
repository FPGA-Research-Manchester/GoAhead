using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GoAhead.Commands;

namespace GoAhead.Commands.GUI
{
    [CommandDescription(Description = "Add user defined buttons to he GUI", Wrapper = false)]
    class AddUserButton : AddUserElement
    {
        protected override void DoCommandAction()
        {
            if (m_toolStrip == null)
            {
                // prevent tracing this command twice
                UpdateCommandTrace = false;
                // postpone execution until GUI comes up
                ShowGUI.GUICommands.Add(this);
            }
            else
            {
                m_toolStrip.Visible = true;
                m_toolStrip.SuspendLayout();

                bool useIcon = System.IO.File.Exists(Image);

                ToolStripButton userBtn = new ToolStripButton();
                userBtn.Visible = true;
                userBtn.Name = useIcon ? "" : Name;
                userBtn.ToolTipText = string.IsNullOrEmpty(ToolTip) ? Name + ": " + Command : ToolTip;
                userBtn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                userBtn.Text = useIcon ? "" : Name;
                userBtn.Image = useIcon ? System.Drawing.Image.FromFile(Image) : null;
                userBtn.Click += new System.EventHandler(UserDefinedAction);

                m_toolStrip.Items.Add(userBtn);
                m_toolStrip.Items.Add(new ToolStripSeparator());
                m_toolStrip.ResumeLayout();
            }
        }



        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public ToolStrip ToolStrip
        {
            get { return m_toolStrip; }
            set { m_toolStrip = value; }
        }

        private ToolStrip m_toolStrip = null;
        
        [Parameter(Comment = "The optional icon for the user button")]
        public string Image = "icon.bmp";
    }
}
