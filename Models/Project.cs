using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public class Project
    {
        public long ID { get; set;}
        public long OwnerId { get; set;}
        public string name { get; set;}
        
    }
}