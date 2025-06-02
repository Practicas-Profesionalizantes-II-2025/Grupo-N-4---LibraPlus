using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entidades
{
    public class Usuarios
    {
        [Key]
        public int UsuarioID { get; set; }
        
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int Reputación { get; set; }
    }

}
