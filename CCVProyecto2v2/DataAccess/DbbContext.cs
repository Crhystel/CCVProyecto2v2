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
        public DbSet<ClaseEstudiante> ClaseEstudiantes { get; set; }
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
                entity.Property(c => c.Grado).HasMaxLength(50);
            });
            modelBuilder.Entity<ClaseEstudiante>(entity =>
            {
                entity.HasKey(c =>c.Id);
            });
            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(c => c.Materia).HasMaxLength(100);
            });

            modelBuilder.Entity<Clase>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(c => c.Nombre).HasMaxLength(100);
            });
            modelBuilder.Entity<Clase>(entity =>
            {
                entity.HasOne(c => c.Profesor)
                .WithMany(c => c.Clases)
                .HasForeignKey(c => c.ProfesorId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ClaseEstudiante>()
                .HasOne(c => c.Clase)
                .WithMany(c => c.ClasesEstudiantes)
                .HasForeignKey(c => c.ClaseId);
            modelBuilder.Entity<ClaseEstudiante>()
                .HasOne(c=>c.Estudiante)
                .WithMany(c=>c.ClasesEstudiantes)
                .HasForeignKey(c=>c.EstudianteId);
        }
    }
}
