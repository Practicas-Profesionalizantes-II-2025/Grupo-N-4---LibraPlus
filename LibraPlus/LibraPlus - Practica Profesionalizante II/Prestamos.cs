using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraPlus___Practica_Profesionalizante_II
{
    public class Prestamos
    {
        [Key]
        public int PrestamoID { get; set; }

        // 🔹 Claves foráneas
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }

        [ForeignKey("Libro")]
        public int LibroID { get; set; }

        // 🔹 Fechas
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // 🔹 Estado
        public bool Devuelto { get; set; }

        // 🔹 Propiedades de navegación (agregadas)
        public virtual Usuarios Usuario { get; set; }
        public virtual Libros Libro { get; set; }
    }
}
