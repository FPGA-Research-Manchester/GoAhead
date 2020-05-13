using System;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;

namespace GoAhead.Code
{
    public class FEScript
    {
        public FEScript(string macroName)
        {
            m_header.AppendLine("setattr main edit-mode read-write");
            m_header.AppendLine("open hm " + macroName);
            m_header.AppendLine("unselect -all");

            m_trailor.AppendLine("unselect -all");
            m_trailor.AppendLine("save -w hm " + macroName);
            m_trailor.AppendLine("exit");
        }

        public void AddCreatePinCommand(XDLMacroPort xdlPort)
        {
            //#  net "bus_enable_read<0>" ,
            //#    inpin "SLICE_X144Y103" F1 ,
            //#    ;
            //unselect -all
            //select pin SLICE_X144Y103.F1
            //delete
            //select net bus_enable_read<0>
            //delete
            //select pin SLICE_X144Y103.F1
            //add extpin
            //post attr pin SLICE_X144Y103.F1
            //setattr pin SLICE_X144Y103.F1 external_name bus_enable_read<0>

            string pin = xdlPort.Slice.SliceName + "." + xdlPort.Port.ToString();
            pin = Regex.Replace(pin, "L_", "");
            pin = Regex.Replace(pin, "M_", "");

            //m_addPinCmds.AppendLine("#command for " + pin);
            m_addPinCmds.AppendLine("unselect -all");
            m_addPinCmds.AppendLine("select pin '" + pin + "'");
            m_addPinCmds.AppendLine("add extpin");
            m_addPinCmds.AppendLine("post attr pin " + pin);

            m_addPinCmds.AppendLine("setattr pin " + pin + " external_name " + xdlPort.PortName);
            m_addPinCmds.AppendLine("unpost pin \"" + pin + "\"");
            m_addPinCmds.AppendLine("");

            m_deleteCmds.AppendLine("unselect -all");
            m_deleteCmds.AppendLine("select net " + XDLFile.GetDummyNetName(xdlPort) + "\"");
            m_deleteCmds.AppendLine("delete");
        }

        public override string ToString()
        {
            StringBuilder scriptCode = new StringBuilder();
            scriptCode.Append(m_header);
            scriptCode.AppendLine("");
            scriptCode.Append(m_deleteCmds);
            scriptCode.AppendLine("");
            scriptCode.Append(m_addPinCmds);
            scriptCode.Append("");
            scriptCode.Append(m_trailor);
            return scriptCode.ToString();
        }

        private StringBuilder m_header = new StringBuilder();
        private StringBuilder m_deleteCmds = new StringBuilder();
        private StringBuilder m_addPinCmds = new StringBuilder();
        private StringBuilder m_trailor = new StringBuilder();
    }
}