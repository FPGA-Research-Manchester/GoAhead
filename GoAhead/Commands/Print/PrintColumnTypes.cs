using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.Identifier;
using GoAhead.FPGA;

namespace GoAhead.Commands
{
    class PrintColumnTypes : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);

            IEnumerable<string> clockRegions = FPGA.FPGA.Instance.GetAllTiles().Select(t => t.ClockRegion).Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(s => s);

            OutputManager.WriteOutput("# clock region wise resource report for " + FPGA.FPGA.Instance.DeviceName);
            OutputManager.WriteOutput("# the following clock regions will be reported " + string.Join(",", clockRegions));
            OutputManager.WriteOutput("# ");

            List<SetColumnTypeNames> addCommandsForUnknownTypes = new List<SetColumnTypeNames>();

            foreach (string clockRegion in clockRegions)
            {
                // get upper row
                int minX = FPGA.FPGA.Instance.GetAllTiles().Where(t => t.ClockRegion.Equals(clockRegion)).Select(t => t.TileKey.X).Min();
                int maxX = FPGA.FPGA.Instance.GetAllTiles().Where(t => t.ClockRegion.Equals(clockRegion)).Select(t => t.TileKey.X).Max();
                int minY = FPGA.FPGA.Instance.GetAllTiles().Where(t => t.ClockRegion.Equals(clockRegion)).Select(t => t.TileKey.Y).Min();
                int maxY = FPGA.FPGA.Instance.GetAllTiles().Where(t => t.ClockRegion.Equals(clockRegion)).Select(t => t.TileKey.Y).Max();
                int tileCount = FPGA.FPGA.Instance.GetAllTiles().Where(t => t.ClockRegion.Equals(clockRegion)).Count();

                OutputManager.WriteOutput("########################################################################################## ");
                OutputManager.WriteOutput("# report section for clock region " + clockRegion + " with " + tileCount + " tiles");
                OutputManager.WriteOutput("# tiles contained in this clock region: " + string.Join(",", FPGA.FPGA.Instance.GetAllTiles().Select(t => t.Location))); 

                for (int x = minX; x <= maxX; x++)
                {
                    string resources = "";
                    for (int y = minY; y <= maxY; y++)
                    {
                        Tile t = FPGA.FPGA.Instance.GetTile(x, y);
                        foreach (Slice s in t.Slices)
                        {
                            resources += s.SliceType + ",";
                        }
                    }
                    if (resources.EndsWith(","))
                    {
                        resources = resources.Remove(resources.Length - 1, 1);
                    }
                    SetColumnTypeNames addCmd = null;
                    string typeName = Objects.ColumnTypeNameManager.Instance.GetColumnTypeNameByResource(resources, out addCmd);
                    if (addCmd != null)
                    {
                        addCommandsForUnknownTypes.Add(addCmd);
                    }
                    OutputManager.WriteOutput("column=" + x + ",clock_region=" + clockRegion + ",type=" + typeName + ",resources=" + resources);
                }
            }

            OutputManager.WriteOutput("########################################################################################## ");
            OutputManager.WriteOutput("# for the columns with resource type unknown ");
            OutputManager.WriteOutput("# you might use the following commands in init.goa ");
            OutputManager.WriteOutput("# to provide a meaningful type name for that resource");
            foreach (SetColumnTypeNames cmd in addCommandsForUnknownTypes)
            {
                OutputManager.WriteOutput("use " + cmd.ToString());
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
