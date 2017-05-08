using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kokks.Models
{
    public class Folder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentID { get; set; }
        public long ProjectID { get; set; }

        [JsonIgnore]
        public Folder Parent { get; set; }
        public Project Project { get; set; }
        public ICollection<File> Files { get; set; }
    }
}