using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    public class PagosDetalle
    {
        public int Id { get; set; }
        public Pago Pago { get; set; }
        public VencimientoFactura Cuota { get; set; }
        public double Monto { get; set; }

        [ForeignKey("Pago")]
        public int IdPago { get; set; }
        [ForeignKey("Cuota")]
        public int IdCuota { get; set; }
    }
}
