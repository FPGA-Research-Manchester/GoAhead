using System;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;

namespace GoAhead.Commands.BlockingShared.DriverConfig
{
    public abstract class ConfigureDriver : NetlistContainerCommand
    {
        public override void Undo()
        {
        }

        public static ConfigureDriver GetConfigureDriver(String location, int sliceNumber, String prefix)
        {
            ConfigureDriver result = null;
            if (FPGA.FPGA.Instance.Family.Equals(FPGA.FPGATypes.FPGAFamily.Virtex4))
            {
                result = new V4DriverConfiguration();
            }
            else if (FPGA.FPGA.Instance.Family.Equals(FPGA.FPGATypes.FPGAFamily.Virtex5))
            {
                result = new V5DriverConfiguration();
            }
            else if (FPGA.FPGA.Instance.Family.Equals(FPGA.FPGATypes.FPGAFamily.Virtex6))
            {
                result = new V6DriverConfiguration();
            }
            else if (FPGA.FPGA.Instance.Family.Equals(FPGA.FPGATypes.FPGAFamily.Kintex7))
            {
                result = new K7DriverConfiguration();
            }
            else if (FPGA.FPGA.Instance.Family.Equals(FPGA.FPGATypes.FPGAFamily.Spartan6))
            {
                result = new S6DriverConfiguration();
            }
            else
            {
                throw new NotImplementedException("CLB-Blocker not implemented for " + FPGA.FPGA.Instance.Family);
            }

            result.Location = location;
            result.Prefix = prefix;
            result.SliceNumber = sliceNumber;
            return result;
        }

        [Parameter(Comment = "The index of the slice the blocker will use")]
        public int SliceNumber = 0;

        [Parameter(Comment = "The location string  of the tile to block, e.g CLBLL_X2Y78")]
        public String Location = "";

        [Parameter(Comment = "The prefix for nets and ports")]
        public String Prefix = "RBB_Blocker";
    }

