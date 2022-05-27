using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    public class MetodosPagos
    {
        public int Id { get; set; }
        public Pago Pago { get; set; }
        public double Monto { get; set; }
        public FormasPagos Formas { get; set; }
        [ForeignKey("Pago")]
        public int IdPago { get; set; }
        [ForeignKey("Formas")]
        public int IdFormasPagos { get; set; }
    }
}
