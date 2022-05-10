using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturaRequestDTO
    {
        public int IdPedido { get; set; }
        public int CantidadCuotas { get; set; }
    }
}