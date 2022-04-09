using cuentasctacte_web_api.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cuentasctacte_web_api.Models
{
    public class Stock : Data
    {
       
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public Producto Producto { get; set; }
        public Deposito Deposito { get; set; }

        [ForeignKey("Producto")]
        public int? IdProducto { get; set; }
        [ForeignKey("Deposito")]
        public int? IdDeposito { get; set; }



    }
}