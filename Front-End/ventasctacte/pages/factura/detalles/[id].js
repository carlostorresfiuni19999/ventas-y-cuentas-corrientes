//imports
//react
import React, { useState, useEffect } from 'react'

//next
import Router from 'next/router'
import Head from 'next/head'
import Link from 'next/link'

//component
import Navbar from '../../../components/Navbar'
import MDPControl from '../../../components/MDPControl'

export default function detalles() {
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
                        <label className='px-3'></label>
                        <label className=''>CIN:</label>
                        <label className='px-3 pe-5'></label>
                    </div>
                    <div>
                        <label className='ps-2'>Fecha:</label>
                        <label className='px-3'></label>
                        <label className=''>Vendedor:</label>
                        <label className='px-3 pe-5'></label>
                    </div>
                </div>
            </div>
            <div className='ps-5 mx-4 pe-5'>
                <table className='table'>
                    <thead className='thead-dark'>
                        <tr>
                            <th scope='col'>Producto</th>
                            <th scope='col'>Cod. Barra</th>
                            <th scope='col'>Cantidad</th>
                            <th scope='col'>Cantidad Facturada</th>
                            <th scope='col'>Saldo</th>
                            <th scope='col'>Precio Total</th>
                            <th scope='col'> </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th>Intel -----</th>
                            <td>123456789</td>
                            <td>2</td>
                            <td>1</td>
                            <td>500.000</td>
                            <td>1.000.000</td>
                            <td></td>
                        </tr>



                    </tbody>

                </table>
                <div>
                    <h6 className='float-end pe-5'>1.000.000 Gs</h6>
                    <h6 className='float-end pe-2'>Total:</h6>
                    <h6 className='float-end pe-5'>500.000</h6>
                    <h6 className='float-end pe-2'>Saldo:</h6>

                </div>
            </div>

            <div className='mt-5 pt-5 container'>
                <div className='row'>
                    <div className='col-8'>
                        <h5 className='pt-2'>Pagos Realizados</h5>
                        <div className='pt-4'>
                            <table className='table'>
                                <thead className='thead-dark'>
                                    <tr>
                                        <th scope='col'>Fecha</th>
                                        <th scope='col'>metodo de pago</th>
                                        <th scope='col'>Saldo</th>
                                        <th scope='col'>Monto de pago</th>
                                        <th scope='col'>Total Pagado</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th>DD/MM/YY</th>
                                        <td>Efectivo</td>
                                        <td>500.000 Gs</td>
                                        <td>500.000 Gs</td>
                                        <td>500.000 Gs</td>
                                    </tr>
                                    <tr>
                                        <th>DD/MM/YY</th>
                                        <td>Efectivo</td>
                                        <td>200.000 Gs</td>
                                        <td>300.000 Gs</td>
                                        <td>800.000 Gs</td>
                                    </tr>


                                </tbody>

                            </table>
                            <div>
                                <h6>Descripcion:</h6>
                                <label></label>
                            </div>
                        </div>
                    </div>
                    <div className='col-4'>
                        <h5>Pagar</h5>
                        <MDPControl  mdp = 'EFECTIVO'  montoMax={200000} />
                    </div>
                </div>
            </div>



        </div>


    )
}