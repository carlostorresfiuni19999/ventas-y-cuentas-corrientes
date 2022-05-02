//librerias
import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import Link from 'next/link'
//componentes
import Navbar from '../../../components/Navbar'
//css
//api
import getPedidos from '../../../API/getPedidos'

export default function Detalles(props) {
    const [datos, setDatos] = useState({})
    const [productos, setProductos] = useState([])
    const router = useRouter()

    useEffect(() => {
        getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
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
            .catch(error => console.log('error', error));


    }, [])


    console.log(productos.map(p=>p.precioTotal).reduce((a,b)=> a+b,0))

    return (
        <div>
            <Navbar rango='ndp' page='ndpDetalles' />
            <div className=''>
                {/*La parte de arriba de la lista */}
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className='ms-5'>
                        <Link href='/' >
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                <path fill-rule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                            </svg>
                        </Link>
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
                    <h1>Notas Detalle</h1>
                    {/* lista de los objetos que se pidio, se deberia tener que cambiar se tiene que implementar
                        selectize, para eso necesito la lista de los productos.
                    */ }
                    <div className=''>
                        <label>Cliente:</label>
                        <label className='px-3'>{datos.nombre}</label>
                        <label className=''>CIN:</label>
                        <label className='px-3 pe-5'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(datos.cin)}</label>
                        <label className='ps-5'>Fecha:</label>
                        <label className='px-3'>{datos.fecha}</label>
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
                                            <tr>
                                                <td>{prod.cantidad}</td>
                                                <td>{prod.codBarr}</td>
                                                <td>{prod.nombre}</td>
                                                <td className='text-center'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(prod.precio)}</td>
                                                <td className='text-center'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(prod.precioTotal)}</td>
                                            </tr>
                                        )
                                    })
                                }

                            </tbody>
                        </table>
                    </div>
                    <div>
                        <label>{datos.desc}</label>
                        <label className='float-end pe-5'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(productos.map(p=>p.precioTotal).reduce((a,b)=> a+b,0))}</label>
                        <label className='float-end pe-2'>Total:</label>
                    </div>

                </div>
            </div>
        </div>
    )
}
