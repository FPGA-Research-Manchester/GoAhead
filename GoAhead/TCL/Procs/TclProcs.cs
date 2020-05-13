using GoAhead.Commands;
using GoAhead.TCL.Procs.cshelp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.TCL
{
    public static class TclProcs
    {
        private static string NL => Environment.NewLine;

        public static unsafe int ExecuteGOACommand(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            string command = TclAPI.GetCsString(argv[0]);
            for (int i = 1; i < objc; i++) command += " " + TclAPI.GetCsString(argv[i]);
            command += ";";

            CommandStringParser parser = new CommandStringParser(command);
            foreach (string cmdstr in parser.Parse())
            {
                bool valid = parser.ParseCommand(cmdstr, true, out Command cmd, out string errorDescr);
                if (valid)
                {
                    CommandExecuter.Instance.Execute(cmd);
                    Objects.CommandStackManager.Instance.Execute();
                    if (Program.mainInterpreter.context != null) Program.mainInterpreter.context.Invalidate();
                }
                else
                {
                    MessageBox.Show("Could not parse command " + Environment.NewLine + cmdstr + Environment.NewLine + "Error: " + errorDescr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }
            }

            return 0;
        }

        public static unsafe int Cs(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            Program.mainInterpreter.ErrorMessage = "";

            if (objc < 3)
            {
                Program.mainInterpreter.ErrorMessage =
                    "Not enough parameters provided for the command." + NL +
                    "Correct command format: 'cs $object $member $parameters'." + NL +
                    "For more info type 'cshelp'.";
                return TclInterpreter.TCL_ERROR;
            }

            Type objType = null;
            object obj = TclAPI.FindCsObject(argv[1]);
            if (obj == null)
            {
                string str = TclAPI.GetCsString(argv[1]);

                objType = TclAPI.GetCsTypeFromName(str);

                if (objType == null)
                {
                    Program.mainInterpreter.ErrorMessage =
                        "The parameter '" + str + "' could not be linked to any C# object or type.";
                    return TclInterpreter.TCL_ERROR;
                }                
            }
            else
            {
                objType = obj.GetType();
            }

            object result = null;

            // Required data - can be Method or Property
            string requiredMember = TclAPI.GetCsString(argv[2]);
            IEnumerable<MemberInfo> candidates =
                objType.GetMembers().Where(m => m.Name == requiredMember && TclAPI.IsAPICompatibleMember(m));

            // Method
            if (candidates.FirstOrDefault() is MethodInfo)
            {
                int totalParams = objc - 3;

                IEnumerable<MethodInfo> matchedMethods = candidates.Where(m => m.MemberType == MemberTypes.Method)
                    .Cast<MethodInfo>().Where(m => m.GetParameters().Count() == totalParams);

                // Convert the tcl parameters to cs objects
                object[] parameters = new object[totalParams];
                for (int i = 0; i < totalParams; i++)
                {
                    object csObj = TclAPI.Tcl2Cs(argv[3 + i]);
                    if(csObj == null)
                    {
                        string args = "Parameters:" + Environment.NewLine;
                        for (int j = 0; j < objc; j++) args += j + ": " + TclAPI.GetCsString(argv[j]) + Environment.NewLine;
                        throw new ArgumentException("Invalid parameter provided at index: " + (3+i).ToString() + Environment.NewLine + args);
                    }
                    parameters[i] = csObj;
                }

                // Try the candidate methods until one works
                bool success = false;
                foreach (MethodInfo method in matchedMethods)
                {
                    try
                    {
                        result = method.Invoke(obj, parameters);
                        /*if(result != null && !result.GetType().Equals(method.ReturnType))
                            Console.WriteLine("Type Difference");*/
                        success = true;
                        break;
                    }
                    catch (Exception) { }
                }

                // If invoked method was void
                if (success)
                {
                    if (result == null)
                    {
                        TclDLL.Tcl_ResetResult(interp);
                        return 0;
                    }
                }
                else
                {
                    Program.mainInterpreter.ErrorMessage += 
                        "No method overload could be executed for the method " + requiredMember + NL;
                }
            }
            // Property
            else if (candidates.Count() == 1 && candidates.FirstOrDefault() is PropertyInfo p)
            {
                result = p.GetValue(obj, null);
            }

            TclDLL.Tcl_SetObjResult(interp, TclAPI.Cs2Tcl(result));

            if (result == null)
            {
                Program.mainInterpreter.ErrorMessage +=
                    "'" + requiredMember + "' returned null.";
                return TclInterpreter.TCL_WARNING;
            }

            return 0;
        }

        public static unsafe int CsList(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            if (objc < 2) return 1;

            object targetObj = null;

            // If only converting an object
            if (objc == 2)
            {
                targetObj = TclAPI.FindCsObject(argv[1]);
            }
            // If invoking cs first
            else
            {
                int r = Cs(clientData, interp, objc, argv);
                if (r != 0) return r;

                targetObj = TclAPI.FindCsObject(TclDLL.Tcl_GetObjResult(interp));
            }

            if (targetObj == null)
            {
                return 0;
            }

            IEnumerable<object> e = null;
            try
            {
                e = ((IEnumerable)targetObj).Cast<object>();
            }
            catch(Exception)
            {
                e = null;
            }
            if (e == null) return 1;

            IntPtr list = TclAPI.GetTclList(e);
            TclDLL.Tcl_SetObjResult(interp, list);

            return 0;
        }

        public static unsafe int CsHelp(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            if(objc == 1)
            {
                CsHelp_GUI.CreateDialog();
                return 0;
            }

            if (objc != 2) return 1;

            string arg = TclAPI.GetCsString(argv[1]).ToLower();

            string scopeStr = "";
            object scopeObj = null;
            List<string> grids = new List<string>();

            if (arg.Equals(CsHelp_GUI.CSH_SINGLETONS))
            {
                grids.Add(CsHelp_GUI.CSH_SINGLETONS);
                scopeStr = "Singletons";
            }
            else if (arg.Equals(CsHelp_GUI.CSH_ENUMS))
            {
                grids.Add(CsHelp_GUI.CSH_ENUMS);
                scopeStr = "Enums";
            }
            else
            {
                // Try find object
                scopeObj = TclAPI.FindCsObject(argv[1]);
                if (scopeObj != null)
                {
                    grids.Add(CsHelp_GUI.CSH_PROPERTIES);
                    grids.Add(CsHelp_GUI.CSH_METHODS);
                    grids.Add(CsHelp_GUI.CSH_ENUMS);
                    grids.Add(CsHelp_GUI.CSH_PROPERTIES_STATIC);
                    grids.Add(CsHelp_GUI.CSH_METHODS_STATIC);
                    scopeStr = scopeObj.GetType().Name + " - " + scopeObj.GetType().ToString();
                }
                // Try find type
                else
                {
                    Type objType = null;
                    string str = TclAPI.GetCsString(argv[1]);

                    objType = TclAPI.GetCsTypeFromName(str);

                    if (objType != null)
                    {
                        grids.Add(CsHelp_GUI.CSH_PROPERTIES_STATIC);
                        grids.Add(CsHelp_GUI.CSH_METHODS_STATIC);
                        scopeStr = objType.FullName;
                    }
                }
            }

            CsHelp_GUI.CreateDialog(grids, scopeStr, scopeObj);

            return 0;
        }

        public static unsafe int Clear(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            if (objc == 1)
            {
                GUI.ConsoleCtrl.TCLTerminal_output.Text = "";
                return 0;
            }

            return 1;
        }

        public static unsafe int ClearContext(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            if (objc == 1)
            {
                TclAPI.ResetContext();
                GUI.ConsoleCtrl.TCLTerminal_output.AppendText("'clearcontext' was executed manually." + NL +
                    "Warning: your existing TCL variables pointing to C# objects will no longer hold a correct reference." + NL, 
                    System.Drawing.Color.OrangeRed); ;
                return 0;
            }

            return 1;
        }


        public static unsafe int Test(IntPtr clientData, IntPtr interp, int objc, IntPtr* argv)
        {
            TclAPI.Tcl2Cs(argv[1]);

            return 0;
        }

    }
}
