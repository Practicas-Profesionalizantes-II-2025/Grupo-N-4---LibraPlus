using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Entidades
{
    public class Compras
    {
        [Key]
        public int CompraID { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }
        public Usuarios Usuario { get; set; }  // navegación

        [ForeignKey("Libro")]
        public int LibroID { get; set; }
        public Libros Libro { get; set; }      // navegación

        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }

        public bool EsDigital { get; set; }            // NUEVO
        public string? DescargaURL { get; set; }       // puede ser null si no es digital
    }

}
