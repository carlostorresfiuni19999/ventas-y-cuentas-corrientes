using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturaRequestDTO
    {
        public int IdPedido { get; set; }
        public double Iva { get; set; }
        public string CondicionVenta { get; set; }

        public List<FacturaDetalleRequestDTO> FacturaDetalles { get; set; }
    }
}