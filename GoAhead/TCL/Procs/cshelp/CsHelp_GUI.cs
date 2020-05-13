using GoAhead.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.TCL.Procs.cshelp
{
    public static class CsHelp_GUI
    {
        public static readonly Color color_extraTcl = Color.Goldenrod;
        public static readonly Color color_standardTcl = Color.DarkGoldenrod;

        public const string CSH_SINGLETONS = "singletons";
        public const string CSH_ENUMS = "enums";
        public const string CSH_PROPERTIES = "properties";
        public const string CSH_PROPERTIES_STATIC = "properties_static";
        public const string CSH_METHODS = "methods";
        public const string CSH_METHODS_STATIC = "methods_static";

        private static DataGridView CreateGrid(string[] columnNames)
        {
            int columns = columnNames.Length;

            DataGridView grid = new DataGridView
            {
                ColumnCount = columns,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.None,
                AutoSize = true,
                ReadOnly = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Margin = new Padding(0,0,0,0)
            };
            grid.GotFocus += Grid_GotFocus;
            grid.MouseClick += Grid_MouseClick;

            for (int i = 0; i < columns; i++)
            {
                grid.Columns[i].Name = columnNames[i];
                grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            return grid;
        }

        public static void CreateDialog()
        {
            CsHelpForm prompt = new CsHelpForm
            {
                Width = 800,
                Height = 600,
                Text = "CS Help"
            };
            prompt.ParameterlessMode();

            CsHelpLayoutPanel panel = new CsHelpLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Margin = new Padding(0, 0, 0, 0),
                Name = "cshelppanel"
            };
            prompt.AddControl(panel);

            RichTextBox txtBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ReadOnly = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                Margin = new Padding(5, 5, 5, 5)
            };

            string NL = "" + Environment.NewLine;

            txtBox.AppendText("TCL version 8.6" + NL);
            txtBox.AppendText(NL + "-------------------------------------------------------------------" + NL + NL + NL);
            txtBox.AppendText("API Commands" + NL + NL);

            txtBox.AppendText("cshelp", color_extraTcl);
            txtBox.AppendText(" singletons" + NL);
            txtBox.AppendText("Opens a list of all singleton objects that exist in GoAhead." + NL);
            txtBox.AppendText("cshelp", color_extraTcl);
            txtBox.AppendText(" enums" + NL);
            txtBox.AppendText("Opens a list of all enums that exist in GoAhead." + NL);
            txtBox.AppendText("cshelp", color_extraTcl);
            txtBox.AppendText(" $object" + NL);
            txtBox.AppendText("Opens a list of all methods and properties that can be executed on 'object', as well as the static members of its type." + NL);
            txtBox.AppendText("'object' can either be the name of a singleton or a GoAhead object in the API's TCL form." + NL);
            txtBox.AppendText("cshelp", color_extraTcl);
            txtBox.AppendText(" $typeName" + NL);
            txtBox.AppendText("Opens a list of all static methods and properties that can be executed from the type 'typeName'." + NL);
            txtBox.AppendText("'typeName' should be the full name of a C# type." + NL + NL);

            txtBox.AppendText("cs", color_extraTcl);
            txtBox.AppendText(" $object $member $parameters" + NL);
            txtBox.AppendText("Used to access the property or method 'member' of the object 'object'." + NL);
            txtBox.AppendText("'object' can either be the name of a singleton or a GoAhead object in the API's TCL form." + NL);
            txtBox.AppendText("If 'member' is a method which requires parameters then we need to supply 'parameters' where each parameter must be prefixed by a type identifier symbol. " +
                "The symbols are automatically supplied when using the 'Right Click -> Send to terminal' functionality on the method in the cshelp window." + NL);
            txtBox.AppendText("cs", color_extraTcl);
            txtBox.AppendText(" $typeName $member $parameters" + NL);
            txtBox.AppendText("Used to access the static property or method 'member' of the type 'typeName'." + NL);
            txtBox.AppendText("'typeName' should be the full name of a C# type." + NL + NL);

            txtBox.AppendText("cslist", color_extraTcl);
            txtBox.AppendText(" $object" + NL);
            txtBox.AppendText("Converts 'object' into a TCL list if a conversion is possible." + NL);
            txtBox.AppendText("'object' is an object in the API's TCL form." + NL);
            txtBox.AppendText("cslist", color_extraTcl);
            txtBox.AppendText(" $object $member $parameters" + NL);
            txtBox.AppendText("First executes the command 'cs $object $member $parameters' and then 'cslist $result' " +
                "where 'result' is the result of the cs command." + NL + NL);

            txtBox.AppendText("clear" + NL, color_extraTcl);
            txtBox.AppendText("Clears the TCL console." + NL + NL);

            txtBox.AppendText("clearcontext" + NL, color_extraTcl);
            txtBox.AppendText("Warning: Use with caution." + NL);
            txtBox.AppendText("Clears the TCL context. This clears all the current TCL to C# object references in the context. " +
                "This is automatically called during OpenBinFPGA." + NL + NL);

            txtBox.AppendText("Every GOA command is also a TCL command." + NL + NL);

            txtBox.AppendText(NL + "-------------------------------------------------------------------" + NL + NL + NL);

            txtBox.AppendText("Standard TCL commands:" + NL + NL);

            foreach (string cmd in CommandsList.Commands)
                txtBox.AppendText(cmd + NL, color_standardTcl);

            txtBox.SelectionStart = 0;
            txtBox.ScrollToCaret();
            panel.Controls.Add(txtBox);

            prompt.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="scope">(keyword, ((abbreviated, row[])))</param>
        public static void CreateDialog(List<string> grids, string scopeStr, object scope)
        {
            CsHelpForm prompt = new CsHelpForm
            {
                Width = 800,
                Height = 600,
                Text = "CS Help"
            };
            prompt.SetScopeLabel(scopeStr);
            if (scope == null) prompt.DeactivateObjectElements();

            CsHelpLayoutPanel panel = new CsHelpLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Margin = new Padding(0,0,0,0),
                Name = "cshelppanel",
                Scope = scope
            };
            prompt.AddControl(panel);

            // Singletons
            if (grids.Contains(CSH_SINGLETONS))
            {
                string[] columns = new string[]
                {
                    "Singleton",
                    "Full Name"
                };
                DataGridView grid = CreateGrid(columns);
                grid.Name = CSH_SINGLETONS;

                panel.Controls.Add(grid);
            }

            // Properties
            if (grids.Contains(CSH_PROPERTIES))
            {
                string[] columns = new string[]
                {
                    "Property",
                    "Value",
                    "Type"
                };
                DataGridView grid = CreateGrid(columns);
                grid.Name = CSH_PROPERTIES;

                panel.Controls.Add(grid);
            }

            // Methods
            if (grids.Contains(CSH_METHODS))
            {
                string[] columns = new string[]
                 {
                    "Method",
                    "Parameters",
                    "Return Type"
                 };
                DataGridView grid = CreateGrid(columns);
                grid.Name = CSH_METHODS;

                panel.Controls.Add(grid);
            }

            // Enums
            if (grids.Contains(CSH_ENUMS))
            {
                string[] columns = new string[]
                 {
                    "Enum",
                    "Full Name",
                    "Values"
                 };
                DataGridView grid = CreateGrid(columns);
                grid.Name = CSH_ENUMS;

                panel.Controls.Add(grid);
            }

            // Static Properties
            if (grids.Contains(CSH_PROPERTIES_STATIC))
            {
                string[] columns = new string[]
                {
                    "Static Property",
                    "Value",
                    "Type"
                };
                DataGridView grid = CreateGrid(columns);
                grid.Name = CSH_PROPERTIES_STATIC;

                panel.Controls.Add(grid);
            }

            // Static Methods
            if (grids.Contains(CSH_METHODS_STATIC))
            {
                string[] columns = new string[]
                 {
                    "Static Method",
                    "Parameters",
                    "Return Type"
                 };
                DataGridView grid = CreateGrid(columns);
                grid.Name = CSH_METHODS_STATIC;

                panel.Controls.Add(grid);
            }

            prompt.Show();
            panel.PopulateGrids(false, scopeStr);
        }

        private static void Grid_GotFocus(object sender, EventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid == null) return;

            foreach (var control in grid.Parent.Controls)
            {
                if (control is DataGridView gv && !gv.Equals(grid))
                {
                    gv.ClearSelection();
                }
            }
        }

        private static void Grid_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dataGridView.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow < 0 || currentMouseOverRow >= dataGridView.RowCount) return;

                DataGridViewRow row = dataGridView.Rows[currentMouseOverRow];
                dataGridView.ClearSelection();
                row.Selected = true;

                ContextMenu m = new ContextMenu();
                CsHelpMenuItem item = new CsHelpMenuItem("Send to terminal", row);
                m.MenuItems.Add(item);

                m.Show(dataGridView, new Point(e.X, e.Y));
            }
        }
    }
}
