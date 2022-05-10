using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    public class Factura : Data
    {
        public int Id { get; set; }
        public double Iva { get; set; }
        public string CondicionVenta { get; set; }
        public string Estado { get; set; }
        public double Monto { get; set; }
        public double Saldo { get; set; }
        public DateTime FechaFactura { get; set; }  

        public int? PedidoId { get; set; }
        public int? ClienteId { get; set; }
        public int? VendedorId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }
        [ForeignKey("ClienteId")]
        public Persona Cliente { get; set; }
        [ForeignKey("VendedorId")]
        public Persona Vendedor { get; set; }

        public int CantidadCuotas { get; set; }
    }
}