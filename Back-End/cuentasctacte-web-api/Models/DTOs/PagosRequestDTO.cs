using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PagosRequestDTO
    {
  
        public int IdCliente { get; set; }
        public int IdFactura { get; set; }

    }
}
