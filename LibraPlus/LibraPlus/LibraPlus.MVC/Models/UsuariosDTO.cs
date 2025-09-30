using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraPlus.MVC.Models
{
    public class UsuariosDTO
    {
        [JsonPropertyName("UsuarioId")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [JsonPropertyName("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email no válido")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [Range(0, 5, ErrorMessage = "La reputación debe estar entre 0 y 5")]
        [JsonPropertyName("Reputacion")]
        public int Reputacion { get; set; }
    }
}

