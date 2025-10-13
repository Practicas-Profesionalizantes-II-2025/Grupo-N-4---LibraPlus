using LibraPlus.Dominio.Entidades;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraPlus___Practica_Profesionalizante_II
{
    [Table("Libros")] // Nombre de tabla en la BD
    public class Libros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibroID { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(100, ErrorMessage = "El autor no puede superar los 100 caracteres")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(50, ErrorMessage = "El género no puede superar los 50 caracteres")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [RegularExpression("^(Digital|Físico)$", ErrorMessage = "El tipo debe ser 'Digital' o 'Físico'")]
        public string Tipo { get; set; } = string.Empty; // “Digital” o “Físico”

        [Range(0.01, 9999.99, ErrorMessage = "El precio debe estar entre 0.01 y 9999.99")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        // 🔗 Relación uno a muchos (Libro → Compras)
        public virtual ICollection<Compras> Compras { get; set; } = new List<Compras>();
        public virtual ICollection<Reseñas> Reseñas { get; set; } = new List<Reseñas>();
    }
}
