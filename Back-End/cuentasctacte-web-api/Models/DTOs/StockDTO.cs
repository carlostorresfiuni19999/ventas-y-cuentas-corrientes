using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class  StockDTO
    {
        public int Id { get; set; }
        public string Producto { get; set; }   
        public string Marca { get; set; }
        
        public string Deposito { get; set; }

        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }

        public double Iva { get; set; }
    }
}
