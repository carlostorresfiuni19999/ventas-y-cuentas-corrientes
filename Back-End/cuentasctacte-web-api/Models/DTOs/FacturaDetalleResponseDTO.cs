namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturaDetalleResponseDTO
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }

        public double Iva { get; set; }
        public double PrecioUnitario { get; set; }
    }
}
