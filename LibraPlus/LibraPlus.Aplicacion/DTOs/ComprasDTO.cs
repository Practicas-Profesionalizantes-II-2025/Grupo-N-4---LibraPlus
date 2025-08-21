using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraPlus.Aplicacion.DTOs
{
    public class ComprasDTO
    {
        public int CompraID { get; set; }          
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public decimal Precio { get; set; }       
        public DateTime Fecha { get; set; }       
        public bool EsDigital { get; set; }       
        public string DescargaURL { get; set; }   
    }
}
