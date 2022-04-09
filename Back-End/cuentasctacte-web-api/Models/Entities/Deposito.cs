using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    public class Deposito : Data
    {
        public int Id { get; set; }
        public string NombreDeposito { get; set; }
        public string DescripcionDeposito { get; set; }

    }
}