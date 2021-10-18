using smart_backup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smart_backup.DTO
{
    public class GitResponse
    {
        public int count { get; set; }
        public List<GitRepoDTO> value { get; set; }
    }
}
