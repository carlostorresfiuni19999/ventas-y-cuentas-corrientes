using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    [Table("NotasDeCreditos")]
    public class NotaDeCredito : Data
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int? FacturaId { get; set; }
        public string NotaDescripcion { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public double Monto { get; set; }

        [ForeignKey("FacturaId")]
        public Factura Factura { get; set; }

        [ForeignKey("ClienteId")]
        public Persona Cliente { get; set; }
    }
}