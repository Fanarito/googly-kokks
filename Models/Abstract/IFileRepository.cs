using System.Collections.Generic;

namespace Kokks.Models
{
    public interface IFileRepository
    {
        void Add(File item);
        IEnumerable<File> GetAll();
        File Find(long id);
        void Remove(long id);
        void Update(File item);
    }
}