using LibraPlus.Aplicacion.DTOs;
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

        // 🟡 Vista principal
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Reseñas";
            return View();
        }

        // 🟢 Obtener lista de reseñas (todas)
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var response = await _httpClient.GetAsync("api/reseñas/all");

            if (!response.IsSuccessStatusCode)
                return Json(new List<ReseñasDTO>());

            var json = await response.Content.ReadAsStringAsync();

            // ✅ Si la respuesta está vacía o nula, devolvemos una lista vacía para evitar error JSON
            if (string.IsNullOrWhiteSpace(json))
                return Json(new List<ReseñasDTO>());

            var reseñas = JsonSerializer.Deserialize<List<ReseñasDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ReseñasDTO>();

            return Json(reseñas);
        }

        // 🟩 Crear reseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ReseñasDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "❌ Datos inválidos en el formulario." });

            try
            {
                // 🟢 Obtener ID del usuario logueado (desde las claims)
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    dto.UsuarioID = userId;
                }
                else
                {
                    return Unauthorized(new { mensaje = "⚠️ No se pudo identificar al usuario logueado." });
                }

                // Enviar a la API
                var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/reseñas", content);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return BadRequest(json ?? "❌ Error al crear la reseña.");

                var nuevaReseña = JsonSerializer.Deserialize<ReseñasDTO>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Ok(new
                {
                    mensaje = "✅ Reseña creada correctamente",
                    reseña = nuevaReseña
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "❌ Error al crear la reseña.", detalle = ex.Message });
            }
        }

        // 🟥 Eliminar reseña
        [HttpDelete]
        [Route("Reseñas/Delete/{id}")]
        [IgnoreAntiforgeryToken] // 👈 necesario para evitar error 400 con fetch DELETE
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/reseñas/{id}");
                var json = await response.Content.ReadAsStringAsync();

                // ✅ si no hay cuerpo, devolvemos mensaje genérico
                var data = string.IsNullOrWhiteSpace(json)
                    ? new { mensaje = "✅ Reseña eliminada correctamente" }
                    : JsonSerializer.Deserialize<object>(json);

                if (response.IsSuccessStatusCode)
                    return Ok(data);

                return BadRequest(data ?? new { mensaje = "❌ Error al eliminar la reseña." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "❌ Error al eliminar la reseña.", detalle = ex.Message });
            }
        }
    }
}
