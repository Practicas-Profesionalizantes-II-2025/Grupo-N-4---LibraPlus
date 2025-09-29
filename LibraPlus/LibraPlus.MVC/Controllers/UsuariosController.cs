using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using LibraPlus.MVC.Models; // Aquí irá tu modelo DTO

namespace LibraPlus.MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            // Esta acción sigue igual: obtiene la lista de usuarios y la pasa a la vista
            var response = await _httpClient.GetAsync("api/usuarios");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<UsuariosDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<UsuariosDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var response = await _httpClient.GetAsync("api/usuarios");
            if (!response.IsSuccessStatusCode) return Json(new List<UsuariosDTO>());

            var json = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<UsuariosDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Json(usuarios);
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] UsuariosDTO usuario)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jsonContent = JsonSerializer.Serialize(usuario, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/usuarios", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var nuevoUsuario = JsonSerializer.Deserialize<UsuariosDTO>(json, options);
                return Ok(nuevoUsuario); 
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText);
        }


        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Leer el cuerpo de la solicitud manualmente
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var requestBody = await reader.ReadToEndAsync();
            var usuario = JsonSerializer.Deserialize<UsuariosDTO>(requestBody, options);

            if (usuario == null || id != usuario.UsuarioId)
                return BadRequest("ID del usuario no coincide o datos inválidos.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jsonContent = JsonSerializer.Serialize(usuario, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Llama a la API usando el método PUT. Aquí es donde se hace la magia.
            // Aunque el frontend envió un POST a este controlador, este controlador envía un PUT a la API.
            var response = await _httpClient.PutAsync($"api/usuarios/{id}", content);

            // ... (El resto del código de la acción Edit es correcto) ...
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(responseBody))
                {
                    var updated = JsonSerializer.Deserialize<UsuariosDTO>(responseBody, options);
                    return Ok(updated);
                }
                return Ok(usuario);
            }
            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText);
        }


        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/usuarios/{id}");

            if (response.IsSuccessStatusCode)
                return Ok();

            var errorText = await response.Content.ReadAsStringAsync();
            return BadRequest(errorText ?? "Error al eliminar el usuario.");
        }

    }
}

