using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;

namespace Kokks.Models
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        public IEnumerable<TodoItem> GetAllForUser(string uid)
        {
            return _context.TodoItems.Where(t => t.UserID == uid).ToList();
        }
        public IEnumerable<TodoItem> GetAllForProject(long pid)
        {
            return _context.TodoItems.Where(t => t.ProjectID == pid).ToList();
        }
        public void Add(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();
        }

        public TodoItem Find(int id)
        {
            return _context.TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public void Remove(int id)
        {
            var entity = _context.TodoItems.First(t => t.Id == id);
            _context.TodoItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(TodoItem item)
        {
            _context.TodoItems.Update(item);
            _context.SaveChanges();
        }
    }
}