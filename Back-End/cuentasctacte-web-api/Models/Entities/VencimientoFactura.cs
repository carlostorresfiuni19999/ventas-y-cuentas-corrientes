using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    public class VencimientoFactura : Data
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public double Monto { get; set; }
        public double Saldo { get; set; }

        [ForeignKey("FacturaId")]
        public Factura Factura { get; set; }
    }
}