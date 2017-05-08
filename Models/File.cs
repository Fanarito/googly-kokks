using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kokks.Models
{
    public enum Syntax {
        JavaScript,
        HTML,
        CSS,
        CPlusPlus,
        C,
        CSharp
    }
    public class File
    {
        public long Id { get; set; }
        public long ParentID { get; set; }
        public long SyntaxID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public Syntax Syntax { get; set; }
        public Folder Parent { get; set; }
    }
}