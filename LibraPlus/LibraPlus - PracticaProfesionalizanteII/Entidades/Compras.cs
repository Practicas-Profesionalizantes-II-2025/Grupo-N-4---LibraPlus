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
        
        [ForeignKey("Libro")]
        public int LibroID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string DescargaURL { get; set; }
    }

}
