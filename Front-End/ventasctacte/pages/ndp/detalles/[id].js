//librerias
import React, { useEffect, useState } from 'react'

//next
import { useRouter } from 'next/router'
import Head from 'next/head'
import Link from 'next/link'
//componentes
import Navbar from '../../../components/Navbar'
import FDPControl from '../../../components/fdpControl'
//api
import getPedido from '../../../API/getPedido'
import getProductos from '../../../API/getProductos'
import putPedido from '../../../API/putPedido'

export default function detalles() {
    const [datos, setDatos] = useState({})
    const [productos, setProductos] = useState([])
    const [listProd, setListProd] = useState([])
    const [precioTotal, setPrecioTotal] = useState({ precio: 0 })
    const [condPago, setCondPago] = useState('CONTADO')
    const [idGenerator, setIdGenerator] = useState(0)
    const router = useRouter()

    useEffect(() => {
        if (router.isReady) {
            getPedido(JSON.parse(sessionStorage.getItem('token')).access_token, router.query.id)
                .then(response => response.text())
                .then(result => {
                    const res = JSON.parse(result)
                    console.log(res)
                    setDatos({
                        nombre: res.Cliente.Nombre + ' ' + res.Cliente.Apellido,
                        cin: res.Cliente.Documento,
                        desc: res.PedidoDescripcion,
                        fecha: res.FechePedido.split('T')[0],
                        estado: res.Estado,
                        costoTotal: res.CostoTotal,
                        pedidosDetalles: res.PedidosDetalles
                    })


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

        }

    }, [router.isReady])




    console.log(datos)

    const crearDetalles = () => {
        if (typeof datos.pedidosDetalles !== 'undefined') {
            setProductos(datos.pedidosDetalles.map((p) => {
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
            }))
            setPrecioTotal({
                precio: datos.pedidosDetalles.map((p) => {
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
                }).map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
            })
        }

    }

    console.log(productos)
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

    const modificarNota = () =>{
        console.log(productos.map(p => {
            const returnValue = {
                "ProductoId": p.prodId,
                "CantidadProducto": p.cantidad
            }
            return returnValue
        }))
        if(productos.length > 0){
            const raw = JSON.stringify({
                "ClienteId": router.query.id,
                "Descripcion": datos.desc,
                "Pedidos": productos.map(p => {
                    const returnValue = {
                        "ProductoId": p.prodId,
                        "CantidadProducto": p.cantidad
                    }
                    return returnValue
                })
            })
            putPedido(JSON.parse(sessionStorage.getItem('token')).access_token, router.query.id, raw)
        }
        
    }

    const facturar = () => {

        const raw = JSON.stringify({
            "IdPedido": selectNota.id,
            "CantidadCuotas": getCantCuotas(),
            "Pedidos": productos.map(p => {
                const returnValue = {
                    "ProductoId": p.prodId,
                    "CantidadProducto": p.cantidad
                }
                return returnValue
            })
        })
        crearFactura(JSON.parse(sessionStorage.getItem('token')).access_token, raw)
        router.back()

    }

    const getCantCuotas = () => {
        if(condPago == "CONTADO"){
            return 1
        }else{
            return document.getElementById("cantCuotasCred").value
        }
    }

    return (
        <div>
            <Head>
                <title>Detalles / Facturacion de pedido</title>
            </Head>
            <Navbar rango='ndp' page='ndpDetalles' />
            <div className=''>
                {/*La parte de arriba de la lista */}
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className='ms-5'>

                        <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16" onClick={() => { router.back() }}>
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
                                                <td><button className="btn btn-info btn-sm" onClick={() => { modificarNota() }}>Guardar Cambios</button></td>
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
                                    <button className='btn btn-danger ' onClick={() => { router.back() }}>Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
