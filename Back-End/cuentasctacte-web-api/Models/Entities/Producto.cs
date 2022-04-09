using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models.Entities
{
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