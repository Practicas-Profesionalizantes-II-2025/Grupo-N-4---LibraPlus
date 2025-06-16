using Microsoft.EntityFrameworkCore;
using Shared.Entidades;


namespace LibraPlus_API.Data
{
    public class ProyectDBContext : DbContext
    {
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Recomendacion> Recomendaciones { get; set; }
        public DbSet<Reseñas> Reseñas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        public ProyectDBContext(DbContextOptions<ProyectDBContext> options)
            : base(options) { }

        // Configuraciones del modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Deshabilitar eliminación en cascada
            DisableCascadingDelete(modelBuilder);
        }

        // Guardar cambios de forma asíncrona
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder
                .Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}

