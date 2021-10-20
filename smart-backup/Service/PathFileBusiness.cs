using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.Service
{
    public static class PathFileBusiness
    {
        public static string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string pathFileOrg = projectDirectory + @"\Config\ListOrganization.txt";
        public static string pathFileProjects = projectDirectory + @"\Config\ProjectResponse.json";
        public static string rootBackup = @"C:\BackupSmartpeg";
        public static string nameBackup = DateTime.Now.ToString("yyyyMMdd")+ "Backup";

        public static string LogFile = rootBackup + "\\" + "log.txt";

        public static string tempClone = rootBackup + "\\" + nameBackup;
    }
}
