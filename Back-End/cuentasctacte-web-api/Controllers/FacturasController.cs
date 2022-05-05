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
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;

namespace cuentasctacte_web_api.Controllers
{
    public class FacturasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Facturas
        public IQueryable<Factura> GetFacturas()
        {
            return db.Facturas;
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
            var Pedido = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor)
                .FirstOrDefault(p => p.Id == factura.IdPedido);

            var Pedidos = db.PedidoDetalles
                .Include(pd => pd.Pedido)
                .Include(pd => pd.Producto)
                .Where(pd => pd.IdPedido == factura.IdPedido);


            var Factura = new Factura
            {
                PedidoId = factura.IdPedido,
                VendedorId = Pedido.IdVendedor,
                ClienteId = Pedido.IdCliente,
                CondicionVenta = factura.CondicionVenta,
                FechaFactura = DateTime.Now
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
            } else
            {
                Factura.Estado = "FACTURADO";
            }

            Pedido.Estado = Factura.Estado;
            Pedido.CondicionVenta = Factura.CondicionVenta;

            db.Entry(Pedido).State = EntityState.Modified;
            db.Facturas.Add(Factura);

            foreach(var facturaDetalle in factura.FacturaDetalles)
            {
                var Producto = db.Productos
                    .FirstOrDefault(pr => pr.Id == facturaDetalle.IdProducto);

                var PedidosDetalles = db.PedidoDetalles
                    .Include(pd => pd.Producto)
                    .Include(pd => pd.Pedido)
                    .Where(pd => pd.IdProducto == facturaDetalle.IdProducto)
                    .Where(pd => pd.IdPedido == factura.IdPedido)
                    .Where(pd => !pd.Deleted);

                int cant_productos = 0;
                foreach(var item in PedidosDetalles)
                {
                    cant_productos += item.CantidadFacturada;
                }
               
                var fd = new FacturaDetalle
                {
                    ProductoId = facturaDetalle.IdProducto,
                    CantidadCuotas = facturaDetalle.CantidadCuotas,
                    PrecioUnitario = Producto.Precio,
                    Iva = Producto.Iva,
                    Cantidad = cant_productos,
                    FacturaId = Factura.Id
                 };

                db.FacturaDetalles.Add(fd);

                for(int i = 0; i < fd.CantidadCuotas; i++)
                {
                    var cuota = new VencimientoFactura
                    {
                        FacturaId = Factura.Id,
                        Monto = (fd.Cantidad * fd.PrecioUnitario + fd.Iva * fd.Cantidad) / fd.CantidadCuotas,
                        Saldo = (fd.Cantidad * fd.PrecioUnitario + fd.Iva * fd.Cantidad) / fd.CantidadCuotas,
                        FechaVencimiento = Factura.FechaFactura.AddMonths(i + 1)

                    };

                    db.VencimientoFacturas.Add(cuota);
                }
             }
            try
            {
                db.SaveChanges();
                return Ok("Se ha guardado con exito");
            }catch(Exception ex)
            {
                return BadRequest("Error al ejecutar la transaccion: "+ex.Message);
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