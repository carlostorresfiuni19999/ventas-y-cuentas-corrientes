using System.Collections.Generic;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PagoRequestDTO
    {
        public int IdCuota { get; set; }

        public List<CuotaRequestDTO> MetodosPagos { get; set; }
    }
}
