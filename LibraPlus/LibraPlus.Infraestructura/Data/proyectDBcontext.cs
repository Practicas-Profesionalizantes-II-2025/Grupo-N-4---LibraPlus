using LibraPlus.Dominio.Entidades;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Data
{
    public class ProyectDBContext : DbContext
    {
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Recomendaciones> Recomendaciones { get; set; }
        public DbSet<Reseñas> Reseñas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Administradores> Admins { get; set; }

        public ProyectDBContext(DbContextOptions<ProyectDBContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔒 Deshabilitar eliminación en cascada por defecto
            var relationships = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            // ✅ Configuración explícita de relaciones principales

            // 📚 Libros → Compras (1:N)
            modelBuilder.Entity<Libros>()
                .HasMany(l => l.Compras)
                .WithOne(c => c.Libro)
                .HasForeignKey(c => c.LibroID)
                .OnDelete(DeleteBehavior.Cascade);

            // 👤 Usuarios → Compras (1:N)
            modelBuilder.Entity<Usuarios>()
                .HasMany(u => u.Compras)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioID)
                .OnDelete(DeleteBehavior.Cascade);

            // ✍️ Libros → Reseñas (1:N)
            modelBuilder.Entity<Libros>()
                .HasMany(l => l.Reseñas)
                .WithOne(r => r.Libro)
                .HasForeignKey(r => r.LibroID)
                .OnDelete(DeleteBehavior.Cascade);

            // 👥 Usuarios → Reseñas (1:N)
            modelBuilder.Entity<Usuarios>()
                .HasMany(u => u.Reseñas)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioID)
                .OnDelete(DeleteBehavior.Cascade);

            // Opcional: Nombres explícitos de tablas
            modelBuilder.Entity<Usuarios>().ToTable("Usuarios");
            modelBuilder.Entity<Compras>().ToTable("Compras");
            modelBuilder.Entity<Libros>().ToTable("Libros");
            modelBuilder.Entity<Prestamos>().ToTable("Prestamos");
            modelBuilder.Entity<Recomendaciones>().ToTable("Recomendaciones");
            modelBuilder.Entity<Reseñas>().ToTable("Reseñas");
            modelBuilder.Entity<Administradores>().ToTable("Administradores");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 💾 Punto central para auditoría o lógica previa a guardar
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
