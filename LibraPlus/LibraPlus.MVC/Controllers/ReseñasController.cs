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

        // GET: Reseñas por libro
        public async Task<IActionResult> PorLibro(int libroId)
        {
            var response = await _httpClient.GetAsync($"api/resenias/libro/{libroId}");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ReseñasDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var reseñas = JsonSerializer.Deserialize<List<ReseñasDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(reseñas);
        }

        // GET: Reseñas por usuario
        public async Task<IActionResult> PorUsuario(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/resenias/usuario/{usuarioId}");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ReseñasDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var reseñas = JsonSerializer.Deserialize<List<ReseñasDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(reseñas);
        }

        // GET: Detalle de reseña
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/resenias/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var reseña = JsonSerializer.Deserialize<ReseñasDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(reseña);
        }

        // GET: Crear reseña
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crear reseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReseñasDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/resenias", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(PorUsuario), new { usuarioId = dto.UsuarioID });
            }

            ModelState.AddModelError("", "Error al crear la reseña.");
            return View(dto);
        }
    }
}
