using Microsoft.EntityFrameworkCore;
using Shared.Entidades;


namespace LibraPlus_API.Data
{
    public class ProyectDBContext : DbContext
    {
        public DbSet<ComprasDTO> Compras { get; set; }
        public DbSet<LibrosDTO> Libros { get; set; }
        public DbSet<PrestamosDTO> Prestamos { get; set; }
        public DbSet<RecomendacionDTO> Recomendaciones { get; set; }
        public DbSet<ReseñasDTO> Reseñas { get; set; }
        public DbSet<UsuariosDTO> Usuarios { get; set; }

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

