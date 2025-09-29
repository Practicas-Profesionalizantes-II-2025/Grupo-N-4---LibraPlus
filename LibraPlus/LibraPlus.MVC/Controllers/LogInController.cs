using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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
        public IActionResult Index()
        {
            return View(); // tu vista Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            // Crear el JSON para enviar a la API
            var loginData = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ApiClient"); // Configurado en Program.cs
            var response = await client.PostAsync("api/administrador/login", content);

            if (response.IsSuccessStatusCode)
            {
                // Login exitoso → guardar sesión
                HttpContext.Session.SetString("AdminEmail", email);
                return RedirectToAction("Index", "Home"); // Redirigir al home
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Si usás cookies de autenticación, esto cierra la sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Por si usás Session o querés limpiar todo
            HttpContext.Session.Clear();

            // (Opcional) borrar cookies de la app
            foreach (var key in Request.Cookies.Keys)
                Response.Cookies.Delete(key);

            // Volver al login
            return RedirectToAction("Index", "LogIn");
        }

    }
}

