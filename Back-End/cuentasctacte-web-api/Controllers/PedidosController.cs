using cuentasctacte_web_api.Helpers;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class PedidosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GetUserLogged GetUserLogged = new GetUserLogged();

        // GET: api/Pedidos
        public List<PedidoResponseDTO> GetPedidos()
        {
            var Pedidos = db.Pedidos
                .Where(p => !p.Deleted)
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor)
                .OrderBy(p => p.Estado.Equals("PENDIENTE"))
                .OrderBy(p => p.FechaPedido)
                .ToList();
            return PedidosMapper(Pedidos);

        }

        // GET: api/Pedidos/5
        [ResponseType(typeof(PedidoResponseDTO))]
        public IHttpActionResult GetPedido(int id)
        {
            var Pedido = db.Pedidos
                .Include(p => p.Vendedor)
                .Include(p => p.Cliente)
                .FirstOrDefault(p => p.Id == id);
            if (null == Pedido)
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
                return BadRequest();
            }
            /*
                *Ver si existe el pedido.
            */

            if (db.Pedidos.Include(p => p.Cliente).ToList().Exists(p => p.Id == id) == false)

            {
                return BadRequest();
            }
            //db.Entry(pedidoDTO_R).State = EntityState.Modified;


            /*VARIABLES*/
            int cantidad_producto_DTO = 0;
            int id_producto_DTO = -1;
            int vandera = 0;
            /****/
            //Primero Buscar un pedido dentro de base de datos con argumento 'id'
            //Luego si su estado no es pendiente, retorna null porque no podemos editar pedidos facturados.
            Pedido pedido_DB = db.Pedidos.Include(p => p.Cliente).FirstOrDefault(p => p.Id == id);

            if (pedido_DB.Estado != "PENDIENTE")
            {
                return BadRequest("Solo se pueden editar pedidos pendientes");

            }

            //Actualizamos descripcion
            pedido_DB.PedidoDescripcion = pedidoDTO_R.Descripcion;


            //Tambien traer los detalles.
            var PedidosDetalles_DB = db.PedidoDetalles
               .Include(pd => pd.Producto)
               .Where(pd => pd.IdPedido == id)
               .Where(pd => !pd.Deleted);

            /*eLIMINAMOS TODO*/
            foreach (var Detalles in PedidosDetalles_DB)
            {
                db.PedidoDetalles.Remove(Detalles);

            }


            List<PedidoDetalle> existentes = new List<PedidoDetalle>();

            //Unificar los pedidos
            foreach (var item in pedidoDTO_R.Pedidos)
            {
                var Detalle = new PedidoDetalle()
                {
                    IdPedido = id,
                    IdProducto = item.ProductoId,
                    CantidadProducto = item.CantidadProducto,
                    CantidadFacturada = 0,
                    Deleted = false
                };

                if (existentes.Count(pd => pd.IdPedido == Detalle.IdPedido && pd.IdProducto == Detalle.IdProducto) > 0)
                {
                    var temp = existentes.FirstOrDefault(e => e.Id == id);
                    existentes.Remove(temp);
                    temp.CantidadProducto += Detalle.CantidadProducto;
                    existentes.Add(temp);
                }
                else
                {
                    existentes.Add(Detalle);
                }


                existentes.ForEach(pd => db.PedidoDetalles.Add(pd));

            }




            try
            {
                db.SaveChanges();
                return Ok(); //Todo salio bien.
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

        }

        [AllowAnonymous]
        [Route("api/Pedidos/Baja")]
        [HttpPost]
        public IHttpActionResult VencerPedido(DateTime referencia)
        {
            var Pedidos = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor)
                .Where(p => !p.Deleted)
                .Where(p => p.Estado == "FACTURANDO");

            foreach (var item in Pedidos)
            {
                if ((referencia - item.FechaPedido).TotalDays > 1)
                {
                    item.Estado = "FACTURADO";

                    var Detalles = db.PedidoDetalles
                        .Include(pd => pd.Pedido)
                        .Where(pd => !pd.Deleted)
                        .Where(pd => pd.IdProducto == item.Id);

                    foreach (var detalle in Detalles)
                    {
                        detalle.CantidadProducto = detalle.CantidadFacturada;
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                    db.Entry(item).State = EntityState.Modified;
                }
            }

            try
            {
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el estado por la fecha: " + ex.Message);
            }
        }


        [Route("api/PedidosSinFactura")]
        [HttpGet]
        public List<PedidoResponseDTO> GetPedidosSinFactura()
        {
            return db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Vendedor)
                .Where(p => p.Estado != "FACTURADO")
                .ToList()
                .ConvertAll(p => PedidoMapper(p));
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


            var Vendedor = GetUserLogged.GetUser(db, UserIdLogged);

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
            PedidoDb = db.Pedidos.Add(PedidoDb);
            var Cliente = db.Personas
                .FirstOrDefault(c => c.Id == Pedido.ClienteId);


            List<PedidoDetalle> PedidosExistentes = new List<PedidoDetalle>();
            if (Pedido.Pedidos == null) return BadRequest("Los Detalles del pedido es requerido");
            //Verificamos si hay stocks disponibles para cada producto Pedido
            foreach (var PedidoDetalle in Pedido.Pedidos)
            {




                PedidoDetalle Detalle;

                bool DetalleExist = 0 < PedidosExistentes
                    .Count(pd =>
                    pd.IdPedido == PedidoDb.Id
                    && pd.IdProducto == PedidoDetalle.ProductoId);



                if (DetalleExist)
                {
                    Detalle = PedidosExistentes

                    .Where(p => !p.Deleted)
                    .FirstOrDefault(p =>
                    p.IdProducto == PedidoDetalle.ProductoId
                    && p.IdPedido == PedidoDb.Id
                    );
                    PedidosExistentes.Remove(Detalle);

                    Detalle.CantidadProducto += Detalle.CantidadProducto;

                    PedidosExistentes.Add(Detalle);

                }
                else
                {
                    Detalle = new PedidoDetalle
                    {
                        IdProducto = PedidoDetalle.ProductoId,
                        IdPedido = PedidoDb.Id,
                        CantidadProducto = PedidoDetalle.CantidadProducto,
                        PrecioUnitario = db.Productos.Find(PedidoDetalle.ProductoId).Precio,
                        CantidadFacturada = 0
                    };
                    PedidosExistentes.Add(Detalle);
                }

            }

            //Guardamos todos los PedidosDetalles
            PedidosExistentes.ForEach(p => db.PedidoDetalles.Add(p));
            try
            {
                db.SaveChanges();
                return Ok("Guardado con exito Detalle");

            }
            catch (Exception ex)
            {
                return BadRequest("Error al ejecutar la transaccion " + ex.Message);
            }





        }
        [ResponseType(typeof(PedidoDetalleResponseDTO))]
        [Route("api/Pedidos/PedidoParaFacturar")]
        [HttpGet]
        public PedidoDTORequest GetPedidoFacturar(int id)
        {
            return ToDTORequest(id);
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

            if (pedido.Estado != "PENDIENTE")
            {
                return BadRequest("Solo se pueden eliminar pedidos pendientes");
            }

            pedido.Deleted = true;
            var PedidosDetalles = db.PedidoDetalles
                .Include(pd => pd.Producto)
                .Where(pd => pd.IdPedido == id);

            foreach (var pedidodetalle in PedidosDetalles)
            {
                pedidodetalle.Deleted = true;
                var CantidadProducto = pedidodetalle.CantidadProducto;
                var Producto = db.Productos.Find(pedidodetalle.IdProducto);

                //Hace el Update de la base de datos            
                pedidodetalle.CantidadProducto -= pedidodetalle.CantidadProducto;
                db.Entry(pedidodetalle).State = EntityState.Modified;


            }

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
                .Where(x => !x.Deleted)
                .Where(x => x.IdPedido == p.Id);
            double precioTotal = 0;
            double ivaTotal = 0;
            foreach (var pdt in detalles)
            {
                precioTotal = precioTotal + pdt.Producto.Precio * pdt.CantidadProducto;
                ivaTotal = ivaTotal + pdt.Producto.Iva * pdt.CantidadProducto;
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
                        .Include(pd => pd.Pedido)
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



        private PedidoDTORequest ToDTORequest(int id)
        {
            var Pedido = db.Pedidos
                .Include(p => p.Vendedor)
                .Include(p => p.Cliente)
                .FirstOrDefault(p => p.Id == id);
            if (Pedido == null) throw new Exception("Pedido no encontrado");

            var Pedidos = db.PedidoDetalles
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .Where(p => p.IdPedido == id);

            if (Pedidos.Count() <= 0) throw new Exception("Pedidos no encontrado");
            PedidoDTORequest result = new PedidoDTORequest()
            {
                ClienteId = Pedido.Cliente.Id,
                Descripcion = Pedido.PedidoDescripcion,
                Pedidos = Pedidos
                 .ToList()
                 .ConvertAll(p => new PedidoDetalleDTORequest()
                 {
                     ProductoId = p.IdProducto,
                     CantidadProducto = p.CantidadProducto - p.CantidadFacturada
                 })
            };

            return result;
        }


        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Reporte pedidos




        [AllowAnonymous]
        
        [Route("api/Pedidos/PedidoReportes")]
        [HttpGet]
        [ResponseType(typeof(PedidoDTORequest))]
        public List<PedidoResponseDTO> FilterByDate(
            DateTime Inicio, DateTime Fin, String estado= "ALL")
        {
            List<PedidoResponseDTO> pedidos_retornados = new List<PedidoResponseDTO>();
            List<PedidoResponseDTO> pedidos_Mapper = new List<PedidoResponseDTO>();
            DateTime desde = Inicio.AddDays(-1);
            DateTime hasta = Fin.AddDays(1);
            

            pedidos_Mapper = GetPedidos_EntreFechas(desde, hasta);

            //Filtramos por estado.
            if (estado == "ALL") return pedidos_Mapper;

            switch (estado)
            {
                case "PENDIENTE":
                    foreach (var pedidoRequest in pedidos_Mapper)
                    {
                        if (pedidoRequest.Estado == "PENDIENTE")
                        {
                            pedidos_retornados.Add(pedidoRequest);
                            //pedidosMapper.Remove(pedidoRequest);
                        }

                    }

                    break;
                case "FACTURANDO":
                    foreach (var pedidoRequest in pedidos_Mapper)
                    {
                        if (pedidoRequest.Estado == "FACTURANDO")
                        {
                            pedidos_retornados.Add(pedidoRequest);
                            //pedidosMapper.Remove(pedidoRequest);
                        }

                    }
                    break;
                case "FACTURADO":
                    foreach (var pedidoRequest in pedidos_Mapper)
                    {
                        if (pedidoRequest.Estado == "FACTURADO")
                        {
                            pedidos_retornados.Add(pedidoRequest);
                            //pedidosMapper.Remove(pedidoRequest);

                        }

                    }
                    break;
            }
            return pedidos_retornados;
        }
       
            //PENDIENTE
            //FACTURANDO
            //FACTURADO

            //Me trae todos los pedidos incluyendo una fecha.
            private List<PedidoResponseDTO> GetPedidos_EntreFechas(DateTime desde, DateTime hasta)
        {

            var Pedidos = db.Pedidos
                .Include(p => p.Vendedor)
                .Include(p => p.Cliente)
                .Where(p => !p.Deleted)
                .Where(p =>
                    (DateTime.Compare(desde, p.FechaPedido) <= 0)
                    && (DateTime.Compare(hasta, p.FechaPedido) >= 0)
                ).ToList();

            return PedidosMapper(Pedidos);


        }

        
            /////////////////////////////////////////////////////////////////////////// Get facturas asociadas a un pedido

       

            public PedidoResponseDTO GetPedido_local(int id)
        {
            var Pedido = db.Pedidos
                .Include(p => p.Vendedor)
                .Include(p => p.Cliente)
                .FirstOrDefault(p => p.Id == id);
   
            return PedidoMapper(Pedido);
        }

        [Route("api/Pedidos/FacturasDelPedido")]
        [HttpGet]
        [ResponseType(typeof(FacturasDePedidoResponseDTO))]
        public IHttpActionResult getFacturasPedido(int id_Pedido) {


            PedidoResponseDTO pedidoResponse = GetPedido_local(id_Pedido);
            var listaFacturas = facturaMapperList(getFacturas(id_Pedido));

            //Creamos el objeto a retornar
            FacturasDePedidoResponseDTO la_cosa = new FacturasDePedidoResponseDTO();
            la_cosa.PedidoFull = pedidoResponse;
            la_cosa.FullFacturas = listaFacturas;

            return Ok(la_cosa);
        }

        private List<Factura> getFacturas(int id_pedido) {

            var Facturas = db.Facturas
                .Include(f => f.Pedido)
                .Include(f => f.Cliente)
                .Where(f => f.PedidoId == id_pedido)
                .Where(f => f.Deleted != true)
                .ToList();
            return Facturas;
        }

        private List<FullFacturaResponseDTO> facturaMapperList(
                                             List<Factura> Facturas) 
        {
            FacturasController Fc = new FacturasController();
            List<FullFacturaResponseDTO> FullFacturas = new List<FullFacturaResponseDTO>();

            foreach (var Factura in Facturas) {//Vamos agregando las facturas
                FullFacturas.Add(Fc.MapToFullFactura(Factura));
            }
            return FullFacturas;
        }
    }
}