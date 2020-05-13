using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands.LibraryElementInstantiation;
using GoAhead.GUI.LibraryElementInstantiation;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.LibraryElementInstantiationManager;

namespace GoAhead.GUI.MacroForm
{
    public partial class LibraryElementPlacerCtrl : UserControl, Interfaces.IObserver
    {
        public LibraryElementPlacerCtrl()
        {
            InitializeComponent();

            TileSelectionManager.Instance.Add(this);

            m_netlistContainerSelector.Label = "Which netlist container to update";
            m_libElementSelector.Label = "Which element to place";
        }

        private void m_btnPlaceMacro_Click(object sender, EventArgs e)
        {
            if (!IsPlacementValidForSingleInstantiation(false))
            {
                return;
            }

            string libraryElementName = null;
            Tile anchor = null;
            string errorMessage = null;
            string instanceName = null;
            string netlistContainerName = null;

            bool paramsValid = FindParametersForSingleInstantiation(out libraryElementName, out anchor, out instanceName, out netlistContainerName, out errorMessage);

            if (!paramsValid)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
                return;
            }

            AddSingleInstantiationByTile addInst = new AddSingleInstantiationByTile();
            addInst.AutoClearModuleSlot = m_chkBoxAutoclearSingle.Checked;
            addInst.AnchorLocation = anchor.Location;
            addInst.InstanceName = instanceName;
            addInst.LibraryElementName = libraryElementName;
            addInst.NetlistContainerName = netlistContainerName;
            Commands.CommandExecuter.Instance.Execute(addInst);
        }

        private void m_btnPlaceBySlice_Click(object sender, EventArgs e)
        {
            string libraryElementName = null;
            Slice slice = null;
            string instanceName = null;
            string errorMessage = null;
            string netlistContainerName = null;

            bool paramsValid = FindParametersForSingleInstantiation(out libraryElementName, out slice, out instanceName, out netlistContainerName, out errorMessage);

            if (!paramsValid)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
                return;
            }

            AddSingleInstantiationBySlice addInst = new AddSingleInstantiationBySlice();
            addInst.AutoClearModuleSlot = m_chkBoxAutoclearSingle.Checked;
            addInst.InstanceName = instanceName;
            addInst.LibraryElementName = libraryElementName;
            addInst.NetlistContainerName = netlistContainerName;
            addInst.SliceName = slice.SliceName;

