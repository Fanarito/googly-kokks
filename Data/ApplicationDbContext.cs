using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Kokks.Models;

namespace Kokks.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Collaborator> Collaborator { get; set; }
        public DbSet<Project> Project { get; set;}
        public DbSet<File> File { get; set;}
        public DbSet<Folder> Folder { get; set;}
        public DbSet<Permission> Permission { get; set;}
        public DbSet<Types> Type { get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
