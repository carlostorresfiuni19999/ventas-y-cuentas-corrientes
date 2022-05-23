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
            //Validamos la cantidad de Cuotas
            if (factura.CantidadCuotas < 0) return BadRequest("Cuota no valida");
            //Obtenemos el Pedido Original de la base de datos
            var Pedido = db.Pedidos
                .Include(c => c.Cliente)
                .Include(c => c.Vendedor)
                .FirstOrDefault(c => c.Id == factura.IdPedido);
            //Creamos la factura

            var FacturaDb = new Factura
            {
                CantidadCuotas = factura.CantidadCuotas,
                ClienteId = factura.Pedido.ClienteId,
                VendedorId = Pedido.IdVendedor,
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
            foreach(var item in factura.Pedido.Pedidos)
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

                var Stock = db.Stocks.Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .Where(s => !s.Deleted)
                    .FirstOrDefault(s => s.IdDeposito == 3 && s.IdProducto == item.ProductoId);

                //Verificamos si hay Stock suficiente para facturar

                if(item.CantidadProducto >= Stock.Cantidad)
                {
                    PedidoDetalle.CantidadFacturada = Stock.Cantidad;
                    Stock.Cantidad = 0;
                }else
                {
                    PedidoDetalle.CantidadFacturada = Stock.Cantidad - item.CantidadProducto;
                    Stock.Cantidad -= item.CantidadProducto;
                }
                //Actualizamos el Stock

                db.Entry(Stock).State = EntityState.Modified;

                //Guardamos el PedidoDetalle Modificado

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
            FacturaDb.Monto = monto;
            FacturaDb.Saldo = saldo;
            FacturaDb.Iva = iva;

            //Si es Credito Verificamos si hay Saldo Disponible
            if(FacturaDb.CantidadCuotas > 1)
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
                    Monto = (FacturaDb.Monto + FacturaDb.Iva) / factura.CantidadCuotas,
                    Saldo = (FacturaDb.Monto + FacturaDb.Iva) / factura.CantidadCuotas,
                };

                db.VencimientoFacturas.Add(cuota);
            }

            //Verificamos si el Pedido se esta Facturando, Pendiente, o ya esta Facturado
            bool pendiente = true;

            //Recuperamos los Pedidos Detalles

            var PedidosDetalles = db.PedidoDetalles
                .Include(p => p.Pedido)
                .Include(p => p.Producto);

            Pedido.Estado = PedidosDetalles
                .Where(p => p.CantidadFacturada == p.CantidadProducto)
                .Count() < PedidosDetalles.Count() ? "PENDIENTE" : "FACTURADO";


            //Guardamos El nuevo estado del Pedido
            db.Entry(Pedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }catch(Exception e)
            {
                return BadRequest("Ocurrio un error al efectuar la transaccion: " + e.Message);
            }
            return Ok("Guardado con exito");
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
        [Route("api/PedidosSinFactura")]
        [HttpGet]
        public List<PedidoResponseDTO> GetPedidosSinFactura()
        {
            return (((from pedido in db.Pedidos

                    join cliente in db.Personas
                    on pedido.IdCliente equals cliente.Id

                    join vendedor in db.Personas
                    on pedido.IdVendedor equals vendedor.Id

                    join factura in db.Facturas.Include(f => f.Pedido)
                    on pedido.Id equals factura.PedidoId

                    into PedidosTotales
                    from p in PedidosTotales.DefaultIfEmpty()

                    where pedido.Deleted == false
                    where p.Pedido == null
                    select new 
                    {
                        Id = (int)pedido.Id,
                        Vendedor = pedido.Vendedor,
                        Cliente = pedido.Cliente,
                        IdCliente = pedido.IdCliente,
                        IdVendedor = pedido.IdVendedor,
                        Estado = pedido.Estado,
                        CondicionVenta = pedido.CondicionVenta,
                        PedidoDescripcion = pedido.PedidoDescripcion,
                        FechaPedido = pedido.FechaPedido,
                        NumeroPedido = pedido.NumeroPedido

                    }))).ToList().ConvertAll(p =>
                    new Pedido{
                        Id = (int)p.Id,
                        Vendedor = p.Vendedor,
                        Cliente = p.Cliente,
                        IdCliente = p.IdCliente,
                        IdVendedor = p.IdVendedor,
                        Estado = p.Estado,
                        CondicionVenta = p.CondicionVenta,
                        PedidoDescripcion = p.PedidoDescripcion,
                        FechaPedido = p.FechaPedido,
                        NumeroPedido = p.NumeroPedido
                    }).ConvertAll(p => PedidoMapper(p));
                /*(from pedido in db.Pedidos.Include(c => c.Cliente)
                    .Include(v => v.Vendedor)
                    join cliente in db.Personas
                    on pedido.IdCliente equals cliente.Id

                    join vendedor in db.Personas
                    on pedido.IdVendedor equals vendedor.Id

                    join factura in db.Facturas.Include(f => f.Pedido)
                    on pedido.Id equals factura.PedidoId

                    into PedidosTotales from p in PedidosTotales.DefaultIfEmpty()
                    where p != null
                    select p).ToList().FindAll(p => p.Pedido!= null).ConvertAll(
                p => new Pedido{
                    Id = (int)p.PedidoId,
                        Vendedor = p.Vendedor,
                        Cliente = p.Cliente,
                        IdCliente = p.ClienteId,
                        IdVendedor = p.VendedorId,
                        Estado = p.Estado,
                        CondicionVenta = p.CondicionVenta,
                        PedidoDescripcion = p.Pedido.PedidoDescripcion,
                        FechaPedido = p.Pedido.FechaPedido,
                        NumeroPedido = p.Pedido.NumeroPedido


                    });
            */
            
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
        private PedidoResponseDTO PedidoMapper(Pedido p)
        {
            var detalles = db.PedidoDetalles
                .Include(x => x.Pedido)
                .Include(x => x.Producto)
                .Where(x => !x.Deleted)
                .Where(x => x.IdPedido == p.Id);
            double precioTotal = 0;
            double ivaTotal = 0;
            foreach (var pdt in detalles)
            {
                precioTotal = precioTotal + pdt.Producto.Precio * pdt.CantidadFacturada;
                ivaTotal = ivaTotal + pdt.Producto.Iva * pdt.CantidadFacturada;
            }
            var prdto = new PedidoResponseDTO
            {
                Id = p.Id,
                Cliente = new PersonaResponseDTO
                {
                    Id = (int)p.IdCliente,
                    Nombre = p.Cliente.Nombre,
                    Apellido = p.Cliente.Apellido,
                    DocumentoTipo = p.Cliente.DocumentoTipo,
                    Documento = p.Cliente.Documento

                },
                Vendedor = new PersonaResponseDTO
                {
                    Id = (int)p.IdVendedor,
                    Nombre = p.Vendedor.Nombre,
                    Apellido = p.Vendedor.Apellido,
                    DocumentoTipo = p.Vendedor.DocumentoTipo,
                    Documento = p.Vendedor.Documento
                },
                PedidoDescripcion = p.PedidoDescripcion,
                Estado = p.Estado,
                CondicionVenta = p.CondicionVenta,
                FechePedido = p.FechaPedido,
                PrecioTotal = precioTotal,
                IvaTotal = ivaTotal,
                CostoTotal = precioTotal + ivaTotal,
                PedidosDetalles = db.PedidoDetalles
                        .Include(pd => pd.Producto)
                        .Where(pd => pd.IdPedido == p.Id)
                        .ToList()
                        .ConvertAll(pd => new PedidoDetalleResponseDTO
                        {
                            Id = pd.Id,
                            Producto = new ProductoResponseDTO
                            {
                                Id = pd.Producto.Id,
                                NombreProducto = pd.Producto.NombreProducto,
                                DescripcionProducto = pd.Producto.DescripcionProducto,
                                CodigoDeBarra = pd.Producto.CodigoDeBarra,
                                MarcaProducto = pd.Producto.MarcaProducto,
                                Precio = pd.Producto.Precio,
                                Iva = pd.Producto.Iva
                            },
                            CantidadFacturada = pd.CantidadFacturada,
                            CantidadProductos = pd.CantidadProducto

                        })
            };
            return prdto;
        }
    }
}