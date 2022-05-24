//librerias
import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import Link from 'next/link'
//componentes
import Navbar from '../../../components/Navbar'
//css
//api
import getPedidos from '../../../API/getPedidos'
import getPedido from '../../../API/getPedido'
import getProductos from '../../../API/getProductos'

export default function Detalles() {
    const [datos, setDatos] = useState({})
    const [productos, setProductos] = useState([])
    const [listProd, setListProd] = useState([])
    const router = useRouter()

    useEffect(() => {
        if (router.isReady) {
            getPedido(JSON.parse(sessionStorage.getItem('token')).access_token, router.query.id)
                .then(response => response.text())
                .then(result => {
                    const res = JSON.parse(result)

                    setDatos({
                        nombre: res.Cliente.Nombre + ' ' + res.Cliente.Apellido,
                        cin: res.Cliente.Documento,
                        desc: res.PedidoDescripcion,
                        fecha: res.FechePedido.split('T')[0],
                        estado: res.Estado,
                        costoTotal: res.CostoTotal
                    })

                    setProductos(res.PedidosDetalles.map((p) => {
                        const newProd = {
                            id: p.Id,
                            cantidad: p.CantidadProductos,
                            nombre: p.Producto.MarcaProducto + ' ' + p.Producto.NombreProducto,
                            codBarr: p.Producto.CodigoDeBarra,
                            precio: p.Producto.Precio + p.Producto.Iva,
                            precioTotal: (p.Producto.Precio + p.Producto.Iva) * p.CantidadProductos
                        }
                        return newProd
                    }))
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

                }).catch
        }


        {/*getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(response => response.text())
            .then(result => {
                const res = JSON.parse(result)
                const pedido = res.filter(p => p.Id == router.query.id)
                console.log(pedido)

                setDatos({
                    nombre: pedido[0].Cliente.Nombre + ' ' + pedido[0].Cliente.Apellido,
                    cin: pedido[0].Cliente.Documento,
                    desc: pedido[0].PedidoDescripcion,
                    fecha: pedido[0].FechePedido.split('T')[0],
                    estado: pedido[0].Estado
                })

                setProductos(pedido[0].PedidosDetalles.map((p) => {
                    const newProd = {
                        cantidad: p.CantidadProductos,
                        nombre: p.Producto.MarcaProducto + ' ' + p.Producto.NombreProducto,
                        codBarr: p.Producto.CodigoDeBarra,
                        precio: p.Producto.Precio + p.Producto.Iva,
                        precioTotal: (p.Producto.Precio + p.Producto.Iva) * p.CantidadProductos
                    }
                    return newProd
                }))


            })
        .catch(error => console.log('error', error));*/}


    }, [router.isReady])

    console.log(productos)

    const formatFecha = (dateStr) => {
        if (dateStr == null) {
            return ''
        }
        const dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[2] + "/" + dArr[1] + "/" + dArr[0].substring(2); //ex out: "18/01/10"
    }
    
    const formatNum = (data) =>{
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(data)
    }

    const getDoc = (id) => {
        return document.getElementById(id)
    }

    const handleChange = (id) =>{
        const modificar = productos.map((prod) =>{
            if(prod.id == id){
                return prod
            }
        })[0]
        modificar.cantidad = parseInt(getDoc(`cantidadProd${id}`).value)
        console.log(modificar)
        productos.forEach((prod) =>{
            if(prod.id == id){
                prod = modificar
            }
        })
    }

    return (
        <div>
            <Navbar rango='ndp' page='ndpDetalles' />
            <div className=''>
                {/*La parte de arriba de la lista */}
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className='ms-5'>

                        <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16" onClick={() => { router.back() }}>
                            <path fillRule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                        </svg>

                    </div>

                    {/*<div className="ms-5">
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
                    </div> */}

                </nav>
                {/*La parte de abajo de la lista */}
                <div className='ms-5 mt-3'>
                    <h5>Detalles</h5>
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
                                </tr>
                            </thead>
                            {//tbody, aca se imprimen los productos que tiene el detalle                        
                            }
                            <tbody>
                                {//en prueba
                                    productos.map(prod => {
                                        return (
                                            <tr key={prod.id}>
                                                <td><input type="number" min={1} max={10} defaultValue={prod.cantidad} onBlur={()=>handleChange(prod.id)} id={`cantidadProd${prod.id}`}></input></td>
                                                <td>{prod.codBarr}</td>
                                                <td>
                                                    <input type="text" list='productosSelectData' className='form-control' defaultValue={prod.nombre} id={`producto${prod.id}`} onBlur={() => handleChange(prod.id)}></input>
                                                    <datalist id="productosSelectData">
                                                        {
                                                            listProd.map((prod)=>{
                                                                return(
                                                                    <option key={prod.id} value={prod.nombre}>{formatNum(prod.precio)} - {formatNum(prod.precioIva)}</option>
                                                                )
                                                            })   
                                                        }
                                                    </datalist>
                                                </td>
                                                <td className='text-center'>{formatNum(prod.precio)}</td>
                                                <td className='text-center'>{formatNum(prod.precioTotal)}</td>
                                            </tr>
                                        )
                                    })
                                }
                                

                            </tbody>
                        </table>
                    </div>
                    <div>
                        <h6 className='float-end pe-5'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(productos.map(p => datos.costoTotal).reduce((a, b) => a + b, 0))}</h6>
                        <h6 className='float-end pe-2'>Total:</h6>
                        <h6>Descripcion:</h6>
                        <label>{datos.desc}</label>
                    </div>

                </div>
            </div>
        </div>
    )
}
