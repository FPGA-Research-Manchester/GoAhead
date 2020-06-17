using System;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.BlockingShared
{
    [CommandDescription(Description = "Instantiate the blocker primitve in all unused selected slice whose indeces match the paramter SliceNumberPattern. " +
                                      "No nets will be created, the primitves will be added to the currently selected macro. " +
                                      "Tiles filter given with AddBlockerTileFilter are applied. " +
                                      "Use the paramter to SliceNumberPattern to select the slice indeces in which this command instantiates primitves. ", Wrapper = false, Publish = true)]
    public class AddPrimitvesInSelection : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer macro = NetlistContainerManager.Instance.Get(this.MacroName);

            // 1 iterate over all not filtered out tiles to instantiate primitves and to find an outpin
            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(t => !Objects.BlockerSettings.Instance.SkipTile(t)))
            {
                // iterate in order
                for (int i = 0; i < t.Slices.Count; i++)
                {
                    Slice s = t.Slices[i];

                    if (!Regex.IsMatch(i.ToString(), this.SliceNumberPattern) || s.Usage != FPGATypes.SliceUsage.Free)
                    {
                        continue;
                    }

                    String template = "";
                    // ignore the SliceNumberPattern given by AddBlockerPrimitveRegexp, this command has its own one
                    // TODO add AddPrimitveRegexp in addition to AddBlockerPrimitveRegexp
                    if (Objects.BlockerSettings.Instance.InsertTemplate(s.SliceName, true, i, out template))
                    {
                        AddTemplateConfig addTemplateCommand = new AddTemplateConfig();
                        addTemplateCommand.Location = t.Location;
                        addTemplateCommand.NetlistContainerName = this.NetlistContainerName;
                        addTemplateCommand.PrimitiveIndex = i;
                        addTemplateCommand.Template = template;
                        CommandExecuter.Instance.Execute(addTemplateCommand);

                        // attach usage
                        s.Usage = FPGATypes.SliceUsage.Blocker;
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The regular expression that identifies the slices to instantiate primitives in, e.g. [0-1]")]
        public String SliceNumberPattern = "0|1";

        [Parameter(Comment = "The macro to extend")]
        public String MacroName = "macro";
    }
}