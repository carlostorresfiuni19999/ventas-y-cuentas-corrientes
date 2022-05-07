using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class PedidosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Pedidos
        public List<PedidoResponseDTO> GetPedidos()
        {
            var Pedidos = db.Pedidos
                .Where(p => !p.Deleted)
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor).ToList();
            return PedidosMapper(Pedidos);

        }

        // GET: api/Pedidos/5
        [ResponseType(typeof(PedidoResponseDTO))]
        public IHttpActionResult GetPedido(int id)
        {
            var Pedido = db.Pedidos
                .Include(p => p.Vendedor)
                .Include(p => p.Cliente)
                .FirstOrDefault(p => p.Id == id );
            if(null == Pedido)
            {
                return NotFound();
            }

            


            return Ok(PedidoMapper(Pedido));
        }
















        // PUT: api/Pedidos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedido(int id, PedidoDTORequest pedidoDTO_R)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*
                *Ver si existe el pedido.
            */
            if ( db.Pedidos.Include(p => p.Cliente).Where(p => p.Id == id) == null)
            {
                return BadRequest();
            }
            db.Entry(pedidoDTO_R).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            /*VARIABLES*/
            int cantidad_producto_DTO = 0;
            int id_producto_DTO = 0;
            int sumatoria = 0;

            //Primero Buscar un pedido dentro de base de datos con argumento 'id'
            Pedido pedido_DB = db.Pedidos.Include(p => p.Cliente).FirstOrDefault(p => p.Id == id);
            //Tambien traer los detalles.
            var PedidosDetalles_DB = db.PedidoDetalles
               .Include(pd => pd.Producto)
               .Where(pd => pd.IdPedido == id);

            //Segundo Rescatar de la data base, los pedidos detalles.
            //--- Tambien ir iterando para sacar cada pedido detalle individual

            foreach (var pedidoDetalle_DB in PedidosDetalles_DB)
            {

                //Tercero Crear copia del stock para cada producto coincidente.

                var Stock_DB = db.Stocks
                        .Include(s => s.Producto)
                        .Include(s => s.Deposito)
                        .Where(s => s.Producto.Id == pedidoDetalle_DB.Producto.Id && s.IdDeposito == 3)
                        .First();
                foreach (var producto in pedidoDTO_R.Pedidos)
                {
                    if (producto.ProductoId == pedidoDetalle_DB.IdProducto)
                    {
                        cantidad_producto_DTO = producto.CantidadProducto;
                        id_producto_DTO = producto.ProductoId;
                        break; //Rompemos el algoritmo para 
                               //porque extrajemos la cantidad en la database
                    }

                }

                var Stock_DB_R = db.Stocks
                        .Include(s => s.Producto)
                        .Include(s => s.Deposito)
                        .Where(s => s.Producto.Id == pedidoDetalle_DB.Producto.Id && s.IdDeposito == 3)
                        .First();






                /*--------------------*/
                if (cantidad_producto_DTO != pedidoDetalle_DB.CantidadProducto) { //Hubo modificacion.
                    //Pregunto si hay stock suficiente
                    int temp = cantidad_producto_DTO - pedidoDetalle_DB.CantidadFacturada;
                    if (temp > 0 && temp <= Stock_DB.Cantidad) { //aumento la cantidad de productos solicitados. y hay stock
                        pedidoDetalle_DB.CantidadProducto = cantidad_producto_DTO;
                        pedidoDetalle_DB.CantidadFacturada = cantidad_producto_DTO;
                        Stock_DB.Cantidad -= temp;
                    }
                    else
                    {if (temp < 0)//se resto la cantidad de productos solicitados.
                        {
                            pedidoDetalle_DB.CantidadProducto = cantidad_producto_DTO;
                            pedidoDetalle_DB.CantidadProducto = cantidad_producto_DTO;
                            Stock_DB.Cantidad -= temp;
                        }


                    }
                    
                    if (temp > 0) {
                        if (temp < Stock_DB.Cantidad)//hay que aumentar saldo cliente.
                        {
                            pedidoDetalle_DB.CantidadProducto = cantidad_producto_DTO;
                            pedidoDetalle_DB.CantidadFacturada = cantidad_producto_DTO;
                            Stock_DB.Cantidad -= temp;
                            sumatoria += temp* (int)pedidoDetalle_DB.PrecioUnitario; //temp positivo, aumentamos saldo
                        }
                        else { 
                        
                        
                        }
                    
                    
                    }



                }

                //Cuarto si la cantidad de producto actualizado es 0, entonces eliminamos el pedidi
                //detalles
                if (cantidad_producto_DTO == 0)
                {   //Devolvemos todo del Pd al estock.
                    //Seteamos a 0 la cantidad en Pdetalles
                    sumatoria -= pedidoDetalle_DB.CantidadFacturada * (int)pedidoDetalle_DB.PrecioUnitario;
                    Stock_DB.Cantidad += pedidoDetalle_DB.CantidadFacturada;
                    pedidoDetalle_DB.CantidadProducto = 0;
                    pedidoDetalle_DB.Deleted = true;
                }else
                {/*Se ha disminuido la cantidad de productos en el pedido*/
                    if (cantidad_producto_DTO < pedidoDetalle_DB.CantidadFacturada)
                    {
                        sumatoria -= (pedidoDetalle_DB.CantidadFacturada - cantidad_producto_DTO) * (int)pedidoDetalle_DB.PrecioUnitario;
                        Stock_DB.Cantidad += pedidoDetalle_DB.CantidadFacturada;
                        pedidoDetalle_DB.CantidadProducto = cantidad_producto_DTO;
                    }
                    else
                    {/*Se ha aumentado la cantidad de productos en el pedido*/
                        sumatoria += (cantidad_producto_DTO - pedidoDetalle_DB.CantidadFacturada) * (int)pedidoDetalle_DB.PrecioUnitario;
                        Stock_DB.Cantidad += pedidoDetalle_DB.CantidadFacturada;
                        pedidoDetalle_DB.CantidadProducto = cantidad_producto_DTO;
                    }
                }
            }
            //Actualizamos descripcion
            pedido_DB.PedidoDescripcion = pedidoDTO_R.Descripcion;

            //Tercero ir actualizando los campos y meterlo dentro de nuestra base de datos




            //Cuarto, confirmar actualizacion.

            
            
            
            
            //Le aumentamos, restamos o dejamos intacto el saldo del cliente.
            //Sumatoria seria un valor, positivo o negativo de acuerdo a si 
            //el cliente a comprado mas productos o restado.
            var Cliente = db.Personas.FirstOrDefault(c => c.Id == pedidoDTO_R.ClienteId);
            Cliente.Saldo = Cliente.Saldo + sumatoria;
            db.Entry(Cliente).State = EntityState.Modified;






            return StatusCode(HttpStatusCode.NoContent);
        }
































        // POST: api/Pedidos
        [ResponseType(typeof(PedidoDTORequest))]
        public IHttpActionResult PostPedido(PedidoDTORequest Pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string UserIdLogged;
            if (!User.Identity.IsAuthenticated) return BadRequest("No autorizado");
            UserIdLogged = User.Identity.GetUserId();

            var user = db.Users
                .FirstOrDefault(u => u.Id.Equals(UserIdLogged));

            string userName = user.UserName;

            var Vendedor = db.Personas
                .FirstOrDefault(p => p.UserName.Equals(userName));

            int CantPedidos = db.Pedidos.Count();

            var PedidoDb = new Pedido
            {
                NumeroPedido = CantPedidos++,
                IdCliente = Pedido.ClienteId,
                IdVendedor = Vendedor.Id,
                PedidoDescripcion = Pedido.Descripcion,
                CondicionVenta = "Credito",
                Estado = "PENDIENTE",
                FechaPedido = DateTime.Now
            };
            db.Pedidos.Add(PedidoDb);
            var Cliente = db.Personas
                .FirstOrDefault(c => c.Id == Pedido.ClienteId);
            double MontoTotal = 0.0;
            bool pendiente = false;

            if (Pedido.Pedidos == null) return BadRequest("Los Detalles del pedido es requerido");
            //Verificamos si hay stocks disponibles para cada producto Pedido
            foreach (var PedidoDetalle in Pedido.Pedidos)
            {

                var Stock = db.Stocks
                    .Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .FirstOrDefault(s => s.IdProducto == PedidoDetalle.ProductoId && s.IdDeposito == 3);

                var StockDisponible = Stock.Cantidad;
                var Detalle = new PedidoDetalle
                {
                    IdProducto = PedidoDetalle.ProductoId,
                    Producto = db.Productos.Find(PedidoDetalle.ProductoId),
                    Pedido = PedidoDb,
                    IdPedido = PedidoDb.Id,
                    CantidadProducto = PedidoDetalle.CantidadProducto,
                    PrecioUnitario = Stock.Producto.Precio
                };

                if (StockDisponible < PedidoDetalle.CantidadProducto)
                {
                    pendiente = true;
                    Detalle.CantidadFacturada = StockDisponible;
                    Stock.Cantidad = 0;
                } else
                {
                    Detalle.CantidadFacturada = PedidoDetalle.CantidadProducto;
                    Stock.Cantidad -= PedidoDetalle.CantidadProducto;
                }

                db.Entry(Stock).State = EntityState.Modified;
                db.PedidoDetalles.Add(Detalle);
                MontoTotal += Stock.Producto.Precio * Detalle.CantidadFacturada;


            }

            if (MontoTotal > Cliente.LineaDeCredito || MontoTotal + Cliente.Saldo > Cliente.LineaDeCredito)
            {
                return BadRequest("Linea de Credito Insuficiente");
            } else
            {
                Cliente.Saldo += MontoTotal;
                db.Entry(Cliente).State = EntityState.Modified;

                if (pendiente)
                {
                    try
                    {
                        db.SaveChanges();
                        return Ok("Guardado, pero Falta Facturar Productos");

                    } catch (Exception ex)
                    {
                        return BadRequest("Error al ejecutar la transaccion " + ex.Message);
                    }
                } else
                {
                    try
                    {
                        PedidoDb.Estado = "FACTURADO";
                        db.Entry(PedidoDb).State = EntityState.Added;
                        db.SaveChanges();
                        return Ok("Guardado, con exito");

                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error al ejecutar la transaccion " + ex.Message);
                    }
                }
            }



        }

        // DELETE: api/Pedidos/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult DeletePedido(int id)
        {
            Pedido pedido = db.Pedidos.Include(p => p.Cliente).FirstOrDefault(p => p.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }
            pedido.Deleted = true;
            var PedidosDetalles = db.PedidoDetalles
                .Include(pd => pd.Producto)
                .Where(pd => pd.IdPedido == id);

            double sumatoria = 0;
            foreach (var pedidodetalle in PedidosDetalles)
            {
                pedidodetalle.Deleted = true;
                var CantidadProducto = pedidodetalle.CantidadProducto;
                var Producto = db.Productos.Find(pedidodetalle.IdProducto);

                var Stock = db.Stocks
                    .Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .Where(s => s.Producto.Id == pedidodetalle.Producto.Id && s.IdDeposito == 3)
                    .First();

                Stock.Cantidad = Stock.Cantidad + pedidodetalle.CantidadFacturada;

                //Vamos sumando cuanta plata devolver al cliente.
                sumatoria += pedidodetalle.CantidadFacturada * pedidodetalle.PrecioUnitario;

                //Hace el Update de la base de datos 
                db.Entry(Stock).State = EntityState.Modified;
                pedidodetalle.CantidadProducto -= pedidodetalle.CantidadProducto;
                pedidodetalle.CantidadFacturada = 0;
                db.Entry(pedidodetalle).State = EntityState.Modified;


            }
            var Cliente = db.Personas.FirstOrDefault(c => c.Id == pedido.IdCliente);
            Cliente.Saldo = Cliente.Saldo - sumatoria;
            db.Entry(Cliente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return BadRequest("No se ha podido ejecutar la transaccion de borrado");
            }


            return Ok("Se ha borrado con exito");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidoExists(int id)
        {
            return db.Pedidos.Count(e => e.Id == id) > 0;
        }

        private List<PedidoResponseDTO> PedidosMapper(List<Pedido> Pedidos)
        {
            var Result = Pedidos
                .ToList()
                .ConvertAll(p => PedidoMapper(p));
            return Result;
        }

        private PedidoResponseDTO PedidoMapper(Pedido p)
        {
            var detalles = db.PedidoDetalles
                .Include(x => x.Pedido)
                .Include(x => x.Producto)
                .Where(x => x.IdPedido == p.Id);
            double precioTotal = 0;
            double ivaTotal = 0;
            foreach(var pdt in detalles)
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