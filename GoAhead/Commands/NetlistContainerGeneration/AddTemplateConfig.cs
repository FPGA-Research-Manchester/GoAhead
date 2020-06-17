using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class AddTemplateConfig : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            XDLContainer nlc = (XDLContainer) this.GetNetlistContainer();
            AddTemplateConfig.AddTemplate(nlc, this.Template, this.Location, this.PrimitiveIndex);
        }

        public static void AddTemplate(XDLContainer netlistContainer, String template, String location, int primitiveIndex)
        {
            StreamReader sr = new StreamReader(template);
            String wholeFile = sr.ReadToEnd();
            sr.Close();    

            Tile t = FPGA.FPGA.Instance.GetTile(location);
            MatchCollection instanceMatches = AddTemplateConfig.m_regExpInstance.Matches(wholeFile);
            foreach (Match match in instanceMatches)
            {
                String instanceCode = match.Groups[1].Value;

                instanceCode = instanceCode.Replace("_LOCATION_", t.Location);
                instanceCode = instanceCode.Replace("_SLICENAME_", t.Slices[primitiveIndex].SliceName);
                instanceCode = instanceCode.Replace("_SLICETYPE_", t.Slices[primitiveIndex].SliceType);

                // attach usage
                t.Slices[primitiveIndex].Usage = FPGATypes.SliceUsage.Blocker;
                netlistContainer.AddSliceCodeBlock(instanceCode);
            }

            MatchCollection netMatches = AddTemplateConfig.m_regExpNets.Matches(wholeFile);
            foreach (Match netMatch in netMatches)
            {
                String netCode = netMatch.Groups[1].Value;

                // DSP and BRAM templates only use _LOCATION_
                netCode = netCode.Replace("_LOCATION_", t.Location);
                // SLICEX templates only use _SLICENAME_
                netCode = netCode.Replace("_SLICENAME_", t.Slices[primitiveIndex].SliceName);

                netlistContainer.AddNetCodeBlock(netCode);
            }
        }

        private static Regex m_regExpInstance = new Regex(@"(inst [^;]*;)", RegexOptions.Multiline & RegexOptions.IgnorePatternWhitespace & RegexOptions.CultureInvariant);
        private static Regex m_regExpNets = new Regex(@"(net [^;]*;)", RegexOptions.Multiline & RegexOptions.IgnorePatternWhitespace & RegexOptions.CultureInvariant);

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The template to use")]
        public String Template = "";

        [Parameter(Comment = "The slice index to use")]
        public int PrimitiveIndex = 0;

        [Parameter(Comment = "The tile identifier to instantiate a slice from")]
        public String Location = "";
    }
}
