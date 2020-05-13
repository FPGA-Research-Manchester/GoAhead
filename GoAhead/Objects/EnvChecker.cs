using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Objects
{
    public class EnvChecker
    {
        /// <summary>
        /// Check XILINX and PATH
        /// </summary>
        public static bool CheckEnv(out StringBuilder errorList)
        {
            errorList = new StringBuilder();

            bool checkPassed = true;

            // XILINX should be set
            string goaheadHome = Environment.GetEnvironmentVariable("GOAHEAD_HOME");
            if (goaheadHome == null)
            {
                errorList.AppendLine("Warning: GOAHEAD_HOME not set");
                checkPassed = false;
            }
            // XILINX should be set
            string xilinx = Environment.GetEnvironmentVariable("XILINX");
            if (xilinx == null)
            {
                errorList.AppendLine("Warning: XILINX not set");
                checkPassed = false;
            }
            // PATH should contain XILINX
            string path = Environment.GetEnvironmentVariable("PATH");
            if (path == null)
            {
                errorList.AppendLine("Warning: PATH not set (Windows only)");
                checkPassed = false;
            }
            else if (!Regex.IsMatch(path, "xilinx", RegexOptions.IgnoreCase))
            {
                errorList.AppendLine("Warning: PATH does not contain XILINX (Windows only)"); ;
                checkPassed = false;
            }

            return checkPassed;
        }
    }
}