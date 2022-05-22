using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturaRequestDTO
    {
        public int IdPedido { get; set; }
        public int CantidadCuotas { get; set; }

        public PedidoDTORequest Pedido { get; set; }
    }
}