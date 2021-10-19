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
        public static string rootBackup = @"C:\";
        public static string tempClone = rootBackup + @"temp\";
    }
}
