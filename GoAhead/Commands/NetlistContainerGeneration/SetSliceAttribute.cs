using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class SetSliceAttribute : Command
    {
        public SetSliceAttribute()
        {
        }

        public SetSliceAttribute(int sliceIndex, string attribute, string newValue)
        {
            SliceIndex = sliceIndex;
            AttributeName = attribute;
            NewValueAttributeValue = newValue;

        }

        public override void Undo()
        {
            FPGA.FPGA.Instance.Current.Slices[SliceIndex].SetAttributeValue(AttributeName, m_oldValue);
        }

       protected override void DoCommandAction()
        {
            m_oldValue = FPGA.FPGA.Instance.Current.Slices[SliceIndex].GetAttributeValue(AttributeName);
            if (!Regex.IsMatch(m_oldValue, "OFF") && !m_oldValue.Equals(NewValueAttributeValue))
                OutputManager.WriteOutput("Overwriting settting " + AttributeName + "=" + m_oldValue + " to " + AttributeName + "=" + NewValueAttributeValue);

            FPGA.FPGA.Instance.Current.Slices[SliceIndex].SetAttributeValue(AttributeName, NewValueAttributeValue);
        }

        [Parameter(Comment = "The index of the slice the blocker will use")]
        public int SliceIndex;
        [Parameter(Comment = "The name of the attribute to set")]
        public string AttributeName;
        [Parameter(Comment = "The new value of the attribute to set")]
        public string NewValueAttributeValue;

        private string m_oldValue;
    }
}
