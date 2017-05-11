using System;
using Xunit;
using Kokks.Models;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Kokks.Services;

namespace Tests
{
    public class PermissionServicesTests : IDisposable
    {
        public PermissionServicesTests()
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

            // Create test users for all permissions
            foreach(Permissions permission in Enum.GetValues(typeof(Permissions)))
            {
                var user = new ApplicationUser
                {
                    Id = permission.ToString(),
                    UserName = $"{permission}User"
                };
                context.Add(user);
            }
            // Create test project
            var project = new Project
            {
                Id = 1,
                Name = "TestProject"
            };
            context.Add(project);

            // Create collaborators with all types of permissions
            long collabId = 1;
            foreach(Permissions permission in Enum.GetValues(typeof(Permissions)))
            {
                var collaborator = new Collaborator
                {
                    Id = collabId++,
                    ProjectID = project.Id,
                    UserID = permission.ToString(),
                    Permission = permission
                };
                context.Add(collaborator);
            }


            int changed = context.SaveChanges();
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [Fact]
        public void CollaboratorHasAccessOwner()
        {
            var collabRepo = new CollaboratorRepository(_context);
            var service = new PermissionServices(collabRepo);
            var collab = collabRepo.Find(1);
            bool res = service.HasWriteAccess(collab);
            Assert.Equal(true, res);
        }

        [Fact]
        public void CollaboratorHasAccessReadWrite()
        {
            var collabRepo = new CollaboratorRepository(_context);
            var service = new PermissionServices(collabRepo);
            var collab = collabRepo.Find(2);
            bool res = service.HasWriteAccess(collab);
            Assert.Equal(true, res);
        }

        [Fact]
        public void CollaboratorHasAccessRead()
        {
            var collabRepo = new CollaboratorRepository(_context);
            var service = new PermissionServices(collabRepo);
            var collab = collabRepo.Find(3);
            bool res = service.HasWriteAccess(collab);
            Assert.Equal(false, res);
        }
    }
}
