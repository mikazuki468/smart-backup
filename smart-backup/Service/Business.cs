using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.Service
{
    public class Business
    {

        //TODO Inserire ILogger

        public void generateExecuteScript(string username, string token, string pathProject, string pathBackup)
        {
            string resultScript = string.Format("clone https://{0}:{1}@dev.azure.com{2}",username,token,pathProject);
            //ProcessStartInfo p = new ProcessStartInfo();

            #region Prima prova 
            //p.FileName = "git.exe";
            //p.Arguments = resultScript;
            //p.UseShellExecute = false;
            //p.RedirectStandardOutput = true;

            //Process proStart = new Process();
            //proStart.StartInfo = p;
            //proStart.Start();            
            //await File.WriteAllTextAsync("output.txt",proStart.StandardOutput.ReadToEnd());
            //proStart.WaitForExit();
            ////return resultScript;
            #endregion

            using (Process process = new Process())
            {

                process.StartInfo.WorkingDirectory = pathBackup;
                process.StartInfo.FileName = "git.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.Arguments = resultScript;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();

                // Write the redirected output to this application's window.
                Console.WriteLine(output);
                //File.WriteAllText("output.txt", output);

                process.WaitForExit();

            }

        }

        public void generateLogGit()
        {
            string log = string.Format("log");
            
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "git.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.Arguments = log;
                process.Start();

                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();

                File.WriteAllText("output.txt", output);

                process.WaitForExit();
            }

            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadLine();
        }

    }

    
}
