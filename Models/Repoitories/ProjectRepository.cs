using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;

namespace Kokks.Models
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            return _context.Projects.ToList();
        }

        public IEnumerable<Project> GetAllForUser(string uid)
        {
            var projects = (from c in _context.Collaborators
                            join p in _context.Projects on c.ProjectID equals p.Id
                            where c.UserID == uid
                            select p)
                            .Include(p => p.Collaborators)
                                .ThenInclude(c => c.User)
                            .AsNoTracking();
            return projects.ToList();
        }

        public bool UserHasAccess(long projectId, string userId)
        {
            bool hasAccess = _context.Collaborators.Any(c => c.ProjectID == projectId && c.UserID == userId);
            return hasAccess;
        }
        
        public void Add(Project item)
        {
            _context.Projects.Add(item);
            _context.SaveChanges();
        }

        public Project Find(long id)
        {
            return _context.Projects
                .Include(p => p.Collaborators)
                    .ThenInclude(c => c.User)
                .Include(p => p.Folders)
                    .ThenInclude(f => f.Files)
                .Include(p => p.Folders)
                    .ThenInclude(f => f.Folders)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Remove(long id)
        {
            var entity = _context.Projects.First(p => p.Id == id);
            _context.Projects.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Project item)
        {
            _context.Projects.Update(item);
            _context.SaveChanges();
        }
    }
}