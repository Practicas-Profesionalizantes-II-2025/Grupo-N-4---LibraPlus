using LibraPlus.MVC.Models;
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

        // GET: Recomendaciones
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Libros";
            var response = await _httpClient.GetAsync("api/recomendaciones");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<RecomendacionesDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var recomendaciones = JsonSerializer.Deserialize<List<RecomendacionesDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(recomendaciones);
        }

        // GET: Recomendaciones/Usuario/5
        public async Task<IActionResult> PorUsuario(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/recomendaciones/usuario/{usuarioId}");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<RecomendacionesDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var recomendaciones = JsonSerializer.Deserialize<List<RecomendacionesDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(recomendaciones);
        }

        // GET: Recomendaciones/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/recomendaciones/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var recomendacion = JsonSerializer.Deserialize<RecomendacionesDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(recomendacion);
        }

        // GET: Recomendaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recomendaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecomendacionesDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/recomendaciones", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al crear la recomendación.");
            return View(dto);
        }

        // GET: Recomendaciones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/recomendaciones/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var recomendacion = JsonSerializer.Deserialize<RecomendacionesDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(recomendacion);
        }

        // POST: Recomendaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecomendacionesDTO dto)
        {
            if (!ModelState.IsValid || id != dto.RecomendacionID)
                return View(dto);

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/recomendaciones/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al actualizar la recomendación.");
            return View(dto);
        }

        // POST: Recomendaciones/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/recomendaciones/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
