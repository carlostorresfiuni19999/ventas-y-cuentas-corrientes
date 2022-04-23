using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PedidoResponseDTO
    {
        public int Id { get; set; }
        public PersonaResponseDTO Cliente { get; set; }
        public PersonaResponseDTO Vendedor { get; set; }

        public string PedidoDescripcion { get; set; }
        public string Estado { get; set; }
        public string CondicionVenta { get; set; }
        public DateTime FechaPedido { get; set; }

        public List<PedidoDetalleResponseDTO> PedidosDetalles { get; set; }

    }
}