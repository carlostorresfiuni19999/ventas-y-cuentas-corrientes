using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FacturaResponseDTO
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public double MontoTotal { get; set; }
        public double SaldoTotal { get; set; }

        public string Estado { get; set; }
        public DateTime FechaFacturada { get; set; }
        public string CondicionVenta { get; set; }
    }
}