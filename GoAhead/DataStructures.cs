using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead
{
    [Serializable]
    public struct Pair<K, V>
    {
        public Pair(K item1, V item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public K Item1 { get; set; }
        public V Item2 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Pair<K, V> pair)
            {
                return pair.Item1.Equals(Item1) && pair.Item2.Equals(Item2);
            }
            else return false;
        }
    }

    public class UIntEqualityComparer : IEqualityComparer<uint[]>
    {
        public bool Equals(uint[] x, uint[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(uint[] obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + (int)obj[i];
                }
            }
            return result;
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, FontStyle style = FontStyle.Regular)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectionFont = new Font(box.Font, style);
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
