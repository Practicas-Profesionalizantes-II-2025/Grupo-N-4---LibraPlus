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
        builder.Configuration.GetConnectionString("ProyectDBContext"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("LibraPlus.Infraestructura");
            sqlOptions.EnableRetryOnFailure(5);
        }
    )
);

// Repositories
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<ILibrosRepository, LibrosRepository>();
builder.Services.AddScoped<IComprasRepository, ComprasRepository>();
builder.Services.AddScoped<IPrestamosRepository, PrestamosRepository>();
builder.Services.AddScoped<IRecomendacionesRepository, RecomendacionesRepository>();
builder.Services.AddScoped<IReseniasRepository, ReseniasRepository>();

// Services / Casos de uso
builder.Services.AddScoped<IUsuarios, UsuariosServicio>();
builder.Services.AddScoped<ILibros, LibrosService>();
builder.Services.AddScoped<ICompras, ComprasService>();
builder.Services.AddScoped<IPrestamo, PrestamoServicio>();
builder.Services.AddScoped<IRecomendaciones, RecomendacionesServicio>();
builder.Services.AddScoped<IReseñas, ReseñasService>();

// ✅ Controllers y JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS para permitir peticiones desde el MVC
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy => policy
            .WithOrigins("https://localhost:7008") // Puerto del MVC
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Seed de Admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProyectDBContext>();

    try
    {
        // Aplica todas las migraciones pendientes
        context.Database.Migrate();

        // Ejecuta seed del admin (solo si no existe)
        DbInitializer.SeedAdmin(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al aplicar migraciones o seedear admin: " + ex.Message);
        throw; // opcional: para detener la app si falla
    }
}

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMVC");
app.UseAuthorization();
app.MapControllers();

app.Run();
