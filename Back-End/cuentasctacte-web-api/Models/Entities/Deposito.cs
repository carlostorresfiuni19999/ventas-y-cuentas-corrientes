using System.ComponentModel.DataAnnotations.Schema;

namespace cuentasctacte_web_api.Models.Entities
{
    [Table("Depositos")]
    public class Deposito : Data
    {

        public int Id { get; set; }
        public string NombreDeposito { get; set; }
        public string DescripcionDeposito { get; set; }

    }
}