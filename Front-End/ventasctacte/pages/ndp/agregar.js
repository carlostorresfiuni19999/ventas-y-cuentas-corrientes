import React, { useEffect, useState } from 'react'
import Link from 'next/link'
import Head from 'next/head'
import { useRouter } from 'next/router'
import Navbar from '../../components/Navbar'
import getPersonas from '../../API/getPersonas'
import getProductos from '../../API/getProductos'
import agregarPedido from '../../API/agregarPedido'
//import selectize from 'selectize'

export default function Agregar() {

    const router = useRouter()

    const [personas, setPersonas] = useState([])
    const [productos, setProductos] = useState([])
    const [listaProductos, setListaProductos] = useState([])


    useEffect(() => {
        getPersonas(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(response => response.text())
            .then(result => {
                const res = JSON.parse(result)
                //console.log(result)
                setPersonas(res.map(persona => {
                    const newpersona = {
                        id: persona.Id,
                        nombre: persona.Nombre + ' ' + persona.Apellido,
                        cin: persona.Documento
                    }
                    return newpersona
                }))
            })
            .catch(error => console.log('error', error));


        getProductos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text())
            .then(response => {
                const res = JSON.parse(response)
                //console.log(res)
                setProductos(res.map(producto => {
                    const newProducto = {
                        id: producto.Id,
                        nombre: producto.MarcaProducto + ' ' + producto.NombreProducto,
                        codigoBar: producto.CodigoDeBarra,
                        precio: producto.Precio,
                        precioIva: producto.Precio + producto.Iva
                    }
                    return newProducto
                }))

            })
            .catch(error => console.log('error', error));


    }, [])

    //console.log(productos)
    const [cliente, setCliente] = useState({ id: '', cin: 0 })
    const [selectProd, setSelectProd] = useState({ precio: 0 })
    const [idListaProd, setIdListaProd] = useState(1)
    const [precioTotalPedido, setPrecioTotalPedido] = useState({ precio: 0 })

    const generarPedido = () => {

        setListaProductos([...listaProductos, {
            id: idListaProd,
            idProd: selectProd.id,
            cantidad: document.getElementById('cantidadInput').value,
            codBar: selectProd.codBar,
            prod: selectProd.prod,
            precio: selectProd.precio,
            precioTotal: selectProd.precio * document.getElementById('cantidadInput').value
        }])

        setPrecioTotalPedido({
            precio: [...listaProductos, {
                id: idListaProd,
                idProd: selectProd.id,
                cantidad: document.getElementById('cantidadInput').value,
                codBar: selectProd.codBar,
                prod: selectProd.prod,
                precio: selectProd.precio,
                precioTotal: selectProd.precio * document.getElementById('cantidadInput').value
            }].map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
        })

        setIdListaProd(idListaProd + 1)
        document.getElementById('cantidadInput').value = 0

    }

    const handleBorrar = (id) => {
        if (confirm("esta seguro?")) {

            setListaProductos(listaProductos.filter(producto => {
                return producto.id != id
            }))
            console.log(listaProductos.filter(producto => {
                return producto.id != id
            }))
            setPrecioTotalPedido({
                precio: listaProductos.filter(producto => {
                    return producto.id != id
                }).map(p => { return p.precioTotal }).reduce((a, b) => a + b, 0)
            })
        }
    }

    const generarPeticion = () => {
        const raw = JSON.stringify({
            "ClienteId": cliente.id,
            "Descripcion": document.getElementById('textAreaDescripcion').value,
            "Pedidos": listaProductos.map(producto => {
                const productoNew = {
                    "ProductoId": producto.idProd,
                    "CantidadProducto": parseInt(producto.cantidad, 10)
                }
                return productoNew
            })
        });
        console.log(JSON.parse(raw))
        agregarPedido(JSON.parse(sessionStorage.getItem('token')).access_token, raw)
        router.push('/ndp/Lista')
    }


    return (

        <div>
            <Head>
                <title>Agregar Pedido</title>
            </Head>
            <Navbar rango='ndp' page='ndpAgregar' />
            <div className=''>
                {/*La parte de arriba de la agregar */}
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className='ms-5'>
                        <Link href='/' >
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                <path fillRule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                            </svg>
                        </Link>
                    </div>


                    {/*<div className="ms-5 collapse navbar-collapse" id="navbarSupportedContent">
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
                    </div>*/}
                </nav>

                {/*La parte de abajo de la agregar */}
                <div className='ms-5 mt-3'>
                    <div>
                        <h1>Agregar pedido</h1>
                    </div>

                    <div className='pt-3'>
                        <label>Cliente: </label>
                        <select className='form-select form-select-sm w-25 pt-2' onChange={() => { setCliente(JSON.parse(document.getElementById('idSelectCliente').value)) }} id='idSelectCliente'>
                            <option value={JSON.stringify({
                                id: 0,
                                cin: 0,
                            })}>seleccionar...</option>
                            {
                                personas.map((persona) => {
                                    return (
                                        <option key={persona.id} value={JSON.stringify({
                                            id: persona.id,
                                            cin: persona.cin,
                                        })}>{persona.nombre}</option>
                                    )
                                })
                            }
                        </select>
                        <div className='p-2 ps-0'>
                            <label>CIN: </label>
                            <label className='ps-2'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(cliente.cin)}</label>
                        </div>
                    </div>

                    <div className='pe-3 h-50'>
                        <div>
                            <table className='table'>
                                <thead className='thead-dark'>
                                    <tr>
                                        <th scope='col'>Cantidad</th>
                                        <th scope='col'>Codigo De Barras</th>
                                        <th scope='col'>Producto</th>
                                        <th className='text-center' scope='col'>Precio</th>
                                        <th className='text-center' scope='col'>PrecioTotal</th>
                                        <th scope='col'></th>
                                    </tr>
                                </thead>
                                <tbody className=''>
                                    {
                                        listaProductos.map(producto => {
                                            return (
                                                <tr key={producto.id}>
                                                    <th scope='row'>{producto.cantidad}</th>
                                                    <td>{producto.codBar}</td>
                                                    <td>{producto.prod}</td>
                                                    <td className='text-center'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(producto.precio)}</td>
                                                    <td className='text-center'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(producto.precioTotal)}</td>
                                                    <td><button className='btn btn-danger' onClick={() => handleBorrar(producto.id)} >
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                                            <path fillRule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                                        </svg>
                                                    </button></td>
                                                </tr>
                                            )
                                        })
                                    }
                                    <tr className='Agregar'>
                                        <td><input type='number' min='0' defaultValue="0" id='cantidadInput' /></td>
                                        <td><label>{selectProd.codBar}</label></td>
                                        <td>
                                            <select className='form-select form-select-sm pt-2' name='producto' onChange={() => { setSelectProd(JSON.parse(document.getElementById('idSelectProducto').value)) }} id='idSelectProducto' >
                                                <option value={
                                                    JSON.stringify({
                                                        id: '',
                                                        codBar: '',
                                                        prod: '',
                                                        precio: 0
                                                    })
                                                }> selecciona... </option>
                                                {
                                                    productos.map((producto) => {
                                                        return (
                                                            <option key={producto.id} value={
                                                                JSON.stringify({
                                                                    id: producto.id,
                                                                    codBar: producto.codigoBar,
                                                                    prod: producto.nombre,
                                                                    precio: producto.precioIva
                                                                })
                                                            }>{producto.nombre}</option>
                                                        )
                                                    })
                                                }
                                            </select>
                                        </td>
                                        <td className='text-center'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(selectProd.precio)}</td>
                                        <td className='text-center'>-</td>
                                        <td><button className='btn btn-primary' onClick={() => { generarPedido() }}>
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-plus" viewBox="0 0 16 16">
                                                <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z" />
                                            </svg>
                                        </button></td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </div>
                    <div className='w-50 float-start h-50'>
                        <label htmlFor='textAreaDescripcion' >Descripcion:</label>
                        <input type='textarea' name='descripcion' className='w-100 h-100' id='textAreaDescripcion' />
                    </div>
                    <div className='w-50 float-end ps-5'>

                        <div>
                            <label className='float-end pe-3'>{new Intl.NumberFormat('us-US', { style: 'decimal' }).format(precioTotalPedido.precio)}</label>
                            <label className='float-end pe-5'>Precio Total:</label>
                        </div>
                        <div>

                        </div>
                    </div>
                    <div className='float-end pe-2 pt-2 pb-5'>
                        <button className='btn btn-success me-2' onClick={() => { generarPeticion() }}>Agregar</button>
                        <button className='btn btn-danger ' onClick={() => { router.back() }}>Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    );
}
