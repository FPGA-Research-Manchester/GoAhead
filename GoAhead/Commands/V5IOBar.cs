using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands
{
    class V5IOBar : Command
    {
        public V5IOBar(String startLocation, String dir, uint length, uint numberOfLines, String inPortName, String outPortName)
        {
            this.m_startLocation = startLocation;
            this.m_dir = Types.ParseDirectionFromString(dir);
            this.m_length = length;
            this.m_numberOfLines = numberOfLines;
            this.m_inPortName = inPortName;
            this.m_outPortName = outPortName;
        }

        public override void Do()
        {
            CommandExecuter.Instance.Execute(new SetFocus(this.m_startLocation));
            CommandExecuter.Instance.Execute(new AddPort("clk", 1, "L_CLK"));
            CommandExecuter.Instance.Execute(new AddSlice(1));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(1, "CLKINV", ":CLK"));

            if (this.m_dir.Equals(Types.Direction.East))
            {
                // A line
                if (this.m_numberOfLines > 0) { this.East_A(); }
                // B line
                if (this.m_numberOfLines > 1) { this.East_B(); }
                // C line
                if (this.m_numberOfLines > 2) { this.East_C(); }
                // D line
                if (this.m_numberOfLines > 3) { this.East_D(); }
            }
            else if (this.m_dir.Equals(Types.Direction.West))
            {
                // A line
                if (this.m_numberOfLines > 1) { this.West_A(); }
                // B line
                if (this.m_numberOfLines > 1) { this.West_B(); }
                // C line
                if (this.m_numberOfLines > 2) { this.West_C(); }
                // D line
                if (this.m_numberOfLines > 3) { this.West_D(); }
            }
            else
            {
                throw new ArgumentException("Direction " + this.m_dir + " for io bar no implemented");
            }

            // instantiate slice, configurations have been set in functions East[A-D] 
            CommandExecuter.Instance.Execute(new AddSlice(1));

            //this.Trace(this.m_numberOfLines + " IO bars run from " + this.m_startLocation + " in direction " + this.m_dir + " to " + FPGA.FPGA.Instance.Current);
        }

        private void East_A()
        {
            this.Start("A");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_AQ", "SITE_LOGIC_OUTS0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS0", "EN2BEG0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("EN2MID0", "ES2BEG0"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
                CommandExecuter.Instance.Execute(new AddPip("ES2MID0", "EN2BEG0"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("EN2MID0", "ES2BEG0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            CommandExecuter.Instance.Execute(new AddPip("ES2MID0", "IMUX_B1"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B1", "L_A4"));

            this.Sink("A", 4);
        }

        private void East_B()
        {
            this.Start("B");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_BQ", "SITE_LOGIC_OUTS1"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS1", "ES2BEG0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("ES2MID0", "EN2BEG0"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
                CommandExecuter.Instance.Execute(new AddPip("EN2MID0", "ES2BEG0"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("ES2MID0", "EN2BEG0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            CommandExecuter.Instance.Execute(new AddPip("EN2MID0", "IMUX_B36"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B36", "L_B6"));

            this.Sink("B", 6);
        }

        private void East_C()
        {
            this.Start("C");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_CQ", "SITE_LOGIC_OUTS2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS2", "ES2BEG1"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("ES2MID1", "EN2BEG1"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
                CommandExecuter.Instance.Execute(new AddPip("EN2MID1", "ES2BEG1"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("ES2MID1", "EN2BEG1"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            CommandExecuter.Instance.Execute(new AddPip("EN2MID1", "IMUX_B8"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B8", "L_C3"));

            this.Sink("C", 3);
        }

        private void East_D()
        {
            this.Start("D");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_DQ", "SITE_LOGIC_OUTS3"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS3", "EL2BEG2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("EL2MID2", "EN2BEG2"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
                CommandExecuter.Instance.Execute(new AddPip("EN2MID2", "EL2BEG2"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("EL2MID2", "EN2BEG2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "east"));
            CommandExecuter.Instance.Execute(new AddPip("EN2MID2", "IMUX_B46"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B46", "L_D4"));

            this.Sink("D", 4);
        }

        private void West_A()
        {
            this.Start("A");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_AQ", "SITE_LOGIC_OUTS0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS0", "WS2BEG0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("WS2MID0", "WN2BEG0"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
                CommandExecuter.Instance.Execute(new AddPip("WN2MID0", "WS2BEG0"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("WS2MID0", "WN2BEG0"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("WN2MID0", "IMUX_B0"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B0", "L_A6"));

            this.Sink("A", 6);
        }

        private void West_B()
        {
            this.Start("B");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_BQ", "SITE_LOGIC_OUTS1"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS1", "WS2BEG1"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("WS2MID1", "WN2BEG1"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
                CommandExecuter.Instance.Execute(new AddPip("WN2MID1", "WS2BEG1"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("WS2MID1", "WN2BEG1"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("WN2MID1", "IMUX_B38"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B38", "L_B5"));

            this.Sink("B", 5);
        }

        private void West_C()
        {
            this.Start("C");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_CQ", "SITE_LOGIC_OUTS2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS2", "WS2BEG2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("WS2MID2", "WN2BEG2"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
                CommandExecuter.Instance.Execute(new AddPip("WN2MID2", "WS2BEG2"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("WS2MID2", "WN2BEG2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("WN2MID2", "IMUX_B10"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B10", "L_C4"));

            this.Sink("C", 4);
        }

        private void West_D()
        {
            this.Start("D");

            // start
            CommandExecuter.Instance.Execute(new AddPip("L_DQ", "SITE_LOGIC_OUTS3"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("LOGIC_OUTS3", "WN2BEG2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));

            // inner
            for (uint i = 0; i < this.m_length; ++i)
            {
                CommandExecuter.Instance.Execute(new AddPip("WN2MID2", "WS2BEG2"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
                CommandExecuter.Instance.Execute(new AddPip("WS2MID2", "WN2BEG2"));
                CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            }

            // sink
            CommandExecuter.Instance.Execute(new AddPip("WN2MID2", "WS2BEG2"));
            CommandExecuter.Instance.Execute(new GotoNext("INT_X", "west"));
            CommandExecuter.Instance.Execute(new AddPip("WS2MID2", "IMUX_B45"));
            CommandExecuter.Instance.Execute(new GotoNext("CLB", "east"));
            CommandExecuter.Instance.Execute(new AddPip("SITE_IMUX_B45", "L_D5"));

            this.Sink("D", 5);
        }

        private void Start(String port)
        {
            // static connection
            CommandExecuter.Instance.Execute(new AddNet());
            CommandExecuter.Instance.Execute(new SetFocus(this.m_startLocation));
            CommandExecuter.Instance.Execute(new AddPort(this.m_inPortName, 1, "L_" + port + "X"));

            CommandExecuter.Instance.Execute(new SetSliceAttribute(1, port + "FF", "#FF"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(1, port + "FFMUX", ":" + port + "X"));
        }

        private void Sink(String port, uint lutPortIndex)
        {
            CommandExecuter.Instance.Execute(new AddPort(this.m_outPortName, 1, "L_" + port));

            CommandExecuter.Instance.Execute(new SetSliceAttribute(1, port + "USED", ":0"));
            CommandExecuter.Instance.Execute(new SetSliceAttribute(1, port + "6LUT", "#LUT:O6=A" + lutPortIndex));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.GetCommandName() + " " + this.m_startLocation + " " + m_dir + " " + this.m_length + " " + this.m_numberOfLines + " " + this.m_inPortName + " " + this.m_outPortName;        
        }

        private readonly String m_startLocation;
        private readonly Types.Direction m_dir;
        private readonly uint m_length;
        private readonly uint m_numberOfLines;
        private readonly String m_inPortName;
        private readonly String m_outPortName;
    }
}
