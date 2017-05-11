using System;
using Xunit;
using Kokks.Models;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Tests
{
    public class FileRepositoryTests : IDisposable
    {
        public FileRepositoryTests()
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

            // Add folder for the files to use
            var folder = new Folder { Name = "TestFolder", ProjectID = project.Id };
            context.Add(folder);

            // Add test files
            var files = Enumerable.Range(1, fileCount)
                        .Select(i => new File 
                        {
                            Id= i,
                            Name=$"Name{i}",
                            Content=$"Content{i}",
                            ParentID=folder.Id
                        });
            context.AddRange(files);
            int changed = context.SaveChanges();
            _context = context;
        }

        private const int fileCount = 10;
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
            var repo = new FileRepository(_context);
            var res = repo.Find(5);
            Assert.Equal(expectedName, res.Name);
        }

        [Fact]
        public void GetAll()
        {
            const int expectedLength = fileCount;
            var repo =  new FileRepository(_context);
            var res = repo.GetAll();
            Assert.Equal(expectedLength, res.Count());
        }

        [Fact]
        public void Remove()
        {
            const int itemId = 1;
            const int expectedLength = fileCount - 1;
            var repo = new FileRepository(_context);
            repo.Remove(itemId);
            var res = repo.GetAll();
            Assert.Equal(expectedLength, res.Count());
        }
    }
}