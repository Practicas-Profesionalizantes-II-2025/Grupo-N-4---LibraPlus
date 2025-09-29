using System.ComponentModel.DataAnnotations;

namespace LibraPlus.MVC.Models
{
    public class PrestamosDTO
    {
        public int PrestamoID { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [Required]
        public int LibroID { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool Devuelto { get; set; }
    }
}
