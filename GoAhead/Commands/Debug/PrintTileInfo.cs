using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Code.XDL;
using GoAhead.Commands.Data;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print information on the given tile", Wrapper = false, Publish = true)]
    class PrintTileInfo : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // click done out of fpga range
            if (!FPGA.FPGA.Instance.Contains(this.Location))
            {
                return;
            }

            Tile where = FPGA.FPGA.Instance.GetTile(this.Location);

            this.OutputManager.WriteOutput(where.Location + " " + Environment.NewLine);
            for (int i = 0; i < where.Slices.Count; i++)
            {
                this.OutputManager.WriteOutput("Slice Usage is: " + where.Slices[i].Usage + Environment.NewLine);
                this.OutputManager.WriteOutput(XDLFile.GetCode(where.Slices[i]));
            }

            if (!FPGA.FPGA.Instance.ContainsSwitchMatrix(where.SwitchMatrixHashCode))
            {
                return;
            }

            if (Objects.Blackboard.Instance.HasToolTipInfo(where))
            {
                this.OutputManager.WriteOutput(Blackboard.Instance.GetToolTipInfo(where));
            }

            this.OutputManager.WriteOutput("Blocked Ports" + Environment.NewLine);
            foreach (String p in where.GetAllBlockedPorts(Tile.BlockReason.Blocked).OrderBy(p => p))
            {
                this.OutputManager.WriteOutput(p + Environment.NewLine);
            }

            this.OutputManager.WriteOutput("Ports to be blocked" + Environment.NewLine);
            foreach (String p in where.GetAllBlockedPorts(Tile.BlockReason.ToBeBlocked).OrderBy(p => p))
            {
                this.OutputManager.WriteOutput(p + Environment.NewLine);
            }

            this.OutputManager.WriteOutput("Ports excluded from Blocking " + Environment.NewLine);
            foreach (String p in where.GetAllBlockedPorts(Tile.BlockReason.ExcludedFromBlocking).OrderBy(p => p))
            {
                this.OutputManager.WriteOutput(p + Environment.NewLine);
            }

            this.OutputManager.WriteOutput("Ports occupied by macros " + Environment.NewLine);
            foreach (String p in where.GetAllBlockedPorts(Tile.BlockReason.OccupiedByMacro).OrderBy(p => p))
            {
                this.OutputManager.WriteOutput(p + Environment.NewLine);
            }

            var ports = where.GetAllBlockedPorts(Tile.BlockReason.Stopover).OrderBy(p => p);
            if (ports.Count() > 0)
            {
                OutputManager.WriteOutput("Ports of arcs with stopovers " + Environment.NewLine);
                foreach (string p in ports)
                {
                    OutputManager.WriteOutput(p + Environment.NewLine);
                }
            }            

            this.OutputManager.WriteOutput("Ports without any usage" + Environment.NewLine);
            foreach (Port p in where.SwitchMatrix.Ports.Where(p => !where.IsPortBlocked(p)).OrderBy(p => p.Name))
            {
                this.OutputManager.WriteOutput(p.Name + Environment.NewLine);
            }


            this.OutputManager.WriteOutput("Slice usage " + Environment.NewLine);
            foreach (Slice s in where.Slices)
            {
                this.OutputManager.WriteOutput(s.SliceName + " " + s.Usage + Environment.NewLine);
            }
            foreach (Slice slice in where.Slices.Where(s => s.Bels != null))
            {
                this.OutputManager.WriteOutput(slice.SliceName + " BELs " + Environment.NewLine);
                foreach (string b in slice.Bels.OrderBy(b => b))
                {
                    this.OutputManager.WriteOutput(b + " (" + slice.GetBelUsage(b) + ") ");
                }
                this.OutputManager.WriteOutput(Environment.NewLine);
            }
            this.OutputManager.WriteOutput(Environment.NewLine);

            this.OutputManager.WriteOutput("Wire List Hash Code: " + where.WireListHashCode + Environment.NewLine);
            this.OutputManager.WriteOutput("Switch Matrix Code: " + where.SwitchMatrixHashCode + Environment.NewLine);
            this.OutputManager.WriteOutput("Switch Matrix Size: " + where.SwitchMatrix.Ports.Count() + Environment.NewLine);
            this.OutputManager.WriteOutput("Switch Matrix Inputs " + where.SwitchMatrix.Inputs + Environment.NewLine);
            this.OutputManager.WriteOutput("Switch Matrix Outputs " + where.SwitchMatrix.Outputs + Environment.NewLine);
            this.OutputManager.WriteOutput("Switch Matrix Arcs " + where.SwitchMatrix.ArcCount + Environment.NewLine);
            this.OutputManager.WriteOutput("Clock Region " + (!string.IsNullOrEmpty(where.ClockRegion) ? where.ClockRegion : "unknown") + Environment.NewLine);

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The location string of the tile to be added to selection, e.g INT_X10Y24")]
        public String Location = "INT_X10Y24";
    }
}
