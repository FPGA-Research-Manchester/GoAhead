using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    class AddSlice : NetlistContainerCommand
	{
        public AddSlice()
        {
        }

        public AddSlice(string netlistContainerName, int sliceNumber)
		{
            NetlistContainerName = netlistContainerName;
            SliceNumber = sliceNumber;
		}

        protected override void DoCommandAction()
		{
            NetlistContainer netlistContainer = GetNetlistContainer();

            // do not store addSlice in constructor already!
            m_addedSlice = FPGA.FPGA.Instance.Current.Slices[SliceNumber];

            /*
            if (netlistContainer.HasSlice(this.m_addedSlice))
            {
                this.OutputManager.WriteOutput("Overwriting slice " + this.m_addedSlice + " in macro " + netlistContainer);
            }*/

            netlistContainer.Add(m_addedSlice);
		}

		public override void Undo()
		{
            throw new NotImplementedException();
		}

        [Parameter(Comment = "The index of the slice to instantiate")]
        public int SliceNumber;


        private Slice m_addedSlice;
    }
}


