using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public class Folder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Folder Parent { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}