using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.TCL.Procs.cshelp
{
    class CsHelpLayoutPanel : TableLayoutPanel
    {
        public object Scope = null;
        public string ScopeStr = "";
        private List<Type> RelevantEnums = null;

        protected override Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }

        public void PopulateGrids(bool abbreviated, string scopeStr)
        {
            ScopeStr = scopeStr;

            IOrderedEnumerable<DataGridView> grids = Controls.OfType<DataGridView>().OrderByDescending(e => e.Name);

            foreach (DataGridView grid in grids)
            {
                PopulateGrid(grid, abbreviated);
            }
        }

        private void PopulateGrid(DataGridView grid, bool abbreviated)
        {
            grid.Rows.Clear();

            switch(grid.Name)
            {
                case CsHelp_GUI.CSH_SINGLETONS:
                    PopulateGrid_Singletons(grid);
                    break;
                case CsHelp_GUI.CSH_ENUMS:
                    PopulateGrid_Enums(grid);
                    break;
                case CsHelp_GUI.CSH_PROPERTIES:
                    PopulateGrid_Properties(grid, abbreviated, BindingFlags.Instance | BindingFlags.Public);
                    break;
                case CsHelp_GUI.CSH_PROPERTIES_STATIC:
                    PopulateGrid_Properties(grid, abbreviated, BindingFlags.Static | BindingFlags.Public);
                    break;
                case CsHelp_GUI.CSH_METHODS:
                    PopulateGrid_Methods(grid, abbreviated, BindingFlags.Instance | BindingFlags.Public);
                    break;
                case CsHelp_GUI.CSH_METHODS_STATIC:
                    PopulateGrid_Methods(grid, abbreviated, BindingFlags.Static | BindingFlags.Public);
                    break;
                default:
                    break;
            }

            grid.Sort(grid.Columns[0], ListSortDirection.Ascending);
            grid.ClearSelection();
        }

        private void PopulateGrid_Singletons(DataGridView grid)
        {
            foreach (object s in TclAPI.Singletons)
            {
                string[] row = new string[]
                {
                    s.GetType().Name,
                    s.GetType().ToString()
                };
                grid.Rows.Add(row);
            }
        }

        private void PopulateGrid_Enums(DataGridView grid)
        {
            List<Type> enumsScope = RelevantEnums ?? TclAPI.Enums;

            foreach (Type e in enumsScope)
            {
                string enumValues = "";
                foreach (var val in Enum.GetNames(e)) enumValues += "[" + val + "] ";

                string[] row = new string[]
                {
                    e.Name,
                    e.ToString(),
                    enumValues
                };
                grid.Rows.Add(row);
            }
        }

        private void PopulateGrid_Properties(DataGridView grid, bool abbreviated, BindingFlags bindingFlags)
        {
            Type objType = null;

            if (Scope == null)
            {
                objType = TclAPI.GetCsTypeFromName(ScopeStr);
                if (objType == null) return;
            }
            else
            {
                objType = Scope.GetType();
            }

            PropertyInfo[] properties = objType.GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                object value = property.GetValue(Scope, null);
                string[] row = new string[]
                {
                    property.Name,
                    value == null ? "null" : value.ToString(),
                    abbreviated ? property.PropertyType.Name : property.PropertyType.ToString()
                };
                grid.Rows.Add(row);
            }
        }

        private void PopulateGrid_Methods(DataGridView grid, bool abbreviated, BindingFlags bindingFlags)
        {
            Type objType = null;

            if (Scope == null)
            {
                objType = TclAPI.GetCsTypeFromName(ScopeStr);
                if (objType == null) return;
            }
            else
            {
                objType = Scope.GetType();
            }

            RelevantEnums = new List<Type>();

            MethodInfo[] methods = objType.GetMethods(bindingFlags);
            foreach (var method in methods)
            {
                if (MethodRows == null) MethodRows = new Dictionary<DataGridViewRow, MethodInfo>();

                string parameters = "";
                ParameterInfo[] p = method.GetParameters();
                for (int i = 0; i < p.Length; i++)
                {
                    // Record relevant enums
                    if(p[i].ParameterType.IsEnum)
                    {
                        RelevantEnums.Add(p[i].ParameterType);
                    }

                    parameters += "[" + 
                        (abbreviated ? p[i].ParameterType.Name : p[i].ParameterType.ToString())
                        + " " + p[i].Name + "]";
                    if (i + 1 < p.Length) parameters += ", ";
                }

                string[] row = new string[]
                {
                    method.Name,
                    parameters,
                    abbreviated ? method.ReturnType.Name : method.ReturnType.ToString()
                };

                int ind = grid.Rows.Add(row);
                MethodRows.Add(grid.Rows[ind], method);
            }
        }

        private Dictionary<DataGridViewRow, MethodInfo> MethodRows = null;
        public MethodInfo GetMethodInfoAtRow(DataGridViewRow row)
        {
            if (MethodRows != null && MethodRows.ContainsKey(row))
                return MethodRows[row];

            return null;
        }
    }
}
