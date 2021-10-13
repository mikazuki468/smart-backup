using Microsoft.Extensions.Logging;
using smart_backup.Service;
using System;
using System.Diagnostics;
using System.IO;

namespace smart_backup
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Prima prova

            string pathBackup = "C:\\";
            string username = "devopsbackup";
            string token = "4y6ttu45wxyo2gebg2xzbk3wqkte66atuhwi7xhku6zmy6g6ss3a";
            string pathProject = "/SmartBackup2/GuruFake2/_git/GuruFake2";

            Business result = new Business();

            result.generateExecuteScript(username, token, pathProject, pathBackup);
           // result.generateLogGit();

            #endregion



        }
    }
}
