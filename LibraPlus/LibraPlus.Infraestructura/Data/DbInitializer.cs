using BCrypt.Net;
using LibraPlus___Practica_Profesionalizante_II;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Infraestructura.Data
{
    public static class DbInitializer
    {
        public static void SeedAdmin(ProyectDBContext context)
        {
            // 🛑 ELIMINAMOS context.Database.Migrate() porque ya está en Program.cs

            // Si ya hay un admin, no hacer nada
            if (context.Admins.Any())
            {
                Console.WriteLine("Usuario Admin ya existe. Omitiendo seed.");
                return;
            }

            var admin = new Administradores
            {
                Email = "mateomferrero@gmail.com",
                // El Hash se genera aquí. Es funcional para un seed inicial.
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Mateo1234")
            };

            try
            {
                context.Admins.Add(admin);
                context.SaveChanges();
                Console.WriteLine("✅ Admin creado correctamente.");
            }
            catch (Exception ex)
            {
                // El Console.WriteLine es muy útil si hay un error de DB/esquema
                Console.WriteLine("Error al seedear admin: " + ex.Message);
            }
        }
    }
}
