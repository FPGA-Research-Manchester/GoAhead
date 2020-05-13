using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.MacroGeneration
{
    class SetSliceAttribute : Command
    {
        public SetSliceAttribute()
        {
        }

        public SetSliceAttribute(int sliceIndex, String attribute, String newValue)
        {
            this.SliceIndex = sliceIndex;
            this.AttributeName = attribute;
            this.NewValueAttributeValue = newValue;

        }

        public override void Undo()
        {
            FPGA.FPGA.Instance.Current.Slices[this.SliceIndex].SetAttributeValue(this.AttributeName, this.m_oldValue);
        }

        public override void Do()
        {
            this.m_oldValue = FPGA.FPGA.Instance.Current.Slices[this.SliceIndex].GetAttributeValue(this.AttributeName);
            if (!Regex.IsMatch(m_oldValue, "OFF") && !this.m_oldValue.Equals(this.NewValueAttributeValue))
                this.WriteDebugTrace("Overwriting settting " + this.AttributeName + "=" + this.m_oldValue + " to " + this.AttributeName + "=" + this.NewValueAttributeValue);

            FPGA.FPGA.Instance.Current.Slices[this.SliceIndex].SetAttributeValue(this.AttributeName, this.NewValueAttributeValue);
        }

        [ParamterField(Comment = "The index of the slice the blocker will use")]
        public int SliceIndex;
        [ParamterField(Comment = "The name of the attribute to set")]
        public String AttributeName;
        [ParamterField(Comment = "The new value of the attribute to set")]
        public String NewValueAttributeValue;

        private String m_oldValue;
    }
}
