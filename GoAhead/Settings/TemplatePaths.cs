using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Settings
{
    class TemplatePaths
    {
        public static String GetRAMInstanceConfigTemplate()
        { 
            // todo if fpga.family ...
            return TemplatePaths.m_ramInstanceConfigTemplate;
        }

        public static String GetCLBBlockConfigTemplate()
        {
            return @"c:\ReCoBus\templates\v5_slicel.txt";
        }

        private static String m_ramInstanceConfigTemplate = @"E:\templates\v5_slicem.txt";

        public static String GetRAMBlockingTemplate()
        {
            return "";
        }
    }
}
