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
        
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Project> Projects { get; set;}
        public DbSet<File> Files { get; set;}
        public DbSet<Folder> Folders { get; set;}
        public DbSet<Permission> Permissions { get; set;}
        public DbSet<Syntax> Syntaxes { get; set;}
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().ToTable("Project");
            builder.Entity<File>().ToTable("File");
            builder.Entity<Folder>().ToTable("Folder");
            builder.Entity<Permission>().ToTable("Permission");
            builder.Entity<Syntax>().ToTable("Syntax");
            builder.Entity<TodoItem>().ToTable("TodoItem");
            builder.Entity<Collaborator>().ToTable("Collaborator");

            builder.Entity<Collaborator>()
                .HasKey(c => new { c.UserID, c.ProjectID });
        }
    }
}
