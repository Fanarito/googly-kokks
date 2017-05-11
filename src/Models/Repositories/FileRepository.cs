using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;

namespace Kokks.Models
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<File> GetAll()
        {
            return _context.Files.ToList();
        }

        public void Add(File item)
        {
            _context.Files.Add(item);
            _context.SaveChanges();
        }

        public void Create(long parentId, Syntax syntax, string name, string content)
        {
            File file = new File();
            file.ParentID = parentId;
            file.Syntax = syntax;
            file.Name = name;
            file.Content = content;
            Add(file);
        }

        public File Find(long id)
        {
            return _context.Files
                .Include(f => f.Parent)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Remove(long id)
        {
            var entity = _context.Files.First(p => p.Id == id);
            _context.Files.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(File item)
        {
            _context.Files.Update(item);
            _context.SaveChanges();
        }
    }
}