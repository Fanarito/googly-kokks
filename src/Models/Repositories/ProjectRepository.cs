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

        public bool UserHasAccess(long projectID, string userId)
        {
            bool hasAccess = _context.Collaborators.Any(c => c.ProjectID == projectID && c.UserID == userId);
            return hasAccess;
        }
        
        public void Add(Project item)
        {
            _context.Projects.Add(item);
            _context.SaveChanges();
        }

        public Project Find(long id)
        {
            var project = _context.Projects
                .Where(p => p.Id == id)
                .Include(p => p.Collaborators)
                    .ThenInclude(c => c.User)
                .Include(p => p.Folders)
                    .ThenInclude(f => f.Files)
                .Include(p => p.Folders)
                    .ThenInclude(f => f.Folders)
                .Include(p => p.TodoItems)
                .FirstOrDefault(p => p.Id == id);

            // Remove folders that are not top level
            project.Folders = project.Folders.Where(f => f.Parent == null).ToList();
            return project;
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