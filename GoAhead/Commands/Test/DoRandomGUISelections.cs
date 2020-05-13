using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.Selection;
namespace GoAhead.Commands.Test
{
    class DoRandomGUISelections : Command
    {
        protected override void DoCommandAction()
        {
            int x1, x2, y1, y2;
            Random rnd = new Random();

            CommandExecuter.Instance.Execute(new SelectAll());
            CommandExecuter.Instance.Execute(new ExpandSelection());
            CommandExecuter.Instance.Execute(new InvertSelection());

            for (int i = 0; i < 10; i++)
            {
                CommandExecuter.Instance.Execute(new ClearSelection());

                x1 = rnd.Next(0, FPGA.FPGA.Instance.MaxX);
                x2 = rnd.Next(x1, FPGA.FPGA.Instance.MaxX);

                y1 = rnd.Next(0, FPGA.FPGA.Instance.MaxY);
                y2 = rnd.Next(y1, FPGA.FPGA.Instance.MaxY);

                CommandExecuter.Instance.Execute(new AddToSelectionXY(x1, y1, x2, y2));
                CommandExecuter.Instance.Execute(new ExpandSelection());
                FPGA.TileSelectionManager.Instance.SelectionChanged();

                CommandExecuter.Instance.Execute(new InvertSelection());
                CommandExecuter.Instance.Execute(new ExpandSelection());
                FPGA.TileSelectionManager.Instance.SelectionChanged();
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

    }
}
