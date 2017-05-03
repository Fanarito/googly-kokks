using System;
using System.Collections.Generic;

namespace Kokks.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public string UserID { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}