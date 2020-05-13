using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands
{
    class IOBar : Command
    {
        public IOBar(String startLocation, String dir, uint length, uint numberOfLines, 
                String logicTemplate, String memoryTemplate, String inPortName, String outPortName)
        {
            this.m_startLocation = startLocation;
            this.m_dir = Types.ParseFromString(dir);
            this.m_length = length;
            this.m_numberOfLines = numberOfLines;
            this.m_logicTemplate= logicTemplate;
            this.m_memoryTemplate = memoryTemplate;
            this.m_inPortName = inPortName;
            this.m_outPortName = outPortName;
        }

        public override void Do()
        {
            CommandExecuter.Instance.Execute(new SetFocus(this.m_startLocation));
            CommandExecuter.Instance.Execute(new AddPort("clk<0>", 1, "L_CLK"));
            CommandExecuter.Instance.Execute(new AddSlice(1, this.m_logicTemplate, this.m_memoryTemplate));

            uint portIndex = 0;
            if (this.m_dir.Equals(Types.Direction.East))
            {
                // A line
                if(this.m_numberOfLines > 0)
                {
                    this.East_A(portIndex);

                    // next line uses next index
                    portIndex++;
                }
                // B line
                if (this.m_numberOfLines > 1)
                {
                    this.East_B(portIndex);

                    // next line uses next index
                    portIndex++;
                }
                // C line
                if (this.m_numberOfLines > 2)
                {
                    this.East_C(portIndex);

                    // next line uses next index
                    portIndex++;
                }

                // D line
                if (this.m_numberOfLines > 3)
                {
                    this.East_D(portIndex);

                    // next line uses next index
                    portIndex++;
                }
            }
            else if (this.m_dir.Equals(Types.Direction.West))
            {
            }
            else
            {
                throw new ArgumentException("Direction " + this.m_dir + " not implemented");
            }

            CommandExecuter.Instance.Execute(new AddSlice(1, this.m_logicTemplate, this.m_memoryTemplate));

            this.Trace(this.m_numberOfLines + " IO bars run from " + this.m_startLocation + " in direction " + this.m_dir + " to " + FPGA.FPGA.Instance.Current);
        }

        private void East_A(uint portIndex)
        {
            this.Start(portIndex, "A");

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

            this.Sink(portIndex, "A");
        }

        private void East_B(uint portIndex)
        {
            this.Start(portIndex, "B");

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

            this.Sink(portIndex, "B");   
        }

        private void East_C(uint portIndex)
        {
            Start(portIndex, "C");

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

            this.Sink(portIndex, "C");
        }

        private void East_D(uint portIndex)
        {
            Start(portIndex, "D");

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

            this.Sink(portIndex, "D");
        }

        private void Start(uint portIndex, String port)
        {
            // static connection
            CommandExecuter.Instance.Execute(new AddNet());
            CommandExecuter.Instance.Execute(new SetFocus(this.m_startLocation));
            CommandExecuter.Instance.Execute(new AddPort(this.m_inPortName + "<" + portIndex + ">", 1, "L_" + port + "X"));
        }

        private void Sink(uint portIndex, String port)
        {
            CommandExecuter.Instance.Execute(new AddPort(this.m_outPortName + "<" + portIndex + ">", 1, "L_" + port));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.GetCommandName() + " " + this.m_startLocation + " " + this.m_dir + " "
                                         + this.m_length + " " + this.m_numberOfLines + " "
                                         + this.m_logicTemplate + " " + this.m_memoryTemplate + " "
                                         + this.m_inPortName + " " + this.m_outPortName;        
        }

        private readonly String m_startLocation;
        private readonly Types.Direction m_dir;
        private readonly uint m_length;
        private readonly uint m_numberOfLines;
        private readonly String m_logicTemplate;
        private readonly String m_memoryTemplate;
        private readonly String m_inPortName;
        private readonly String m_outPortName;
    }
}
