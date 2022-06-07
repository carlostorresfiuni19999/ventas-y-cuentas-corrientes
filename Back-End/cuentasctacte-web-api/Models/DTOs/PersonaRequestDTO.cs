using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PersonaRequestDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Doc { get; set;}
        public string DocumentoTipo { get; set; }
        public double LineaDeCreadito { get; set; }
        public List<string> Roles { get; set; } 
        public string Telefono { get; set; }
        public string UserName { get; set; }
    }
}
