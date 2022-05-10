using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models
{
    [Table("Modificaciones")]
    public class Modificaciones
    {
        public int Id { get; set; }
        public string ModificadoPor { get; set; }
        public int IdPersona { get; set; }
        public string Data { get; set; }
        public string ModificacionTipo { get; set; }
        public DateTime FechaModificacion { get; set; }

    }
}