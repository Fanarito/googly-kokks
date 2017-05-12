using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Kokks.Data;
using Kokks.Models;

namespace Kokks.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170512032056_FinalTableLayout")]
    partial class FinalTableLayout
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.3");

            modelBuilder.Entity("Kokks.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Kokks.Models.Collaborator", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Permission");

                    b.Property<long>("ProjectID");

                    b.Property<string>("UserID");

                    b.HasKey("Id");

                    b.HasIndex("ProjectID");

                    b.HasIndex("UserID");

                    b.ToTable("Collaborator");
                });

            modelBuilder.Entity("Kokks.Models.File", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("Name");

                    b.Property<long>("ParentID");

                    b.Property<int>("Syntax");

                    b.HasKey("Id");

                    b.HasIndex("ParentID");

                    b.ToTable("File");
                });

            modelBuilder.Entity("Kokks.Models.Folder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<long?>("ParentID");

                    b.Property<long>("ProjectID");

                    b.HasKey("Id");

                    b.HasIndex("ParentID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("Kokks.Models.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Kokks.Models.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<long>("ProjectID");

                    b.Property<string>("UserID");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectID");

                    b.ToTable("TodoItem");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Kokks.Models.Collaborator", b =>
                {
                    b.HasOne("Kokks.Models.Project", "Project")
                        .WithMany("Collaborators")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kokks.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Kokks.Models.File", b =>
                {
                    b.HasOne("Kokks.Models.Folder", "Parent")
                        .WithMany("Files")
                        .HasForeignKey("ParentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kokks.Models.Folder", b =>
                {
                    b.HasOne("Kokks.Models.Folder", "Parent")
                        .WithMany("Folders")
                        .HasForeignKey("ParentID");

                    b.HasOne("Kokks.Models.Project", "Project")
                        .WithMany("Folders")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kokks.Models.TodoItem", b =>
                {
                    b.HasOne("Kokks.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("Kokks.Models.Project", "Project")
                        .WithMany("TodoItems")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Kokks.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Kokks.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kokks.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
