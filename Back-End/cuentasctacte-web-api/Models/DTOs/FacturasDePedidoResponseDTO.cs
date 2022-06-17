
﻿using System;

using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturasDePedidoResponseDTO
    {
        
        public PedidoResponseDTO PedidoFull { get; set; }
        public List<FullFacturaResponseDTO> FullFacturas{ get; set; }


    }
}