using System;
using Xunit;
using Kokks.Models;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Tests
{
    public class TodoRepositoryTests : IDisposable
    {
        public TodoRepositoryTests()
        {
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            DbContextOptions<ApplicationDbContext> options;
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlite(connection);
            options = builder.Options;
            var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Add users for the collaborators to use
            var users = Enumerable.Range(1, usersCount)
                .Select(i => new ApplicationUser
                        {
                            Id = i.ToString(),
                            UserName = $"UserName{i}"
                        });
            context.AddRange(users);

            // Add projects for the collaborators to use
            var projects = Enumerable.Range(1, projectCount)
                .Select(i => new Project
                        {
                            Id = i,
                            Name = $"Project{i}"
                        });
            context.AddRange(projects);

            var todos = Enumerable.Range(1, todoCount)
                .Select(i => new TodoItem
                        {
                            Id = i,
                            Name = $"Todo{i}",
                            IsComplete = false,
                            UserID = i.ToString(),
                            ProjectID = i
                        });
            context.AddRange(todos);

            int changed = context.SaveChanges();
            _context = context;
        }

        private const int projectCount = 10;
        private const int usersCount = 10;
        private const int todoCount = 10;
        private readonly ApplicationDbContext _context;

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [Fact]
        public void GetAll()
        {
            var repo = new TodoRepository(_context);
            var res = repo.GetAll();
            Assert.Equal(todoCount, res.Count());
        }

        [Fact]
        public void GetAllForUser()
        {
            const int expectedCount = 1;
            const string userId = "1";
            var repo = new TodoRepository(_context);
            var res = repo.GetAllForUser(userId);
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void GetAllForProject()
        {
            const int expectedCount = 1;
            const long projectId = 1;
            var repo = new TodoRepository(_context);
            var res = repo.GetAllForProject(projectId);
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void Add()
        {
            const int expectedCount = todoCount + 1;
            var todo = new TodoItem {
                Name = "NewTodo",
                UserID = "1",
                ProjectID = 1
            };
            var repo = new TodoRepository(_context);
            repo.Add(todo);
            var res = repo.GetAll();
            Assert.Equal(expectedCount, res.Count());

        }

        [Fact]
        public void Find()
        {
            const int todoId = 1;
            var repo = new TodoRepository(_context);
            var res = repo.Find(todoId);
            Assert.Equal(todoId, res.Id);
        }

        [Fact]
        public void Remove()
        {
            const int expectedCount = todoCount - 1;
            const int todoId = 1;
            var repo = new TodoRepository(_context);
            repo.Remove(todoId);
            var res = repo.GetAll();
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void RemoveCorrectTodo()
        {
            const int expectedCount = 0;
            const int todoId = 1;
            var repo = new TodoRepository(_context);
            repo.Remove(todoId);
            var res = repo.GetAll().Where(t => t.Id == todoId);
            Assert.Equal(expectedCount, res.Count());
        }
    }
}
