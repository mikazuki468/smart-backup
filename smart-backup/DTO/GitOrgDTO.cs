using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.Model
{
    class GitOrgDTO
    {
        public string NameOrganization { get; set; }
        public List<GitRepoDTO> Project { get; set;}
    }
}
