using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using smart_backup.DTO;
using smart_backup.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBackup.service
{
    public class Business
    {
        //
        internal void Run()
        {
            Log.Logger = new LoggerConfiguration()
            //.ReadFrom.AppSettings()
            .MinimumLevel.Debug()
            .WriteTo.File("output.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

            Log.Information("Application Started at {dateTime}", DateTime.UtcNow);

            //Business Logic START

            try
            {
                //generateExecuteScript();
                JsonReader("C:\\Users\\roberto.galanti\\source\\repos\\smart-backup\\smart-backup\\Config\\response.json");
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Qualcosa è andato storto");
            }


            //Business logic END
            Log.Information("Application Ended at {dateTime}", DateTime.UtcNow);
        }

        private void generateExecuteScript()
        {
            using (Process process = new Process())
            {

                string gitUser = "devopsbackup";
                string gitToken = "4y6ttu45wxyo2gebg2xzbk3wqkte66atuhwi7xhku6zmy6g6ss3a";
                string sourceUrl = "https://SmartBackup2@dev.azure.com/SmartBackup2/GuruFake2/_git/GuruFake2";

                string nameProject = "GuruFake2";
                string workdirPath = "C:\\" + nameProject;
                CloneOptions co = new CloneOptions();
                co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = gitUser, Password = gitToken };

                Log.Information("inizio clonazione repo {0}", sourceUrl);

                try
                {
                    LibGit2Sharp.Repository.Clone(sourceUrl, workdirPath, co);

                }
                catch (Exception ex)
                {
                    Log.Warning("Attenzione {0} già esistente", nameProject);
                    Log.Information(ex, "Verificare errore seguente:");
                }


            }
        }


        public void JsonReader(string pathJason)
        {
            

            try
            {
                string jsonFromFile;
                using (var reader = new StreamReader(pathJason))
                {
                    jsonFromFile = reader.ReadToEnd();
                    GitResponse GitRepoFromJson=JsonConvert.DeserializeObject<GitResponse>(jsonFromFile);
                }
               
            }
            catch (Exception ex)
            {

                Log.Error(ex, "file non trovato");
            }
        }

    }
}
