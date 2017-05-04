using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public class Collaborator
    {
        public string UserID { get; set; }
        public long ProjectID { get; set; }
        public long PermissionID { get; set; }
        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
        public Permission Permission { get; set; }
    }

}