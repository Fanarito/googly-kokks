using System.Collections.Generic;

namespace Kokks.Models
{
    public interface ICollaboratorRepository
    {
        void Add(Collaborator item);
        void Create(string userId, long projectId, Permissions permissions);
        IEnumerable<Collaborator> GetAll();
        IEnumerable<Collaborator> GetAllConnectedToUser(string uid);
        Collaborator Find(long id);
        Collaborator Find(long projectId, string userId);
        IEnumerable<Collaborator> FindForProject(long id);
        bool AlreadyConnected(long projectId, string userId);
        void Remove(long id);
        void Update(Collaborator item);
    }
}