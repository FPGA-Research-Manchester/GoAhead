using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.Code
{
    static class HelperMethodsLibrary
    {
        public static bool CompareListsIgnoringOrder<T>(List<T> aListA, List<T> aListB)
        {
            if (aListA == null || aListB == null || aListA.Count != aListB.Count)
                return false;
            if (aListA.Count == 0)
                return true;
            Dictionary<T, int> lookUp = new Dictionary<T, int>();
            for (int i = 0; i < aListA.Count; i++)
            {
                int count = 0;
                if (!lookUp.TryGetValue(aListA[i], out count))
                {
                    lookUp.Add(aListA[i], 1);
                    continue;
                }
                lookUp[aListA[i]] = count + 1;
            }
            for (int i = 0; i < aListB.Count; i++)
            {
                int count = 0;
                if (!lookUp.TryGetValue(aListB[i], out count))
                {
                    return false;
                }
                count--;
                if (count <= 0)
                    lookUp.Remove(aListB[i]);
                else
                    lookUp[aListB[i]] = count;
            }
            return lookUp.Count == 0;
        }

        public static void RemoveRowInTableLayoutPanel(TableLayoutPanel panel, int rowIndex)
        {
            if (rowIndex >= panel.RowCount) return;


            panel.SuspendLayout();

            // Delete controls in the row
            for (int i = panel.ColumnCount - 1; i >= 0; i--)
            {
                Control control = panel.GetControlFromPosition(i, rowIndex);
                panel.Controls.Remove(control);
                control.Dispose();
            }

            // Move up the remaining rows
            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    Control control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            // Remove last row
            var removeStyle = panel.RowCount - 1;
            if (panel.RowStyles.Count > removeStyle)
                panel.RowStyles.RemoveAt(removeStyle);
            panel.RowCount--;

            panel.ResumeLayout();
        }
    }
}
