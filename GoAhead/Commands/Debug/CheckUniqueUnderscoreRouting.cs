using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;


namespace GoAhead.Commands.Debug
{
    class CheckUniqueUnderscoreRouting : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            foreach(Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                foreach (Port portWithUnderscore in tile.SwitchMatrix.Ports.Where(p => p.Name.Contains("END") && p.Name.Count(c => c.Equals('_')) == 2))
                {
                    IEnumerable<Port> reachableUnderScore = tile.SwitchMatrix.GetDrivenPorts(portWithUnderscore);

                    string otherPortName = portWithUnderscore.Name.Substring(0, portWithUnderscore.Name.IndexOf('_'));
                    otherPortName += portWithUnderscore.Name[portWithUnderscore.Name.Length-1];

                    if(!tile.SwitchMatrix.Contains(otherPortName))
                    {

                    }

                    IEnumerable<Port> reachableWithoutUnderScore = tile.SwitchMatrix.GetDrivenPorts(new Port(otherPortName));

                    foreach(Port u in reachableUnderScore)
                    {
                        Port conflict = reachableWithoutUnderScore.FirstOrDefault(p => p.Equals(u));
                        if(conflict != null)
                        {

                        }

                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
