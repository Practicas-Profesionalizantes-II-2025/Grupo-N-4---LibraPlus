using LibraPlus.Aplicacion.DTOs;
using LibraPlus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LibraPlus.MVC.Controllers
{
    public class ReseñasController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReseñasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Reseñas/Index
        public IActionResult Index()
        {
            return View();
        }

        // 🔹 API interna para cargar tabla AJAX
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            ViewData["ActivePage"] = "Reseñas";
            var response = await _httpClient.GetAsync("api/resenias");
            if (!response.IsSuccessStatusCode) return Json(new List<ReseñasDTO>());

            var json = await response.Content.ReadAsStringAsync();
            var reseñas = JsonSerializer.Deserialize<List<ReseñasDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Json(reseñas);
        }

        // 🔹 Crear reseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ReseñasDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/resenias", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var nuevaReseña = JsonSerializer.Deserialize<ReseñasDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return Ok(nuevaReseña);
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText ?? "Error al crear la reseña.");
        }

        // 🔹 Eliminar reseña
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/resenias/{id}");

            if (response.IsSuccessStatusCode)
                return Ok();

            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText ?? "Error al eliminar la reseña.");
        }
    }
}
