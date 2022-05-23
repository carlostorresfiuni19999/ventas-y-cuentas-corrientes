using System;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FullCuotaResponseDTO
    {
        public DateTime FechaVencimiento { get; set; }

        public double Monto { get; set; }
        public double Saldo { get; set; }
    }
}