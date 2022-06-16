using System;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PagoResponseDTO
    {
        public int Id { get; set; }
        public double MontoTotal { get; set; }
        public string Cliente { get; set; }
        public DateTime FechaCreado { get; set; }
        public string CI { get; set; }
    }
}