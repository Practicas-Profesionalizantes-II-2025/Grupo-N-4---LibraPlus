using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus___Practica_Profesionalizante_II
{
    public class Administradores
    {
        [Key]
        public int IdAdmin { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // nunca en texto plano
    }
}
