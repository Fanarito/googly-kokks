using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kokks.Models
{
    public enum Syntax {
        JavaScript,
        HTML,
        CSS,
        CPP,
        CSharp,
        Python
    }
    public class File
    {
        public long Id { get; set; }
        public long ParentID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Syntax Syntax { get; set; }
        public Folder Parent { get; set; }
    }
}