            Commands.CommandExecuter.Instance.Execute(addInst);
        }

        private void m_btnCheck_Click(object sender, EventArgs e)
        {
            string libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            string instanceName = m_txtSingleInstanceName.Text;
            if (instanceName.Length == 0 && libraryElementName != null)
            {
                m_txtSingleInstanceName.Text = LibraryElementInstanceManager.Instance.ProposeInstanceName(libraryElementName);
            }
            else if (LibraryElementInstanceManager.Instance.HasInstanceName(instanceName) && libraryElementName != null)
            {
                m_txtSingleInstanceName.Text = LibraryElementInstanceManager.Instance.ProposeInstanceName(libraryElementName);
            }
            IsPlacementValidForSingleInstantiation(true);
        }

        private void m_btnCheckPlacementForSelection_Click(object sender, EventArgs e)
        {
            string libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            string instanceName = m_txtMultiInstanceName.Text;
            if (instanceName.Length == 0 && libraryElementName != null)
            {
                m_txtMultiInstanceName.Text = libraryElementName;
            }
            else if (LibraryElementInstanceManager.Instance.HasInstanceName(instanceName) && libraryElementName != null)
            {
                m_txtMultiInstanceName.Text = libraryElementName;
            }
            IsPlacementValidForMultiInstantiation();
        }

        private void m_btnPlaceInSelection_Click(object sender, EventArgs e)
        {
            string macroName = null;
            string errorMessage = null;
            string instanceName = null;
            string netlistContainerName = null;

            bool paramsValid = FindParametersForMultiInstantiation(out macroName, out instanceName, out netlistContainerName, out errorMessage);

            if (!paramsValid)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
                return;
            }

            LibraryElement el = Library.Instance.GetElement(macroName);

            AddInstantiationInSelectedTiles cmd = new AddInstantiationInSelectedTiles();
            cmd.AutoClearModuleSlot = m_chkBoxAutoclearMulti.Checked;
            cmd.Horizontal = m_drpDownHorizontalOrder.Text;
            cmd.InstanceName = instanceName;
            cmd.LibraryElementName = macroName;
            cmd.Mode = m_drpDownMode.Text;
            cmd.NetlistContainerName = netlistContainerName;
            cmd.SliceNumber = el.ResourceShape.Anchor.AnchorSliceNumber;
            cmd.Vertical = m_drpDownVerticallOrder.Text;
            
            Commands.CommandExecuter.Instance.Execute(cmd);
        }

        private bool FindParametersForSingleInstantiation(out string libraryElementName, out Slice slice, out string instanceName, out string netlistContainerName, out string errorMessage)
        {
            libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            slice = FPGA.FPGA.Instance.GetSlice(m_txtAnchorSlice.Text);
            instanceName = m_txtSingleInstanceName.Text;
            netlistContainerName = "";
            errorMessage = "";

            if(string.IsNullOrEmpty(m_netlistContainerSelector.SelectedNetlistContainerName))
            {
                errorMessage = "No netlist container selected";
                return false;
            }
            netlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;

            if (libraryElementName == null)
            {
                errorMessage = "No library element selected";
                return false;
            }

            if (!Library.Instance.Contains(libraryElementName))
            {
                errorMessage = "Library element " + libraryElementName + " not found";
                return false;
            }

            if (slice == null)
            {
                errorMessage = "Slice " + m_txtAnchorSlice.Text + " not found";
                return false;
            }

            if (LibraryElementInstanceManager.Instance.HasInstanceName(instanceName))
            {
                errorMessage = "Instance name " + instanceName + " already in use";
                return false;
            }

            if (instanceName.Length == 0)
            {
                errorMessage = "No instance name given";
                return false;
            }

            return true;
        }

        private bool FindParametersForSingleInstantiation(out string libraryElementName, out Tile anchor, out string instanceName, out string netlistContainerName, out string errorMessage)
        {
            libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            anchor = null;
            errorMessage = "";
            instanceName = m_txtSingleInstanceName.Text;
            netlistContainerName = null;

            if (string.IsNullOrEmpty(m_netlistContainerSelector.SelectedNetlistContainerName))
            {
                errorMessage = "No netlist container selected";
                return false;
            }
            netlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;

            if (libraryElementName == null)
            {
                errorMessage = "No library element selected";
                return false;
            }

            if (!Library.Instance.Contains(libraryElementName))
            {
                errorMessage = "Library element " + libraryElementName + " not found";
                return false;
            }

            // no multiple selection
            if (!FPGA.FPGA.Instance.Contains(m_txtAnchorLocation.Text))
            {
                errorMessage = "Anchor " + m_txtAnchorLocation.Text + " not found";
                return false;
            }

            // no multiple selection
            if (!IdentifierManager.Instance.IsMatch(m_txtAnchorLocation.Text, IdentifierManager.RegexTypes.CLB))
            {
                errorMessage = "Select a CLB for placement";
                return false;
            }

            if (instanceName.Length == 0)
            {
                errorMessage = "No instance name given";
                return false;
            }

            if (LibraryElementInstanceManager.Instance.HasInstanceName(instanceName))
            {
                errorMessage = "Instance name " + instanceName + " already in use";
                return false;
            }

            Tile clickedAnchor = FPGA.FPGA.Instance.GetTile(m_txtAnchorLocation.Text);
            //clickedAnchor = FPGA.FPGA.Instance.GetTile("BRAMSITE2_X3Y28");
            LibraryElement libElement = Library.Instance.GetElement(libraryElementName);
            anchor = clickedAnchor;

            return true;
        }

        private bool FindParametersForMultiInstantiation(out string libraryElementName, out string instanceName, out string netlistContainerName, out string errorMessage)
        {
            libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            errorMessage = "";
            instanceName = m_txtMultiInstanceName.Text;
            netlistContainerName = null;

            if (string.IsNullOrEmpty(m_netlistContainerSelector.SelectedNetlistContainerName))
            {
                errorMessage = "No netlist container selected";
                return false;
            }
            netlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;
            if (libraryElementName == null)
            {
                errorMessage = "No Macro selected";
                return false;
            }

            if (!Library.Instance.Contains(libraryElementName))
            {
                errorMessage = "Macro " + libraryElementName + " not found";
                return false;
            }

            if (instanceName.Length == 0)
            {
                errorMessage = "No instance name given";
                return false;
            }

            if (LibraryElementInstanceManager.Instance.HasInstanceName(instanceName))
            {
                errorMessage = "Instance name " + instanceName + " already in use";
                return false;
            }

            return true;
        }

        private bool IsPlacementValidForMultiInstantiation()
        {
            string libElementName = null;
            string errorMessage = null;
            string instanceName = null;
            string netlistContainerName = null;

            bool paramsValid = FindParametersForMultiInstantiation(out libElementName, out instanceName, out netlistContainerName, out errorMessage);

            if (!paramsValid)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (Tile clb in TileSelectionManager.Instance.GetSelectedTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
            {
                StringBuilder errorList = null;
                LibraryElement libElement = Library.Instance.GetElement(libElementName);
                bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(clb, libElement, out errorList);
                if (!placementOk)
                {
                    MessageBox.Show("Library element " + libElementName + " can not be placed at " + clb.Location + ": " + errorList.ToString(), "Placement failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
           
            // placement is ok
            int clbsInSelection = TileSelectionManager.Instance.GetSelectedTiles().Count(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            MessageBox.Show("Library element " + libElementName + " can be placed at the selected " + clbsInSelection + " position(s)", "Placement OK", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);          
            return true;
        }

        private bool IsPlacementValidForSingleInstantiation(bool showMessageBoxForValidPlacement)
        {
            string libraryElementName = null;
            Tile anchor = null;
            string errorMessage = null;
            string instanceName = null;
            string netlistContainerName = null;
            bool paramsValid = FindParametersForSingleInstantiation(out libraryElementName, out anchor, out instanceName, out netlistContainerName, out errorMessage);

            if (!paramsValid)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            StringBuilder errorList = null;
            bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(anchor, Library.Instance.GetElement(libraryElementName), out errorList);

            if (placementOk)
            {
                if (showMessageBoxForValidPlacement)
                {
                    MessageBox.Show("Library element " + libraryElementName + " can be placed at " + anchor.Location, "Placement OK", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Library element " + libraryElementName + " can not be placed at " + anchor.Location + ": " + errorList.ToString(), "Placement failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return placementOk;
        }

        public void Notify(object obj)
        {
            UpdateAnchorTextBoxes();
        }

        private void UpdateAnchorTextBoxes()
        {
            if (TileSelectionManager.Instance.NumberOfSelectedTiles != 2)
            {
                // "Select exactly one tile (one pair of CLB and Interconnect Tile)", "Error");
                return;
            }

            Tile clb =
                TileSelectionManager.Instance.GetSelectedTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));

            if (clb == null)
            {
                // no clb selected
                return;
            }

            m_txtAnchorLocation.Text = clb.Location;

            string libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            if (string.IsNullOrEmpty(libraryElementName))
            {
                // no macro selected
                m_txtAnchorSlice.Text = clb.Slices[0].SliceName;
                return;
            }

            LibraryElement el = Library.Instance.GetElement(libraryElementName);
            /*
            AnchorInfo clbAnchor = el.GetAnchorInfo(IdentifierManager.RegexTypes.CLBRegex);
            // slice out of range
            int sliceNumber = clb.Slices.Count < clbAnchor.AnchorSliceNumber ? 0 : clbAnchor.AnchorSliceNumber;
            */
            m_txtAnchorSlice.Text = clb.Slices[0].SliceName;
        }

        private void ParentFormClosed(object sender, FormClosedEventArgs e)
        {
            TileSelectionManager.Instance.Remove(this);
        }

        private void MacroPlacerCtrl_Load(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.FormClosed += new FormClosedEventHandler(ParentFormClosed);
                UpdateAnchorTextBoxes();
            }
        }

        private void m_btnPrintPossiblePlacements_Click(object sender, EventArgs e)
        {
            string libraryElementName = m_libElementSelector.SelectedLibraryElementName;
            if (string.IsNullOrEmpty(libraryElementName))
            {
                MessageBox.Show("No library element selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!Library.Instance.Contains(libraryElementName))
            {
                MessageBox.Show("Library element " + libraryElementName + " not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                MessageBox.Show("No tiles selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            PrintPossibleLibraryElementPlacementsInSelection printCmd = new PrintPossibleLibraryElementPlacementsInSelection();
            printCmd.LibraryElementName = libraryElementName;
            Commands.CommandExecuter.Instance.Execute(printCmd);
        }
    }
}
