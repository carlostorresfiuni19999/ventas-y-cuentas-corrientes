import React, { useEffect, useState } from 'react'
import Link from 'next/link'
import Navbar from '../../components/Navbar'
import styles from '../../styles/nlBody.module.css'
import getPedidos from '../../API/getPedidos'
import agregarPedido from '../../API/agregarPedido'
import getPersonas from '../../API/getPersonas'

export default function lista() {

    const [notas, setNotas] = useState([]);

    useEffect( () =>{
        console.log(JSON.parse(sessionStorage.getItem('token')).access_token)
        getPersonas(JSON.parse(sessionStorage.getItem('token')).access_token)
        //agregarPedido(JSON.parse(sessionStorage.getItem('token')).access_token)
        getPedidos(JSON.parse(sessionStorage.getItem('token')).access_token)
        .then(res => res.text()).
        then(result => {
            const n = JSON.parse(result)
            console.log(n)
            setNotas(n.map(nota=>{
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
    }, [])
    
    const eliminar = (id)  =>{
        console.log('se intento eliminar la nota con id ' + id)
    }
    

    return (
        <div>
            <Navbar rango='ndp' page='ndpLista' />
            <div className={styles.ndplbody} >
                {/*La parte de arriba de la lista */}
                <div className={styles.ndpltop}>
                    <Link href='/'>
                        <a> {'<'} </a>
                    </Link>
                    <label> {'<'} nombre de vendedor {'>'}</label>
                    <label>V</label>
                </div>
                {/*La parte del medio de la lista */}
                <div className={styles.ndplcenter}>
                    <label>Notas de Pedidos</label>
                    <label> {'>'} </label>
                    <label>Lista</label>
                </div>
                {/*La parte de abajo de la lista */}
                <div className={styles.ndplbottom}>
                    <div>
                        <h1>Notas de Pedidos</h1>
                        <Link href='/ndp/agregar'>
                            <button className={styles.bAdd}>Agregar</button>
                        </Link>
                    </div>
                    <div>
                        <label>Buscador: </label>
                        <input type="text" />
                        <button >Aplicar</button>
                        <label>Filtros:</label>
                        <input type="checkbox" />
                        <label>Completados</label>
                        <input type="checkbox" />
                        <label>Pendientes</label>
                    </div>

                    {/*ver como refreshear */}
                    <div className={styles.ndpltablediv} >
                        <div>
                            <table className={styles.ndpltable}>
                                <thead>
                                    <tr>
                                        <th>Cliente</th>
                                        <th>CIN</th>
                                        <th>Estado</th>
                                        <th>Vendedor</th>
                                        <th>Fecha</th>
                                        <th>Detalles</th>
                                        <th>Eliminar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        notas.map(nota => {
                                            return (
                                                <tr key={nota.id}>
                                                    <td>{nota.cliente}</td>
                                                    <td>{nota.cin}</td>
                                                    <td>{nota.estado}</td>
                                                    <td>{nota.vendedor}</td>
                                                    <td>{nota.fecha}</td>
                                                    <td><button className={styles.bDetalle}>Detalles</button></td>
                                                    <td><button className={styles.bEliminar} onClick={eliminar(nota.id)}>Eliminar</button></td>
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
    );

}
