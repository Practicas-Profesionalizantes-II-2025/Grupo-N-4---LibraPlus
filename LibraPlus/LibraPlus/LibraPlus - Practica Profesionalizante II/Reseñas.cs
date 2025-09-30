using LibraPlus___Practica_Profesionalizante_II;
using System.ComponentModel.DataAnnotations;

namespace LibraPlus.Dominio.Entidades
{
    public class Reseñas
    {

        [Key]
        public int ReseñaID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public string? Comentario { get; set; }
        public int Puntuación { get; set; }

        // Relaciones
        public Usuarios Usuario { get; set; }
        public Libros Libro { get; set; }
    }
}
