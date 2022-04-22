using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    public class PedidoDetalle : Data
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public Pedido Pedido { get; set; }
        public int CantidadProducto { get; set; }
        public int CantidadFacturada { get; set; }
        public double PrecioUnitario { get; set; }

        [ForeignKey("Producto")]
        public int IdProducto { get; set; }
        [ForeignKey("Pedido")]
        public int IdPedido { get; set; }
    }
}