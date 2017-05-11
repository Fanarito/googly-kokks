using System;
using Xunit;
using Kokks.Models;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Tests
{
    public class ProjectRepositoryTests : IDisposable
    {
        public ProjectRepositoryTests()
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

            // Add test Folders
            var collaborators = Enumerable.Range(1, collaboratorCount)
                .Select(i => new Collaborator
                        {
                            Id = i,
                            UserID = i.ToString(),
                            ProjectID = i
                        });
            context.AddRange(collaborators);

            int changed = context.SaveChanges();
            _context = context;
        }

        private const int projectCount = 10;
        private const int usersCount = 10;
        private const int collaboratorCount = 10;
        private readonly ApplicationDbContext _context;

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [Fact]
        public void GetAll()
        {
            var repo = new ProjectRepository(_context);
            var res = repo.GetAll();
            Assert.Equal(projectCount, res.Count());
        }

        [Fact]
        public void GetAllForUser()
        {
            const string userId = "1";
            const int expectedCount = 1;
            var repo = new ProjectRepository(_context);
            var res = repo.GetAllForUser(userId);
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void UserHasAccessTrue()
        {
            const string userId = "1";
            const long projectId = 1;
            var repo = new ProjectRepository(_context);
            var res = repo.UserHasAccess(projectId, userId);
            Assert.Equal(true, res);
        }

        [Fact]
        public void UserHasAccessFalse()
        {
            const string userId = "2";
            const long projectId = 3;
            var repo = new ProjectRepository(_context);
            var res = repo.UserHasAccess(projectId, userId);
            Assert.Equal(false, res);
        }

        [Fact]
        public void Add()
        {
            const int expectedCount = projectCount + 1;
            var project = new Project {
                Name = "NewProject"
            };
            var repo = new ProjectRepository(_context);
            repo.Add(project);
            var res = repo.GetAll();
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void Find()
        {
            const long projectId = 1;
            var repo = new ProjectRepository(_context);
            var res = repo.Find(projectId);
            Assert.Equal(projectId, res.Id);
        }

        [Fact]
        public void Remove()
        {
            const int expectedCount = projectCount - 1;
            const long projectId = 1;
            var repo = new ProjectRepository(_context);
            repo.Remove(projectId);
            var res = repo.GetAll();
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void RemovedCorrectProject()
        {
            const int expectedCount = 0;
            const long projectId = 1;
            var repo = new ProjectRepository(_context);
            repo.Remove(projectId);
            var res = repo.GetAll().Where(p => p.Id == projectId);
            Assert.Equal(expectedCount, res.Count());
        }

        [Fact]
        public void Update()
        {
            const long projectId = 1;
            var repo = new ProjectRepository(_context);
            var project = repo.Find(projectId);
            project.Name = "NewName";
            var updatedProject = repo.Find(projectId);
            Assert.Equal(project, updatedProject);
        }
    }
}
