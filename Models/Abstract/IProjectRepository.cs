using System.Collections.Generic;

namespace Kokks.Models
{
    public interface IProjectRepository
    {
        void Add(Project item);
        IEnumerable<Project> GetAll();
        IEnumerable<Project> GetAllForUser(string uid);
        Project Find(long id);
        void Remove(long id);
        void Update(Project item);
    }
}