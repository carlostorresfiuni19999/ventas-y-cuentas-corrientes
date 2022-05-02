import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import Link from 'next/link'
import Head from 'next/head'
import Navbar from '../../components/Navbar'
import getPedidos from '../../API/getPedidos'
import borrarPedido from '../../API/borrarPedido'

export default function Lista() {

    const router = useRouter()

    const [notas, setNotas] = useState([]);

    useEffect(() => {
        getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const n = JSON.parse(result)
                setNotas(n.map(nota => {
                    const notaNew = {
                        id: nota.Id,
                        cliente: nota.Cliente.Nombre + ' ' + nota.Cliente.Apellido,
                        cin: nota.Cliente.Documento,
                        estado: nota.Estado,
                        vendedor: nota.Vendedor.Nombre + ' ' + nota.Vendedor.Apellido,
                        fecha: nota.FechePedido,
                        precioTotal: 0
                    }
                    return notaNew
                }))

            })
            .catch(error => console.log(error))

    }, [notas])

    useEffect(() => {
        getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text()).
            then(result => {
                const n = JSON.parse(result)
                console.log(n)
                setNotas(n.map(nota => {
                    const notaNew = {
                        id: nota.Id,
                        cliente: nota.Cliente.Nombre + ' ' + nota.Cliente.Apellido,
                        cin: nota.Cliente.Documento,
                        estado: nota.Estado,
                        vendedor: nota.Vendedor.Nombre + ' ' + nota.Vendedor.Apellido,
                        fecha: nota.FechePedido
                    }
                    return notaNew
                }))

            })
            .catch(error => console.log(error))
        return () => {
            setNotas([[]])
        }
    }, [])



    const eliminar = (id) => {
        if (confirm('Esta seguro que desea eliminar esta nota?')) {
            if (confirm("Este acto sera irreversible, esta seguro?")) {
                borrarPedido(JSON.parse(sessionStorage.getItem('token')).access_token, id)
            }
        }

    }


    return (
        <div>
            <Head>
                <title>Lista de Pedidos</title>
            </Head>
            <div className=''>
                <Navbar rango='ndp' page='ndpLista' />
            </div>
            <div className='ms-4'>
                <div>
                    {/*La parte de arriba de la lista */}
                    <nav className="navbar navbar-expand-lg navbar-light bg-light">
                        <div className='ms-4'>
                            <Link href='/' >
                                <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                    <path fill-rule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                                </svg>
                            </Link>
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
                            <h1>Notas de Pedidos</h1>

                        </div>

                        {/* 
                            <label>Buscador: </label>
                            <input type="text" />
                            <label>Filtros:</label>
                            <input type="checkbox" />
                            <label>Completados</label>
                            <input type="checkbox" />
                            <label>Pendientes</label>
                        */}

                        {/*<button className='btn btn-primary float-right' onClick={() => { router.push('Agregar') }}>Agregar</button> */}

                        <div className='' >
                            <div>
                                <table className='table'>
                                    <thead className='thead-dark'>
                                        <tr>
                                            <th scope='col'>Cliente</th>
                                            <th scope='col'>CIN</th>
                                            <th scope='col'>Estado</th>
                                            <th scope='col'>Vendedor</th>
                                            <th scope='col'>PrecioTotal</th>
                                            <th scope='col'>Fecha</th>
                                            <th scope='col'> </th>
                                            <th scope='col'> <button className='btn btn-primary float-right btn-sm' onClick={() => { router.push('Agregar') }}>Agregar</button></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            notas.map(nota => {
                                                return (
                                                    <tr key={nota.id}>
                                                        <th scope='row'>{nota.cliente}</th>
                                                        <td>{nota.cin}</td>
                                                        <td>{nota.estado}</td>
                                                        <td>{nota.vendedor}</td>
                                                        <td></td>
                                                        <td>{nota.fecha}</td>
                                                        <Link href={`/ndp/Detalles/${nota.id}`} >
                                                            <td><button className='btn btn-info btn-sm'><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-box-arrow-in-right" viewBox="0 0 16 16">
                                                                <path fill-rule="evenodd" d="M6 3.5a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-2a.5.5 0 0 0-1 0v2A1.5 1.5 0 0 0 6.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-8A1.5 1.5 0 0 0 5 3.5v2a.5.5 0 0 0 1 0v-2z" />
                                                                <path fill-rule="evenodd" d="M11.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H1.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3z" />
                                                            </svg></button></td>
                                                        </Link>
                                                        <td><button className='btn btn-danger btn-sm' onClick={() => { eliminar(nota.id) }}>
                                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                                                <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                                            </svg>
                                                        </button></td>
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
            </div>

        </div>
    );

}
