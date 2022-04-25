using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.Entities;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class ProductosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Productos
        public IQueryable<Producto> GetProductos()
        {
            return db.Productos.Where(p => !p.Deleted );
        }

        // GET: api/Productos/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult GetProducto(int id)
        {
            Producto producto = db.Productos.Find(id);
            if (producto == null || producto.Deleted)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductoExists(int id)
        {
            return db.Productos.Count(e => e.Id == id) > 0;
        }
    }
}