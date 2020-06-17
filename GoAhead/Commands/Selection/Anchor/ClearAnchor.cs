using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection.Anchor
{
    [CommandDescription(Description = "Clear the selection anchor. All AddToSelectionXY command will have absolute values of X1, X2, Y1 and Y2", Wrapper = false)]
    class ClearAnchor : Command
    {
       protected override void DoCommandAction()
        {
            this.m_lastAnchor = Objects.SelectionManager.Instance.Anchor;
            Objects.SelectionManager.Instance.Anchor = null;
        }

        public override void Undo()
        {
            Objects.SelectionManager.Instance.Anchor = this.m_lastAnchor;
        }

        /// <summary>
        /// undo
        /// </summary>
        private Tile m_lastAnchor = null;
    }
}
