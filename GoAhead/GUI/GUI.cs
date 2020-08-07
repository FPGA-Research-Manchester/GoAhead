using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Commands.Data;
using GoAhead.Commands.GUI;
using GoAhead.Commands.Selection;
using GoAhead.GUI.AddLibraryManager;
using GoAhead.GUI.AddLibraryManager.Save;
using GoAhead.GUI.Blocker;
using GoAhead.GUI.CommandForms.ParseTimingData;
using GoAhead.GUI.ExpandSelection;
using GoAhead.GUI.ExtractModules;
using GoAhead.GUI.FPGAView;
using GoAhead.GUI.Macros.XDLGeneration;
using GoAhead.GUI.UCF;
using GoAhead.Settings;
using System.Drawing;

namespace GoAhead.GUI
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();

            m_console.Parent = this;
            m_hook = new GUICommandHook(this, m_console);
            CommandExecuter.Instance.AddHook(m_hook);

            Painter allTileStrategy = new AllTilesPainter(m_fpgaViewAll);
            allTileStrategy.HighLighter.Add(new RAMHighLighter(m_fpgaViewAll));
            allTileStrategy.HighLighter.Add(new ClockRegionHighlighter(m_fpgaViewAll));
            allTileStrategy.HighLighter.Add(new MacroHighLighter(m_fpgaViewAll));
            allTileStrategy.HighLighter.Add(new PossibleMacroPlacementHighLighter(m_fpgaViewAll));
            allTileStrategy.HighLighter.Add(new SelectionHighLighter(m_fpgaViewAll));
            m_fpgaViewAll.TilePaintStrategy = allTileStrategy;
            m_fpgaViewAll.XYConverter = new ConvertPosition(m_fpgaViewAll.ZoomPictureBox);
            m_fpgaViewAll.Reset();

            Painter blockStrategy = new BlockPainter(m_fpgaViewBlock);
            blockStrategy.HighLighter.Add(new RAMHighLighter(m_fpgaViewBlock));
            blockStrategy.HighLighter.Add(new ClockRegionHighlighter(m_fpgaViewBlock));
            blockStrategy.HighLighter.Add(new MacroHighLighter(m_fpgaViewBlock));
            blockStrategy.HighLighter.Add(new PossibleMacroPlacementHighLighter(m_fpgaViewBlock));
            blockStrategy.HighLighter.Add(new SelectionHighLighter(m_fpgaViewBlock));
            m_fpgaViewBlock.TilePaintStrategy = blockStrategy;
            m_fpgaViewBlock.XYConverter = new ConvertPosition(m_fpgaViewBlock.ZoomPictureBox);
            m_fpgaViewBlock.Reset();

            /*
            Painter fpgaEditorStylePainter = new FPGAEditorStylePainter(this.m_fpgaViewFPGAEditorStyle);
            fpgaEditorStylePainter.HighLighter.Add(new RAMHighLighter(this.m_fpgaViewBlock));
            fpgaEditorStylePainter.HighLighter.Add(new ClockRegionHighlighter(this.m_fpgaViewBlock));
            fpgaEditorStylePainter.HighLighter.Add(new MacroHighLighter(this.m_fpgaViewBlock));
            fpgaEditorStylePainter.HighLighter.Add(new PossibleMacroPlacementHighLighter(this.m_fpgaViewBlock));
            this.m_fpgaViewFPGAEditorStyle.TilePaintStrategy = fpgaEditorStylePainter;
            this.m_fpgaViewFPGAEditorStyle.XYConverter = new ConvertPosition(this.m_fpgaViewBlock.ZoomPictureBox);
            this.m_fpgaViewFPGAEditorStyle.Reset();
            */

            m_FPGAView = m_fpgaViewBlock;
            StoredPreferences.Instance.GUISettings.Open(this);

            AllowDrop = true;
            DragEnter += new DragEventHandler(m_txtInput_DragEnter);
            DragDrop += new DragEventHandler(m_txtInput_DragDrop);
        }

        #region DragAndDrop
        private void m_txtInput_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void m_txtInput_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (extension.Equals(".binFPGA"))
                    {
                        OpenBinFPGA openCmd = new OpenBinFPGA();
                        openCmd.FileName = file;
                        openCmd.PrintProgress = true;
                        CommandExecuter.Instance.Execute(openCmd);
                    }
                    else if (extension.Equals(".goa"))
                    {
                        RunScript runCmd = new RunScript();
                        runCmd.FileName = file;
                        runCmd.PrintProgress = true;
                        CommandExecuter.Instance.Execute(runCmd);
                    }
                    else if (extension.Equals(".xdl"))
                    {
                        ReadXDL readXDLCmd = new ReadXDL();
                        readXDLCmd.FileName = file;
                        readXDLCmd.PrintProgress = true;
                        CommandExecuter.Instance.Execute(readXDLCmd);
                    }
                    else if (extension.Equals(".viv_rpt"))
                    {
                        ReadVivadoFPGA readVivadoReportCmd = new ReadVivadoFPGA();
                        readVivadoReportCmd.FileName = file;
                        readVivadoReportCmd.PrintProgress = true;
                        CommandExecuter.Instance.Execute(readVivadoReportCmd);
                    }
                    else
                    {
                        MessageBox.Show("Unknown file extensions " + extension + " found. Other files are skipped. GoAhead suppors Drag&Drop of .binFPGA and .goa files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
        #endregion DragAndDrop

        #region FileMenu
        private void m_menuFPGAOpenXDL_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an XDL File";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Xilinx Design Language|*.xdl";

            string caller = "m_menuFPGAOpenXDL_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(openFileDialog.FileName));

            ReadXDL cmd = new ReadXDL();
            cmd.PrintProgress = true;
            cmd.FileName = openFileDialog.FileName;

            CommandExecuter.Instance.Execute(cmd);
        }

        private void m_menuFileOpenVivado_Click(object sender, EventArgs e)
        {
            ReadVivadoFPGADialog();
        }

        public static void ReadVivadoFPGADialog(bool excludePipsFromBlocking = true)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a Vivado Report File";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Vivado Report|*.viv_rpt";

            string caller = "m_menuFileOpenVivado_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(openFileDialog.FileName));

            ReadVivadoFPGA cmd = new ReadVivadoFPGA
            {
                PrintProgress = true,
                FileName = openFileDialog.FileName,
                ExcludePipsToBidirectionalWiresFromBlocking = excludePipsFromBlocking
            };

            CommandExecuter.Instance.Execute(cmd);
        }

        private void m_menuFileDebugVivado_Click(object sender, EventArgs e)
        {
            ReadVivadoFPGADebugger.ReadVivadoFPGADebugger window = new ReadVivadoFPGADebugger.ReadVivadoFPGADebugger();
            window.Show();
        }


        private void m_menuFileOpenDesign_Click(object sender, EventArgs e)
        {
            OpenDesignForm frm = new OpenDesignForm();
            frm.Show();
        }

        private void m_menuAddTimingData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a Timing Data CSV File",
                Multiselect = false,
                CheckFileExists = true,
                Filter = "CSV|*.csv"
            };

            string caller = "m_menuAddTimingData_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(openFileDialog.FileName));

            ParseTimingDataForm form = new ParseTimingDataForm
            {
                FileName = openFileDialog.FileName
            };
            form.ShowDialog();
        }

        private void m_menuFPGASaveRaw_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save FPGA as binFPGA File";
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.FileName = FPGA.FPGA.Instance.DeviceName + ".binFPGA";
            saveFileDialog.Filter = "binFPGA File|*.binFPGA";

            string caller = "m_menuFPGASaveRaw_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                saveFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(saveFileDialog.FileName));

            SaveBinFPGA saveCmd = new SaveBinFPGA();
            saveCmd.FileName = saveFileDialog.FileName;

            CommandExecuter.Instance.Execute(saveCmd);
        }

        private void m_menuFileSaveAsDesign_Click(object sender, EventArgs e)
        {
            SaveForm saveForm = new SaveForm(SaveForm.SaveType.SaveAsDesign);
            saveForm.Show();
        }

        private void m_menuFileSaveAsMacro_Click(object sender, EventArgs e)
        {
            SaveForm saveForm = new SaveForm(SaveForm.SaveType.SaveAsMacro);
            saveForm.Show();
        }

        private void m_menuFileSaveAsBlocker_Click(object sender, EventArgs e)
        {
            SaveForm saveForm = new SaveForm(SaveForm.SaveType.SaveAsBlocker);
            saveForm.Show();
        }

        private void m_menuFileSaveBitmap_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save FPGA as PNG File";
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.FileName = FPGA.FPGA.Instance.DeviceName + ".png";
            saveFileDialog.Filter = "Portable Network Graphics|*.png";

            string caller = "m_menuFileSavePNG_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                saveFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            Commands.GUI.SaveFPGAViewAsPNG saveCmd = new Commands.GUI.SaveFPGAViewAsPNG();
            saveCmd.FileName = saveFileDialog.FileName;

            CommandExecuter.Instance.Execute(saveCmd);

            //this.m_fpgaViewBlock.SaveBitmapToFile(saveFileDialog.FileName);
        }
        private void m_menuFPGAOpenRaw_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open binFPGA File";
            openFileDialog.CheckFileExists = false;
            openFileDialog.FileName = "";
            openFileDialog.Filter = "binFPGA File|*.binFPGA";

            string caller = "m_menuFPGAOpenRaw_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(openFileDialog.FileName));

            OpenBinFPGA openCmd = new OpenBinFPGA();
            openCmd.FileName = openFileDialog.FileName;
            openCmd.PrintProgress = true;
            CommandExecuter.Instance.Execute(openCmd);
        }
        private void m_menuFPGAResetAll_Click(object sender, EventArgs e)
        {
            CommandExecuter.Instance.Execute(new Reset());
        }
        private void m_menuFPGADeviceInfo_Click(object sender, EventArgs e)
        {
            DeviceInfoForm dlg = new DeviceInfoForm();
            dlg.Show();
        }
        private void bUFGInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoAhead.GUI.BUFGInfo.BUFGInfoForm dlg = new BUFGInfo.BUFGInfoForm();
            dlg.Show();
        }
        private void m_menuFilePartGenAll_Click(object sender, EventArgs e)
        {
            PartGen.PartgenAllForm dlg = new PartGen.PartgenAllForm();
            dlg.Show();
        }
        private void m_menuFileExit_Click(object sender, EventArgs e)
        {
            // save settings
            StoredPreferences.SavePrefernces();

            Environment.Exit(0);
        }

        #endregion FileMenu

        #region CommandMenu
        private void m_menuCommandsRunScript_Click(object sender, EventArgs e)
        {
            string caller = "m_menuCommandsRunScript_Click";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a Script File";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "TCL Script File|*.tcl|GoAhead Script File|*.goa";

            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(openFileDialog.FileName));

            if (openFileDialog.FileName.EndsWith(".goa"))
            {
                RunScript cmd = new RunScript();
                cmd.FileName = openFileDialog.FileName;

                CommandExecuter.Instance.Execute(cmd);
            }
            else
            {
                string script = File.ReadAllText(openFileDialog.FileName);
                int r = Program.mainInterpreter.EvalScript(script);
                if (r != 0) Console.WriteLine("Error while executing TCL script: " + Program.mainInterpreter.Result);
            }            

            Invalidate();
        }
        private void m_menuCommandsSingleStepScript_Click(object sender, EventArgs e)
        {
            ScriptDebugger.ScriptDebuggerForm dlg = new ScriptDebugger.ScriptDebuggerForm(this);
            dlg.Show();
        }
        private void m_menuCommandsBlock_Click(object sender, EventArgs e)
        {
            if (FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                MessageBox.Show("No tiles selected", "GoAhead", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BlockerForm dlg = new BlockerForm();
            dlg.ShowDialog();
        }

        private void m_menuCommandsTunnel_Click(object sender, EventArgs e)
        {
            if (FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                MessageBox.Show("No tiles selected", "GoAhead", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            PortSelectionForm dlg = new PortSelectionForm();
            dlg.Show();
        }

        private void m_menuCommandsSaveSelectionExternalFile_Click(object sender, EventArgs e)
        {
            ExtractModules.ExtractModuleForm dlg = new ExtractModules.ExtractModuleForm(ExtractModuleForm.ModuleSourceType.FromNetlist);
            dlg.Show();
        }

        private void m_menuCommandsSaveSelectionFromCurrentDesign_Click(object sender, EventArgs e)
        {
            ExtractModules.ExtractModuleForm dlg = new ExtractModules.ExtractModuleForm(ExtractModuleForm.ModuleSourceType.FromSelection);
            dlg.Show();
        }

        private void m_menuCommandsSingleCommand_Click(object sender, EventArgs e)
        {
            CommandInterfaceForm cif = new CommandInterfaceForm();
            cif.Show();
        }
        #endregion CommandMenu

        #region EditMenu


        private void m_menuInvertSelection_Click(object sender, EventArgs e)
        {
            CommandExecuter.Instance.Execute(new InvertSelection());
            if (m_fpgaViewBlock != null)
            {
                m_fpgaViewAll.Invalidate();
                m_fpgaViewBlock.Invalidate();
            }
        }
        private void m_menuEditClearSelection_Click(object sender, EventArgs e)
        {
            CommandExecuter.Instance.Execute(new ClearSelection());
            if (m_fpgaViewBlock != null)
            {
                m_fpgaViewAll.Invalidate();
                m_fpgaViewBlock.Invalidate();
            }
        }

        private void m_menuEditExpandSelection_Click(object sender, EventArgs e)
        {
            Commands.Selection.ExpandSelection expandCmd = new Commands.Selection.ExpandSelection();
            CommandExecuter.Instance.Execute(expandCmd);
            if (m_fpgaViewBlock != null)
            {
                m_fpgaViewAll.Invalidate();
                m_fpgaViewBlock.Invalidate();
            }
        }
        private void m_menuEditPreferncesExpandLeftRight_Click(object sender, EventArgs e)
        {
            ExpandSelectionForm expandForm = new ExpandSelectionForm(this);
            expandForm.Show();
        }

        private void m_menuEditPreferncesExpandSpecialTiles_Click(object sender, EventArgs e)
        {
            ExpandSelectionSpecialTiles expandCmd = new ExpandSelectionSpecialTiles();
            CommandExecuter.Instance.Execute(expandCmd);
            if (m_fpgaViewBlock != null)
            {
                m_fpgaViewAll.Invalidate();
                m_fpgaViewBlock.Invalidate();
            }
        }

        private void m_menuEditSelectAll_Click(object sender, EventArgs e)
        {
            CommandExecuter.Instance.Execute(new SelectAll());
            if (m_fpgaViewBlock != null)
            {
                m_fpgaViewAll.Invalidate();
                m_fpgaViewBlock.Invalidate();
            }
        }
        private void m_menuEditPrefernces_Click(object sender, EventArgs e)
        {
            PreferencesForm dlg = new PreferencesForm(this);
            dlg.Show();
        }

        #endregion EditMenu

        #region HelpMenu
        private void m_menuHelpAbout_Click(object sender, EventArgs e)
        {
            AboutGoAhead dlg = new AboutGoAhead();
            dlg.ShowDialog();
        }
        private void m_menuHelpPrintCommandHelp_Click(object sender, EventArgs e)
        {
            CommandExecuter.Instance.Execute(new PrintCommandHelp());
        }

        #endregion HelpMenu

        #region Macro
        private void m_menuMacroView_Click(object sender, EventArgs e)
        {
            MacroForm.MacroManagerForm macroFrm = new MacroForm.MacroManagerForm();
            macroFrm.Show();
        }

        private void m_menuMacroGnerateXDLCode_Click(object sender, EventArgs e)
        {
            XDLGenerationForm xdlGenForm = new XDLGenerationForm();
            xdlGenForm.ShowDialog();
        }

        private void m_menuMacroLaunchFE_Click(object sender, EventArgs e)
        {
            LaunchFEGUI dlg = new LaunchFEGUI();
            dlg.ShowDialog();
        }
        #endregion

        #region UCF
        private void m_menuUCFProhibits_Click(object sender, EventArgs e)
        {
            ProhibitStatementForm dlg = new ProhibitStatementForm();
            dlg.ShowDialog();
        }

        private void m_menuUCFLocations_Click(object sender, EventArgs e)
        {
            LocationConstraintsGUI dlg = new LocationConstraintsGUI();
            dlg.ShowDialog();
        }

        private void m_menuUCFArea_Click(object sender, EventArgs e)
        {
            PrintAreaConstraintsForm dlg = new PrintAreaConstraintsForm();
            dlg.ShowDialog();
        }
        #endregion

        #region Extras
        private void m_menuExtrasExportAG_Click(object sender, EventArgs e)
        {
            ArchitectureGraph.ExportArchitecture exportForm = new ArchitectureGraph.ExportArchitecture();
            exportForm.Show();
        }

        private void m_menuExtrasPrintCommandNames_Click(object sender, EventArgs e)
        {
            PrintCommandNames cmd = new PrintCommandNames();
            CommandExecuter.Instance.Execute(cmd);
        }

        private void m_menuPrintCommandParameter_Click(object sender, EventArgs e)
        {
            PrintCommandParameters cmd = new PrintCommandParameters();
            CommandExecuter.Instance.Execute(cmd);
        }
        private void m_menuExtrasRedraw_Click(object sender, EventArgs e)
        {
            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }

        private void watchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoAhead.GUI.Watch.VariableWatchForm watch = new Watch.VariableWatchForm();
            watch.Show();
        }

        private void m_menuExtrasHookScripts_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Program.AssemblyDirectory);
        }

        private void m_menuExtrasGOAHEAD_HOME_Click(object sender, EventArgs e)
        {
            string goaheadHome = Environment.GetEnvironmentVariable("GOAHEAD_HOME");
            if (goaheadHome == null)
            {
                MessageBox.Show("GOAHEAD_HOME not set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                System.Diagnostics.Process.Start("explorer.exe", goaheadHome);
            }
        }

        #endregion Extras

        private GUICommandHook m_hook;

        /// <summary>
        /// If top level gui invalidates, invalidate all (currently two) views
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUI_Paint(object sender, PaintEventArgs e)
        {
            m_fpgaViewAll.Invalidate();
            m_fpgaViewBlock.Invalidate();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            // check vars
            /*
            StringBuilder errorList;
            if (!Objects.EnvChecker.CheckEnv(out errorList))
            {
                MessageBox.Show(errorList.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            Assembly asm = Assembly.GetExecutingAssembly();

            // start preselected form
            if (FormsToLoadOnStartup.Count > 0)
            {

                foreach (Type type in asm.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Form))))
                {
                    foreach (string formName in FormsToLoadOnStartup.Where(s => !string.IsNullOrEmpty(s) && Regex.IsMatch(type.Name, s)))
                    {
                        try
                        {
                            Form dlg = (Form)Activator.CreateInstance(type);
                            dlg.Show();
                        }
                        catch(Exception)
                        {
                            Console.WriteLine("No parameter less constructor found. Can not open GUI " + formName);
                        }
                    }
                }
            }

            m_commandsToExecuteOnLoad.ForEach(cmd => CommandExecuter.Instance.Execute(cmd));
            PositionControls();
                        
            // add debug menu
            CommandExecuter.Instance.MuteCommandTrace = true;
            foreach (Type type in asm.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Command)) && !t.Name.Equals(GetType().Name)))
            {
                // Namespace may be null -> do not include in Where statement
                if (type.Namespace.EndsWith("Debug"))
                {
                    Command cmd = (Command)Activator.CreateInstance(type);

                    AddUserMenu addMenuCmd = new AddUserMenu();
                    addMenuCmd.Name = type.Name;
                    addMenuCmd.Command = type.Name + ";";
                    addMenuCmd.ToolTip = cmd.GetCommandDescription();
                    addMenuCmd.ToolStrip = m_menuExtrasDebug;
                    CommandExecuter.Instance.Execute(addMenuCmd);
                }
            }
            CommandExecuter.Instance.MuteCommandTrace = false;
        }

        private void GUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoredPreferences.Instance.GUISettings.Close(this);
        }

        private void GUI_Resize(object sender, EventArgs e)
        {
            PositionControls();
        }

        private void PositionControls()
        {
            // hide empty tool bar
            m_toolStripUserButtons.Height = m_toolStripUserButtons.Visible ? 25 : 0;

            int top = m_menuStrip.Height + m_toolStripUserButtons.Height;

            int right = 5;
            int heightForControls = Height - (top + 50);

            double consoleShare = StoredPreferences.Instance.ConsoleGUIShare / 100;
            double tabViewShare = 1 - consoleShare;

            m_tabView.Top = top;
            m_tabView.Left = 0;
            m_tabView.Width = Width - right;
            m_tabView.Height = (int)((double)tabViewShare * heightForControls);

            m_console.Top = top + m_tabView.Height + 40;
            m_console.Left = 0;
            m_console.Height = (int)((double)consoleShare * heightForControls);
            m_console.Width = Width - right;
        }

        public List<string> FormsToLoadOnStartup
        {
            get { return m_formsToLoadOnStartup; }
            set { m_formsToLoadOnStartup = value; }
        }

        public List<Command> CommandToExecuteOnLoad
        {
            get { return m_commandsToExecuteOnLoad; }
            set { m_commandsToExecuteOnLoad = value; }
        }

        public ToolStrip UserToolStrip
        {
            get { return m_toolStripUserButtons; }
        }

        public ToolStripMenuItem UserMenuItem
        {
            get { return m_menuUserMenu; }
        }

        public FPGAViewCtrl FPGAView
        {
            get { return m_FPGAView; }
        }

        private void GUI_OnMouseMove(object sender, EventArgs e)
        {
            if(m_fpgaViewAll.ZoomPictureBox.Sync && m_fpgaViewBlock.ZoomPictureBox.Sync)
                SyncViews();
        }

        private float SharedZoom
        {
            get { return m_SharedZoom; }
            set { m_SharedZoom = value; }
        }
        private Point SharedAutoScroll
        {
            get { return m_SharedAutoScroll; }
            set { m_SharedAutoScroll = value; }
        }

        public void SyncViews()
        {
            float viewAllZoom = m_fpgaViewAll.ZoomPictureBox.Zoom;
            float viewBlockZoom = m_fpgaViewBlock.ZoomPictureBox.Zoom;
            Point viewBlockAutoScroll = m_fpgaViewBlock.ZoomPictureBox.AutoScrollPosition;
            Point viewAllAutoScroll = m_fpgaViewAll.ZoomPictureBox.AutoScrollPosition;

            //Sync zoom on both views.
            if (  Math.Abs( viewAllZoom - this.m_SharedZoom)   < Math.Abs(viewBlockZoom - this.SharedZoom ))
            {
                m_fpgaViewAll.ZoomPictureBox.Zoom = viewBlockZoom;
                m_SharedZoom = viewBlockZoom;    
            }
            else if (Math.Abs(viewAllZoom - this.m_SharedZoom) > Math.Abs(viewBlockZoom - this.SharedZoom))
            {
                m_fpgaViewBlock.ZoomPictureBox.Zoom = viewAllZoom;
                m_SharedZoom = viewAllZoom;
            }      
            
            
       
            //Only update autoscrolls value.
            if ( viewBlockAutoScroll == this.SharedAutoScroll && viewAllAutoScroll != this.SharedAutoScroll)
            {
                m_fpgaViewBlock.ZoomPictureBox.AutoScrollPosition = new Point((-1) * viewAllAutoScroll.X, (-1) * viewAllAutoScroll.Y);
                this.SharedAutoScroll = viewAllAutoScroll;
            }
            else if( viewAllAutoScroll == this.SharedAutoScroll && viewBlockAutoScroll != this.SharedAutoScroll)
            {
                m_fpgaViewAll.ZoomPictureBox.AutoScrollPosition = new Point((-1) * viewBlockAutoScroll.X, (-1) * viewBlockAutoScroll.Y);
                this.SharedAutoScroll = viewBlockAutoScroll;
            }
            
        }



        private FPGAViewCtrl m_FPGAView = null;
        private List<string> m_formsToLoadOnStartup = new List<string>();
        private List<Command> m_commandsToExecuteOnLoad = new List<Command>();





    }
}
