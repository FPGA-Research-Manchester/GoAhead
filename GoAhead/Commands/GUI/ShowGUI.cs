using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.GUI;

namespace GoAhead.Commands.GUI
{
    [CommandDescription(Description = "Start the GoAhead GUI with the given Forms", Wrapper = false)] // wrapper = FALSE as inside ShowGUI other commands are executed!!!
    class ShowGUI : Command
    {
        protected override void DoCommandAction()
        {         
            CommandExecuter.Instance.GUIActive = true;
            GoAhead.GUI.GUI gui = new GoAhead.GUI.GUI();
            gui.FormsToLoadOnStartup.AddRange(this.FormsToLoadOnStartup);
            gui.CommandToExecuteOnLoad.AddRange(ShowGUI.GUICommands.Where(cmd => cmd is OpenScriptInDebugger));
            
            foreach (GUICommand addCmd in ShowGUI.GUICommands.Where(cmd => cmd is AddUserElement))
            {
                CommandExecuter.Instance.Execute(addCmd);
            }
            ShowGUI.GUICommands.Clear();

            // store FPGA View for SaveFPGAViewAsBitmap
            ShowGUI.m_FPGAView = gui.FPGAView;
            try
            {
                gui.ShowDialog();
            }
            catch (Exception) { }

            CommandHook hook = CommandExecuter.Instance.GetAllHooks().FirstOrDefault(h => h is PrintProgressToGUIHook);
            PrintProgressToGUIHook progressHook = (PrintProgressToGUIHook)hook;
            progressHook.m_forms.Add(gui);

            CommandExecuter.Instance.GUIActive = false;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public static FPGAViewCtrl FPGAView
        {
            get { return ShowGUI.m_FPGAView; }
        }

        private static FPGAViewCtrl m_FPGAView;

        public static List<GUICommand> GUICommands = new List<GUICommand>();

        [Parameter(Comment = "Those forms that match on of this String will be loaded. E.g. InterfaceManager")]
        public List<String> FormsToLoadOnStartup = new List<String>();
    }
}
