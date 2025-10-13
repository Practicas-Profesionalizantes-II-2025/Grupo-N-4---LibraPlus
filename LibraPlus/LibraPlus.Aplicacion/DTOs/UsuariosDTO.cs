using System.ComponentModel.DataAnnotations;

namespace LibraPlus.Aplicacion.DTOs
{
    public class UsuariosDTO
    {
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La contraseña no puede tener más de 100 caracteres")]
        public string Password { get; set; }

        [Range(0, 5, ErrorMessage = "La reputación debe estar entre 0 y 5")]
        public int Reputación { get; set; }
    }
}
