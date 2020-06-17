using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Define colors for tiles", Wrapper = false, Publish=true)]
    class SetColorSetting : SetIdentifierCommand
    {
        protected override void DoCommandAction()
        {
            bool useColorString = !String.IsNullOrEmpty(this.Color);
            bool userRGB = this.RGB == null ? false : this.RGB.Count > 0;

            if (useColorString && !userRGB)
            {
                Settings.ColorSettings.Instance.SetColor(this.IdentifierRegexp, this.Color);
            }
            else if (!useColorString && userRGB)
            {
                if (this.RGB.Count != 3)
                {
                    throw new ArgumentException("Specify three values. E.g RGB=0,0,255");
                }
                Color c = System.Drawing.Color.FromArgb(this.RGB[0], this.RGB[1], this.RGB[2]);
                Settings.ColorSettings.Instance.SetColor(this.IdentifierRegexp, c.Name);
            }
            else
            {
                throw new ArgumentException("Either specify Color or the RGB. Do not user both");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "the name of the color to use")]
        public String Color = "";

        [Parameter(Comment = "the comma separeted RGB value")]
        public List<int> RGB = new List<int>();
    }
}
