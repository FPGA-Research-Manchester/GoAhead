﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    class PrintExcludePlacementProperty : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            CheckParameters();

            OutputManager.WriteTCLOutput($"set_property EXCLUDE_PLACEMENT true [get_pblocks pb_{InstanceName}]; # generated by GoAhead");
        }

        private void CheckParameters()
        {
            bool instanceNameIsCorrect = !string.IsNullOrEmpty(InstanceName);

            if (!instanceNameIsCorrect)
            {
                throw new ArgumentException("Unexpected format in parameters InstanceName.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Instance name of the component")]
        public string InstanceName = "inst_ConnMacro";
    }
}
