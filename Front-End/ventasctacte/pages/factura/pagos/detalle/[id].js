//imports
//react
import React, { useState, useEffect } from 'react'

//next
import { useRouter } from 'next/router'
import Head from 'next/head'

//component
import Navbar from '../../../../components/Navbar'

//API
import NavMain from '../../../../components/NavMain'
import getPagos from '../../../../API/GetPagos'

export default function detalle() {
    const router = useRouter()

    const [pagos, setpagos] = useState([])
    const [selec, setSelect] = useState([])

    useEffect(() => {
        if (typeof router.query.id !== "undefined") {
            getPagos(JSON.parse(sessionStorage.getItem('token')).access_token, router.query.id)
                .then(res => res.text())
                .then(res => {
                    const r = JSON.parse(res)
                    console.log(r)
                    setpagos(r.map(p=>{
                        const returnValue ={
                            fecha: p.FechaPago.split('T')[0],
                            cliente: p.Cliente.Nombre + " " + p.Cliente.Apellido,
                            cajero: p.Cajero.Nombre + " " + p.Cajero.Apellido,
                            montoTot: p.MontoTotal,
                            detallePago: p.FormasPagos.map(fp=>{
                                const newFP ={
                                    metodo: fp.FormaPago,
                                    monto: fp.Monto
                                }
                                return newFP
                            }) 
                        }
                        return returnValue
                    }))
                })
        }

    }, [router.isReady])

    useEffect(() => {
        if (typeof router.query.id !== "undefined") {
            getPagos(JSON.parse(sessionStorage.getItem('token')).access_token, router.query.id)
                .then(res => res.text())
                .then(res => {
                    const r = JSON.parse(res)
                    console.log(r)
                    setpagos(r.map(p=>{
                        const returnValue ={
                            fecha: p.FechaPago.split('T')[0],
                            cliente: p.Cliente.Nombre + " " + p.Cliente.Apellido,
                            cajero: p.Cajero.Nombre + " " + p.Cajero.Apellido,
                            montoTot: p.MontoTotal,
                            detallePago: p.FormasPagos.map(fp=>{
                                const newFP ={
                                    metodo: fp.FormaPago,
                                    monto: fp.Monto
                                }
                                return newFP
                            }) 
                        }
                        return returnValue
                    }))
                })
        }

    }, [pagos])

    const formatNum = (data) => {
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(data)
    }


    return (
        <div>
            <Head>
                <title>Pago de Cuotas</title>
            </Head>
            <div className='ms-4'>
                <div className=''>
                    <Navbar rango='fac' page='facDetalles' />
                </div>

                <NavMain person="Cajero" pag="Detalle De Pago" />

            </div>
            <div className='ms-5 container pe-5 py-3'>
                <h4 className='py-2 ps-5'>Pago</h4>
                
            </div>
            <div className='ps-5 mx-4 pe-5'>
                <table className='table'>
                    <thead className='thead-dark'>
                        <tr>   
                            <th scope='col'>Fecha</th>
                            <th scope='col'>Cliente</th>
                            <th scope='col'>Cajero</th>
                            <th scope='col'>Monto</th>
                            <th scope='col'> <button className='btn btn-sm btn-primary' onClick={() => { router.push(`/factura/pagos/Crear/${router.query.id}`)}}> Nuevo Pago </button> </th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            pagos.map(p=>{
                                return(
                                    <tr key={p.fecha} onClick={()=>{setSelect(p.detallePago)}}>
                                        <th>{p.fecha}</th>
                                        <td>{p.cliente}</td>
                                        <td>{p.cajero}</td>
                                        <td>{formatNum(p.montoTot)} Gs.</td>
                                        <td></td>
                                    </tr>
                                )
                            })
                        }


                    </tbody>

                </table>
                <div>
                    <label className='float-end mx-3'>Saldo: {formatNum(0)} Gs</label>
                    <label className='float-end'> Precio Total: {formatNum(0)} Gs </label>

                </div>
            </div>

            <div className='ps-5 mx-4 pe-5'>
                <table className='table'>
                    <thead className='thead-dark'>
                        <tr> 
                            <th scope='col'>Metodo</th>
                            <th scope='col'>Monto</th>  
                        </tr>
                    </thead>
                    <tbody>
                        {
                            selec.map(dp=>{
                                return(
                                    <tr>
                                        <th>{dp.metodo }</th>
                                        <td>{formatNum(dp.monto)} Gs.</td>
                                    </tr>
                                )
                            })
                        }


                    </tbody>

                </table>
            </div>
        </div>


    )
}