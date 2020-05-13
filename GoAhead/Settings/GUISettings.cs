using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoAhead.Settings
{
    [Serializable]
    public class GUISettings
    {
        public void Open(Form form)
        {
            if (m_sizes.ContainsKey(form.Name))
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Size = m_sizes[form.Name];
                if (m_location[form.Name].X > 0 && m_location[form.Name].Y > 0)
                {
                    form.Location = m_location[form.Name];
                }
            }
        }

        public void Close(Form form)
        {
            if (!m_sizes.ContainsKey(form.Name))
            {
                m_sizes.Add(form.Name, form.Size);
                m_location.Add(form.Name, form.Location);
            }
            else
            {
                m_sizes[form.Name] = form.Size;
                m_location[form.Name] = form.Location;
            }
        }

        private Dictionary<string, Size> m_sizes = new Dictionary<string, Size>();
        private Dictionary<string, Point> m_location = new Dictionary<string, Point>();
    }
}