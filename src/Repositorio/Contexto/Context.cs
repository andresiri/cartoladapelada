﻿using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Reflection;

namespace Repositorio.Contexto
{
    public class Context : DbContext
    {
        private IHttpContextAccessor _context;
        public Context(DbContextOptions<Context> options, IHttpContextAccessor context) : base(options)
        {
            _context = context;
        }        
        public DbSet<User> User { get; set; }
        public DbSet<Pelada> Pelada { get; set; }
        public DbSet<PeladaUser> PeladaUser { get; set; }
        public DbSet<Arena> Arena { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureUser(builder);
            ConfigurePelada(builder);
            ConfigurePeladaUser(builder);
            ConfigureArena(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("user");
                b.HasKey(u => u.Id);
                b.Property(u => u.Id).HasColumnName("id");
                b.Property(u => u.FirstName).IsRequired().HasMaxLength(100).HasColumnName("firstName");
                b.Property(u => u.LastName).HasMaxLength(100).HasColumnName("lastName");
                b.Property(u => u.Email).IsRequired().HasMaxLength(100).HasColumnName("email");
                b.Property(u => u.Password).IsRequired().HasMaxLength(100).HasColumnName("password");
                b.Property(u => u.Nickname).HasMaxLength(100).HasColumnName("nickname");
                b.Property(u => u.Number).HasColumnName("number");
                b.Property(u => u.Position).HasMaxLength(100).HasColumnName("position");
                b.Property(u => u.CreatedAt).IsRequired().HasColumnName("createdAt");
            });
        }

        private static void ConfigurePelada(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pelada>(b =>
            {
                b.ToTable("pelada");
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.Description).HasColumnName("description").HasMaxLength(50);
                b.Property(p => p.CreatedByUserId).HasColumnName("createdByUserId");
                b.Property(p => p.CreatedAt).IsRequired().HasColumnName("createdAt");
                b.HasOne(p => p.CreatedByUser).WithMany(p => p.Peladas).OnDelete(DeleteBehavior.Restrict).HasForeignKey(p => p.CreatedByUserId).HasConstraintName("ForeignKey_Pelada_UserId");
            });
        }

        private static void ConfigurePeladaUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeladaUser>(b =>
            {
                b.ToTable("peladaUser");
                b.HasKey(p => p.Id);
                b.HasKey(p => new { p.PeladaId, p.UserId });                
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.PeladaId).IsRequired().HasColumnName("peladaId");
                b.Property(p => p.UserId).IsRequired().HasColumnName("userId");
                b.Property(p => p.CreatedAt).IsRequired().HasColumnName("createdAt");
                b.HasOne(p => p.User).WithMany(p => p.PeladaUsers).HasForeignKey(p => p.UserId).HasConstraintName("ForeignKey_PeladaUser_UserId");
                b.HasOne(p => p.Pelada).WithMany(p => p.PeladaUsers).HasForeignKey(p => p.PeladaId).HasConstraintName("ForeignKey_PeladaUser_PeladaId");
            });
        }

        private static void ConfigureArena(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arena>(b =>
            {
                b.ToTable("arena");
                b.HasKey(a => a.Id);
                b.Property(a => a.Id).HasColumnName("id");
                b.Property(a => a.Description).IsRequired().HasMaxLength(100).HasColumnName("description");
                b.Property(a => a.Latitude).HasMaxLength(30).HasColumnName("latitude");
                b.Property(a => a.Longitude).HasMaxLength(30).HasColumnName("longitude");
            });
        }

        public override int SaveChanges()
        {            
            var modifiedEntries = ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);            

            foreach (EntityEntry<BaseEntity> entry in modifiedEntries)
            {                
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;                    

                    var createdByUserId = entry.Entity.GetType().GetRuntimeProperties().Where(w => w.Name.Equals("CreatedByUserId")).FirstOrDefault();
                    if (createdByUserId != null && _context.HttpContext != null)
                    {
                        var userId = Convert.ToInt32(_context.HttpContext.User.Claims.Where(w => w.Type.Equals("id")).First().Value);
                        createdByUserId.SetValue(entry.Entity, userId);
                    }
                }                               
            }

            return base.SaveChanges();
        }
    }
}
