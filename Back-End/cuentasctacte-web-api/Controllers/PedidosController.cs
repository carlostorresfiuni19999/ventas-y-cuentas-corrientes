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
   // [Authorize]  //////----------------------------........//////
    public class PedidosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Pedidos
        [AllowAnonymous]
        public List<PedidoResponseDTO> GetPedidos()
        {
            var Pedidos = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor).ToList();
            return PedidosMapper(Pedidos);
            
        }

        // GET: api/Pedidos/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult GetPedido(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
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
            Pedido PedidoSaved = db.Pedidos.Add(PedidoDb);
            var Cliente = db.Personas
                .FirstOrDefault(c => c.Id == Pedido.ClienteId);
            double MontoTotal = 0.0;
            double IvaTotal = 0.0;
            
            //Verificamos si hay stocks disponibles para cada producto Pedido
            foreach(var Detalle in Pedido.Pedidos)
            {
                var Producto = db.Productos
                    .FirstOrDefault(p => p.Id == Detalle.ProductoId);


                MontoTotal += (Producto.Precio) * (Detalle.CantidadProducto);

                if (MontoTotal > Cliente.LineaDeCredito) return BadRequest("Linea de Credito Insuficiente");
                
                IvaTotal += Producto.Iva;
                var Stock = db.Stocks
                    .Include(p => p.Producto)
                    .Include(d => d.Deposito)
                    .FirstOrDefault(s =>
                        s.IdDeposito == 3
                        && s.IdProducto == Detalle.ProductoId
                    );
                var PedidoDetalle = new PedidoDetalle()
                {
                    PrecioUnitario = Stock.Producto.Precio,
                    IdProducto = Detalle.ProductoId,
                    CantidadFacturada = 0,
                    CantidadProducto = Detalle.CantidadProducto,
                    IdPedido = PedidoSaved.Id
                };
                if (Detalle.CantidadProducto > Stock.Cantidad)
                {
                    db.PedidoDetalles.Add(PedidoDetalle);
                }
                else
                {
                    PedidoDetalle.CantidadFacturada = Detalle.CantidadCuotas;

                    //Restar del Stock la Cantidad.
                    Stock.Cantidad = Stock.Cantidad - Detalle.CantidadProducto;

                    //Guardar en DB
                    db.Entry(Stock).State = EntityState.Modified;

                    //Aumentar El Saldo Que le Queda al Cliente.
                    db.PedidoDetalles.Add(PedidoDetalle);
                    Cliente.Saldo += MontoTotal;

                    //Guardar En Db
                    db.Entry(Cliente).State = EntityState.Modified;

                }
            }
            try
            {
                db.SaveChanges();
            } catch (Exception ex)
            {
                BadRequest("Ocurrio un error al ejecutar la transaccion " + ex.Message);
            }
            return Ok("Cargado con exito");
        }

        // DELETE: api/Pedidos/5
        [ResponseType(typeof(Pedido))]

        ///-------------------------------------------
        [ResponseType(typeof(PedidoDTORequest))]
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
                var Producto = db.Productos.Find( pedidodetalle.IdProducto);
                //var Producto = db.Productos.FirstOrDefault(prod => prod.Id == pedidodetalle.IdProducto);

                /*
                var Stock = db.Stocks
                    .Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .FirstOrDefault(s => s.Producto.Id == pedidodetalle.Producto.Id && s.IdDeposito == 3);
                */
                var Stock = db.Stocks
                    .Include(s => s.Producto)
                    .Include(s => s.Deposito)
                    .Where(s => s.Producto.Id == pedidodetalle.Producto.Id && s.IdDeposito == 3)
                    .First();
                Stock.Cantidad = Stock.Cantidad + CantidadProducto;



                //Vamos sumando cuanta plata devolver al cliente.
                sumatoria += pedidodetalle.CantidadProducto * pedidodetalle.PrecioUnitario;
                
                //HAce el Update de la base de datos 
                db.Entry(Stock).State = EntityState.Modified;
                pedidodetalle.CantidadProducto -= pedidodetalle.CantidadProducto;
                pedidodetalle.CantidadFacturada = 0;
                db.Entry(pedidodetalle).State = EntityState.Modified;
                //sumatoria += pedidodetalle.Producto.Precio;

                
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
        ///-------------------------------------------




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