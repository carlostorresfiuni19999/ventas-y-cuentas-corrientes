using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{

    public class FacturaDetalle : Data
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public double Iva { get; set; }
        public double PrecioUnitario { get; set; }

        [ForeignKey("FacturaId")]
        public Factura Factura { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

    }
}