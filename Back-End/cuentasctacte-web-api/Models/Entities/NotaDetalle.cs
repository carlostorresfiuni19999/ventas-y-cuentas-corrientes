using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    public class NotaDetalle : Data
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int NotaId { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Iva { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
        [ForeignKey("NotaId")]
        public NotaDeCredito Nota { get; set; }

    }
}