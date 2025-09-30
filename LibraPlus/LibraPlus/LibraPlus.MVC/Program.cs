using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores y vistas
builder.Services.AddControllersWithViews();

// Habilitar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LogIn/Index";   // Si no está logueado → redirige al login
        options.LogoutPath = "/LogIn/Logout"; // Logout
        options.AccessDeniedPath = "/Home/AccesoDenegado"; // opcional
    });

// Leer la URL de la API desde configuración (appsettings.json o appsettings.Development.json)
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrEmpty(apiBaseUrl))
{
    throw new InvalidOperationException("La clave ApiSettings:BaseUrl no está configurada en appsettings.json o appsettings.Development.json");
}

// Configurar HttpClient con la URL de la API
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

// Configuración del pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 👇 IMPORTANTE: primero autenticación, después autorización
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// Ruta por defecto → LogIn
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LogIn}/{action=Index}/{id?}");

app.Run();
