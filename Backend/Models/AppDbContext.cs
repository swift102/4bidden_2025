using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Backend.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<DiagnosticTest> DiagnosticTests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: Call base method first to configure Identity entities
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}