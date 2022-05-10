namespace cuentasctacte_web_api.Models.DTOs
{
    public class ProductoResponseDTO
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string MarcaProducto { get; set; }
        public string CodigoDeBarra { get; set; }
        public double Precio { get; set; }
        public double Iva { get; set; }
    }
}