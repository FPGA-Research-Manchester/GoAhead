using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Commands.Variables;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.GUI.SetVariables
{
    public partial class SetVarCtrl : UserControl, Interfaces.IObserver
    {
        public SetVarCtrl()
        {
            InitializeComponent();
            TileSelectionManager.Instance.Add(this);
        }

        public void Notify(object obj)
        {
            if (Input == null)
            {
                return;
            }
            if (Input.Domain != Input.DomainType.TileSelection)
            {
                return;
            }
            else if (TileSelectionManager.Instance.NumberOfSelectedTiles > 0)
            {
                Tile ul = TileSelectionManager.Instance.GetFirstSelectedTile(FPGATypes.Placement.UpperLeft, IdentifierManager.RegexTypes.Interconnect);
                Tile lr = TileSelectionManager.Instance.GetFirstSelectedTile(FPGATypes.Placement.LowerRight, IdentifierManager.RegexTypes.CLB);

                string varNameWithoutBraces = m_input.VariableName;
                varNameWithoutBraces = varNameWithoutBraces.Replace("(", "");
                varNameWithoutBraces = varNameWithoutBraces.Replace(")", "");

                string[] varNames = varNameWithoutBraces.Split(')', '(', ' ');
                if (varNames.Length == 4)
                {
                    string text = "";
                    text += varNames[0] + " = " + ul.TileKey.X + ";";
                    text += varNames[1] + " = " + ul.TileKey.Y + ";";
                    text += varNames[2] + " = " + lr.TileKey.X + ";";
                    text += varNames[3] + " = " + lr.TileKey.Y;

                    m_txtValue.Text = text;
                }
                else
                {
                    Console.WriteLine("Warning: Ranges of type TileSelection required four variables, e.g. (X1, Y1, X2, Y2). Found " + m_input.VariableName);
                }

                //this.m_txtTileIdentifier.Text = tile.Location;
            }
        }

        public Set[] SetCommands
        {
            get { return m_setCommands.ToArray(); }
        }

        public Input Input
        {
            get { return m_input; }
            set
            {
                m_input = value;
                m_setCommands.Clear();
                m_setCommands.Add(new Set());

                m_setCommands[0].Variable = m_input.VariableName;

                m_lblVar.Text = m_input.VariableName;

                if (m_input.Domain == Input.DomainType.Range)
                {
                    m_txtValue.Visible = false;
                    m_cmbBox.Location = m_location;
                    m_cmbBox.Visible = true;
                    m_cmbBox.Items.Clear();
                    m_cmbBox.Items.AddRange(m_input.ExplicitRange.ToArray());
                    m_cmbBox.SelectedItem = m_input.ExplicitRange.Count > 0 ? m_input.ExplicitRange[0] : "";
                }
                else if (m_input.Domain == Input.DomainType.TileSelection)
                {
                    m_txtValue.Visible = true;
                    m_txtValue.Location = m_location;
                    m_cmbBox.Visible = false;

                    string varNames = m_input.VariableName;
                    varNames = varNames.Replace("(", "");
                    varNames = varNames.Replace(")", "");
                    m_lblVar.Text = varNames;
                }
                else
                {
                    m_txtValue.Visible = true;
                    m_txtValue.Location = m_location;
                    m_cmbBox.Visible = false;

                    if (VariableManager.Instance.IsSet(m_input.VariableName))
                    {
                        m_txtValue.Text = VariableManager.Instance.GetValue(m_input.VariableName);
                    }
                }
            }
        }


        private void m_txtValue_TextChanged_1(object sender, EventArgs e)
        {
            SetValue(m_txtValue.Text);
        }


        private void m_cmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selection = m_cmbBox.SelectedItem;
            SetValue(selection.ToString());
        }

        private void SetValue(string value)
        {
            m_setCommands.Clear();
            if (Input.Domain == Input.DomainType.TileSelection)
            {
                string[] assignements = value.Split(';');

                if (assignements.Length != 4)
                {
                    Console.WriteLine("Warning: Select a range on the TileView to update this text box");
                }

                foreach (string assignement in assignements)
                {
                    string[] tupel = assignement.Split('=');
                    if (tupel.Length != 2)
                    {
                        Console.WriteLine("Warning: Select a range on the TileView to update this text box");
                    }

                    Set setCmd = new Set();

                    setCmd.Variable = tupel[0].Trim();
                    setCmd.Value = tupel[1].Trim();
                    m_setCommands.Add(setCmd);
                }

            }
            else
            {               
                Set setCmd = new Set();
                setCmd.Variable = Input.VariableName;
                setCmd.Value = value;
                setCmd.Value = setCmd.Value.Trim();

                if (Regex.IsMatch(setCmd.Value, @"\s"))
                {
                    setCmd.Value = "\"" + setCmd.Value + "\"";
                }

                m_setCommands.Add(setCmd);
            }
        }

        private List<Set> m_setCommands = new List<Set>();
        private Input m_input = null;
        private Point m_location = new Point(126, 22);


    }
}
