using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PedidoDTORequest
    {
        public int ClienteId { get; set; }
        public string Descripcion { get; set; }
        public List<PedidoDetalleDTORequest> Pedidos { get; set; }

    }
}