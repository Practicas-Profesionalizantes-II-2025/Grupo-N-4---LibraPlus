using System.ComponentModel.DataAnnotations;

namespace LibraPlus.MVC.Models
{
    public class ReseñasDTO
    {
        public int ReseñaID { get; set; }

        [Required(ErrorMessage = "El Usuario es obligatorio")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El Libro es obligatorio")]
        public int LibroID { get; set; }

        [Required(ErrorMessage = "El Comentario es obligatorio")]
        [StringLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres")]
        public string Comentario { get; set; }

        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
        public int Puntuacion { get; set; }
    }
}
