using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class FacturasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Facturas

        public List<FacturaResponseDTO> GetFacturas()
        {


            return db.Facturas
                .Include(c => c.Cliente)
                .Where(f => !f.Deleted)
                .ToList()
                .ConvertAll(f => new FacturaResponseDTO
                {
                    Id = f.Id,
                    MontoTotal = f.Monto,
                    SaldoTotal = f.Saldo,
                    FechaFacturada = f.FechaFactura,
                    Cliente = f.Cliente.Nombre + " " + f.Cliente.Apellido,
                    CondicionVenta = f.CondicionVenta,
                    Estado = f.Estado
                })
                ;
        }

        // GET: api/Facturas/5
        [ResponseType(typeof(Factura))]
        public IHttpActionResult GetFactura(int id)
        {
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return NotFound();
            }

            return Ok(factura);
        }

        // PUT: api/Facturas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFactura(int id, Factura factura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != factura.Id)
            {
                return BadRequest();
            }

            db.Entry(factura).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Facturas
        [ResponseType(typeof(FacturaRequestDTO))]
        public IHttpActionResult PostFactura(FacturaRequestDTO factura)
        {
            if (factura.CantidadCuotas < 1) return BadRequest("Cuota no valida");
            var Pedido = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor)
                .FirstOrDefault(p => p.Id == factura.IdPedido);
            bool exist = db.Facturas
                .Include(f => f.Pedido)
                .Where(f => !f.Deleted)
                .ToList()
                .Exists(f => f.PedidoId == factura.IdPedido);

            if (exist) return BadRequest("El pedido que intenta facturar, ya existe");
            var Pedidos = db.PedidoDetalles
                .Include(pd => pd.Pedido)
                .Include(pd => pd.Producto)
                .Where(pd => !pd.Deleted)
                .Where(pd => pd.IdPedido == factura.IdPedido);


            var Factura = new Factura
            {
                PedidoId = factura.IdPedido,
                VendedorId = Pedido.IdVendedor,
                ClienteId = Pedido.IdCliente,
                CondicionVenta = factura.CantidadCuotas > 1 ? "CREDITO" : "CONTADO",
                FechaFactura = DateTime.Now,
                CantidadCuotas = factura.CantidadCuotas
            };

            double monto = 0.0;
            double iva = 0.0;
            bool pendiente = false;
            foreach (var item in Pedidos)
            {
                if (item.CantidadProducto > item.CantidadFacturada) pendiente = true;
                monto += item.CantidadFacturada * item.Producto.Precio;
                iva += item.CantidadFacturada * item.Producto.Iva;

            }

            Factura.Saldo = monto + iva;
            Factura.Monto = monto + iva;
            Factura.Iva = iva;

            if (pendiente)
            {
                Factura.Estado = "PENDIENTE";
            }
            else
            {
                Factura.Estado = "FACTURADO";
            }

            Pedido.Estado = Factura.Estado;
            Pedido.CondicionVenta = Factura.CondicionVenta;

            db.Entry(Pedido).State = EntityState.Modified;
            db.Facturas.Add(Factura);

            foreach (var item in Pedidos)
            {
                var FacturaDetalle = new FacturaDetalle
                {
                    FacturaId = Factura.Id,
                    ProductoId = item.IdProducto,
                    Cantidad = item.CantidadFacturada,
                    PrecioUnitario = item.PrecioUnitario,
                    Iva = item.Producto.Iva
                };

                db.FacturaDetalles.Add(FacturaDetalle);
            }
            for (int i = 0; i < factura.CantidadCuotas; i++)
            {
                var cuota = new VencimientoFactura
                {
                    FacturaId = Factura.Id,
                    FechaVencimiento = Factura.FechaFactura.AddMonths(i),
                    Monto = Factura.Monto / Factura.CantidadCuotas,
                    Saldo = Factura.Monto / Factura.CantidadCuotas
                };

                db.VencimientoFacturas.Add(cuota);
            }

            try
            {
                db.SaveChanges();
                return Ok("Se ha guardado con exito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al ejecutar la transaccion: " + ex.Message);
            }
        }

        // DELETE: api/Facturas/5
        [ResponseType(typeof(Factura))]
        public IHttpActionResult DeleteFactura(int id)
        {
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return NotFound();
            }

            db.Facturas.Remove(factura);
            db.SaveChanges();

            return Ok(factura);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FacturaExists(int id)
        {
            return db.Facturas.Count(e => e.Id == id) > 0;
        }
    }
}