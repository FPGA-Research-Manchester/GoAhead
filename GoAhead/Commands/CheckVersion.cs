using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;
using GoAhead.Settings;

namespace GoAhead.Commands
{
    class CheckVersion : Command
    {
        protected override void DoCommandAction()
        {
            String filePath = Process.GetCurrentProcess().MainModule.FileName;

            String dir = Program.AssemblyDirectory;
            String hashFile = dir + Path.DirectorySeparatorChar + "GOA.hash";

            String oldHash = "";
            if (File.Exists(hashFile))
            {
                TextReader tr = new StreamReader(hashFile);
                oldHash = tr.ReadToEnd();
                tr.Close();
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            String currentHash = HashFile(fs);
            fs.Close();

            TextWriter tw = new StreamWriter(hashFile, false);
            tw.Write(currentHash);
            tw.Close();

            String question = "You switched to a new GoAhead version. Delete the preferences " + StoredPreferences.GetPreferenceFileName() + "?";

            if (!oldHash.Equals(currentHash) || true)
            {       
                switch (this.Action.ToUpper())
                {
                    case "WARN" : 
                        {
                            this.OutputManager.WriteOutput("Warning: You switched to a new GoAhead version. Make sure you delete your preferences file.");
                            break;
                        }
                    case "DEL" :
                        {
                            this.CreateBackupAndDelete();
                            break;
                        }
                    case "ASK" : 
                        {
                            bool delete = false;
                            if (CommandExecuter.Instance.GUIActive)
                            {
                                DialogResult result = MessageBox.Show(question, "New version detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    delete = true;
                                }
                                else
                                {
                                    delete = false;
                                }

                            }
                            else
                            {
                                while(true)
                                {
                                    Console.WriteLine(question + " (yes|no)");
                                    String value = Console.ReadLine();
                                    if (value.ToUpper().Equals("YES"))
                                    {
                                        delete = true;
                                        break;
                                    }
                                    else if (value.ToUpper().Equals("NO"))
                                    {
                                        delete = false;
                                        break;
                                    }
                                };
                            }
                            if(delete)
                            {
                                this.CreateBackupAndDelete();
                            }
                            break;
                        }
                    default:
                        { 
                            throw new ArgumentException("Invalid action specified. See parameter Action for valied values");
                        }
                }
            }

            // reload settings
            StoredPreferences.LoadPrefernces();
        }

        private void CreateBackupAndDelete()
        {
            String prefFile = StoredPreferences.GetPreferenceFileName();
            if (!File.Exists(prefFile))
            {
                return;
            }
            // create backup (overwrite existing bcakup)
            String backup = Path.GetFileNameWithoutExtension(prefFile) + ".backup";
            if (File.Exists(backup))
            {
                File.Delete(backup);
            }
            File.Copy(prefFile, backup);
            File.Delete(prefFile);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public string HashFile(FileStream stream)
        {
            StringBuilder sb = new StringBuilder();

            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);

                MD5 md5 = MD5CryptoServiceProvider.Create();
                byte[] hash = md5.ComputeHash(stream);
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));

                stream.Seek(0, SeekOrigin.Begin);
            }

            return sb.ToString();
        }


        [Parameter(Comment = "What to do, if a new version is detected: Issue a warning (WARN), delete preferences.bin (DEL) or ask the user whether to delete preferences.bin (ASK)")]
        public String Action = "WARN";
    }
}
