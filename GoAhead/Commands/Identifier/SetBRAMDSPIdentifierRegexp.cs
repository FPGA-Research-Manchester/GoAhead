using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description="Set the regexp GoAhead uses to identify BRAMs tiles for the given Family", Wrapper=false, Publish=true)]
    class SetBRAMIdentifierRegexp : SetIdentifierCommand
    {
        protected override void DoCommandAction()
        {
            Objects.BRAMDSPSettingsManager.Instance.SetBRAMParameters(this.FamilyRegexp, this.Width, this.Height, this.LeftRightHandling, this.ButtomLeft, this.ButtomRight);
            Objects.IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.BRAM, this.FamilyRegexp, this.IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The width of BRAM")]
        public int Width = 3;

        [Parameter(Comment = "The height of BRAM")]
        public int Height = 4;

        [Parameter(Comment = "Wheter some BRAM are buttom left and some are buttom right. Only true for Kintex7. If set to true, ButtomLeft and ButtomLeft match in left and right tiles")]
        public bool LeftRightHandling = false;

        [Parameter(Comment = "")]
        public String ButtomLeft = "_L_";

        [Parameter(Comment = "")]
        public String ButtomRight = "_R_";    
    }

    [CommandDescription(Description = "Set the regexp GoAhead uses to identify DSP tiles for the given Family", Wrapper = false, Publish = true)]
    class SetDSPIdentifierRegexp : SetIdentifierCommand
    {
       protected override void DoCommandAction()
        {
            Objects.BRAMDSPSettingsManager.Instance.SetDSPParameters(this.FamilyRegexp, this.Width, this.Height, this.LeftRightHandling, this.ButtomLeft, this.ButtomRight);
            Objects.IdentifierManager.Instance.SetRegex(IdentifierManager.RegexTypes.DSP, this.FamilyRegexp, this.IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The width of BRAM")]
        public int Width = 3;

        [Parameter(Comment = "The height of BRAM")]
        public int Height = 4;

        [Parameter(Comment = "Wheter some BRAM are buttom left and some are buttom right. Only true for Kintex7. If set to true, ButtomLeft and ButtomLeft match in left and right tiles")]
        public bool LeftRightHandling = false;

        [Parameter(Comment = "")]
        public String ButtomLeft = "_L_";

        [Parameter(Comment = "")]
        public String ButtomRight = "_R_";
    }
}
