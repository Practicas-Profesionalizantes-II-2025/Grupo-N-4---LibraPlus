using LibraPlus.Aplicacion;
using LibraPlus.Aplicacion.Interfaces;
using LibraPlus.Aplicacion.Services;
using LibraPlus.Aplicacion.Servicios;
using LibraPlus.Infraestructura.Data;
using LibraPlus.Infraestructura.Interfaces.Infra;
using LibraPlus.Infraestructura.Repositorys;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ProyectDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("LibraPlus.Infraestructura");
            sqlOptions.EnableRetryOnFailure(5); // reintenta hasta 5 veces
        }
    )
);


// Repositories
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<ILibrosRepository, LibrosRepository>();
builder.Services.AddScoped<IComprasRepository, ComprasRepository>();
builder.Services.AddScoped<IPrestamosRepository, PrestamosRepository>();
builder.Services.AddScoped<IRecomendacionesRepository, RecomendacionesRepository>();
builder.Services.AddScoped<IReseñasRepository, ReseñasRepository>();

// Services / Casos de uso
builder.Services.AddScoped<IUsuarios, UsuariosServicio>();
builder.Services.AddScoped<ILibros, LibrosServicio>();
builder.Services.AddScoped<ICompras, ComprasService>();
builder.Services.AddScoped<IPrestamo, PrestamoServicio>();
builder.Services.AddScoped<IRecomendaciones, RecomendacionesServicio>();
builder.Services.AddScoped<IReseñas, ReseñasService>();

// Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed de Admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProyectDBContext>();
    LibraPlus.Infraestructura.Data.DbInitializer.SeedAdmin(context);
}

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
