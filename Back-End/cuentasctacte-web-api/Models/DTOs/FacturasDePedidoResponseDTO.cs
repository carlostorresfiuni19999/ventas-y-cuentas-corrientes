using System;
using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturasDePedidoResponseDTO
    {
        
        public PedidoDetalleResponseDTO PedidoDetalleResponseDTO { get; set; }
        public List<FacturaDetalleResponseDTO> FacturasDetalles { get; set; }

    }
}