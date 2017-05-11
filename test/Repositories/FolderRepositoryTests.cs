using System;
using Xunit;
using Kokks.Models;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Tests
{
    public class FolderRepositoryTests : IDisposable
    {
        public FolderRepositoryTests()
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

            // Add project for the folder to use
            var project = new Project { Name = "TestProject" };
            context.Add(project);

            // Add test Folders
            var folders = Enumerable.Range(1, folderCount)
                        .Select(i => new Folder 
                        {
                            Id = i,
                            ProjectID = project.Id,
                            Name = $"Name{i}"
                        });

            context.AddRange(folders);
            int changed = context.SaveChanges();
            _context = context;
        }

        private const int folderCount = 10;
        private readonly ApplicationDbContext _context;

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [Fact]
        public void FindById()
        {
            const string expectedName = "Name5";
            var repo = new FolderRepository(_context);
            var res = repo.Find(5);
            Assert.Equal(expectedName, res.Name);
        }

        [Fact]
        public void GetAll()
        {
            const int expectedLength = folderCount;
            var repo =  new FolderRepository(_context);
            var res = repo.GetAll();
            Assert.Equal(expectedLength, res.Count());
        }

        [Fact]
        public void RemoveRemovedFolder()
        {
            const int itemId = 1;
            const int expectedLength = folderCount - 1;
            var repo = new FolderRepository(_context);
            repo.Remove(itemId);
            var res = repo.GetAll();
            Assert.Equal(expectedLength, res.Count());
        }

        [Fact]
        public void CreateCorrectlyAddedFolder()
        {
            const long parentId = 1;
            const long projectId = 1;
            const string name = "NewFolderTest";
            const int expectedLength = folderCount + 1;
            
            var repo = new FolderRepository(_context);
            repo.Create(name, parentId, projectId);
            var res = repo.GetAll();
            Assert.Equal(expectedLength, res.Count());
        }

        [Fact]
        public void AddCorrectlyAddedFolder()
        {
            var Folder = new Folder {
                ProjectID = 1,
                ParentID = 1,
                Name = "NewNestedTestFolder"
            };

            var repo = new FolderRepository(_context);
            repo.Add(Folder);
            var res = repo.Find(Folder.Id);
            Assert.Equal(Folder, res);
        }

        [Fact]
        public void UpdateUpdatesFolder()
        {
            const long FolderId = 4;
            const string updatedName = "UpdatedName";
            var repo = new FolderRepository(_context);

            var Folder = repo.Find(FolderId);
            Folder.Name = updatedName;
            repo.Update(Folder);

            var updatedFolder = repo.Find(FolderId);

            Assert.Equal(Folder, updatedFolder);
        }
    }
}