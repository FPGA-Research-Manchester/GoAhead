using GoAhead.Commands.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "", Wrapper = true)]
    abstract class IncludeFlagCommand : Command
    {
        [Parameter(Comment = "The FPGA Family this command applies to")]
        public string FamilyRegexp = "";

        [Parameter(Comment = "The flag to set. Default=true")]
        public bool Flag = true;

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void DoCommandAction()
        {
            WireHelper.AddIncludeFlag(FlagType, FamilyRegexp, Flag);
        }

        protected abstract WireHelper.IncludeFlag FlagType { get; }
    }

    [CommandDescription(Description = "Using this command will make GoAhead add all the wires that start and end " +
        "on the same tile to the model - while parsing a Vivado Device Description", Wrapper = true)]
    class IncludeUTurnWires : IncludeFlagCommand
    {
        protected override WireHelper.IncludeFlag FlagType => WireHelper.IncludeFlag.UTurnWires;
    }

    [CommandDescription(Description = "Using this command will make GoAhead add switch matrix arcs for " +
        "any two slice ports in the model that only have a single BEL on the path between them - while parsing a Vivado Device Description", Wrapper = true)]
    class IncludeSingleStopoverArcs : IncludeFlagCommand
    {
        protected override WireHelper.IncludeFlag FlagType => WireHelper.IncludeFlag.SingleStopoverArcs;
    }

    [CommandDescription(Description = "Using this command will make GoAhead add a stopover-blocked tag to all " +
        "ports of the switch matrix arcs that route through stopovers/non-routing BELs - while parsing a Vivado Device Description", Wrapper = true)]
    class BlockStopoverArcPorts : IncludeFlagCommand
    {
        protected override WireHelper.IncludeFlag FlagType => WireHelper.IncludeFlag.BlockStopoverPorts;
    }

    [CommandDescription(Description = "Using this command will make GoAhead record all the tiles that wires are passing through" +
        " - while parsing a Vivado Device Description. Warning: this will dramatically increase the size of the model.", Wrapper = true)]
    class IncludeWiresTrajectoriesData : IncludeFlagCommand
    {
        protected override WireHelper.IncludeFlag FlagType => WireHelper.IncludeFlag.WiresTrajectoriesData;
    }

    [CommandDescription(Description = "Using this command will make GoAhead record for each tile the wires that start elsewhere and end in it" +
        " - while parsing a Vivado Device Description.", Wrapper = true)]
    class IncludeIncomingWires : IncludeFlagCommand
    {
        protected override WireHelper.IncludeFlag FlagType => WireHelper.IncludeFlag.IncomingWires;
    }

    [CommandDescription(Description = "Using this command will make GoAhead add switch matrix arcs in both directions for bi-directional pips" +
        " - while parsing a Vivado Device Description.", Wrapper = true)]
    class IncludeBiDirectionalPips : IncludeFlagCommand
    {
        protected override WireHelper.IncludeFlag FlagType => WireHelper.IncludeFlag.BiDirectionalPips;
    }
}
