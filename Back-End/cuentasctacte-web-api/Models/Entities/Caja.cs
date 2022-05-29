using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    [Table("Cajas")]
    public class Caja
    {

        public int Id { get; set; }
        public int NumCaja { get; set; }
        public string NombreCaja { get; set; }
        public Persona Cajero { get; set; }

        [ForeignKey("Cajero")]
        public int IdCajero { get; set; }

    }
}
