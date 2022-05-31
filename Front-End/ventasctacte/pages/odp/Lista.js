import React, { useState, useEffect } from 'react'

import Head from 'next/head'

import Navbar from '../../components/Navbar'

import getOrdenes from '../../API/getOrdenes'

export default function Lista() {

    const [ordenes, setOrdenes] = useState([])

    useEffect(() => {
        getOrdenes(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const n = JSON.parse(result)
                console.log(n)

            })
            .catch(error => console.log(error))
        }, [ordenes])


    return (
        <div>
            <Head>
                <title>Listar de Ordenes de Cobro</title>
            </Head>
            <div className=''>
                <Navbar rango='ndp' page='ndpLista' />
            </div>
            <div className='ms-4'>
                <div>
                    <nav className="navbar navbar-expand-lg navbar-light bg-light">
                        <div className='ms-4'>
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" className="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                <path fillRule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                            </svg>
                        </div>


                        <div className="ms-5 collapse navbar-collapse" id="navbarSupportedContent">
                            <ul className=" navbar-nav mr-auto">
                                <li className="nav-item active">
                                    <h6 className='pt-3 nav-link'>Notas de Pago</h6>
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

                    {/*La parte de abajo de la lista */}
                    <div className='p-4'>
                        <div>
                            <h5>Ordenes de Cobro</h5>

                        </div>


                        <div className='' >
                            <div>
                                <table className='table'>
                                    <thead className='thead-dark'>
                                        <tr>
                                            <th scope='col'>Cliente</th>
                                            <th scope='col'>CIN</th>
                                            <th scope='col'>Monto Total</th>
                                            <th scope='col'>Fecha Creada</th>
                                            <th scope='col'> </th>
                                        </tr>
                                    </thead>
                                    <tbody>


                                    </tbody>

                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    )
}