using System.ComponentModel.DataAnnotations;

namespace LibraPlus.MVC.Models
{
    public class ComprasDTO
    {
        public int CompraID { get; set; }

        [Required(ErrorMessage = "El Usuario es obligatorio")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El Libro es obligatorio")]
        public int LibroID { get; set; }

        // Estos campos no son necesarios en el formulario, se completan en el servidor.
        public decimal Precio { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool EsDigital { get; set; }
        public string? DescargaURL { get; set; }
    }
}
