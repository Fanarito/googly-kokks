using System.Collections.Generic;

namespace Kokks.Models
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);
        IEnumerable<TodoItem> GetAll();
        IEnumerable<TodoItem> GetAllForUser(string uid);
        IEnumerable<TodoItem> GetAllForProject(long pid);
        TodoItem Find(int id);
        void Remove(int id);
        void Update(TodoItem item);
    }
}