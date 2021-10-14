using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBackup.service
{
    public class Business
    {

        private readonly ILogger _logger;
        public Business(ILogger<Business> logger)
        {
            _logger = logger;

        }
        internal void Run()
        {
            _logger.LogInformation("Application Started at {dateTime}", DateTime.UtcNow);

            //Business Logic START

            generateExecuteScript();

            //Business logic END
            _logger.LogInformation("Application Ended at {dateTime}", DateTime.UtcNow);
        }

        private void generateExecuteScript()
        {
            using (Process process = new Process())
            {
                
                string gitUser = "devopsbackup";
                string gitToken = "4y6ttu45wxyo2gebg2xzbk3wqkte66atuhwi7xhku6zmy6g6ss3a";
                string sourceUrl = "https://SmartBackup2@dev.azure.com/SmartBackup2/GuruFake2/_git/GuruFake2";

                string nameProject = "GuruFake2";
                string workdirPath = "C:\\"+nameProject;
                CloneOptions co = new CloneOptions();
                co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = gitUser, Password = gitToken };
                _logger.LogInformation("inizio clonazione repo {0}", sourceUrl);
                
                
                LibGit2Sharp.Repository.Clone(sourceUrl, workdirPath, co);
                
            }
        }

    }
}
