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
using System.Timers;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class FacturasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Facturas
        [Authorize(Roles = "Administrador,Vendedor,Cajero")]
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
        [Authorize(Roles ="Administrador,Vendedor,Cajero")]
        [ResponseType(typeof(FullFacturaResponseDTO))]
        public IHttpActionResult GetFactura(int id)
        {
            Factura factura = db.Facturas
                .Include(f => f.Pedido)
                .Include(f => f.Cliente)
                .FirstOrDefault(f => f.Id == id);
            if (factura == null || factura.Deleted)
            {
                return NotFound();
            }


            return Ok(MapToFullFactura(factura));
        }

     
        // POST: api/Facturas
        [Authorize(Roles ="Vendedor,Administrador")]
        [ResponseType(typeof(FacturaRequestDTO))]
        [Route("api/Facturas/create")]
        public IHttpActionResult PostFactura(FacturaRequestDTO factura)
        {
            //Validamos la cantidad de Cuotas
            if (factura.CantidadCuotas < 1) return BadRequest("Cuota no valida");
            //Obtenemos el Pedido Original de la base de datos
            var Pedido = db.Pedidos
                .Include(c => c.Cliente)
                .Include(c => c.Vendedor)
                .FirstOrDefault(c => c.Id == factura.IdPedido);
            //Creamos la factura
            var Vendedor = Helpers.GetUserLogged.GetUser(db, User.Identity.GetUserId());
            var FacturaDb = new Factura
            {
                CantidadCuotas = factura.CantidadCuotas,
                ClienteId = factura.Pedido.ClienteId,
                VendedorId = Vendedor.Id,
                PedidoId = factura.IdPedido,
                FechaFactura = DateTime.Now,
                Estado = "PENDIENTE",
                CondicionVenta = factura.CantidadCuotas > 1 ? "CREDITO" : "CONTADO",
                Monto = 0.0,
                Saldo = 0.0,
                Iva = 0.0
            };

            //Guardamos la factura en el contexto

            var FacturaSaved = db.Facturas.Add(FacturaDb);
            double monto = 0.0;
            double saldo = 0.0;
            double iva = 0.0;

            //Iterar sobre los Productos que se van a facturar
            foreach (var item in factura.Pedido.Pedidos)
            {
                //Obtenemos el PedidoDetalle que queremos Setear
                var PedidoDetalle = db.PedidoDetalles
                    .Include(p => p.Pedido)
                    .Include(p => p.Producto)
                    .Where(p => p.IdPedido == factura.IdPedido)
                    .Where(p => !p.Deleted)
                    .Where(p => p.IdProducto == item.ProductoId)
                    .FirstOrDefault();

                //Ahora, Seleccionamos el Stock que se va a modificar
                if (PedidoDetalle == null)
                {
                    PedidoDetalle = new PedidoDetalle();
                    PedidoDetalle.CantidadProducto = item.CantidadProducto;
                    PedidoDetalle.IdProducto = item.ProductoId;
                    PedidoDetalle.IdPedido = factura.IdPedido;
                }
                var Stock = db.Stocks.Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .Where(s => !s.Deleted)
                    .FirstOrDefault(s => s.IdDeposito == 3 && s.IdProducto == item.ProductoId);

                //Verificamos si hay Stock suficiente para facturar

                if (item.CantidadProducto >= Stock.Cantidad)
                {
                    PedidoDetalle.CantidadFacturada = Stock.Cantidad;
                    Stock.Cantidad = 0;
                }
                else
                {
                    PedidoDetalle.CantidadFacturada = item.CantidadProducto;
                    Stock.Cantidad -= item.CantidadProducto;
                }
                //Actualizamos el Stock

                db.Entry(Stock).State = EntityState.Modified;

                //Guardamos el PedidoDetalle Modificado

                if (PedidoDetalle.CantidadFacturada > PedidoDetalle.CantidadProducto) PedidoDetalle.CantidadProducto = PedidoDetalle.CantidadFacturada;

                db.Entry(PedidoDetalle).State = EntityState.Modified;

                //Calculamos el Monto, Saldo e Iva de todos los Productos Facturados

                monto += PedidoDetalle.CantidadFacturada * PedidoDetalle.Producto.Precio;
                iva += PedidoDetalle.CantidadFacturada * PedidoDetalle.Producto.Iva;

                //Empezamos con la Carga de los detalles de las Facturas
                var FacturaDetalle = new FacturaDetalle
                {
                    FacturaId = FacturaDb.Id,
                    ProductoId = PedidoDetalle.IdProducto,
                    PrecioUnitario = PedidoDetalle.Producto.Precio,
                    Iva = PedidoDetalle.Producto.Iva,
                    Cantidad = PedidoDetalle.CantidadFacturada

                };

                //Guardamos los detalles de las Facturas
                db.FacturaDetalles.Add(FacturaDetalle);
            }

            saldo = monto;
            //Verificamos si la factura es Contado o Credito
            FacturaDb.Monto = monto + iva;
            FacturaDb.Saldo = saldo + iva;
            FacturaDb.Iva = iva;

            //Si es Credito Verificamos si hay Saldo Disponible
            if (FacturaDb.CantidadCuotas > 1)
            {
                var Cliente = FacturaDb.Cliente;

                Cliente.Saldo += monto;
                if (Cliente.Saldo > Cliente.LineaDeCredito) return BadRequest("Linea de Credito insuficiente");

                //Guardamos los cambios hechos al cliente
                db.Entry(Cliente).State = EntityState.Modified;
            }
            //Guardamos los cambios Generados en el Contexto
            db.Entry(FacturaDb).State = EntityState.Added;

            //Generamos los vencimientos de las Facturas
            for (int i = 0; i < factura.CantidadCuotas; i++)
            {
                var Vencimiento = FacturaDb.FechaFactura;
                var cuota = new VencimientoFactura()
                {
                    FacturaId = FacturaDb.Id,
                    FechaVencimiento = Vencimiento.AddMonths(i + 1),
                    Monto = (FacturaDb.Monto) / factura.CantidadCuotas,
                    Saldo = (FacturaDb.Monto) / factura.CantidadCuotas,
                    Deleted = false
                };

                db.VencimientoFacturas.Add(cuota);
            }

            //Verificamos si el Pedido se esta Facturando, Pendiente, o ya esta Facturado


            //Recuperamos los Pedidos Detalles

            var PedidosDetalles = db.PedidoDetalles
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .Where(p => !p.Deleted)
                .Where(p => p.IdPedido == FacturaSaved.PedidoId);

            Pedido.Estado = PedidosDetalles
                .Where(p => p.CantidadFacturada == p.CantidadProducto)
                .Count() < PedidosDetalles.Count() ? "FACTURANDO" : "FACTURADO";
            Pedido.CondicionVenta = FacturaDb.CondicionVenta;

            //Guardamos El nuevo estado del Pedido
            db.Entry(Pedido).State = EntityState.Modified;

            //Unificando los PedidosDetalles
            List<PedidoDetalle> Existentes =new List<PedidoDetalle>();
            foreach (var pedidoDetalle in PedidosDetalles )
            {
                var dets = db.PedidoDetalles
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .Where(p => p.IdProducto == pedidoDetalle.IdProducto
                && p.IdPedido == pedidoDetalle.IdPedido);

                if(dets.Count() > 1)
                {
                    var newDetalle = pedidoDetalle;
                    newDetalle.CantidadProducto = dets.Sum(p => p.CantidadProducto);
                    newDetalle.CantidadFacturada = dets.Sum(p => p.CantidadFacturada);
                    
                    Existentes.Add(newDetalle);

                    dets.ForEachAsync(p =>
                    {
                        p.Deleted = true;
                        db.Entry(p).State = EntityState.Modified;
                    });
                }
                else
                {
                    Existentes.Add(dets.FirstOrDefault());
                }
            }

            Existentes.ForEach(p => db.PedidoDetalles.Add(p));
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrio un error al efectuar la transaccion: " + e.Message);
            }
            return Ok("Guardado con exito");
        }

        // DELETE: api/Facturas/5
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteFactura(int id)
        {
            Factura factura = db.Facturas
                .Where(f => !f.Deleted)
                .Include(f => f.Pedido)
                .Include(f => f.Cliente)
                .FirstOrDefault(f => f.Id == id);
            if (factura == null)
            {
                return NotFound();
            }
            if (factura.Estado != "PENDIENTE") return BadRequest("No se puede borrar una factura que ha comenzado el proceso de Pago");
            var FacturaDetalles = db.FacturaDetalles
                .Include(fd => fd.Factura)
                .Include(fd => fd.Producto)
                .Where(fd => !fd.Deleted)
                .Where(fd => fd.FacturaId == id);

            //Iteramos sobre las facturasdetalles y seteamos los valores del PedidoDetalle

            foreach (var item in FacturaDetalles)
            {
                var PedidoDetalle = db.PedidoDetalles
                    .Include(pd => pd.Pedido)
                    .Include(pd => pd.Producto)
                    .FirstOrDefault(pd =>
                    pd.IdPedido == factura.PedidoId
                    && pd.IdProducto == item.ProductoId);

                PedidoDetalle.CantidadFacturada -= item.Cantidad;
                item.Deleted = true;
                db.Entry(PedidoDetalle).State = EntityState.Modified;
                db.Entry(item).State = EntityState.Modified;
            }
            var Cuotas = db.VencimientoFacturas
                .Include(c => c.Factura)
                .Where(c => !c.Deleted);

            //Eliminamos las cuotas
            foreach (var item in Cuotas)
            {
                item.Deleted = true;
                db.Entry(item).State = EntityState.Modified;
            }

            factura.Deleted = true;

            db.Entry(factura).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                return Ok("Eliminado Con Exito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al ejecutar la transaccion: " + ex.Message);
            }

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

        public FullFacturaResponseDTO MapToFullFactura(Factura factura)
        {
            var Result = new FullFacturaResponseDTO()
            {
                IdFactura = (int) factura.Id,
                IdPedido = (int) factura.PedidoId,
                Cliente = factura.Cliente.Nombre + " " + factura.Cliente.Apellido,
                DocCliente = factura.Cliente.Documento,
                PrecioTotal = factura.Monto,
                IvaTotal = factura.Iva,
                SaldoTotal = factura.Saldo,
                Estado = factura.Estado,
                FechaFacturacion = factura.FechaFactura,
                Detalles = db.FacturaDetalles
                .Include(fd => fd.Factura)
                .Include(fd => fd.Producto)
                .Where(fd => fd.FacturaId == factura.Id)
                .ToList()
                .ConvertAll(fd => new FacturaDetalleResponseDTO
                {
                    Id = fd.Id,
                    Producto = fd.Producto.MarcaProducto + " " + fd.Producto.NombreProducto,
                    Cantidad = fd.Cantidad,
                    Iva = fd.Iva,
                    PrecioUnitario = fd.PrecioUnitario
                }),
                Cuotas = db.VencimientoFacturas
                    .Include(c => c.Factura)
                    .Where(c => !c.Deleted)
                    .Where(c => c.FacturaId == factura.Id)
                    .ToList()
                    .ConvertAll(c => new FullCuotaResponseDTO()
                    {
                        Id = c.Id,
                        Monto = c.Monto,
                        Saldo = c.Saldo,
                        FechaVencimiento = c.FechaVencimiento
                    })

            };
            return Result;
        }

        [Authorize(Roles = "Administrador,Cajero,Vendedor")]
        [Route("api/Pedidos/FacturaReporte")]
        [HttpGet]
        [ResponseType(typeof(FullFacturaResponseDTO))]
        public List<FullFacturaResponseDTO> FacturaReporte(DateTime fechaInicio , DateTime fechaFin , String estado = "ALL")
        {
            DateTime desde = fechaInicio.AddDays(-1);
            DateTime hasta = fechaFin.AddDays(1);


            List<FullFacturaResponseDTO> Facturas_entreFechas_RespondeDTO = new List<FullFacturaResponseDTO>();
            //Filtramos por fechas           
            List<Factura> facturaList = filtrarReporteFactura_tiempo(desde, hasta);
           
            //Filtramos por estado
            facturaList = filtrarReporteFacturas_estado(facturaList, estado);

            //Voy creando un MapFactura de cada una de las facturas ya filtradas por fecha.
            // y estado
            foreach (Factura factura in facturaList)
            {
                Facturas_entreFechas_RespondeDTO.Add(MapToFullFactura(factura));
            }

            return Facturas_entreFechas_RespondeDTO;
        }
        private static DateTime ParseDate(string providedDate)
        {
            DateTime validDate;
            string[] formats = { "dd/MM/yyyy hh:mm:ss", "yyyy-MM-dd'T'hh:mm:ss'Z'" };
            var dateFormatIsValid = DateTime.TryParseExact(
                providedDate,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out validDate);
            return dateFormatIsValid ? validDate : DateTime.MinValue;
        }

        public List<Factura> filtrarReporteFactura_tiempo(DateTime desde, DateTime hasta)

        {        
            List<Factura> facturaList = db.Facturas
                .Include(c => c.Cliente)
                .Where(f => !f.Deleted)   
                .Where(p =>
                      (DateTime.Compare(desde, p.FechaFactura) <= 0)
                      && (DateTime.Compare(hasta, p.FechaFactura) >= 0)
                  )
                .ToList();
            return facturaList;
        }
        //Esta funcion filtra deacuerdo alestado y devuelve una lista de facturas.
        public List<Factura> filtrarReporteFacturas_estado(List<Factura> F, string estado = "ALL")
        {

            List<Factura> facturas_filtradas_estado = new List<Factura>();


            if (estado == "ALL") return F; //Nada a filtrar
            //Filtramos por estado.
            switch (estado)
            {
                case "PENDIENTE":
                    foreach (var factura in F)
                    {
                        if (factura.Estado == "PENDIENTE")
                        {
                            facturas_filtradas_estado.Add(factura);
                        }

                    }

                    break;
                case "PROCESANDO":
                    foreach (var factura in F)
                    {
                        if (factura.Estado == "FACTURANDO")
                        {
                            facturas_filtradas_estado.Add(factura);
                        }

                    }
                    break;
                case "PAGADO":
                    foreach (var factura in F)
                    {
                        if (factura.Estado == "PAGADO")
                        {
                            facturas_filtradas_estado.Add(factura);
                        }

                    }
                    break;
            }

            return facturas_filtradas_estado;

        }

        
    }


    



}

