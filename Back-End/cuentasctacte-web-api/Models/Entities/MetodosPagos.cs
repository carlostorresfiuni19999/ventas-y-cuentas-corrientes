using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    public class MetodosPagos
    {
        public int Id { get; set; }
        public Pago Pago { get; set; }
        public double Monto { get; set; }
        
        [ForeignKey("Pago")]
        public int IdPago { get; set; }

        public DateTime FechaDelPago { get; set; }
  
    }
}
