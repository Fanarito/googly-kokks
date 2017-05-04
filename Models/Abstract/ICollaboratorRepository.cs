using System.Collections.Generic;

namespace Kokks.Models
{
    public interface ICollaboratorRepository
    {
        void Add(Collaborator item);
        IEnumerable<Collaborator> GetAll();
        Collaborator Find(long id);
        void Remove(long id);
        void Update(Collaborator item);
    }
}