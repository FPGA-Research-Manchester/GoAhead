using System;
using System.Collections.Generic;
using GoAhead;
using GoAhead.Commands;
using GoAhead.Commands.Data;
using GoAhead.FPGA;

namespace SimpleScripts
{
    public class SampleScript : ScriptingInterface.IGoAheadScript
    {
        public int RunScript()
        {
            Console.WriteLine("start");

            CommandExecuter.Instance.AddHook(new ConsoleCommandHook());
            CommandExecuter.Instance.AddHook(new PrintOutputHook());

            OpenBinFPGA loadCmd = new OpenBinFPGA();
            loadCmd.FileName = "C:/Users/CFB/Dropbox/GoAhead/Devices/xc7a35tcpg236.binFPGA";
            CommandExecuter.Instance.Execute(loadCmd);

            string result = "";
            List<string> types = new List<string>();
            for (int x = 0; x < FPGA.Instance.MaxX; x++)
            {
                string type = "";
                for (int y = 0; y < FPGA.Instance.MaxY; y++)
                {
                    Tile t = FPGA.Instance.GetTile(x, y);
                    foreach (Slice s in t.Slices)
                    {
                        type += s.SliceType + ",";
                    }
                }
                if (!types.Contains(type))
                {
                    types.Add(type);
                }
                result += types.IndexOf(type).ToString() + (x == FPGA.Instance.MaxX - 1 ? "" : ",");
            }

            Console.WriteLine("# column types for " + FPGA.Instance.DeviceName);
            Console.WriteLine("# the following line constains comma seperated numbers. each number codes a particular column type");
            Console.WriteLine("types = " + result);
            Console.WriteLine("");
            Console.WriteLine("# what the numbers stand for");
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine("# " + i + " stands for " + types[i]);
            }

            Console.WriteLine("ende");
            return 0;
        }
    }
}