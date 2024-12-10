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
        public DbSet<Usuario> Usuarios { get; set; }

        public DbbContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename ={ConexionDB.DevolverRuta("CCVProyecto2.db")}";

            optionsBuilder.UseSqlite(conexionDB);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estudiante>().ToTable("Estudiantes");
            modelBuilder.Entity<Profesor>().ToTable("Profesores");

            // Configuración de clave primaria en la clase base Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.NombreUsuario).IsRequired();
                entity.Property(u => u.Contrasenia).IsRequired();
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.Property(e => e.Grado).HasMaxLength(50);
            });
        }


    }
}
