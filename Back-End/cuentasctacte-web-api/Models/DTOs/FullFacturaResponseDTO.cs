using System;
using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FullFacturaResponseDTO
    {
    
        public int IdFactura { get; set; }
        public int IdPedido { get; set; }
        public string DocCliente { get; set; }
        public string Cliente { get; set; }
        public string Documento { get; set; }
        public string DocumentoTipo { get; set; }
        public string Estado { get; set; }
        public double PrecioTotal { get; set; }

        public double SaldoTotal { get; set; }
        public double IvaTotal { get; set; }
        public DateTime FechaFacturacion { get; set; }
        public List<FacturaDetalleResponseDTO> Detalles { get; set; }
        public List<FullCuotaResponseDTO> Cuotas { get; set; }


    }
}
