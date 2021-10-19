using Newtonsoft.Json;
using Serilog;
using smart_backup.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.Service
{
    public static class GitRepository
    {
        
        public static void RequestApiAllProjectByOrganization(string nomeOrg)
        {
            try
            {
                var personalaccesstoken = "4y6ttu45wxyo2gebg2xzbk3wqkte66atuhwi7xhku6zmy6g6ss3a";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));

                    using (HttpResponseMessage response = client.GetAsync("https://dev.azure.com/" + nomeOrg + "/_apis/projects").GetAwaiter().GetResult())
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Console.WriteLine(responseBody);

                        File.WriteAllText(PathFileBusiness.pathFileProjects, responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public static GitProjectsJson JsonProjectsReader()
        {
            try
            {
                string jsonFromFile;
                using (var reader = new StreamReader(PathFileBusiness.pathFileProjects))
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

    }
}
