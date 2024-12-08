using CCVProyecto2v2.Models;
using CCVProyecto2v2.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace CCVProyecto2v2.DataAccess
{
    public class DbbContext : DbContext
    {
        public DbSet<Profesor> Profesor { get; set; }
        public DbSet<Estudiante> Estudiante { get; set; }
        public DbSet<Clase> Clase { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename ={ConexionDB.DevolverRuta("CrhysteProyecto.db")}";
            Console.WriteLine($"Ruta de la base de datos: {ConexionDB.DevolverRuta("CrhysteProyecto.db")}");

            optionsBuilder.UseSqlite(conexionDB);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.Property(e => e.Grado).HasMaxLength(50);
            });
            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasOne(e => e.Clase)
                  .WithOne(c => c.Estudiante)
                 .HasForeignKey<Estudiante>(e => e.ClaseId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Profesor>(entity =>
            {

                entity.Property(p => p.Materia).HasMaxLength(100);
            });
            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.HasOne(p => p.Clase)
                .WithOne(c => c.Profesor)
                .HasForeignKey<Profesor>(p => p.ClaseId)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Clase>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            });



        }
    }
}
