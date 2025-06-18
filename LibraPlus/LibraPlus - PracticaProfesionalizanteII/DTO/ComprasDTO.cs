namespace Shared.DTO
{
    public class ComprasDTO
    {
        public int CompraID { get; set; }

        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }      

        public int LibroID { get; set; }
        public string TituloLibro { get; set; }        

        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }

        public bool EsDigital { get; set; }            
        public string? DescargaURL { get; set; }
    }

}
