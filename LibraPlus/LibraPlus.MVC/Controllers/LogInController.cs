using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using LibraPlus.MVC.Models;

namespace LibraPlus.MVC.Controllers
{
    public class LogInController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LogInController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index() => View();

        // ✅ LOGIN general (detecta si es admin o usuario)
        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var loginData = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            // Primero intentamos como ADMIN
            var adminResponse = await client.PostAsync("api/administrador/login", content);
            if (adminResponse.IsSuccessStatusCode)
            {
                await CrearSesion("Admin", email, 0);
                return RedirectToAction("Index", "Home");
            }

            // Si no es admin, probamos como USUARIO
            var userResponse = await client.PostAsync("api/usuarios/login", content);
            if (userResponse.IsSuccessStatusCode)
            {
                var json = await userResponse.Content.ReadAsStringAsync();
                var usuario = JsonSerializer.Deserialize<UsuariosDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                await CrearSesion("Usuario", usuario.Email, usuario.UsuarioId);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

      

        // ✅ Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LogIn");
        }

        // Helper para crear sesión
        private async Task CrearSesion(string rol, string email, int id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, rol),
                new Claim("Id", id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });
        }
        // GET: LogIn/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: LogIn/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(string nombre, string email, string password)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            // Creamos el objeto que la API espera (igual a UsuariosDTO de la API)
            var nuevoUsuario = new
            {
                Nombre = nombre,
                Email = email,
                Password = password,
                Reputacion = 0
            };

            var content = new StringContent(JsonSerializer.Serialize(nuevoUsuario), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/usuarios", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "✅ Usuario registrado con éxito. Redirigiendo al login...";
                return RedirectToAction("Registro"); // vuelve a la vista para mostrar el mensaje
            }

            TempData["MensajeError"] = "❌ Error al registrar usuario. Inténtalo nuevamente.";
            return RedirectToAction("Registro");
        }


    }
}
