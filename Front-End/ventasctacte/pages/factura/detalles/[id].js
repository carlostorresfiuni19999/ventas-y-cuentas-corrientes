//imports
//react
import React, { useState, useEffect } from 'react'

//next
import { useRouter } from 'next/router'
import Head from 'next/head'

//component
import Navbar from '../../../components/Navbar'
import MDPControl from '../../../components/MDPControl'

//API
import getFactura from '../../../API/getFactura'
import NavMain from '../../../components/NavMain'
import hasRole from '../../../API/hasRole'

export default function Detalles() {

    const [factura, setFactura] = useState({
        cliente: "",
        cin: 0,
        fechaFact: 'YYYY-MM-DD',
        detalles: [],
        cuotas: []
    })
    const Router = useRouter()

    useEffect(() => {
        if (sessionStorage.getItem('token') == null) {
            Router.push('/LogIn')
        } else {
            const token = JSON.parse(sessionStorage.getItem('token'));
            hasRole(token.access_token, token.userName, "Cajero")
            .then(r => {
                if(r == 'false'){
                    hasRole(token.access_token, token.userName, "Vendedor")
                    .then(k => {
                        if(k == 'false'){
                            Router.push("/LogIn");
                        }
                    }).catch(console.log)
                    
                } 
            }).catch(console.log);
                
            if (Router.isReady) {
                getFactura(JSON.parse(sessionStorage.getItem('token')).access_token, Router.query.id)
                    .then(response => response.text())
                    .then(result => {
                        const res = JSON.parse(result)
                        console.log(res)
                        setFactura({
                            cliente: res.Cliente,
                            cin: parsearCIN(res.DocumentoTipo, res.DocCliente),
                            fechaFact: res.FechaFacturacion.split('T')[0],
                            precioTotal: res.PrecioTotal + res.IvaTotal,
                            ivaTotal: res.IvaTotal,
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
        }
    }, [Router])

    const parsearCIN = (tipo, cin) => {
        switch (tipo) {
            case "CI":
                const returnValue = formatNum(cin)
                return returnValue
            case "RUC":
                return formatNum(cin.split('-')[0]) + "-" + cin.split('-')[1]
            case "DNI":
                return cin
        }
    }

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

    const getButton = (key, saldo) => {
        if (saldo == 0) {
            return (
                <button className='btn btn-primary btn-sm' onClick={() => { Router.push(`/factura/pagos/crear/${key}`) }} disabled> Pagar</button>
            )
        } else {
            return (
                <button className='btn btn-primary btn-sm' onClick={() => { Router.push(`/factura/pagos/crear/${key}`) }}> Pagar</button>
            )
        }
    }
    return (
        <div>
            <Head>
                <title>Detalle de Factura</title>
            </Head>
            <div className='ms-4'>
                <div className=''>
                    <Navbar rol='c' rango='fac' page='facDetalles' />
                </div>

                {/*La parte de arriba*/}
                <NavMain person="Cajero" pag="Detalles de la Factura" />

            </div>
            <div className='ms-5 container pe-5'>
                <h4 className='pt-2 ps-5'>Detalles de Factura</h4>
                <div className='ps-5 py-2'>
                    <div>
                        <label className='ps-2'>Cliente:</label>
                        <label className='px-3'>{factura.cliente}</label>
                        <label className=''>CIN:</label>
                        <label className='px-3 pe-5'>{factura.cin}</label>
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
                        <tr>
                            <th></th>
                            <td></td>
                            <td>Iva: {formatNum(factura.ivaTotal)} </td>
                            <td className='table-success'>Total: {formatNum(factura.precioTotal)} </td>
                            <td></td>
                        </tr>



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
                                    <th scope='col'>Detalles</th>
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
                                                <td>
                                                    <button className='btn btn-sm btn-secondary' onClick={() => { Router.push(`/factura/pagos/detalle/${cuota.key}`) }}>
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-eye" viewBox="0 0 16 16" >
                                                            <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.133 13.133 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.133 13.133 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5c-2.12 0-3.879-1.168-5.168-2.457A13.134 13.134 0 0 1 1.172 8z" />
                                                            <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z" />
                                                        </svg>
                                                    </button>
                                                </td>
                                                <td>{getButton(cuota.key, cuota.saldo)}</td>
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