using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public class Project
    {
        public long Id { get; set;}
        public string Name { get; set;}

        public ICollection<Collaborator> Collaborators { get; set; }
        public ICollection<Folder> Folders { get; set; }
    }
}