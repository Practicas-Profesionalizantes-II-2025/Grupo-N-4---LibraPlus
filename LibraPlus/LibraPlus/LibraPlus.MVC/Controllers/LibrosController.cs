using LibraPlus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LibraPlus.MVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly HttpClient _httpClient;

        public LibrosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // 📚 Listado de Libros
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Libros";

            var response = await _httpClient.GetAsync("api/libros");
            if (!response.IsSuccessStatusCode)
                return View(new List<LibrosDTO>());

            var json = await response.Content.ReadAsStringAsync();
            var libros = JsonSerializer.Deserialize<List<LibrosDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(libros);
        }

        // ➕ Crear Libro
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibrosDTO libro)
        {
            var content = new StringContent(JsonSerializer.Serialize(libro), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/libros", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return BadRequest($"Error al crear libro: {errorMsg}");
            }

            // 👇 Leer el contenido devuelto (DTO con ID generado)
            var result = await response.Content.ReadAsStringAsync();
            var created = JsonSerializer.Deserialize<LibrosDTO>(result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Json(created);
        }


        // ✏️ Editar Libro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] LibrosDTO libro)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos");

            var content = new StringContent(JsonSerializer.Serialize(libro), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/libros/{id}", content);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Error al editar libro");

            var result = await response.Content.ReadAsStringAsync();
            var updated = JsonSerializer.Deserialize<LibrosDTO>(result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Json(updated);
        }

        // 🗑️ Eliminar Libro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/libros/{id}");
            if (!response.IsSuccessStatusCode)
                return BadRequest("Error al eliminar libro");

            return Ok();
        }
    }
}
