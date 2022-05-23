using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FullFacturaResponseDTO
    {
        public string DocCliente { get; set; }
        public string Cliente { get; set; }

        public double PrecioTotal { get; set; }

        public double SaldoTotal { get; set; }
        public double IvaTotal { get; set; }
        public DateTime FechaFacturacion { get; set; }
        public List<FacturaDetalleResponseDTO> Detalles { get; set; }
        public List<FullCuotaResponseDTO> Cuotas { get; set; }


    }
}
