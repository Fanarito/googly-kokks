using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kokks.Models
{
    public enum Permissions
    {
        Owner,
        ReadWrite,
        Read
    }

    public class Collaborator
    {
        public long Id { get; set; }
        public string UserID { get; set; }
        public long ProjectID { get; set; }

        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Permissions Permission { get; set; }
    }

}