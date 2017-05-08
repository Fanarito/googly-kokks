using System.Collections.Generic;

namespace Kokks.Models
{
    public interface IFolderRepository
    {
        void Add(Folder item);
        IEnumerable<Folder> GetAll();
        Folder Find(long id);
        void Remove(long id);
        void Update(Folder item);
    }
}