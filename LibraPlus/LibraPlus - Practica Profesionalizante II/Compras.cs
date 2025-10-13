using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraPlus___Practica_Profesionalizante_II
{
    [Table("Compras")]
    public class Compras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompraID { get; set; }

        // FK Usuario
        [Required]
        public int UsuarioID { get; set; }

        // FK Libro
        [Required]
        public int LibroID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public bool EsDigital { get; set; } = false;

        [StringLength(200)]
        public string? DescargaURL { get; set; }

        // 🔗 Navegaciones
        public virtual Usuarios Usuario { get; set; }
        public virtual Libros Libro { get; set; }
    }
}
