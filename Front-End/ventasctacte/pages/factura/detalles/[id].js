//imports
//react
import React, { useState, useEffect } from 'react'

//next
import { useRouter } from 'next/router'
import Head from 'next/head'
import Link from 'next/link'

//component
import Navbar from '../../../components/Navbar'
import MDPControl from '../../../components/MDPControl'

//API
import getFactura from '../../../API/getFactura'

export default function detalles() {

    const [factura, setFactura] = useState({
        cliente: "",
        cin: 0,
        fechaFact: 'YYYY-MM-DD',
        detalles: [],
        cuotas: []
    })
    const [keyCounter, setKeyCounter] = useState(0)
    const [keyCuota, setKeyCuota] = useState(0)
    const router = useRouter()

    useEffect(() => {
        if (router.isReady) {
            getFactura(JSON.parse(sessionStorage.getItem('token')).access_token, router.query.id)
                .then(response => response.text())
                .then(result => {
                    const res = JSON.parse(result)
                    console.log(res)
                    setFactura({
                        cliente: res.Cliente,
                        cin: parseInt(res.DocCliente),
                        fechaFact: res.FechaFacturacion.split('T')[0],
                        precioTotal: res.PrecioTotal + res.IvaTotal,
                        saldoTotal: res.SaldoTotal,
                        cuotas: res.Cuotas.map(cuota => {
                            const returnValue = {
                                fechaVenc: cuota.FechaVencimiento.split('T')[0],
                                monto: cuota.Monto,
                                saldo: cuota.Saldo,
                                key: cuota.Id
                            }
                            return returnValue
                        }),
                        detalles: res.Detalles.map(detalle => {
                            const returnValue = {
                                cantidad: detalle.Cantidad,
                                nombreProd: detalle.Producto,
                                precioUnit: detalle.PrecioUnitario + detalle.Iva,
                                precioTotal: (detalle.PrecioUnitario + detalle.Iva) * detalle.Cantidad,
                                key: detalle.Id
                            }
                            return returnValue
                        })
                    })


                }).catch(err => console.log(err))


        }

    }, [router.isReady])

    console.log(factura)

    const formatfecha = (dateStr) => {
        if (dateStr == null) {
            return ' '
        }
        const dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[2] + "/" + dArr[1] + "/" + dArr[0].substring(2); //ex out: "18/01/10"
    }

    const formatNum = (data) => {
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(data)
    }

    return (
        <div>
            <Head>
                <title>Detalle de Factura</title>
            </Head>
            <div className='ms-4'>
                <div className=''>
                    <Navbar rango='fac' page='facDetalles' />
                </div>

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

            </div>
            <div className='ms-5 container pe-5'>
                <h4 className='pt-2 ps-5'>Detalles de Factura</h4>
                <div className='ps-5 py-2'>
                    <div>
                        <label className='ps-2'>Cliente:</label>
                        <label className='px-3'>{factura.cliente}</label>
                        <label className=''>CIN:</label>
                        <label className='px-3 pe-5'>{formatNum(factura.cin)}</label>
                    </div>
                    <div>
                        <label className='ps-2'>Fecha Facturada:</label>
                        <label className='px-3'>{formatfecha(factura.fechaFact)}</label>
                    </div>
                </div>
            </div>
            <div className='ps-5 mx-4 pe-5'>
                <table className='table'>
                    <thead className='thead-dark'>
                        <tr>
                            <th scope='col'>Producto</th>
                            <th scope='col'>Cantidad</th>
                            <th scope='col'>Precio Unitario</th>
                            <th scope='col'>Precio Total</th>
                            <th scope='col'> </th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            factura.detalles.map(detalle => {
                                return (
                                    <tr key={detalle.key}>
                                        <th>{detalle.nombreProd}</th>
                                        <td>{detalle.cantidad}</td>
                                        <td>{formatNum(detalle.precioUnit)}</td>
                                        <td>{formatNum(detalle.precioTotal)}</td>
                                        <td></td>
                                    </tr>
                                )
                            })
                        }




                    </tbody>

                </table>
            </div>

            <div className=' pt-5 mx-5 px-4'>
                <div className=''>
                    <h5 className='pt-2'>Cuotas</h5>
                    <div className='pt-4'>
                        <table className='table'>
                            <thead className='thead-dark'>
                                <tr>
                                    <th scope='col'>Fecha Vencimiento</th>
                                    <th scope='col'>Monto Pago</th>
                                    <th scope='col'>Saldo</th>
                                    <th>PAGAR</th>

                                </tr>
                            </thead>
                            <tbody>
                                {
                                    factura.cuotas.map(cuota => {
                                        return (
                                            <tr key={cuota.key}>
                                                <th>{formatfecha(cuota.fechaVenc)}</th>
                                                <td>{formatNum(cuota.monto)}</td>
                                                <td>{formatNum(cuota.saldo)}</td>
                                                <td><button className='btn btn-primary btn-sm' onClick={()=>{}}> Pagar</button></td>
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