//librerias
import React, { useEffect, useState } from 'react'

//next
import { useRouter } from 'next/router'
import Head from 'next/head'
import Link from 'next/link'
//componentes
import Navbar from '../../../components/Navbar'
import FDPControl from '../../../components/fdpControl'
import NavMain from '../../../components/NavMain'

//api
import getPedido from '../../../API/getPedido'
import getProductos from '../../../API/getProductos'
import putPedido from '../../../API/putPedido'
import crearFactura from '../../../API/crearFactura'
import hasRole from '../../../API/hasRole'
import getFacturasDePedidos from '../../../API/getFacturasDePedidos'

export default function Detalles() {
    const [datos, setDatos] = useState({})
    const [productos, setProductos] = useState([])
    const [listProd, setListProd] = useState([])
    const [precioTotal, setPrecioTotal] = useState({ precio: 0 })
    const [condPago, setCondPago] = useState('CONTADO')
    const [idGenerator, setIdGenerator] = useState(0)

    const [listaFacturas, setListaFacturas] = useState([])

    const Router = useRouter()

    useEffect(() => {
        if (sessionStorage.getItem('token') == null) {
            Router.push('/Login')
        } else {
            const token = JSON.parse(sessionStorage.getItem('token'));
            hasRole(token.access_token, token.userName, "Vendedor")
                .then(r => {
                    if (r == 'false') {
                        Router.push("/LogIn")

                    }
                }).catch(console.log);

            if (Router.isReady) {
                getPedido(JSON.parse(sessionStorage.getItem('token')).access_token, Router.query.id)
                    .then(response => response.json())
                    .then(res => {
                        console.log(res)
                        if (typeof res.Message !== 'undefined') {
                            alert(res.Message)
                            Router.back()
                        } else {
                            setDatos({
                                idClient: res.Cliente.Id,
                                nombre: res.Cliente.Nombre + ' ' + res.Cliente.Apellido,
                                cin: res.Cliente.Documento,
                                desc: res.PedidoDescripcion,
                                fecha: res.FechePedido.split('T')[0],
                                estado: res.Estado,
                                costoTotal: res.CostoTotal,
                                pedidosDetalles: res.PedidosDetalles
                            })
                        }




                    }).catch(err => console.log(err))

                getProductos(JSON.parse(sessionStorage.getItem('token')).access_token)
                    .then(response => response.text())
                    .then(result => {
                        const res = JSON.parse(result)
                        setListProd(res.map(producto => {
                            const newProducto = {
                                id: producto.Id,
                                nombre: producto.MarcaProducto + ' ' + producto.NombreProducto,
                                codigoBar: producto.CodigoDeBarra,
                                precio: producto.Precio,
                                precioIva: producto.Precio + producto.Iva
                            }
                            return newProducto
                        }))
                    }).catch(err => console.log(err))

                getFacturasDePedidos(JSON.parse(sessionStorage.getItem('token')).access_token, Router.query.id)
                    .then(res => res.json())
                    .then(res => {
                        if (typeof res.Message === 'undefined') {
                            console.log(res.FullFacturas)
                            setListaFacturas(
                                res.FullFacturas.map(f => {
                                    const returnValue = {
                                        id: f.IdFactura,
                                        fecha: f.FechaFacturacion.split('T')[0],
                                        detalles: f.Detalles,
                                        saldo: f.SaldoTotal + f.IvaTotal
                                    }
                                    return returnValue
                                })
                            )

                        } else {
                            alert(res.Message)
                            Router.back()
                        }
                    }).catch(err => console.log(err))
            }
        }
    }, [Router])


    const crearDetalles = () => {
        if (typeof datos.pedidosDetalles !== 'undefined') {
            setProductos(datos.pedidosDetalles.filter((p) => p.CantidadProductos > 0).map((p) => {
                const newProd = {
                    id: p.Id,
                    prodId: p.Producto.Id,
                    cantidad: p.CantidadProductos - p.CantidadFacturada,
                    nombre: p.Producto.MarcaProducto + ' ' + p.Producto.NombreProducto,
                    codBarr: p.Producto.CodigoDeBarra,
                    precio: p.Producto.Precio + p.Producto.Iva,
                    precioTotal: (p.Producto.Precio + p.Producto.Iva) * p.CantidadProductos
                }
                return newProd
            }).filter((p) => p.cantidad > 0))
            setPrecioTotal({
                precio: datos.pedidosDetalles.filter((p) => p.CantidadProductos > 0).map((p) => {
                    const newProd = {
                        id: p.Id,
                        prodId: p.Producto.Id,
                        cantidad: p.CantidadProductos - p.CantidadFacturada,
                        nombre: p.Producto.MarcaProducto + ' ' + p.Producto.NombreProducto,
                        codBarr: p.Producto.CodigoDeBarra,
                        precio: p.Producto.Precio + p.Producto.Iva,
                        precioTotal: (p.Producto.Precio + p.Producto.Iva) * p.CantidadProductos
                    }
                    return newProd
                }).filter((p) => p.cantidad > 0).map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
            })
        }

    }

    const crearDetallesFacturado = () => {
        if (typeof datos.pedidosDetalles !== 'undefined') {
            setProductos(datos.pedidosDetalles.filter((p) => p.CantidadProductos > 0).map((p) => {
                const newProd = {
                    id: p.Id,
                    prodId: p.Producto.Id,
                    cantidad: p.CantidadProductos,
                    nombre: p.Producto.MarcaProducto + ' ' + p.Producto.NombreProducto,
                    codBarr: p.Producto.CodigoDeBarra,
                    precio: p.Producto.Precio + p.Producto.Iva,
                    precioTotal: (p.Producto.Precio + p.Producto.Iva) * p.CantidadProductos
                }
                return newProd
            }).filter((p) => p.cantidad > 0))
            setPrecioTotal({
                precio: datos.pedidosDetalles.filter((p) => p.CantidadProductos > 0).map((p) => {
                    const newProd = {
                        id: p.Id,
                        prodId: p.Producto.Id,
                        cantidad: p.CantidadProductos,
                        nombre: p.Producto.MarcaProducto + ' ' + p.Producto.NombreProducto,
                        codBarr: p.Producto.CodigoDeBarra,
                        precio: p.Producto.Precio + p.Producto.Iva,
                        precioTotal: (p.Producto.Precio + p.Producto.Iva) * p.CantidadProductos
                    }
                    return newProd
                }).filter((p) => p.cantidad > 0).map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
            })
        }
    }

    const formatFecha = (dateStr) => {
        if (dateStr == null) {
            return ''
        }
        const dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[2] + "/" + dArr[1] + "/" + dArr[0].substring(2); //ex out: "18/01/10"
    }

    const formatNum = (data) => {
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(data)
    }

    const getDoc = (id) => {
        return document.getElementById(id)
    }

    const generadorId = () => {
        const idNew = idGenerator
        setIdGenerator(idGenerator + 1)
        return idNew
    }

    const handleChange = (id) => {
        productos.filter(prod => prod.id == id)[0].cantidad = parseInt(getDoc(`cantidadProd${id}`).value)
        if (productos.filter(prod => prod.id == id)[0] != getDoc(`producto${id}`).value) {
            const prodS = listProd.filter(prod => prod.nombre == getDoc(`producto${id}`).value)[0]
            if (typeof prodS !== 'undefined') {
                productos.filter(prod => prod.id == id)[0].nombre = prodS.nombre
                productos.filter(prod => prod.id == id)[0].precio = prodS.precioIva
                productos.filter(prod => prod.id == id)[0].prodId = prodS.id
                productos.filter(prod => prod.id == id)[0].codBarr = prodS.codigoBar
                productos.filter(prod => prod.id == id)[0].precioTotal = productos.filter(prod => prod.id == id)[0].precio * productos.filter(prod => prod.id == id)[0].cantidad
            }
        }
        if (productos.filter(prod => prod.id == id)[0].cantidad > 10) {
            getDoc(`tr${id}`).className = 'table-danger'
        } else {
            getDoc(`tr${id}`).className = ''
        }
        setProductos(productos.map(p => { return p }))
        setPrecioTotal({
            precio: productos.map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
        })
    }

    const agregarNuevo = () => {
        const idNew = generadorId()
        setProductos([...productos, {
            id: idNew,
            prodId: listProd[0].id,
            cantidad: 1,
            nombre: listProd[0].nombre,
            codBarr: listProd[0].codigoBar,
            precio: listProd[0].precioIva,
            precioTotal: listProd[0].precioIva
        }])
        setPrecioTotal({
            precio: [...productos, {
                id: idNew,
                prodId: listProd[0].id,
                cantidad: 1,
                nombre: listProd[0].nombre,
                codBarr: listProd[0].codigoBar,
                precio: listProd[0].precioIva,
                precioTotal: listProd[0].precioIva
            }].map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
        })
    }

    const eliminar = (id) => {
        if (confirm("Esta seguro?")) {
            const arrNew = productos.filter(p => { return p.id != id })
            setProductos(arrNew.map(p => { return p }))
            setPrecioTotal(productos.filter(p => { return p.id != id }).map(p => { return p }).map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0))
        }
    }

    const modificarNota = () => {
        if (productos.length > 0) {
            const raw = JSON.stringify({
                "ClienteId": datos.idClient,
                "Descripcion": datos.desc,
                "Pedidos": productos.map(p => {
                    const returnValue = {
                        "ProductoId": p.prodId,
                        "CantidadProducto": p.cantidad
                    }
                    return returnValue
                })
            })
            putPedido(JSON.parse(sessionStorage.getItem('token')).access_token, Router.query.id, raw)
        }

    }

    const facturar = () => {
        if (productos.length > 0) {
            const raw = JSON.stringify({
                "IdPedido": Router.query.id,
                "CantidadCuotas": getCantCuotas(),
                "Pedido": {
                    "ClienteId": datos.idClient,
                    "Descripcion": datos.desc,
                    "Pedidos": productos.map(p => {
                        const returnValue = {
                            "ProductoId": p.prodId,
                            "CantidadProducto": p.cantidad
                        }
                        return returnValue
                    })
                }

            })
            crearFactura(JSON.parse(sessionStorage.getItem('token')).access_token, raw)
            Router.back()
        }
    }

    const getCantCuotas = () => {
        if (condPago == "CONTADO") {
            return 1
        } else {
            return parseInt(document.getElementById("cantCuotasCred").value)
        }
    }

    const getButtonEditable = () => {
        if (datos.estado == "PENDIENTE") {
            return (<button className="btn btn-info btn-sm" onClick={() => { modificarNota() }}>Guardar Cambios</button>)
        } else {
            return (<button className="btn btn-info btn-sm" onClick={() => { modificarNota() }} disabled>Guardar Cambios</button>)
        }
    }

    const handleClickFacturaDetalle = (arr) => {
        document.getElementById('facturaDetallesShowTbody').innerHTML = arr.map(fd =>{
            return `<tr key=${fd.Id}>
                <th>${fd.Producto}</th>
                <td>${fd.Cantidad}</td>
                <td>${formatNum(fd.PrecioUnitario + fd.Iva)}</td>
            </tr>`
        }).join('')
    }

    if (datos.estado != "FACTURADO") {
        return (
            <div>
                <Head>
                    <title>Detalles / Facturacion de pedido</title>
                </Head>
                <Navbar rol='v' rango='ndp' page='ndpDetalles' />
                <div className=''>
                    {/*La parte de arriba de la lista */}
                    <NavMain person="vendedor" pag="Detalles de Factura / Facturacion" />
                    {/*La parte de abajo de la lista */}


                    <div className='ms-5 mt-3'>
                        <div className='pt-4 container'>
                            <div className='row'>
                                <div className='col-8'>
                                    <h5>Detalles / Facturacion</h5>
                                    {/* lista de los objetos que se pidio, se deberia tener que cambiar se tiene que implementar
                        selectize, para eso necesito la lista de los productos.
                        */ }
                                    <div className=''>
                                        <label>Cliente:</label>
                                        <label className='px-3'>{datos.nombre}</label>
                                        <label className=''>CIN:</label>
                                        <label className='px-3 pe-5'>{formatNum(datos.cin)}</label>
                                        <label className='ps-5'>Fecha:</label>
                                        <label className='px-3'>{formatFecha(datos.fecha)}</label>
                                    </div>

                                    <div className='pe-3'>
                                        <table className='table'>
                                            {//thead
                                            }
                                            <thead>
                                                <tr>
                                                    <th>Cantidad</th>
                                                    <th>Codigo de Barra</th>
                                                    <th>Producto</th>
                                                    <th>Precio Unit.</th>
                                                    <th>Precio Total</th>
                                                    <th>Borrar</th>
                                                </tr>
                                            </thead>
                                            {//tbody, aca se imprimen los productos que tiene el detalle                        
                                            }
                                            <tbody>

                                                {//en prueba
                                                    productos.map(prod => {
                                                        return (
                                                            <tr key={prod.id} id={`tr${prod.id}`}>
                                                                <td><input type="number" min={1} defaultValue={prod.cantidad} onBlur={() => handleChange(prod.id)} id={`cantidadProd${prod.id}`}></input></td>
                                                                <td>{prod.codBarr}</td>
                                                                <td>
                                                                    <input type="text" list='productosSelectData' className='form-control' defaultValue={prod.nombre} id={`producto${prod.id}`} onBlur={() => handleChange(prod.id)}></input>
                                                                    <datalist id="productosSelectData">
                                                                        {
                                                                            listProd.map((prod) => {
                                                                                return (
                                                                                    <option key={prod.id} value={prod.nombre}>{formatNum(prod.precio)} - {formatNum(prod.precioIva)}</option>
                                                                                )
                                                                            })
                                                                        }
                                                                    </datalist>
                                                                </td>
                                                                <td className='text-center'>{formatNum(prod.precio)}</td>
                                                                <td className='text-center'>{formatNum(prod.precioTotal)}</td>
                                                                <td><button className='btn btn-light btn-sm' onClick={() => { eliminar(prod.id) }}>
                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                                                        <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                                                        <path fillRule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                                                    </svg>
                                                                </button></td>
                                                            </tr>
                                                        )
                                                    })
                                                }

                                                <tr key="button" >
                                                    <td><button className="btn btn-secondary btn-sm" onClick={() => { crearDetalles() }}>Cargar</button></td>
                                                    <td><button className="btn btn-primary btn-sm" onClick={() => { agregarNuevo() }}>Agregar</button></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td>{getButtonEditable()}</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div>
                                        <h6 className='float-end pe-5'>{formatNum(precioTotal.precio)}</h6>
                                        <h6 className='float-end pe-2'>Total:</h6>
                                        <h6>Descripcion:</h6>
                                        <label>{datos.desc}</label>
                                    </div>

                                </div>
                                <div className='col-4 bg-light h-100 '>
                                    <h5 className='py-2'>Seleccione la forma de pago</h5>
                                    <select className='form-select form-select-sm ' name='formaPago' onChange={() => { setCondPago(document.getElementById('formaPago').value) }} id='formaPago' >
                                        <option value='CONTADO'> Contado </option>
                                        <option value='CREDITO'> Credito </option>
                                    </select>

                                    <FDPControl fdp={condPago} />

                                    <div className='float-end pe-2 pt-2 pb-5'>
                                        <button className='btn btn-success me-2' onClick={() => { facturar() }}>Facturar</button>
                                        <button className='btn btn-danger ' onClick={() => { Router.back() }}>Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className='pe-3 pt-5 container'>
                                    <div className='row'>
                                        <div className='col-6'>
                                            <h5>Factras Creadas desde este Pedido</h5>
                                            <table className='table'>
                                                <thead>
                                                    <tr>
                                                        <th>Fecha</th>
                                                        <th>Saldo</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {
                                                        listaFacturas.map(f => {
                                                            return (
                                                                <tr key={f.id} onClick={()=>handleClickFacturaDetalle(f.detalles)}>
                                                                    <th>{formatFecha(f.fecha)}</th>
                                                                    <td>{formatNum(f.saldo)}</td>
                                                                </tr>
                                                            )
                                                        })
                                                    }
                                                </tbody>
                                            </table>
                                        </div>
                                        <div className='col-6'>
                                            <table className='table'>
                                                <thead>
                                                    <tr>
                                                        <th>Producto</th>
                                                        <th>Cantidad</th>
                                                        <th>Precio</th>
                                                    </tr>
                                                </thead>
                                                <tbody id='facturaDetallesShowTbody'>

                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                </div>
                </div>
            </div>
        )
    }
    else {
        return (
            <div>
                <Head>
                    <title>Detalles / Facturacion de pedido</title>
                </Head>
                <Navbar rol='v' rango='ndp' page='ndpDetalles' />
                <div className=''>
                    {/*La parte de arriba de la lista */}
                    <nav className="navbar navbar-expand-lg navbar-light bg-light">
                        <div className='ms-5'>

                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16" onClick={() => { Router.back() }}>
                                <path fillRule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                            </svg>

                        </div>

                        <div className="ms-5">
                            <ul className=" navbar-nav mr-auto">
                                <li className="nav-item active">
                                    <h6 className='pt-3 nav-link'>Notas de Pago</h6>
                                </li>
                                <li>
                                    <h6 className='pt-3 nav-link'> - </h6>
                                </li>
                                <li className="nav-item">
                                    <h6 className='pt-3 nav-link'>Agregar</h6>
                                </li>

                            </ul>
                        </div>

                    </nav>
                    {/*La parte de abajo de la lista */}


                    <div className='ms-5 mt-3'>
                        <div className='pt-4 '>
                            <div className=''>
                                <h5>Detalles / Facturacion</h5>

                                <div className=''>
                                    <label>Cliente:</label>
                                    <label className='px-3'>{datos.nombre}</label>
                                    <label className=''>CIN:</label>
                                    <label className='px-3 pe-5'>{formatNum(datos.cin)}</label>
                                    <label className='ps-5'>Fecha:</label>
                                    <label className='px-3'>{formatFecha(datos.fecha)}</label>
                                </div>

                                <div className='pe-3'>
                                    <table className='table'>
                                        {//thead
                                        }
                                        <thead>
                                            <tr>
                                                <th>Cantidad</th>
                                                <th>Codigo de Barra</th>
                                                <th>Producto</th>
                                                <th>Precio Unit.</th>
                                                <th>Precio Total</th>
                                            </tr>
                                        </thead>
                                        {//tbody, aca se imprimen los productos que tiene el detalle                        
                                        }
                                        <tbody>

                                            {//en prueba
                                                productos.map(prod => {
                                                    return (
                                                        <tr key={prod.id} id={`tr${prod.id}`}>
                                                            <td><label>{prod.cantidad}</label></td>
                                                            <td>{prod.codBarr}</td>
                                                            <td>
                                                                <label>{prod.nombre}</label>
                                                            </td>
                                                            <td className='text-center'>{formatNum(prod.precio)}</td>
                                                            <td className='text-center'>{formatNum(prod.precioTotal)}</td>

                                                        </tr>
                                                    )
                                                })
                                            }

                                            <tr key="button" >
                                                <td><button className="btn btn-secondary btn-sm" onClick={() => { crearDetallesFacturado() }}>Cargar</button></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div>
                                    <h6 className='float-end pe-5'>{formatNum(precioTotal.precio)}</h6>
                                    <h6 className='float-end pe-2'>Total:</h6>
                                    <h6>Descripcion:</h6>
                                    <label>{datos.desc}</label>
                                </div>
                                <div className='pe-3 pt-5 container'>
                                    <div className='row'>
                                        <div className='col-6'>
                                            <h5>Factras Creadas desde este Pedido</h5>
                                            <table className='table'>
                                                <thead>
                                                    <tr>
                                                        <th>Fecha</th>
                                                        <th>Saldo</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {
                                                        listaFacturas.map(f => {
                                                            return (
                                                                <tr key={f.id} onClick={()=>handleClickFacturaDetalle(f.detalles)}>
                                                                    <th>{formatFecha(f.fecha)}</th>
                                                                    <td>{formatNum(f.saldo)}</td>
                                                                </tr>
                                                            )
                                                        })
                                                    }
                                                </tbody>
                                            </table>
                                        </div>
                                        <div className='col-6'>
                                            <table className='table'>
                                                <thead>
                                                    <tr>
                                                        <th>Producto</th>
                                                        <th>Cantidad</th>
                                                        <th>Precio</th>
                                                    </tr>
                                                </thead>
                                                <tbody id='facturaDetallesShowTbody'>

                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
