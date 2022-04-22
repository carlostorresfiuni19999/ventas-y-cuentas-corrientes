using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    [Table("Pedidos")]
    public class Pedido : Data
    {
        public int Id { get; set; }
        public Persona Cliente { get; set; }
        public Persona Vendedor { get; set; }
        public string PedidoDescripcion { get; set; }
        public int NumeroPedido { get; set; }
        public string CondicionVenta { get; set; }
        public string Estado { get; set; }
        public DateTime FechaPedido { get; set; }

        [ForeignKey("Cliente")]
        public int? IdCliente { get; set; }

        [ForeignKey("Vendedor")]
        public int? IdVendedor { get; set; }

    }
}