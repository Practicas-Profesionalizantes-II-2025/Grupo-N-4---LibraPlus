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

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Range(0, 5, ErrorMessage = "La reputación debe estar entre 0 y 5")]
        [JsonPropertyName("reputacion")]
        public int Reputacion { get; set; }
    }
}
