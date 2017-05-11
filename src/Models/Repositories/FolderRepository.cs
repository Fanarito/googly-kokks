using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;

namespace Kokks.Models
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _context;

        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Folder> GetAll()
        {
            return _context.Folders.ToList();
        }

        public void Add(Folder item)
        {
            _context.Folders.Add(item);
            _context.SaveChanges();
        }

        public Folder Create(string name, long? parentId, long projectID)
        {
            Folder folder = new Folder();
            folder.Name = name;
            folder.ParentID = parentId;
            folder.ProjectID = projectID;
            Add(folder);
            return folder;
        }

        public Folder Find(long id)
        {
            return _context.Folders.FirstOrDefault(p => p.Id == id);
        }

        public void Remove(long id)
        {
            var entity = _context.Folders.First(p => p.Id == id);
            _context.Folders.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Folder item)
        {
            _context.Folders.Update(item);
            _context.SaveChanges();
        }
    }
}