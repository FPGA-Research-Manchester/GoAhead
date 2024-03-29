﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Print the selected resources of given netlst", Wrapper = false, Publish = true)]
    class PrintResourceConsumptionInSelection : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = GetNetlistContainer();

            int clbCount = 0;
            int bramCount = 0;
            int dspCount = 0;
            TileSelectionManager.Instance.GetRessourcesInSelection(TileSelectionManager.Instance.GetSelectedTiles(), out clbCount, out bramCount, out dspCount);
            int sliceCount = clbCount*2;

            int sliceInstances = 0;
            int bramInstances = 0;
            int dspInstances = 0;
            foreach (XDLInstance inst in nlc.Instances.Where(i => TileSelectionManager.Instance.IsSelected(i.TileKey)))
            {
                if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.CLB))
                {
                    sliceInstances++;
                }
                else if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.DSP))
                {
                    dspInstances++;
                }
                else if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.BRAM))
                {
                    bramInstances++;
                }
            }

            OutputManager.WriteOutput("Slices: " + sliceInstances + " out of " + sliceCount);
            OutputManager.WriteOutput("DSPs: " + dspInstances + " out of " + dspCount);
            OutputManager.WriteOutput("BRAMs: " + bramInstances + " out of " + bramCount);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
