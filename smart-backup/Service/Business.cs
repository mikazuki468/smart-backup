using LibGit2Sharp;
using Serilog;
using Serilog.Events;
using smart_backup.DTO;
using smart_backup.Model;
using smart_backup.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SmartBackup.service
{
    public class Business
    {
        
        internal void Run()
        {
            Log.Logger = new LoggerConfiguration()
            //.ReadFrom.AppSettings()
            .MinimumLevel.Debug()
            .WriteTo.File(PathFileBusiness.LogFile, restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

            Log.Information("Application Started at {dateTime}", DateTime.UtcNow);

            DeleteDirectory(PathFileBusiness.tempClone);

            //Business Logic START
            List<String> ListOrganization = File.ReadAllLines(PathFileBusiness.pathFileOrg).ToList();
            try
            {

                foreach (var nomeOrg in ListOrganization)
                {
                                        
                   generateExecuteScript(nomeOrg);
                   
   
                }
                

            }
            catch (Exception ex)
            {

                Log.Error(ex, "Qualcosa è andato storto");
            }


            //Business logic END
            Log.Information("Application Ended at {dateTime}", DateTime.UtcNow);

            string ZipPath = PathFileBusiness.tempClone + ".zip";
            string FolderToZip = PathFileBusiness.tempClone;


            ZipTheFolder(ZipPath, FolderToZip);
            DeleteDirectory(PathFileBusiness.tempClone);
        }

        private void ZipTheFolder(string ZipPath,string FolderFromZipLocation)
        {
            try
            {
                if (File.Exists(ZipPath))
                {
                    File.Delete(ZipPath);
                }
                ZipFile.CreateFromDirectory(FolderFromZipLocation, ZipPath);
            }
            catch (Exception ex)
            {
                Log.Error("Error when zip:", ex);
            }
        }

        public static void DeleteDirectory(string target_dir)
        {
            if (Directory.Exists(target_dir))
            {
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, false);
            }
            
        }

        private void generateExecuteScript(string nomeOrg)
        {
            GitRepository.RequestApiAllProjectByOrganization(nomeOrg);
            GitProjectsJson ListProject = GitRepository.JsonProjectsReader();
            GitOrgDTO currentOrg = new GitOrgDTO(nomeOrg, ListProject);
            
            using (Process process = new Process())
            {

                string gitUser = "devopsbackup";
                string gitToken = "4y6ttu45wxyo2gebg2xzbk3wqkte66atuhwi7xhku6zmy6g6ss3a";

                CloneOptions co = new CloneOptions();
                co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = gitUser, Password = gitToken };

                //co.OnTransferProgress = (tp) =>
                //{
                //    var progress = string.Format("{0} di {1}", tp.ReceivedObjects, tp.TotalObjects);
                //    Console.Out.WriteLine(progress);
                //    return true;
                //};


                foreach (var proj in currentOrg.Projects.value)
                {
                    string url = "https://" + currentOrg.NameOrganization + "@dev.azure.com/" + currentOrg.NameOrganization + "/" + proj.name + "/_git/" + proj.name;
                    

                    try
                    {
                        string workdirPath = PathFileBusiness.tempClone+ "\\" + currentOrg.NameOrganization + "\\" + proj.name;

                        #region pull
                        //if (Directory.Exists(workdirPath))
                        //{
                        //    //pull
                        //    Log.Information("inizio pull repo {0}", url);

                        //    ProcessStartInfo gitInfo = new ProcessStartInfo();
                        //    gitInfo.CreateNoWindow = true;
                        //    gitInfo.RedirectStandardError = true;
                        //    gitInfo.RedirectStandardOutput = true;
                        //    //gitInfo.FileName = YOUR_GIT_INSTALLED_DIRECTORY + @"\bin\git.exe";

                        //    Process gitProcess = new Process();
                        //    gitInfo.Arguments = ""; // such as "fetch orign"
                        //    gitInfo.WorkingDirectory = work;

                        //    gitProcess.StartInfo = gitInfo;
                        //    gitProcess.Start();

                        //    string stderr_str = gitProcess.StandardError.ReadToEnd();  // pick up STDERR
                        //    string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

                        //    gitProcess.WaitForExit();
                        //    gitProcess.Close();


                        //    Log.Information("fine pull repo {0}", url);
                        //}
                        //else
                        //{
                        //}
                        #endregion

                        //clone

                        Log.Information("inizio clonazione repo {0}", url);
                        LibGit2Sharp.Repository.Clone(url, workdirPath, co);
                        Log.Information("fine clonazione repo {0}", url);
                       
                    }
                    catch (Exception ex)
                    {
                        Log.Warning("Attenzione {0} già esistente", url);
                        Log.Information(ex, "Verificare errore seguente:");
                    }
                }


            }
        }

        
        
    }
}
