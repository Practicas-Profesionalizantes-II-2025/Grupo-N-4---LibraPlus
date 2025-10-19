using LibraPlus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

public class ComprasController : Controller
{
    private readonly HttpClient _httpClient;

    public ComprasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    // ✅ Página principal
    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Compras";
        return View();
    }

    // ✅ Obtener todas las compras
    [HttpGet]
    public async Task<IActionResult> Lista()
    {
        var response = await _httpClient.GetAsync("api/compras");
        if (!response.IsSuccessStatusCode)
            return Json(new List<ComprasDTO>());

        var json = await response.Content.ReadAsStringAsync();
        var compras = JsonSerializer.Deserialize<List<ComprasDTO>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Json(compras);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] ComprasDTO compraDto)
    {
        Console.WriteLine("💾 Llegó al MVC → Create()");

        if (compraDto == null)
            return Json(new { success = false, mensaje = "No se recibieron datos." });

        try
        {
            // ✅ Obtener el rol y el ID del usuario logueado correctamente
            var usuarioIdClaim = User.FindFirst("Id")?.Value;                   // <--- usar "Id" como definiste en LogInController
            var rolClaim = User.FindFirst(ClaimTypes.Role)?.Value;             // <--- usar ClaimTypes.Role

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Json(new { success = false, mensaje = "No se pudo determinar el usuario logueado." });

            // ⚙️ Si no es admin, forzamos su propio ID
            if (rolClaim != "Admin")
                compraDto.UsuarioID = int.Parse(usuarioIdClaim);

            Console.WriteLine($"➡️ UsuarioID={compraDto.UsuarioID}, LibroID={compraDto.LibroID}");

            // --- POST a la API ---
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(compraDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("api/compras", jsonContent);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return Json(new { success = false, mensaje = "Error al registrar compra.", detalle = body });

            var newCompra = JsonSerializer.Deserialize<ComprasDTO>(
                body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Json(new { success = true, compra = newCompra });
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error en MVC Create: " + ex.Message);
            return Json(new { success = false, mensaje = "Excepción: " + ex.Message });
        }
    }


    // ✅ Eliminar compra
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return Json(new { success = false, mensaje = "ID inválido." });

        try
        {
            var response = await _httpClient.DeleteAsync($"api/compras/{id}");

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, mensaje = "✅ Compra eliminada correctamente." });

            var detalle = await response.Content.ReadAsStringAsync();
            return Json(new { success = false, mensaje = "❌ No se pudo eliminar la compra.", detalle });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, mensaje = "⚠️ Error al eliminar: " + ex.Message });
        }
    }

    // ✅ Obtener lista de usuarios
    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var response = await _httpClient.GetAsync("api/usuarios");
        if (!response.IsSuccessStatusCode)
            return Json(new List<UsuariosDTO>());

        var json = await response.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<UsuariosDTO>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Json(usuarios);
    }

    // ✅ Obtener lista de libros
    [HttpGet]
    public async Task<IActionResult> GetLibros()
    {
        var response = await _httpClient.GetAsync("api/libros");
        if (!response.IsSuccessStatusCode)
            return Json(new List<LibrosDTO>());

        var json = await response.Content.ReadAsStringAsync();
        var libros = JsonSerializer.Deserialize<List<LibrosDTO>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Json(libros);
    }
}