    public class S6DriverConfiguration : ConfigureDriver
    {
        protected override void DoCommandAction()
        {
            // configure slice
            if (!Regex.IsMatch(this.Location, "DUMMY"))
            {
                Tile clb = FPGA.FPGA.Instance.GetTile(this.Location);

                // export ports to pass drc
                CommandExecuter.Instance.Execute(new SetFocus(this.Location));
                CommandExecuter.Instance.Execute(new AddSlice(this.NetlistContainerName, this.SliceNumber));
                foreach (Port port in clb.Slices[this.SliceNumber].PortMapping.Ports)
                {
                    String portName = port.ToString();
                    if (Regex.IsMatch(port.ToString(), "CLK"))
                    {
                        CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_clk", this.SliceNumber, portName));
                    }
                    else if (Regex.IsMatch(port.ToString(), "[A-D](X|Q|)$"))
                    {
                        CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_out", this.SliceNumber, portName));
                    }
                    else if (Regex.IsMatch(port.ToString(), "[A-D][1-6]$"))
                    {
                        CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_in", this.SliceNumber, portName));
                    }
                    else
                    {
                    }
                }
                foreach (String lut in new String[] { "A", "B", "C", "D" })
                {
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFSRINIT", ":SRINIT0"));
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FF", "#FF"));
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFMUX", ":" + lut + "X"));
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "USED", ":0"));
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "6LUT", "#LUT:O6=A1+A2+A3+A4+A5+A6"));
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CLKINV", ":CLK"));
                    CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "SYNC_ATTR", ":SYNC"));
                }
            }
        }
    }

    public class V4DriverConfiguration : ConfigureDriver
    {
        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetTile(this.Location);

            // export ports to pass drc
            CommandExecuter.Instance.Execute(new SetFocus(this.Location));
            CommandExecuter.Instance.Execute(new AddSlice(this.NetlistContainerName, this.SliceNumber));
            foreach (Port port in clb.Slices[this.SliceNumber].PortMapping.Ports)
            {
                if (Regex.IsMatch(port.ToString(), "CLK"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_clk", this.SliceNumber, "CLK"));
                }
                else if (Regex.IsMatch(port.ToString(), "^(X|Y)(MUX|Q|B){0,1}_PINWIRE" + this.SliceNumber))
                {
                    String portName = Regex.Replace(port.Name, @"_PINWIRE\d+$", "");
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_out", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "(F|G)[1-4]_PINWIRE" + this.SliceNumber))
                {
                    String portName = Regex.Replace(port.Name, @"_PINWIRE\d+$", "");
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_in", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "B(X|Y)_PINWIRE" + this.SliceNumber))
                {
                    String portName = Regex.Replace(port.Name, @"_PINWIRE\d+$", "");
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_in", this.SliceNumber, portName));
                }
                else
                {
                }
            }

            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CLKINV", ":CLK"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "BXINV", ":BX"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "BYINV", ":BY"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "DXMUX", ":BX"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "DYMUX", ":BY"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "FFX", "#FF"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "FFY", "#FF"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "F", "#LUT:D=A1+A2+A3+A4"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "G", "#LUT:D=A1+A2+A3+A4"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "FXMUX", ":FXOR"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "GYMUX", ":GXOR"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XMUXUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YMUXUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XBUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YBUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CYMUXF", ":1"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CYMUXG", ":1"));

            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CLKINV", ":CLK"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "BXINV", ":BX"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "BYINV", ":BY"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "DXMUX", ":BX"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "DYMUX", ":BY"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "FFX", "#FF"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "FFY", "#FF"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "F", "#LUT:D=A1+A2+A3+A4"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "G", "#LUT:D=A1+A2+A3+A4"));
            if (clb.Slices[this.SliceNumber].SliceType.Equals("SLICEM"))
            {
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XBMUX", ":1"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YBMUX", ":1"));
            }
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "FXMUX", ":FXOR"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "GYMUX", ":GXOR"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XMUXUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YMUXUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "XBUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "YBUSED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CYMUXF", ":1"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CYMUXG", ":1"));
        }
    }

    public class V5DriverConfiguration : ConfigureDriver
    {
        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetTile(this.Location);

            // export ports to pass drc
            CommandExecuter.Instance.Execute(new SetFocus(this.Location));
            CommandExecuter.Instance.Execute(new AddSlice(this.NetlistContainerName, this.SliceNumber));
            foreach (Port port in clb.Slices[this.SliceNumber].PortMapping.Ports)
            {
                String portName = port.ToString();
                if (Regex.IsMatch(port.ToString(), "CLK"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_clk", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "[A-D](MUX|X|Q|)$"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_out", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "[A-D][1-6]$"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_in", this.SliceNumber, portName));
                }
                else
                {
                }
            }
            foreach (String lut in new String[] { "A", "B", "C", "D" })
            {
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FF", "#FF"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFMUX", ":" + lut + "X"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "OUTMUX", ":O6"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "USED", ":0"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "6LUT", "#LUT:O6=A1+A2+A3+A4+A5+A6"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CLKINV", ":CLK"));
            }
        }
    }

    public class V6DriverConfiguration : ConfigureDriver
    {
        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetTile(this.Location);

            // export ports to pass drc
            CommandExecuter.Instance.Execute(new SetFocus(this.Location));
            CommandExecuter.Instance.Execute(new AddSlice(this.NetlistContainerName, this.SliceNumber));
            foreach (Port port in clb.Slices[this.SliceNumber].PortMapping.Ports)
            {
                String portName = port.ToString();
                if (Regex.IsMatch(port.ToString(), "CLK"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_clk", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "[A-D](X|Q|)$"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_out", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "[A-D][1-6]$"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_in", this.SliceNumber, portName));
                }
                else
                {
                }
            }
            foreach (String lut in new String[] { "A", "B", "C", "D" })
            {
                //CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFSRINIT", ":SRINIT0"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FF", "#FF"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFMUX", ":" + lut + "X"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "USED", ":0"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "6LUT", "#LUT:O6=A1+A2+A3+A4+A5+A6"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CLKINV", ":CLK"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "SYNC_ATTR", ":SYNC"));
            }
        }
    }

    public class K7DriverConfiguration : ConfigureDriver
    {
        protected override void DoCommandAction()
        {
            Tile clb = FPGA.FPGA.Instance.GetTile(this.Location);

            // export ports to pass drc
            CommandExecuter.Instance.Execute(new SetFocus(this.Location));
            CommandExecuter.Instance.Execute(new AddSlice(this.NetlistContainerName, this.SliceNumber));
            foreach (Port port in clb.Slices[this.SliceNumber].PortMapping.Ports)
            {
                String portName = port.ToString();
                if (Regex.IsMatch(port.ToString(), "CLK"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_clk", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "[A-D](X|Q|)$"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_out", this.SliceNumber, portName));
                }
                else if (Regex.IsMatch(port.ToString(), "[A-D][1-6]$"))
                {
                    CommandExecuter.Instance.Execute(new AddPort(this.NetlistContainerName, this.Prefix + "blocker_in", this.SliceNumber, portName));
                }
                else
                {
                }
            }
            foreach (String lut in new String[] { "A", "B", "C", "D" })
            {
                //CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFSRINIT", ":SRINIT0"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FF", "#FF"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "FFMUX", ":" + lut + "X"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "USED", ":0"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, lut + "6LUT", "#LUT:O6=A1+A2+A3+A4+A5+A6"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "CLKINV", ":CLK"));
                CommandExecuter.Instance.Execute(new SetSliceAttribute(this.SliceNumber, "SYNC_ATTR", ":SYNC"));
            }
        }
    }
}