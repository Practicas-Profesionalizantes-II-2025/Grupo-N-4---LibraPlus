using System.ComponentModel.DataAnnotations;

namespace LibraPlus.MVC.Models
{
    public class RecomendacionesDTO
    {
        public int RecomendacionID { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [Required]
        public int LibroID { get; set; }

        [Required]
        [StringLength(100)]
        public string Fuente { get; set; }
    }
}
