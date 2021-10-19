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
using System.Net.Http;
using System.Net.Http.Headers;
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
                //TODO costruire la org
                //string nomeOrg = "SmartBackup1";

                List<string> ListOrganization = GetListOrganization(@"C:\Users\roberto.galanti\source\repos\smart-backup\smart-backup\Config\ListOrganization.txt");

                foreach (var nomeOrg in ListOrganization)
                {
                    RequestApiAllProjectByOrganization(nomeOrg);

                    GitProjectsJson ListProject = JsonProjectsReader(@"C:\Users\roberto.galanti\source\repos\smart-backup\smart-backup\Config\ProjectResponse.json");


                    GitOrgDTO currentOrg = new GitOrgDTO(nomeOrg, ListProject);
                    generateExecuteScript(currentOrg);

                }
                

            }
            catch (Exception ex)
            {

                Log.Error(ex, "Qualcosa è andato storto");
            }


            //Business logic END
            Log.Information("Application Ended at {dateTime}", DateTime.UtcNow);
        }

        public static async void RequestApiAllProjectByOrganization(string nomeOrg)
        {
            try
            {
                var personalaccesstoken = "4y6ttu45wxyo2gebg2xzbk3wqkte66atuhwi7xhku6zmy6g6ss3a";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));

                    using (HttpResponseMessage response = client.GetAsync("https://dev.azure.com/"+nomeOrg+"/_apis/projects").GetAwaiter().GetResult())
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Console.WriteLine(responseBody);
                  
                        File.WriteAllText(@"C:\Users\roberto.galanti\source\repos\smart-backup\smart-backup\Config\ProjectResponse.json", responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void generateExecuteScript(GitOrgDTO CurrentOrg)
        {
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


                foreach (var proj in CurrentOrg.Projects.value)
                {
                    string url = "https://" + CurrentOrg.NameOrganization + "@dev.azure.com/" + CurrentOrg.NameOrganization + "/" + proj.name + "/_git/" + proj.name;
                    Log.Information("inizio clonazione repo {0}", url);

                    try
                    {
                        //workdirPath=workdirPath+CurrentOrg.NameOrganization+"\\"+proj.name;
                        string workdirPath = "C:\\" + CurrentOrg.NameOrganization + "\\" + proj.name;
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


        public GitProjectsJson JsonProjectsReader(string pathProjectsJson)
        {

            try
            {
                string jsonFromFile;
                using (var reader = new StreamReader(pathProjectsJson))
                {
                    GitProjectsJson ListProjects = new GitProjectsJson();
                    jsonFromFile = reader.ReadToEnd();
                    ListProjects = JsonConvert.DeserializeObject<GitProjectsJson>(jsonFromFile);
                    return ListProjects;
                }

            }
            catch (Exception ex)
            {

                Log.Error(ex, "file non trovato");
                return null;
            }
        }

        public List<String> GetListOrganization(string pathOrganizationFile)
        {
            List<String> ListOrganization = File.ReadAllLines(pathOrganizationFile).ToList();

            return ListOrganization;
        }


        


    }
}
