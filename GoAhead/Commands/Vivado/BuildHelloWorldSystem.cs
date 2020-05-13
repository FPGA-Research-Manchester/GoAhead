using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Vivado
{
    class BuildHelloWorldSystem : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.Execute(new Reset());
            // find horizontal placement
            int x1 = 0;
            int x2 = 0;
            for (int y = 0; y < FPGA.FPGA.Instance.MaxY; y++)
            {
                for (int x = 0; x < FPGA.FPGA.Instance.MaxX; x++)
                {
                    string currentResourceString = "";
                    int i = 0;
                    while (currentResourceString.Length < ResourceString.Length)
                    {
                        // left FPGA
                        if (!FPGA.FPGA.Instance.Contains(x + i, y))
                        {
                            break;
                        }
                        Tile current = FPGA.FPGA.Instance.GetTile(x + i++, y);
                        // only cosider CLB, DSP, and BRAM
                        if(!(
                            IdentifierManager.Instance.IsMatch(current.Location, IdentifierManager.RegexTypes.CLB) || 
                            IdentifierManager.Instance.IsMatch(current.Location, IdentifierManager.RegexTypes.DSP) || 
                            IdentifierManager.Instance.IsMatch(current.Location, IdentifierManager.RegexTypes.BRAM))
                        )
                        {
                            continue;
                        }


                        currentResourceString += current.Location[0];
                        Console.WriteLine(currentResourceString);
                        if (currentResourceString.Equals(ResourceString))
                        {
                            x1 = x;
                            x2 = x+i-1;
                        }
                       
                    }
                }
            }
            int y1 = 0;
            int y2 = 0;
            for (int y = 0; y < FPGA.FPGA.Instance.MaxY; y++)
            {
                int hCount = 0;
                for (int h = y; y + h < FPGA.FPGA.Instance.MaxY; h++)
                {                                            
                    Tile current = FPGA.FPGA.Instance.GetTile(x1, y);
                    if (
                        IdentifierManager.Instance.IsMatch(current.Location, IdentifierManager.RegexTypes.CLB) ||
                        IdentifierManager.Instance.IsMatch(current.Location, IdentifierManager.RegexTypes.DSP) ||
                        IdentifierManager.Instance.IsMatch(current.Location, IdentifierManager.RegexTypes.BRAM)
                    )
                    {
                        hCount++;
                    }

                    if (hCount == Height - 1)
                    {
                        y1 = y;
                        y2 = y + h;
                        break;
                    }
                       
                    
                }
                if (y1 != 0 && y2 != 0)
                {
                    break;
                }
            }
            AddToSelectionXY selectModuleArea = new AddToSelectionXY(x1, y1, x2, y2);

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Height of the reconfigurable area in number of CLBs")]
        public int Height = 120;

        [Parameter(Comment = "The resource string")]
        public string ResourceString = "BCC";
    }
}
