﻿using Microsoft.EntityFrameworkCore;
using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.DbContexts
{
    public class ProdDbContext : DbContext
    {
        public ProdDbContext()
        {
        }

        public ProdDbContext(DbContextOptions opt) : base(opt)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.IdUser);
                user.Property(u => u.UserTag).IsRequired();
                user.Property(u => u.Email).IsRequired().HasMaxLength(50);
                user.Property(u => u.Login).IsRequired().HasMaxLength(50);
                user.Property(u => u.Password).IsRequired().HasMaxLength(75);
                user.Property(u => u.PasswordSalt).IsRequired().HasMaxLength(10);
                user.Property(u => u.RegisteredAt).IsRequired();
                user.Property(u => u.RefreshToken).HasMaxLength(255);
            });
        }

        public DbSet<User> Users { get; set; }
    }
}
