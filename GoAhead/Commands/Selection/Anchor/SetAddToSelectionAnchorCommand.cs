﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Commands.Variables;

namespace GoAhead.Commands.Selection.Anchor
{
    abstract class SetAddToSelectionAnchorCommand : Command
    {
        protected void SetAnchor(Tile anchor)
        {
            Objects.SelectionManager.Instance.Anchor = anchor;
            Objects.SelectionManager.Instance.XAnchorName = XName;
            Objects.SelectionManager.Instance.YAnchorName = YName;

            Set setXDefineCmd = new Set();
            setXDefineCmd.Variable = XName;
            setXDefineCmd.Value = anchor.TileKey.X.ToString();
            CommandExecuter.Instance.Execute(setXDefineCmd);

            Set setYDefineCmd = new Set();
            setYDefineCmd.Variable = YName;
            setYDefineCmd.Value = anchor.TileKey.Y.ToString();
            CommandExecuter.Instance.Execute(setYDefineCmd);
        }

        [Parameter(Comment = "The name of the X Anchor")]
        public string XName = "xAnchor";
                
        [Parameter(Comment = "The name of the Y Anchor")]
        public string YName = "yAnchor";
    }
}
