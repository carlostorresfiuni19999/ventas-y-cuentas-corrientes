using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PedidoDetalleDTORequest
    {
        public int ProductoId { get; set; }
        public int CantidadProducto { get; set; }
    }
}