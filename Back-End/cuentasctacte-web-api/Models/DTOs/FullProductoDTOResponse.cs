namespace cuentasctacte_web_api.Models.DTOs
{
    public class FullProductoDTOResponse
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }

        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }

        public string Cliente { get; set; }
        public int IdCliente { get; set; }
        public string Vendedor { get; set; }
        public int IdVendedor { get; set; }
        public double PrecioTotal { get; set; }

    }
}