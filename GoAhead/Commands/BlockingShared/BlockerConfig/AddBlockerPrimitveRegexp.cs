using System;
using System.IO;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    public class AddBlockerPrimitveRegexp : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            if (!File.Exists(Template))
            {
                string fileNameWithotherSeparator = Template.Replace('/', '\\');
                if (File.Exists(fileNameWithotherSeparator))
                {
                    Console.WriteLine("Changing " + Template + " to " + fileNameWithotherSeparator);
                    Objects.BlockerSettings.Instance.AddPrimitveTemplate(FamilyRegexp, PrimitveRegexp, SliceNumberPattern, Template);
                    return;
                }
                throw new ArgumentException("Could not find the template " + Template);
            }

            Objects.BlockerSettings.Instance.AddPrimitveTemplate(FamilyRegexp, PrimitveRegexp, SliceNumberPattern, Template);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The configuration template")]
        public string Template = "";

        [Parameter(Comment = "The regular expression which identifies the primitives to apply the template on")]
        public string PrimitveRegexp = "";

        [Parameter(Comment = "The indeces of the slices the blocker will use")]
        public string SliceNumberPattern = "[0-1]";
    }
}