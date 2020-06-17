using System;
using System.IO;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    public class AddBlockerPrimitveRegexp : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            if (!File.Exists(this.Template))
            {
                String fileNameWithotherSeparator = this.Template.Replace('/', '\\');
                if (File.Exists(fileNameWithotherSeparator))
                {
                    Console.WriteLine("Changing " + this.Template + " to " + fileNameWithotherSeparator);
                    Objects.BlockerSettings.Instance.AddPrimitveTemplate(this.FamilyRegexp, this.PrimitveRegexp, this.SliceNumberPattern, this.Template);
                    return;
                }
                throw new ArgumentException("Could not find the template " + this.Template);
            }

            Objects.BlockerSettings.Instance.AddPrimitveTemplate(this.FamilyRegexp, this.PrimitveRegexp, this.SliceNumberPattern, this.Template);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The configuration template")]
        public String Template = "";

        [Parameter(Comment = "The regular expression which identifies the primitives to apply the template on")]
        public String PrimitveRegexp = "";

        [Parameter(Comment = "The indeces of the slices the blocker will use")]
        public String SliceNumberPattern = "[0-1]";
    }
}