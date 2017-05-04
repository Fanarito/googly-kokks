using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;

namespace Kokks.Models
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly ApplicationDbContext _context;

        public CollaboratorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Collaborator> GetAll()
        {
            return _context.Collaborators.ToList();
        }

        public void Add(Collaborator item)
        {
            _context.Collaborators.Add(item);
            _context.SaveChanges();
        }

        public void Create(string userId, long projectId, Permissions permission)
        {
            var col = new Collaborator();
            col.UserID = userId;
            col.ProjectID = projectId;
            col.Permission = permission;
            Add(col);
        }

        public IEnumerable<Collaborator> FindForProject(long id)
        {
            return _context.Collaborators.Where(c => c.ProjectID == id);
        }

        public void Remove(string id)
        {
            var entity = _context.Collaborators.First(c => c.UserID == id);
            _context.Collaborators.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Collaborator item)
        {
            _context.Collaborators.Update(item);
            _context.SaveChanges();
        }

    }
}