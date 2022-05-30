using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    [Table("Pagos")]
    public class Pago
    {
        public int Id { get; set; }
        public Persona Cliente { get; set; }
        public Persona Cajero { get; set; }
        public Caja Caja { get; set; }
        public double MontoTotal { get; set; }
        public DateTime FechaPago { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        [ForeignKey("Caja")]
        public int IdCaja { get; set; }
        [ForeignKey("Cajero")]
        public int IdCajero { get; set; }
    }
}
