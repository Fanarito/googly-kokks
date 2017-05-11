using System;
using Xunit;
using Kokks.Models;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Tests
{
    public class CollaboratorRepositoryTests : IDisposable
    {
        public CollaboratorRepositoryTests()
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
            var users = Enumerable.Range(1, usersAndProjectCount)
                .Select(i => new ApplicationUser
                        {
                            Id = i.ToString(),
                            UserName = $"UserName{i}"
                        });
            context.AddRange(users);

            // Add projects for the collaborators to use
            var projects = Enumerable.Range(1, usersAndProjectCount)
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

        private const int usersAndProjectCount = 10;
        private const int collaboratorCount = 10;
        private readonly ApplicationDbContext _context;

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [Fact]
        public void FindById()
        {
            const long collabId = 5;
            var repo = new CollaboratorRepository(_context);
            var res = repo.Find(collabId);
            Assert.Equal(collabId, res.Id);
        }

        [Fact]
        public void FindByProjectAndUser()
        {
            const long expectedCollabId = 1;
            const long projectId = 1;
            const string userId = "1";
            var repo = new CollaboratorRepository(_context);
            var res = repo.Find(projectId, userId);
            Assert.Equal(expectedCollabId, res.Id);
        }

        [Fact]
        public void FindForProject()
        {
            const long projectId = 5;
            const long expectedLength = 1;
            var repo = new CollaboratorRepository(_context);
            var res = repo.FindForProject(projectId);
            Assert.Equal(expectedLength, res.Count());
        }

        [Fact]
        public void AlreadyConnected()
        {
            const long projectId = 4;
            const string userId = "4";
            var repo = new CollaboratorRepository(_context);
            var res = repo.AlreadyConnected(projectId, userId);
            Assert.Equal(true, res);
        }

        [Fact]
        public void GetAll()
        {
            const long expectedLength = collaboratorCount;
            var repo = new CollaboratorRepository(_context);
            var res = repo.GetAll();
            Assert.Equal(expectedLength, res.Count());
        }

        [Fact]
        public void Create()
        {
            const long projectId = 2;
            const string userId = "8";
            const Permissions permission = Permissions.ReadWrite;
            var repo = new CollaboratorRepository(_context);
            repo.Create(userId, projectId, permission);
            var res = repo.AlreadyConnected(projectId, userId);
            Assert.Equal(true, res);
        }

        [Fact]
        public void Add()
        {
            const long projectId = 2;
            const string userId = "8";
            const Permissions permission = Permissions.ReadWrite;
            var collab = new Collaborator {
                ProjectID = projectId,
                UserID = userId,
                Permission = permission
            };
            var repo = new CollaboratorRepository(_context);
            repo.Add(collab);
            var res = repo.AlreadyConnected(projectId, userId);
            Assert.Equal(true, res);
        }

        [Fact]
        public void Remove()
        {
            const long collabId = 1;
            const long projectId = 1;
            const string userId = "1";
            var repo = new CollaboratorRepository(_context);
            repo.Remove(collabId);
            var res = repo.AlreadyConnected(projectId, userId);
            Assert.Equal(false, res);
        }

        [Fact]
        public void Update()
        {
            const long collabId = 1;
            var repo = new CollaboratorRepository(_context);
            var collab = repo.Find(collabId);
            collab.Permission = Permissions.Read;
            repo.Update(collab);
            var updatedCollab = repo.Find(collabId);
            Assert.Equal(collab, updatedCollab);
        }
    }
}
