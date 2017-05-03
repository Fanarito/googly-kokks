using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public class File
    {
        public long ID { get; set;}
        public string Name { get; set;}
        public string content {get; set;} 
        public long FolderId { get; set;}
        public long TypeId { get; set;}
    }
}