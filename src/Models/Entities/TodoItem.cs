using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kokks.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public string UserID { get; set; }
        public long ProjectID { get; set; }

        public ApplicationUser Owner { get; set; }
        public Project Project { get; set; }
    }
}
