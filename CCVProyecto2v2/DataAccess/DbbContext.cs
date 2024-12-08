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
                entity.Property(c => c.Grado).HasMaxLength(50);
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
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Estudiantes)
                .WithMany(c => c.Clases)
                .UsingEntity<Dictionary<string, object>>(
                    "ClaseEstudiante",
                    c => c.HasOne<Estudiante>()
                    .WithMany()
                    .HasForeignKey("EstudianteId")
                    .OnDelete(DeleteBehavior.Cascade),
                    c => c.HasOne<Clase>()
                    .WithMany()
                    .HasForeignKey("ClaseId")
                    .OnDelete(DeleteBehavior.Cascade));
            });


        }
    }
}
