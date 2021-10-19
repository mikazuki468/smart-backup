using smart_backup.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.Model
{
    public class GitOrgDTO
    {
        public string NameOrganization { get; set; }
        public GitProjectsJson Projects { get; set;}

        public GitOrgDTO(string nameOrganization,GitProjectsJson projects)
        {
            NameOrganization = nameOrganization;
            Projects = projects;
        }
    }
}
