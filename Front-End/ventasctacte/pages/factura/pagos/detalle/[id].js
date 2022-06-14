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
import hasRole from '../../../../API/hasRole'

export default function Detalle() {
    const Router = useRouter()

    const [Pagos, setPagos] = useState([]);
    const [Key, SetKey] = useState(0);
    const [Pagado, setPagado] = useState(0)

    useEffect(() => {
        if (sessionStorage.getItem('token') == null) {
            Router.push('/LogIn')
        } else {
            const token = JSON.parse(sessionStorage.getItem('token'));
            hasRole(token.access_token, token.userName, "Cajero")
                .then(r => {
                    if (r == 'false') Router.push("/LogIn");
                });
            if (typeof Router.query.id !== "undefined") {
                getPagos(JSON.parse(sessionStorage.getItem('token')).access_token, Router.query.id)
                    .then(res => res.text())
                    .then(res => {
                        const r = JSON.parse(res)
                        setPagos(r.map(p => {
                            const returnValue = {
                                fecha: p.FechaPago,
                                cliente: p.Cliente.Nombre + " " + p.Cliente.Apellido,
                                cajero: p.Cajero.Nombre + " " + p.Cajero.Apellido,
                                montoTot: p.MontoTotal,
                                detallePago: p.FormasPagos.map(fp => {
                                    const newFP = {
                                        metodo: fp.FormaPago,
                                        monto: fp.Monto
                                    }
                                    return newFP
                                })
                            }
                            return returnValue
                        }))
                        setPagado(
                            r.map(p => {
                                return p.MontoTotal
                            }).reduce((x, y) => x + y)
                        )
                    })
                    .catch(error => console.log(error))
            }
        }
    }, [Router])

    const formatNum = (data) => {
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(data)
    }

    const getKey = () => {
        const returnValue = Key;
        SetKey(Key + 1)
        return returnValue
    }

    const handleClickPago = (fecha) => {

        document.getElementById('tbodyDetPago').innerHTML =
            Pagos.filter(p => { return p.fecha == fecha })[0].detallePago.map(dp => {
                return `<tr key=${getKey().toString()}>
                <th>${dp.metodo}</th>
                <td>${formatNum(dp.monto)} Gs.</td>
            </tr>`
            }).join("")

    }

    return (
        <div>
            <Head>
                <title>Pago de Cuotas</title>
            </Head>
            <div className='ms-4'>
                <div className=''>
                    <Navbar rol='c' rango='fac' page='facDetalles' />
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
                            <th scope='col'> <button className='btn btn-sm btn-primary' onClick={() => { Router.push(`/factura/pagos/crear/${Router.query.id}`) }}> Nuevo Pago </button> </th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            Pagos.map(p => {

                                return (
                                    <tr key={p.fecha} onClick={() => { handleClickPago(p.fecha) }}>
                                        <th>{p.fecha.split('T')[0]}</th>
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
                    <label className='float-end'> Pagado Total: {formatNum(Pagado)} Gs </label>

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
                    <tbody id='tbodyDetPago'>
                        <tr>
                            <th scope='col-2'><h6>Clickea en un pago de arriba para ver el detalle de un pago...</h6></th>
                        </tr>


                    </tbody>

                </table>
            </div>
        </div>


    )
}