﻿using System;
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
        [ResponseType(typeof(FullProductoDTOResponse))]
        public IHttpActionResult GetPedido(int id)
        {

            var Pedido = db.Pedidos.Find(id);

            if (Pedido == null || Pedido.Deleted) return BadRequest("No se encontro ningun pedido");
            var PedidosDetalles = db.PedidoDetalles
                .Include(pd => pd.Pedido)
                .Include(pd => pd.Producto)
                .Where(pd => !pd.Deleted)
                .Where(pd => pd.IdPedido == id);
            var result = PedidosDetalles.ToList()
                .ConvertAll(pd => new FullProductoDTOResponse
                {
                    IdPedido = Pedido.Id,
                    IdProducto = pd.IdProducto,
                    Producto = pd.Producto.MarcaProducto + ": " + pd.Producto.NombreProducto,
                    Cantidad = pd.CantidadProducto,
                    PrecioUnitario = pd.Producto.Precio,
                    PrecioTotal = pd.CantidadProducto * pd.Producto.Precio,
                    Cliente = Pedido.Cliente.Nombre + " " + Pedido.Cliente.Apellido,
                    IdCliente = Pedido.Cliente.Id,
                    Vendedor = Pedido.Vendedor.Nombre + " " + Pedido.Vendedor.Apellido,
                    IdVendedor = Pedido.Vendedor.Id

                });

            return Ok(result);
        }

        // PUT: api/Pedidos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedido(int id, Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedido.Id)
            {
                return BadRequest();
            }

            db.Entry(pedido).State = EntityState.Modified;

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
                CondicionVenta = Pedido.CondicionVenta,
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
            }else
            {
                Cliente.Saldo += MontoTotal;
                db.Entry(Cliente).State = EntityState.Modified;

                if (pendiente)
                {
                    try
                    {
                        db.SaveChanges();
                        return Ok("Guardado, pero Falta Facturar Productos");

                    }catch (Exception ex)
                    {
                        return BadRequest("Error al ejecutar la transaccion "+ex.Message);
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
            db.Entry(Cliente).State = EntityState.Added;

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
                .ConvertAll(p => new PedidoResponseDTO
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
                            CantidadCuotas = pd.CantidadFacturada,
                            CantidadProductos = pd.CantidadProducto

                        })

                });
            return Result;
        }
    }
}