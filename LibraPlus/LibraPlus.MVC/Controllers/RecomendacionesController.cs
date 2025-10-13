using LibraPlus.Aplicacion.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LibraPlus.MVC.Controllers
{
    public class RecomendacionesController : Controller
    {
        private readonly HttpClient _httpClient;

        public RecomendacionesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // 🔹 Vista principal
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Recomendaciones";
            return View();
        }

        // 🔹 Obtener lista completa de recomendaciones
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var response = await _httpClient.GetAsync("api/recomendaciones");
            if (!response.IsSuccessStatusCode)
                return Json(new List<RecomendacionesDTO>());

            var json = await response.Content.ReadAsStringAsync();
            var recomendaciones = JsonSerializer.Deserialize<List<RecomendacionesDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Json(recomendaciones);
        }

        // 🔹 Crear recomendación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] RecomendacionesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/recomendaciones", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { mensaje = "✅ Recomendación creada correctamente" });
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText ?? "Error al crear la recomendación.");
        }

        // 🔹 Eliminar recomendación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/recomendaciones/{id}");

            if (response.IsSuccessStatusCode)
                return Ok(new { mensaje = "✅ Recomendación eliminada correctamente" });

            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText ?? "Error al eliminar la recomendación.");
        }


    }
}
