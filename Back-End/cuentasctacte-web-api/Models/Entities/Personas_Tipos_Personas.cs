using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    public class Personas_Tipos_Personas : Data
    {
        public int Id { get; set; }
        public Persona Persona { get; set; }
        public TipoPersona TipoPersona { get; set; }
        [ForeignKey("Persona")]
        public int? IdPersona { get; set; } 
        [ForeignKey("TipoPersona")]
        public int? IdTipoPersona { get; set; }
    }
}