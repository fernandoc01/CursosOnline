using System;
using Microsoft.EntityFrameworkCore;
using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions<CursosOnlineContext> options) : base(options){

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new{ci.InstructorId, ci.CursoId});
        modelBuilder.Entity<Precio>()
        .HasOne(a => a.Curso)
        .WithOne(b => b.precioPromocion)
        .HasForeignKey<Curso>(b => b.CursoId);
        }

        public DbSet<Comentario> Comentario{set;get;}

        public DbSet<Curso> Curso{set;get;}

        public DbSet<CursoInstructor> CursoInstructor{set;get;}

        public DbSet<Instructor> Instructor{set;get;}

        public DbSet<Precio> Precio{set;get;}

    }
}