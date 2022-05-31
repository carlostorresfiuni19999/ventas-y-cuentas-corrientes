//'react'
import React, { useEffect, useState } from 'react'

//'next'
import Head from 'next/head'
import Link from 'next/link'
import { useRouter } from 'next/router'

//'component/*'
import Navbar from '../../components/Navbar'

//api
import getFacturas from '../../API/getFacturas'
import deleteFactura from '../../API/deleteFactura'
import postOrden from '../../API/postOrden'

export default function Lista() {

    const router = useRouter()

    const [facturas, setFacturas] = useState([])

    useEffect(() => {
        getFacturas(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const f = JSON.parse(result)
                setFacturas(f.map(fac => {
                    const facturaNew = {
                        id: fac.Id,
                        cliente: fac.Cliente,
                        fecha: fac.FechaFacturada.split('T')[0],
                        monto: fac.MontoTotal,
                        saldo: fac.SaldoTotal,
                        condicion: fac.CondicionVenta,
                        estado: fac.Estado
                    }
                    return facturaNew
                }))

            })
            .catch(error => console.log(error))

    }, [facturas])

    useEffect(() => {
        getFacturas(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const f = JSON.parse(result)
                setFacturas(f.map(factura => {
                    const facturaNew = {
                        id: fac.Id,
                        cliente: fac.Cliente,
                        fecha: fac.FechaFacturada.split('T')[0],
                        monto: fac.MontoTotal,
                        saldo: fac.SaldoTotal,
                        condicion: fac.CondicionVenta,
                        estado: fac.Estado
                    }
                    return facturaNew
                }))

            })
            .catch(error => console.log(error))

        return () => {
            setFacturas([[]])
        }
    }, [])

    const formatfecha = (dateStr) => {
        if (dateStr == null) {
            return ' '
        }
        const dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[2] + "/" + dArr[1] + "/" + dArr[0].substring(2); //ex out: "18/01/10"
    }

    const borrar = (id) =>{
        if (confirm('Esta seguro que desea eliminar esta nota?')) {
            if (confirm("Este acto sera irreversible, esta seguro?")) {
                deleteFactura(JSON.parse(sessionStorage.getItem('token')).access_token, id)
            }
        }
    }

    const tagEstado = (estado) =>{
        switch(estado){
            case "PENDIENTE":
                return (<button className='btn btn-secondary btn-sm disabled'>{estado}</button>)
            case "PROCESANDO":
                return (<button className='btn btn-warning btn-sm disabled'>{estado}</button>)
            case "PAGADO":
                return (<button className='btn btn-success btn-sm disabled'>{estado}</button>)
        }
    }

    const crearOrden = (id) =>{
        postOrden(JSON.parse(sessionStorage.getItem('token')).access_token, id)
    }

    return (
        <div>
            <Head>
                <title>Crear Nueva Factura</title>
            </Head>
            <div className=''>
                <Navbar rango='fac' page='facLista' />
            </div>
            <div className='ms-4'>
                {/*La parte de arriba*/}
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className='ms-4'>
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16" onClick={() => { router.back() }}>
                                <path fillRule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                            </svg>
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
                                <h6 className='pt-3 nav-link'>Lista</h6>
                            </li>

                        </ul>
                    </div>

                </nav>
                <div className='pt-4 ps-4'>
                    <h5>Lista de Facturas</h5>
                    
                </div>

                <div className='ps-4 pt-3' >
                    <div>
                        <table className='table'>
                            <thead className='thead-dark'>
                                <tr>
                                    <th scope='col'>Cliente</th>
                                    <th scope='col'>PrecioTotal</th>
                                    <th scope='col'>Saldo</th>
                                    <th scope='col'>Fecha</th>
                                    <th scope='col'>Estado</th>
                                    <th scope='col'>Condicion</th>
                                    <th scope='col'> </th>
                                    <th scope='col'> </th>
                                </tr>
                            </thead>
                            <tbody>

                                {
                                    facturas.map(factura => {
                                        return (
                                            <tr key={factura.id}>
                                                <th scope='row'>{factura.cliente}</th>
                                                <td>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(factura.monto)}</td>
                                                <td>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(factura.saldo)}</td>
                                                <td>{formatfecha(factura.fecha)}</td>
                                                <td>{tagEstado(factura.estado)}</td>
                                                <td>{factura.condicion}</td>
                                                <td><button className='btn btn-secondary btn-sm' onClick={() => { router.push(`/factura/detalles/${factura.id}`) }}><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-eye" viewBox="0 0 16 16" >
                                                    <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.133 13.133 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.133 13.133 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5c-2.12 0-3.879-1.168-5.168-2.457A13.134 13.134 0 0 1 1.172 8z" />
                                                    <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z" />
                                                </svg></button></td>

                                                <td><button className='btn btn-secondary btn-sm' onClick={() => { borrar(factura.id) }}>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                                        <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                                        <path fillRule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                                    </svg>
                                                </button></td>

                                                <td><button className='btn btn-primary btn-sm float-end me-5' onClick={()=>{crearOrden(factura.id)}}> Crear Orden de Cobro</button></td>
                                            </tr>

                                        )
                                    })
                                }


                            </tbody>

                        </table>
                    </div>
                </div>
            </div>
        </div>
    )
}