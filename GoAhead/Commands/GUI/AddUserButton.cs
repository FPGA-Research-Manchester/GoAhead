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
            if (this.m_toolStrip == null)
            {
                // prevent tracing this command twice
                this.UpdateCommandTrace = false;
                // postpone execution until GUI comes up
                ShowGUI.GUICommands.Add(this);
            }
            else
            {
                this.m_toolStrip.Visible = true;
                this.m_toolStrip.SuspendLayout();

                bool useIcon = System.IO.File.Exists(this.Image);

                ToolStripButton userBtn = new ToolStripButton();
                userBtn.Visible = true;
                userBtn.Name = useIcon ? "" : this.Name;
                userBtn.ToolTipText = String.IsNullOrEmpty(this.ToolTip) ? this.Name + ": " + this.Command : this.ToolTip;
                userBtn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                userBtn.Text = useIcon ? "" : this.Name;
                userBtn.Image = useIcon ? Bitmap.FromFile(this.Image) : null;
                userBtn.Click += new System.EventHandler(this.UserDefinedAction);

                this.m_toolStrip.Items.Add(userBtn);
                this.m_toolStrip.Items.Add(new ToolStripSeparator());
                this.m_toolStrip.ResumeLayout();
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
        public String Image = "icon.bmp";
    }
}
