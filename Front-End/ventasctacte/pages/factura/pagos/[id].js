//imports
//react
import React, { useState, useEffect } from 'react'

//next
import { useRouter } from 'next/router'
import Head from 'next/head'

//component
import Navbar from '../../../components/Navbar'

//API
import NavMain from '../../../components/NavMain'

export default function pagos() {
    const router = useRouter()

    return (
        <div>
            <Head>
                <title>Pago de Cuotas</title>
            </Head>
            <div className='ms-4'>
                <div className=''>
                    <Navbar rango='fac' page='facDetalles' />
                </div>

                <NavMain person="Cajero" pag="Pago de Cuotas" />

            </div>
            <div className='ms-5 container pe-5 py-3'>
                <h4 className='pt-2 ps-5'>Pago</h4>

            </div>
            <div className='ps-5 mx-4 pe-5'>
                <table className='table'>
                    <thead className='thead-dark'>
                        <tr>
                            <th scope='col'>Pago Numero</th>
                            <th scope='col'>Fecha</th>
                            <th scope='col'>Pago Total</th>
                            <th scope='col'>Saldo</th>
                            <th scope='col'> </th>
                            <th scope='col'> <button className='btn btn-sm btn-primary' onClick={() => { }}> Nuevo Pago </button> </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th> 1 </th>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td scope='col'> <button className='btn btn-sm btn-secondary' onClick={() => { router.push("/factura/pagos/detalle/1")}}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-eye" viewBox="0 0 16 16" >
                                    <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.133 13.133 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.133 13.133 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5c-2.12 0-3.879-1.168-5.168-2.457A13.134 13.134 0 0 1 1.172 8z" />
                                    <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z" />
                                </svg>
                            </button> </td>
                        </tr>




                    </tbody>

                </table>
            </div>

        </div>


    )
}