using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.MacroGeneration
{
	class AddSlice : Command
	{
        public AddSlice()
        {
        }

        public AddSlice(int sliceNumber)
		{
            this.SliceNumber = sliceNumber;
		}

		public override void Do()
		{
            // do not store addSlice in ctor already!
            this.m_addedSlice = FPGA.FPGA.Instance.Current.Slices[this.SliceNumber];

            if (MacroManager.Instance.CurrentMacro == null)
            {
                throw new ArgumentException("No macro found");
            }

            if (MacroManager.Instance.CurrentMacro.HasSlice(this.m_addedSlice))
                this.WriteDebugTrace("Overwriting slice " + this.m_addedSlice + " in macro " + MacroManager.Instance.CurrentMacro);
            
            MacroManager.Instance.CurrentMacro.Add(this.m_addedSlice);
		}

		public override void Undo()
		{
            MacroManager.Instance.CurrentMacro.Remove(this.m_addedSlice);
		}

        [ParamterField(Comment = "The index of the slice to instantiate")]
        public int SliceNumber;


        private Slice m_addedSlice;
    }
}


