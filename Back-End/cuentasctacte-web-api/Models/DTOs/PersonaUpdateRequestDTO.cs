using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PersonaUpdateRequestDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Doc { get; set; }
        [Required]
        public string DocumentoTipo { get; set; }
        [Required]
        public double LineaDeCredito { get; set; }
        [Required]
        public List<string> Roles { get; set; }
        [Required]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "Correo electrónico")]
        public string UserName { get; set; }
    }
}
