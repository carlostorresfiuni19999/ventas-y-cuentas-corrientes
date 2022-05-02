using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PedidoDTORequest
    {
        public int ClienteId { get; set; }
        public string Descripcion { get; set; }
        public string CondicionVenta { get; set; }
        public List<PedidoDetalleDTORequest> Pedidos { get; set; }

    }
}