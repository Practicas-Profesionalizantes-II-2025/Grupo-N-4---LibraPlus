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

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/libros");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<LibrosDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var libros = JsonSerializer.Deserialize<List<LibrosDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(libros);
        }

        [HttpGet]
        public async Task<IActionResult> GetLibros()
        {
            var response = await _httpClient.GetAsync("api/libros");
            if (!response.IsSuccessStatusCode) return Json(new List<LibrosDTO>());

            var json = await response.Content.ReadAsStringAsync();
            var libros = JsonSerializer.Deserialize<List<LibrosDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Json(libros);
        }


        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/libros/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var libro = JsonSerializer.Deserialize<LibrosDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] LibrosDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/libros", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest("Error al crear el libro.");
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/libros/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var libro = JsonSerializer.Deserialize<LibrosDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] LibrosDTO dto)
        {
            if (id != dto.LibroID) return BadRequest();

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/libros/{id}", content);

            if (response.IsSuccessStatusCode)
                return Json(dto); // devuelve JSON para actualizar la tabla vía JS

            return StatusCode((int)response.StatusCode, "Error al actualizar el libro");
        }

      
        // POST: Libros/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/libros/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest("Error al eliminar el libro.");
        }
    }
}
