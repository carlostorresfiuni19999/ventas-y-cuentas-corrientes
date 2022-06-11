using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuentasctacte_web_api.Models.DTOs
{
    public class PersonaRequestDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Doc { get; set;}
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

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
