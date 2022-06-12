//import
//react
import React, { useEffect, useState } from 'react'

//component
import Navbar from '../../../../components/Navbar'
import NavMain from '../../../../components/NavMain'

//next
import Head from 'next/head'
import { useRouter } from 'next/router'
import getCuota from '../../../../API/getCuota'
import postPago from '../../../../API/postPago'
import hasRole from '../../../../API/hasRole'
import { getRouteMatcher } from 'next/dist/shared/lib/router/utils'



export default function Crear() {

    const [Pagos, setPagos] = useState([])
    const [PrecioMax, setPrecioMax] = useState(0)
    const [Key, setKey] = useState(0)
    const Router = useRouter()
    const [Saldo, setSaldo] = useState(0)

    useEffect(() => {
        if (sessionStorage.getItem('token') == null) {
            Router.push('/LogIn')
        } else {
            const token = JSON.parse(sessionStorage.getItem('token'));
            hasRole(token.access_token, token.userName, "Cajero")
            .then(r => {
                if(r == 'false'){
                    console.log(r == 'false');
                    Router.push("/LogIn");
                
                } 
            }).catch(console.log);
                
                
            if (typeof Router.query.id !== "undefined") {
                getCuota(JSON.parse(sessionStorage.getItem('token')).access_token, Router.query.id)
                    
                    .then(res => res.text())
                    .then(res => {
                        console.log(resp);
                        const r = JSON.parse(res)
                        setSaldo(r.Saldo)
                    })
                    .catch(console.log);
                    
            }
        }
    }, [Router])

    const getMax = () => {
        if ((Saldo - PrecioMax) < 0) {
            alert("Esta pagando mas que la cuota")
            return 0
        }
        return Saldo - PrecioMax
    }

    const getKey = () => {
        const newKey = Key
        setKey(Key + 1)
        return newKey
    }

    const docGet = (id) => {
        return document.getElementById(id)
    }

    const getSaldo = () => {
        return Saldo - PrecioMax
    }

    const agregarPagos = () => {
        if (getMax() == 0) {
            alert("El pago esta completo")
        }
        else if (docGet('addPagoMetodo').value == "" || docGet('addPagoMonto').value == 0) {
            alert("Esta faltando datos...")
        } else {
            const newPago = {
                key: getKey(),
                metodo: docGet('addPagoMetodo').value,
                monto: parseInt(docGet('addPagoMonto').value)
            }
            setPagos([...Pagos, newPago])
            setPrecioMax([...Pagos, newPago].map(p => { return p.monto }).reduce((a, b) => a + b, 0))
            console.log([...Pagos, newPago])
            docGet('addPagoMetodo').value = ""
            docGet('addPagoMonto').value = 0
        }

    }

    const controlNum = () => {
        if (parseInt(docGet('addPagoMonto').value) > getMax()) {
            docGet('addPagoMonto').value = getMax()
        }
    }

    const handleBorrar = (key) => {
        setPagos(Pagos.filter(p => { return p.key != key }))
        setPrecioMax(Pagos.filter(p => { return p.key != key }).map(p => { return p.monto }).reduce((a, b) => a + b, 0))
    }

    const handleSubmit = () => {
        if (PrecioMax == 0) {
            alert("faltan datos")
        } else {
            const raw = JSON.stringify({
                "IdCuota": Router.query.id,
                "MetodosPagos": Pagos.map(p => {
                    const returnValue = {
                        "MetodoPago": p.metodo,
                        "Monto": p.monto
                    }
                    return returnValue
                })
            })
            console.log({
                "IdCuota": parseInt(Router.query.id),
                "MetodosPagos": Pagos.map(p => {
                    const returnValue = {
                        "MetodoPago": p.metodo,
                        "Monto": p.monto
                    }
                    return returnValue
                })
            })
            postPago(JSON.parse(sessionStorage.getItem('token')).access_token, raw);
            Router.back()
        }
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

                <NavMain person="Cajero" pag="Pago de Cuotas" />

            </div>
            <div className='ms-5 container pe-5 py-3'>
                <h4 className='pt-2 ps-5'>Pago</h4>

            </div>
            <div className='ps-5 mx-4 pe-5'>
                <table className='table'>
                    <thead className='thead-dark'>
                        <tr>
                            <th scope='col'>Metodo</th>
                            <th scope='col'>Precio</th>
                            <th scope='col'></th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            Pagos.map(p => {
                                return (
                                    <tr key={p.key}>
                                        <th>{p.metodo}</th>
                                        <td>{p.monto}</td>
                                        <td>
                                            <button className='btn btn-sm btn-danger' onClick={() => handleBorrar(p.key)} >
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                                    <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                                    <path fillRule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                                </svg>
                                            </button>
                                        </td>
                                    </tr>
                                )
                            })
                        }
                        <tr>
                            <th>
                                <input type='text' id='addPagoMetodo' />
                            </th>
                            <td>
                                <input type='number' step={5000} max={getMax()} id='addPagoMonto' onBlur={() => { controlNum() }} />
                            </td>
                            <td>
                                <button className='btn btn-sm btn-primary' onClick={() => { agregarPagos() }}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-plus" viewBox="0 0 16 16">
                                        <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z" />
                                    </svg>
                                </button>
                            </td>
                        </tr>
                    </tbody>

                </table>
                <div>
                    <button className='btn btn-sm btn-success float-end mx-3' onClick={() => { handleSubmit() }}>Confirmar</button>
                    <button className='btn btn-sm btn-danger float-end mx-3' onClick={() => { Router.back() }}>Cancelar</button>
                </div>
                <div>
                    <label className='float-end'>{getSaldo()}</label>
                    <label className='float-end px-3'> Saldo:</label>
                    <label className='float-end'>{PrecioMax}</label>
                    <label className='float-end px-3'> Precio Total:</label>
                </div>

            </div>

        </div>


    )
}