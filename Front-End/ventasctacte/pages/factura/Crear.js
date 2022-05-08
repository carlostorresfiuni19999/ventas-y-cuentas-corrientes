//react
import React, { useEffect, useState } from 'react'

//next
import Head from 'next/head'
import Link from 'next/link'
import { useRouter } from 'next/router'

//components
import Navbar from '../../components/Navbar'

//api
import getPedidos from '../../API/getPedidos'

export default function Crear() {

    const router = useRouter()

    const [notas, setNotas] = useState([])
    const [selectNota, setSelectNota] = useState({
        id: 0,
        cliente: "",
        cin: "",
        vendedor: "",
        precioTotal: 0,
        pedidoDesc: 0,
        productos: []
    })
    //recive las notas
    useEffect(() => {
        getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const n = JSON.parse(result)
                setNotas(n.map(nota => {
                    const notaNew = {
                        id: nota.Id,
                        cliente: nota.Cliente.Nombre + ' ' + nota.Cliente.Apellido,
                        cin: nota.Cliente.Documento,
                        vendedor: nota.Vendedor.Nombre + ' ' + nota.Vendedor.Apellido,
                        fecha: nota.FechePedido.split('T')[0],
                        precioTotal: nota.CostoTotal,
                        pedidoDesc: nota.PedidoDescripcion,
                        productos: nota.PedidosDetalles.map((detalle) => {
                            const newProdRow = {
                                id: detalle.Id,
                                cantidad: detalle.CantidadProductos,
                                nombre: detalle.Producto.MarcaProducto + ' ' + detalle.Producto.NombreProducto,
                                precio: detalle.Producto.Precio + detalle.Producto.Iva,
                                codBarra: detalle.Producto.CodigoDeBarra,
                                precioTotal: (detalle.Producto.Precio + detalle.Producto.Iva) * detalle.CantidadProductos
                            }
                            return newProdRow
                        })
                    }
                    return notaNew
                }))

            })
            .catch(error => console.log(error))

    }, [])

    useEffect(() => {
        getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const n = JSON.parse(result)
                setNotas(n.map(nota => {
                    const notaNew = {
                        id: nota.Id,
                        cliente: nota.Cliente.Nombre + ' ' + nota.Cliente.Apellido,
                        cin: nota.Cliente.Documento,
                        vendedor: nota.Vendedor.Nombre + ' ' + nota.Vendedor.Apellido,
                        fecha: nota.FechePedido.split('T')[0],
                        precioTotal: nota.CostoTotal,
                        pedidoDesc: nota.PedidoDescripcion,
                        productos: nota.PedidosDetalles.map((detalle) => {
                            const newProdRow = {
                                id: detalle.Id,
                                cantidad: detalle.CantidadProductos,
                                nombre: detalle.Producto.MarcaProducto + ' ' + detalle.Producto.NombreProducto,
                                precio: detalle.Producto.Precio + detalle.Producto.Iva,
                                codBarra: detalle.Producto.CodigoDeBarra,
                                precioTotal: (detalle.Producto.Precio + detalle.Producto.Iva) * detalle.CantidadProductos
                            }
                            return newProdRow
                        })
                    }
                    return notaNew
                }))

            })
            .catch(error => console.log(error))
        return () => {
            setNotas([[]])
        }
    }, [])

    //funciones
    function formatfecha(dateStr) {
        if (dateStr == null) {
            return ''
        }
        const dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[2] + "/" + dArr[1] + "/" + dArr[0].substring(2); //ex out: "18/01/10"
    }

    function formatNum(changeInt) {
        if (changeInt == null) {
            return '---'
        }
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(changeInt)
    }

    console.log(notas)

    return (
        <div>
            <Head>
                <title>Crear Nueva Factura</title>
            </Head>
            <div>
                <Navbar rango='fac' page='facLista' />
            </div>
            <div className='ms-4'>
                {/*La parte de arriba*/}
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className='ms-4'>
                        <Link href='/' >
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                <path fillRule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                            </svg>
                        </Link>
                    </div>


                    <div className="ms-5 collapse navbar-collapse" id="navbarSupportedContent">

                        <ul className=" navbar-nav mr-auto">

                            <li className="nav-item active">
                                <h6 className='pt-3 nav-link'>Factura</h6>
                            </li>
                            <li>
                                <h6 className='pt-3 nav-link'> - </h6>
                            </li>
                            <li className="nav-item">
                                <h6 className='pt-3 nav-link'>Crear</h6>
                            </li>

                        </ul>

                    </div>

                </nav>

                <div className='pt-4 container'>
                    <div className='row'>
                        <div className='col-8'>
                            <h5>Crear una Nueva Factura</h5>
                            {/* lista de los objetos que se pidio, se deberia tener que cambiar se tiene que implementar
                                selectize, para eso necesito la lista de los productos.
                            */ }
                            <select className='form-select form-select-sm pt-2' name='producto' onChange={() => { setSelectNota(JSON.parse(document.getElementById('idSelectPedido').value)) }} id='idSelectPedido' >
                                <option value={
                                    JSON.stringify({
                                        id: 0,
                                        cliente: "",
                                        cin: "",
                                        vendedor: "",
                                        precioTotal: 0,
                                        pedidoDesc: 0,
                                        productos: []
                                    })
                                }> selecciona... </option>
                                {
                                    notas.map((nota) => {
                                        return (
                                            <option key={nota.id} value={
                                                JSON.stringify(nota)
                                            }>{nota.cliente}{'('}{formatNum(nota.cin)}{')'} - {formatfecha(nota.fecha)} - {formatNum(nota.precioTotal)}</option>
                                        )
                                    })
                                }
                            </select>
                            <div className='py-2'>
                                <div>
                                    <label className='ps-2'>Cliente:</label>
                                    <label className='px-3'>{selectNota.cliente}</label>
                                    <label className=''>CIN:</label>
                                    <label className='px-3 pe-5'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(selectNota.cin)}</label>
                                </div>
                                <div>
                                    <label className='ps-2'>Fecha:</label>
                                    <label className='px-3'>{formatfecha(selectNota.fecha)}</label>
                                    <label className=''>Vendedor:</label>
                                    <label className='px-3 pe-5'>{selectNota.vendedor}</label>
                                </div>
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

                                        {
                                            selectNota.productos.map(prod => {
                                                return (
                                                    <tr key={prod.id}>
                                                        <td>{prod.cantidad}</td>
                                                        <td>{prod.codBarr}</td>
                                                        <td>{prod.nombre}</td>
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
                                <h6 className='float-end pe-5'>{formatNum(selectNota.precioTotal)}</h6>
                                <h6 className='float-end pe-2'>Total:</h6>
                                <h6>Descripcion:</h6>
                                <label>{selectNota.pedidoDesc}</label>
                            </div>

                        </div>
                        <div className='col-4 bg-light h-100'>
                            <label>b</label>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    )
}