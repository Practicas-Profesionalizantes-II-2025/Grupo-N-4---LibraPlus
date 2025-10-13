using LibraPlus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LibraPlus.MVC.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly HttpClient _httpClient;

        public PrestamosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Prestamos";
            return View(); // 👈 devolvemos solo la vista, el JS se encarga de cargar los datos
        }

        // ✅ NUEVO: Devuelve los préstamos del usuario logueado en formato JSON
        [HttpGet]
        public async Task<IActionResult> GetPrestamosUsuario()
        {
            var usuarioIdClaim = User?.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized();
            }

            var response = await _httpClient.GetAsync($"api/prestamos/usuario/{usuarioId}");
            if (!response.IsSuccessStatusCode)
                return BadRequest("Error al obtener préstamos");

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/prestamos/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var prestamo = JsonSerializer.Deserialize<PrestamosDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(prestamo);
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] PrestamosDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/prestamos", content);

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest("Error al crear el préstamo.");
        }

        // POST: Prestamos/Devolver/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            var response = await _httpClient.PutAsync($"api/prestamos/{id}/devolver", null);
            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest("Error al marcar el préstamo como devuelto.");
        }

        // GET: Prestamos/Pendientes (opcional, si usás vista aparte)
        public async Task<IActionResult> Pendientes()
        {
            var response = await _httpClient.GetAsync("api/prestamos/pendientes");
            if (!response.IsSuccessStatusCode)
                return View(new List<PrestamosDTO>());

            var json = await response.Content.ReadAsStringAsync();
            var prestamos = JsonSerializer.Deserialize<List<PrestamosDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(prestamos);
        }
    }
}
