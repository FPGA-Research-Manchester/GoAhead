using CsvHelper.Configuration;
using CsvHelper;
using GoAhead.Commands;
using GoAhead.GUI.Macros.BusInterface;
using System;
using System.IO;

public class InterfaceConstraint
{
    public string SignalName { get; set; }
    public int BusWidth { get; set; }
    public string Direction { get; set; }
    public int StartIndex { get; set; }

}

public sealed class InterfaceConstraintMap : CsvClassMap<InterfaceConstraint>
{
    public InterfaceConstraintMap()
    {
        Map(m => m.SignalName).Name("Signal_name");
        Map(m => m.BusWidth).Name("Bus_width");
        Map(m => m.Direction).Name("Direction");
        Map(m => m.StartIndex).Name("LSB");
    }
}

