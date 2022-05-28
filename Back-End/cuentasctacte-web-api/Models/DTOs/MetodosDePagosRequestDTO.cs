using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class MetodosDePagosRequestDTO
    {
        public string FormaDePago { get; set; }
        public double Monto { get; set; }
    }
}
