using System;

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

        public string DocumentoTipo { get; set; }
        public string Documento { get; set; }
    }
}