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
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase();
            var context = new ApplicationDbContext(builder.Options);
            var files = Enumerable.Range(1, fileCount)
                            .Select(i => new File { Id=i, Name=$"Name{i}", Content=$"Content{i}" });
            context.AddRange(files);
            int changed = context.SaveChanges();
            _context = context;
        }

        private const int fileCount = 10;
        private ApplicationDbContext _context;

        public void Dispose()
        {
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
    }
}