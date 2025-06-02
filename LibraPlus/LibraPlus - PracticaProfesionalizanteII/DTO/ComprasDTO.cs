namespace Shared.Entidades
{
    public class ComprasDTO
    {
        public int CompraID { get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string DescargaURL { get; set; }
    }

}
