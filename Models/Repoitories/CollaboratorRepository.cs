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
            return _context.Collaborator.ToList();
        }
        public void Add(Collaborator item)
        {
            _context.Collaborator.Add(item);
            _context.SaveChanges();
        }

        public Collaborator Find(long id)
        {
            return _context.Collaborator.FirstOrDefault(c => c.UserId == id);
        }

        public void Remove(long id)
        {
            var entity = _context.Collaborator.First(c => c.UserId == id);
            _context.Collaborator.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Collaborator item)
        {
            _context.Collaborator.Update(item);
            _context.SaveChanges();
        }

    }
}