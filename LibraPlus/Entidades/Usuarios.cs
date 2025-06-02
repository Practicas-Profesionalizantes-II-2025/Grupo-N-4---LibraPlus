using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus
{
    public class Usuarios
    {
        public Usuarios(int usuarioID, string nombre, string email, int reputación)
        {
            UsuarioID = usuarioID;
            Nombre = nombre;
            Email = email;
            Reputación = reputación;
        }

        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int Reputación { get; set; }
    }
}
