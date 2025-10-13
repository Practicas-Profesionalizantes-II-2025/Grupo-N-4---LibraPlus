using LibraPlus.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraPlus___Practica_Profesionalizante_II
{
    public class Usuarios
    {
        [Key]
        public int UsuarioID { get; set; }

        public string Nombre { get; set; }
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } 


        public int Reputacion { get; set; }

        public virtual ICollection<Compras> Compras { get; set; }
        public virtual ICollection<Reseñas> Reseñas { get; set; } // Agregado para solucionar el error
    }
}
