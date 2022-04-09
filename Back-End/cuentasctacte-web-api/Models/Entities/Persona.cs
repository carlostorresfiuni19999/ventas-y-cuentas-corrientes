using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    public class Persona : Data
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string DocumentoTipo { get; set; }
        public string Documento { get; set; }
        public double LineaDeCredito { get; set; }
        public double Saldo { get; set; }
    }
}