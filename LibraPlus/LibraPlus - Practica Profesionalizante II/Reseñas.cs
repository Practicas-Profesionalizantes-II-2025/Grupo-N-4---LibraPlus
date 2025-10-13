using LibraPlus___Practica_Profesionalizante_II;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraPlus.Dominio.Entidades
{
    public class Reseñas
    {
        [Key]
        public int ReseñaID { get; set; }

        public int UsuarioID { get; set; }
        public int LibroID { get; set; }

        [StringLength(500)]
        public string? Comentario { get; set; }

        [Column("Puntuacion")]
        public int Puntuacion { get; set; }

        // Relaciones
        public Usuarios Usuario { get; set; }
        public Libros Libro { get; set; }
    }
}
