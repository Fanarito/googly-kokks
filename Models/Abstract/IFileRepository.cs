using System.Collections.Generic;

namespace Kokks.Models
{
    public interface IFileRepository
    {
        void Add(File item);
        void Create(long parentId, long syntaxId, string name, string content);
        IEnumerable<File> GetAll();
        File Find(long id);
        void Remove(long id);
        void Update(File item);
    }
}