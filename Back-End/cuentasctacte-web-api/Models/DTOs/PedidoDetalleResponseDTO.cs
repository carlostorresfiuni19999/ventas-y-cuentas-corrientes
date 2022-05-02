using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PedidoDetalleResponseDTO
    {
        public int Id { get; set; }
        public ProductoResponseDTO Producto { get; set;}
        public int CantidadFacturada { get; set; }
        public int CantidadProductos { get; set; }
    }
}