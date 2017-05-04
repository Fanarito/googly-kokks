using System.Collections.Generic;

namespace Kokks.Models
{
    public interface ICollaboratorRepository
    {
        void Add(Collaborator item);
        void Create(string userId, long projectId, Permissions permissions);
        IEnumerable<Collaborator> GetAll();
        IEnumerable<Collaborator> FindForProject(long id);
        void Remove(string id);
        void Update(Collaborator item);
    }
}