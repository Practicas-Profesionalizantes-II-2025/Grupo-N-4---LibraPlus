using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraPlus.MVC.Models;

public class ComprasController : Controller
{
    private readonly HttpClient _httpClient;

    public ComprasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    // GET: Compras
    public async Task<IActionResult> Index()
    {
        // Start all three API calls simultaneously
        var comprasTask = _httpClient.GetAsync("api/compras");
        var usuariosTask = _httpClient.GetAsync("api/usuarios");
        var librosTask = _httpClient.GetAsync("api/libros");

        // Wait for all tasks to complete
        await Task.WhenAll(comprasTask, usuariosTask, librosTask);

        // Now, retrieve and process the results
        var comprasResponse = await comprasTask;
        var usuariosResponse = await usuariosTask;
        var librosResponse = await librosTask;

        var compras = comprasResponse.IsSuccessStatusCode
            ? JsonSerializer.Deserialize<List<ComprasDTO>>(await comprasResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            : new List<ComprasDTO>();

        var usuarios = usuariosResponse.IsSuccessStatusCode
            ? JsonSerializer.Deserialize<List<UsuariosDTO>>(await usuariosResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            : new List<UsuariosDTO>();
        ViewBag.Usuarios = usuarios;

        var libros = librosResponse.IsSuccessStatusCode
            ? JsonSerializer.Deserialize<List<LibrosDTO>>(await librosResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            : new List<LibrosDTO>();
        ViewBag.Libros = libros;

        return View(compras);
    }

    // GET: Compras/Usuario/5
    public async Task<IActionResult> ComprasPorUsuario(int usuarioId)
    {
        var response = await _httpClient.GetAsync($"api/compras/usuario/{usuarioId}");
        if (!response.IsSuccessStatusCode)
        {
            return View(new List<ComprasDTO>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var compras = JsonSerializer.Deserialize<List<ComprasDTO>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(compras);
    }

    // GET: Compras/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var compra = JsonSerializer.Deserialize<ComprasDTO>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(compra);
    }

    // GET: Compras/Create
    public IActionResult Create()
    {
        return View();
    }

    // Dentro de tu ComprasController.cs
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] ComprasDTO compraDto)
    {
        if (compraDto == null)
            return BadRequest("Datos de la compra inválidos.");

        // Solo enviamos UsuarioID y LibroID, la API se encarga de Precio, Tipo y DescargaURL
        var envio = new
        {
            UsuarioID = compraDto.UsuarioID,
            LibroID = compraDto.LibroID
        };

        var content = new StringContent(JsonSerializer.Serialize(envio), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/compras", content);

        if (response.IsSuccessStatusCode)
        {
            var newCompra = JsonSerializer.Deserialize<ComprasDTO>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return Ok(newCompra);
        }

        var errorText = await response.Content.ReadAsStringAsync();
        return BadRequest(errorText ?? "Error al crear la compra.");
    }

    public async Task<IActionResult> GetCompras()
    {
        var response = await _httpClient.GetAsync("api/compras");
        if (!response.IsSuccessStatusCode)
            return Json(new List<ComprasDTO>());

        var json = await response.Content.ReadAsStringAsync();
        var compras = JsonSerializer.Deserialize<List<ComprasDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return Json(compras);
    }
}