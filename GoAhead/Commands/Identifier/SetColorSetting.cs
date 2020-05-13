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
            bool useColorString = !string.IsNullOrEmpty(Color);
            bool userRGB = RGB == null ? false : RGB.Count > 0;

            if (useColorString && !userRGB)
            {
                Settings.ColorSettings.Instance.SetColor(IdentifierRegexp, Color);
            }
            else if (!useColorString && userRGB)
            {
                if (RGB.Count != 3)
                {
                    throw new ArgumentException("Specify three values. E.g RGB=0,0,255");
                }
                Color c = System.Drawing.Color.FromArgb(RGB[0], RGB[1], RGB[2]);
                Settings.ColorSettings.Instance.SetColor(IdentifierRegexp, c.Name);
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
        public string Color = "";

        [Parameter(Comment = "the comma separeted RGB value")]
        public List<int> RGB = new List<int>();
    }
}
