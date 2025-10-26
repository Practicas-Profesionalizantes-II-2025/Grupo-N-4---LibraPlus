using System.ComponentModel.DataAnnotations;

namespace LibraPlus.Aplicacion.DTOs
{
    public class LibrosDTO
    {
        [Key]
        public int LibroID { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(150)]
        public string Autor { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(500)]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Debe especificar el tipo de libro.")]
        [StringLength(20)]
        public string Tipo { get; set; } // "Digital" o "Físico"

        [Range(0.01, 99999, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        // 🧩 Opcional: si querés mostrar dependencias en detalle (solo lectura)
        public int CantidadCompras { get; set; }
        public int CantidadReseñas { get; set; }
    }
}
