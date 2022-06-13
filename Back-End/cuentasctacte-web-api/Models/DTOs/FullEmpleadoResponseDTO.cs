using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class FullEmpleadoResponseDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public string DocumentoTipo { get; set; }
        public List<string> Roles { get; set; }

        public double LineaDeCredito { get; set; }
        public double Saldo { get; set; }
        public string Telefono { get; set; }
    }

   }

