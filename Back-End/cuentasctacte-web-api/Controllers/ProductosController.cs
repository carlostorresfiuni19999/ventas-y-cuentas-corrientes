using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.Entities;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Collections.Generic;
using cuentasctacte_web_api.Models.DTOs;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class ProductosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Productos
        public IQueryable<Producto> GetProductos()
        {
            return db.Productos.Where(p => !p.Deleted);
        }
        [HttpGet]
        [Authorize(Roles ="Administrador")]
        [Route("api/Productos/Stocks")]
        [ResponseType(typeof(List<StockDTO>))]
        public IHttpActionResult GetStocks()
        {
            return Ok(
                db.Stocks
                .Include(s => s.Producto)
                .Include(s => s.Deposito)
                .Where(p => !p.Deleted)
                .ToList()
                .ConvertAll(s => new StockDTO()
                {
                    Id = s.Id,
                    PrecioUnitario = s.Producto.Precio,
                    Marca = s.Producto.MarcaProducto,
                    Producto = s.Producto.NombreProducto,
                    Cantidad = s.Cantidad,
                    Deposito = s.Deposito.NombreDeposito,
                    Iva = s.Producto.Iva
                })
                ); 
            ;

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