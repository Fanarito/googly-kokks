using System.Collections.Generic;

namespace Kokks.Models
{
    public interface ICollaboratorRepository
    {
        void Add(Collaborator item);
        void Create(string userId, long projectID, Permissions permissions);
        IEnumerable<Collaborator> GetAll();
        Collaborator Find(long id);
        Collaborator Find(long projectID, string userId);
        IEnumerable<Collaborator> FindForProject(long id);
        bool AlreadyConnected(long projectId, string userId);
        void Remove(long id);
        void Update(Collaborator item);
    }
}
