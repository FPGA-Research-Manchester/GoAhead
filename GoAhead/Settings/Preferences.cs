using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GoAhead.Settings
{
    [Serializable]
    public class StoredPreferences
    {
        private StoredPreferences()
        {
            ExecuteExpandSelection = true;
            PrintSelectionResourceInfo = true;
        }

        /// <summary>
        /// Singelton
        /// </summary>
        public static StoredPreferences Instance = new StoredPreferences();

        public IOBarSettings IOBarSettings
        {
            get
            {
                if (m_IOBarSettings == null)
                {
                    m_IOBarSettings = new IOBarSettings();
                }
                return m_IOBarSettings;
            }
        }

        public DialogSettings TextBoxSettings
        {
            get
            {
                if (m_textBoxSetting == null)
                {
                    m_textBoxSetting = new DialogSettings();
                }
                return m_textBoxSetting;
            }
        }

        public DialogSettings FileDialogSettings
        {
            get
            {
                if (m_fileDialogSetting == null)
                {
                    m_fileDialogSetting = new DialogSettings();
                }
                return m_fileDialogSetting;
            }
        }

        public GUISettings GUISettings
        {
            get
            {
                if (m_guiSetting == null)
                {
                    m_guiSetting = new GUISettings();
                }
                return m_guiSetting;
            }
        }

        private IOBarSettings m_IOBarSettings = new IOBarSettings();
        private DialogSettings m_fileDialogSetting = new DialogSettings();
        private DialogSettings m_textBoxSetting = new DialogSettings();
        private GUISettings m_guiSetting = new GUISettings();

        // TODO move to subclass
        public bool XDL_RunFEScript { get; set; }

        public bool XDL_IncludePortStatements { get; set; }

        // TODO move to subclass
        public bool HighLightClockRegions { get; set; }

        public bool HighLightPlacedMacros { get; set; }
        public bool HighLightPossibleMacroPlacements { get; set; }
        public bool HighLightRAMS { get; set; }
        public bool HighLightSelection { get; set; }

        public bool ExecuteExpandSelection { get; set; }
        public bool ShowToolTips { get; set; }
        public bool PrintSelectionResourceInfo { get; set; }

        /// <summary>
        /// Ranges between 0 and 100
        /// </summary>
        public double ConsoleGUIShare
        {
            get { return m_consoleGUIShare; }
            set { m_consoleGUIShare = value; }
        }

        private double m_consoleGUIShare = 20;

        /// <summary>
        /// Ranges between 0 and 100
        /// </summary>
        public float RectangleWidth
        {
            get { return m_rectangleWidth; }
            set { m_rectangleWidth = value; }
        }

        private float m_rectangleWidth = 1;

        /// <summary>
        /// Return the path to application data
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationDataPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "GoAhead" + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Return the full path including the filename to the preferences.bin files in which we store our user settings
        /// </summary>
        /// <returns></returns>
        public static string GetPreferenceFileName()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "GoAhead" + Path.DirectorySeparatorChar + "preferences.bin";
        }

        /// <summary>
        /// Return the full path including to the directory in which GoAhead searches for detailed Command documentation
        /// </summary>
        /// <returns></returns>
        public static string GetCommandDocDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "GoAhead" + Path.DirectorySeparatorChar + "CmdDoc" + Path.DirectorySeparatorChar;
        }

        public static void LoadPrefernces()
        {
            string prefFile = GetPreferenceFileName();

            if (!File.Exists(prefFile))
            {
                //load default values
                Instance = new StoredPreferences();
            }
            else
            {
                //load setting from file
                Stream stream = File.OpenRead(prefFile);

                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    Instance = (StoredPreferences)formatter.Deserialize(stream);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Could not open " + prefFile + ": " + error);
                    Instance = new StoredPreferences();
                    stream.Close();
                    File.Delete(prefFile);
                    return;
                }
                stream.Close();
            }
        }

        public static void SavePrefernces()
        {
            // create directory if it does not exist
            string appDataPath = GetApplicationDataPath();
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            string prefFile = GetPreferenceFileName();

            //save config
            Stream stream = File.Open(prefFile, FileMode.OpenOrCreate);

            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(stream, Instance);
            }
            catch (Exception error)
            {
                Console.WriteLine("Could not write to " + prefFile + ": " + error.Message);
            }
            stream.Close();
        }
    }
}