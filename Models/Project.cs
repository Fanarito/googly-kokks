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
        public IEnumerable<Collaborator> Collaborators;
        public IEnumerable<Folder> Folders;
    }
}