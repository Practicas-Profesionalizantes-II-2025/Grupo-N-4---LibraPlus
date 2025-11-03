using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraPlus.MVC.Models
{
    public class UsuariosDTO
    {
        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        // Contraseña solo requerida en creación
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
