using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    public class FormasPagos : Data
    {
        public int Id { get; set; }

        public Pago Pago { get; set; }
        public string MetodoPago { get; set; }
        public double Monto { get; set; }

        [ForeignKey("Pago")]
        public int IdPago { get; set; }


    }
}
