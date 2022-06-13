using System;
using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FullPagoResponseDTO
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        public PersonaResponseDTO Cliente { get; set; }
        public PersonaResponseDTO Cajero { get; set; }
        public double MontoTotal { get; set; }
        public List<FormasPagosResponseDTO> FormasPagos { get; set; }
    }
}
