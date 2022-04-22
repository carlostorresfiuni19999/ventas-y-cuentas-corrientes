using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
    [Table("Productos")]
    public class Producto : Data
    {
       
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string MarcaProducto { get; set; }
        public string CodigoDeBarra { get; set; }
        public bool TieneIva { get; set; }
        public double Precio { get; set; }
        public double Iva { get; set; }

    }
}