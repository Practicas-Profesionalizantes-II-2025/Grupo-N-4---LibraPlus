using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Data
{
    public class ProyectDBContext : DbContext
    {
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Recomendaciones> Recomendaciones { get; set; }
        public DbSet<Reseñas> Resenias { get; set; }  
        public DbSet<Usuarios> Usuarios { get; set; }

        public ProyectDBContext(DbContextOptions<ProyectDBContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Deshabilitar eliminación en cascada para todas las relaciones
            var relationships = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Opcional: configurar nombres explícitos de tablas
            modelBuilder.Entity<Usuarios>().ToTable("Usuarios");
            modelBuilder.Entity<Compras>().ToTable("Compras");
            modelBuilder.Entity<Libros>().ToTable("Libros");
            modelBuilder.Entity<Prestamos>().ToTable("Prestamos");
            modelBuilder.Entity<Recomendaciones>().ToTable("Recomendaciones");
            modelBuilder.Entity<Reseñas>().ToTable("Resenias");

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Aquí podés agregar lógica extra antes de guardar si necesitás
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

}
