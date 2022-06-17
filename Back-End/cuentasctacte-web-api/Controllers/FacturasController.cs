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
            List<PedidoDetalle> NoExistentes = new List<PedidoDetalle>();   
            //Iterar sobre los Productos que se van a facturar
            foreach (var item in factura.Pedido.Pedidos)
            {
                //Obtenemos el PedidoDetalle que queremos Setear
                var PedidoDetalle = db.PedidoDetalles
                    .Include(p => p.Pedido)
                    .Include(p => p.Producto)
                    .Where(p => p.IdPedido == factura.IdPedido)
                    .Where(p => p.IdProducto == item.ProductoId)
                    .Where(p => !p.Deleted)
                    .FirstOrDefault();

                var Producto = db.Productos.Find(item.ProductoId);
                var FacturaDetalle = new FacturaDetalle();
                FacturaDetalle.FacturaId = FacturaDb.Id;
                FacturaDetalle.ProductoId = item.ProductoId;
                FacturaDetalle.PrecioUnitario = Producto.Precio;
                var Stock = db.Stocks
                    .Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .Where(s => !s.Deleted)
                    .FirstOrDefault(s => s.IdProducto == Producto.Id);

                //Verificamos si es NUll. si es null creamos otro y guardamos en el context
                if (PedidoDetalle == null)
                {
                    PedidoDetalle = new PedidoDetalle();
                    PedidoDetalle.CantidadProducto = item.CantidadProducto;
                    PedidoDetalle.IdProducto = item.ProductoId;
                    PedidoDetalle.IdPedido = factura.IdPedido;

                    if (Stock.Cantidad <= PedidoDetalle.CantidadProducto)
                    {
                        PedidoDetalle.CantidadFacturada = Stock.Cantidad;
                        
                        FacturaDetalle.Cantidad = Stock.Cantidad;
                        db.FacturaDetalles.Add(FacturaDetalle);
                        Stock.Cantidad = 0;


                    }
                    else
                    {
                        PedidoDetalle.CantidadFacturada += item.CantidadProducto;
                        Stock.Cantidad -= item.CantidadProducto;
                        FacturaDetalle.Cantidad = item.CantidadProducto;
                        db.FacturaDetalles.Add(FacturaDetalle);

                    }
                    db.Entry(Stock).State = EntityState.Modified;
                    NoExistentes.Add(PedidoDetalle);

                }

                else //Validamos los productos que existen en la base de datos
                {
                    
                    int cantProd = item.CantidadProducto;

                    if (cantProd >= PedidoDetalle.CantidadProducto)
                    {

                        int cantStock = Stock.Cantidad;


                        if (cantProd >= cantStock)
                        {
                            Stock.Cantidad = 0;
                            PedidoDetalle.CantidadFacturada = Stock.Cantidad;
                            PedidoDetalle.CantidadProducto = item.CantidadProducto;
                        }
                        else
                        {
                            Stock.Cantidad -= item.CantidadProducto;
                            PedidoDetalle.CantidadProducto = item.CantidadProducto;
                            PedidoDetalle.CantidadFacturada = item.CantidadProducto;
                        }

                        db.Entry(Stock).State = EntityState.Modified;
                        db.Entry(PedidoDetalle).State = EntityState.Modified;

                    }
                    else
                    {
                        if (item.CantidadProducto >= Stock.Cantidad)
                        {
                            FacturaDetalle.Cantidad = Stock.Cantidad;
                            Stock.Cantidad = 0;
                            PedidoDetalle.CantidadFacturada = Stock.Cantidad;
                            db.FacturaDetalles.Add(FacturaDetalle);
                        }
                        else
                        {
                            Stock.Cantidad -= item.CantidadProducto;
                            FacturaDetalle.Cantidad = item.CantidadProducto;
                            PedidoDetalle.CantidadFacturada += item.CantidadProducto;
                            db.FacturaDetalles.Add(FacturaDetalle);
                        }

                        db.Entry(Stock).State = EntityState.Modified;
                        db.Entry(PedidoDetalle).State = EntityState.Modified;


                    }

                }



                monto += FacturaDetalle.Cantidad * Producto.Precio;
                iva += FacturaDetalle.Cantidad * Producto.Iva;

               

                
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

 
            //Unificamos los nuevos detalles agregados
            
            NoExistentes.ForEach(nx =>
            {
                var query = NoExistentes.Where(x => x.IdPedido == nx.IdPedido
                && x.IdProducto == nx.IdProducto);
                int cant = query.Count();

                if(cant > 1)
                {
                    var temp = nx;
                    temp.CantidadProducto = query.Sum(p => p.CantidadProducto);
                    temp.CantidadFacturada = query.Sum(p => p.CantidadFacturada);
                    NoExistentes.Remove(nx);
                    NoExistentes.Add(temp);

                }

            });

            NoExistentes.ForEach(nx => db.PedidoDetalles.Add(nx));

            bool facturado = db.PedidoDetalles
                .Where(pd => !pd.Deleted)
                .Where(pd => pd.IdPedido == FacturaDb.PedidoId)
                .ToList()
                .All(pd => pd.CantidadFacturada == pd.CantidadProducto);

            
            Pedido.Estado = facturado ? "FACTURADO" : "FACTURANDO";

            db.Entry(Pedido).State = EntityState.Modified;
 
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

            var Pedido = db.Facturas
                .Where(f => !f.Deleted)
                .Include(f => f.Pedido)
                .Include(f => f.Cliente)
                .Where(f => f.Id == id)
                .Select(f => f.Pedido)
                .FirstOrDefault();
            //Contar todas las facturas de ese pedido
            bool esUltimo = db.Facturas
                    .Where(f => f.PedidoId == Pedido.Id && (!f.Deleted))
                    .Count() == 1;

            Pedido.Estado = esUltimo ? "PENDIENTE" : "FACTURANDO";
            db.Entry(Pedido).State = EntityState.Modified;

            if (factura.Estado != "PENDIENTE") return BadRequest("No se puede borrar una factura que ha comenzado el proceso de Pago");
            var FacturaDetalles = db.FacturaDetalles
                .Include(fd => fd.Factura)
                .Include(fd => fd.Producto)
                .Where(fd => !fd.Deleted)
                .Where(fd => fd.FacturaId == id);

            //Iteramos sobre las facturasdetalles y seteamos los valores del PedidoDetalle
            
            foreach (var item in FacturaDetalles)
            {
                var Stock = db.Stocks
                    .Include(s => s.Deposito)
                    .Include(s => s.Producto)
                    .FirstOrDefault(s =>
                        s.IdProducto == item.ProductoId && 3 == s.IdDeposito);

                Stock.Cantidad += item.Cantidad;

                var PedidoDetalle = db.PedidoDetalles
                    .Include(pd => pd.Pedido)
                    .Include(pd => pd.Producto)
                    .FirstOrDefault(pd =>
                    pd.IdPedido == factura.PedidoId
                    && pd.IdProducto == item.ProductoId);

                PedidoDetalle.CantidadFacturada -= item.Cantidad;
                item.Deleted = true;
                db.Entry(Stock).State = EntityState.Modified;
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

            var Cliente = db.Facturas
                .Where(f => f.Id == factura.Id)
                .Select(f => f.Cliente)
                .FirstOrDefault();

            Cliente.Saldo -= factura.Monto;
            db.Entry(Cliente).State = EntityState.Modified;
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
                        if (factura.Estado == "PROCESANDO")
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

