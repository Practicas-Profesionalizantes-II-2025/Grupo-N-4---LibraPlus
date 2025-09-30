using System.ComponentModel.DataAnnotations;

namespace LibraPlus.MVC.Models
{
    public class LibrosDTO
    {
        public int LibroID { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
    }
}
