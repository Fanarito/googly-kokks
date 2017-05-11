using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Collaborator> GetAllConnectedToUser(string uid)
        {
            var collabs = _context.Collaborators.Where(c => c.UserID == uid)
                                                .Include(c => c.Project);
            
            // Go through all connected projects and return all connected users
            var connected = new List<Collaborator>();
            foreach (var collab in collabs)
            {
                connected.Concat(collab.Project.Collaborators.Where(c => c.UserID != uid).ToList());
            }
            return connected;
        }

        public void Add(Collaborator item)
        {
            _context.Collaborators.Add(item);
            _context.SaveChanges();
        }

        public void Create(string userId, long projectID, Permissions permission)
        {
            var col = new Collaborator();
            col.UserID = userId;
            col.ProjectID = projectID;
            col.Permission = permission;
            Add(col);
        }

        public Collaborator Find(long id)
        {
            return _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);
        }

        public Collaborator Find(long projectID, string userId)
        {
            return _context.Collaborators.FirstOrDefault(c => c.ProjectID == projectID && c.UserID == userId);
        }

        public IEnumerable<Collaborator> FindForProject(long id)
        {
            return _context.Collaborators.Where(c => c.ProjectID == id);
        }

        public bool AlreadyConnected(long projectID, string userId)
        {
            return _context.Collaborators.Any(c => c.ProjectID == projectID && c.UserID == userId);
        }

        public void Remove(long id)
        {
            var entity = _context.Collaborators.First(c => c.Id== id);
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