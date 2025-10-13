using System.Text;
using System.Text.Json;
using LibraPlus.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraPlus.MVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly HttpClient _httpClient;

        public LibrosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // ✅ Vista principal
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Libros";

            try
            {
                var response = await _httpClient.GetAsync("api/libros");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(json))
                    return View(new List<LibrosDTO>()); // devolvemos lista vacía en vez de null

                var libros = JsonSerializer.Deserialize<List<LibrosDTO>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LibrosDTO>();

                return View(libros); // ✅ pasamos el modelo a la vista
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error cargando libros: " + ex.Message);
                return View(new List<LibrosDTO>()); // también devolvemos lista vacía ante error
            }
        }


        // ✅ GET: Libros
        [HttpGet]
        public async Task<IActionResult> GetLibros()
        {
            var response = await _httpClient.GetAsync("api/libros");

            if (!response.IsSuccessStatusCode)
                return Json(new List<LibrosDTO>());

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                return Json(new List<LibrosDTO>());

            var libros = JsonSerializer.Deserialize<List<LibrosDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LibrosDTO>();

            return Json(libros);
        }

        // ✅ POST: Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] LibrosDTO libro)
        {
            var json = JsonSerializer.Serialize(libro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/libros", content);
            if (!response.IsSuccessStatusCode)
                return BadRequest("❌ Error al crear el libro.");

            return Ok();
        }

        // ✅ POST: Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] LibrosDTO libro)
        {
            if (id != libro.LibroID)
                return BadRequest("El ID no coincide.");

            var json = JsonSerializer.Serialize(libro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/libros/{id}", content);
            if (!response.IsSuccessStatusCode)
                return BadRequest("❌ Error al actualizar el libro.");

            return Ok();
        }

        // 🧨 DELETE normal
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/libros/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // ⚠️ Dependencias detectadas
                var msg = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, dependencias = true, mensaje = msg });
            }

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, mensaje = "✅ Libro eliminado correctamente." });

            return Json(new { success = false, mensaje = "❌ No se pudo eliminar el libro." });
        }

        // 🔥 DELETE forzado
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ForceDelete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/libros/force/{id}");

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, mensaje = "✅ Libro y dependencias eliminados correctamente." });

            var msg = await response.Content.ReadAsStringAsync();
            return Json(new { success = false, mensaje = msg });
        }
    }
}
