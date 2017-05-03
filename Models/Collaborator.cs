using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public class Collaborator
    {
        public long ID { get; set;}
        public long UserId { get; set;}
        public long ProjectId { get; set;}
        public long PermissionId { get; set;}
    }

}