using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    public class FormasPagos : Data
    {
        public int Id { get; set; }
        public PagosDetalle Pago { get; set; }

        public MetodosPagos MetodosPagos { get; set; }
        public string FormaDePago { get; set; }
        public double Monto { get; set; }

        [ForeignKey("Pago")]
        public int IdPagoDetalle { get; set; }

        [ForeignKey("MetodosPagos")]
        public int IdMetodosPagos { get; set; }
    }
}
