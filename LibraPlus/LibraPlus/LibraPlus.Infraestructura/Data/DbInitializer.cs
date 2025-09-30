using LibraPlus___Practica_Profesionalizante_II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace LibraPlus.Infraestructura.Data
{
    public static class DbInitializer
    {
        public static void SeedAdmin(ProyectDBContext context)
        {
            // Si ya hay un admin, no hacer nada
            if (context.Admins.Any())
                return;

            var admin = new Administradores
            {
                Email = "mateomferrero@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Mateo1234") // contraseña inicial
            };

            context.Admins.Add(admin);
            context.SaveChanges();
        }
    }
}
