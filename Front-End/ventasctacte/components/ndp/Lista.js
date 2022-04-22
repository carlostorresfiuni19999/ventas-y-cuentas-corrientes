import React, { useEffect, useState } from "react";
import Link from 'next/link'
import Navbar from '../Navbar'
import styles from '../../styles/nlBody.module.css'

function Lista() {

    const [notas, setNotas] = useState([]);
    const [comp, setComp] = useState({bool:false});
    const [pend, setPend] = useState({bool:false});


    const fetchNotas = async () => {
        const response = await fetch('/api/lista')
        const data = await response.json()
        setNotas(data)
        console.log(data)
    }

    const changeComp = () => {
        if(!comp.bool){
            setComp({bool:true})
            console.log('comp = true')
        }else{
            setComp({bool:false})
            console.log('comp = false')
        }
    }

    const changePend = () => {
        if(!comp.pend){
            setPend({bool: true})
        }else{
            setPend({bool: false})
        }
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
                        <button onClick = {fetchNotas}>Aplicar</button>
                        <label>Filtros:</label>
                        <input type="checkbox" onChange={changeComp}/>
                        <label>Completados</label>
                        <input type="checkbox" onChange={changePend}/>
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
                                        <th>Monto Total</th>
                                        <th>Saldo</th>
                                        <th>Cantidad</th>
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
                                                    <td>Gs. {nota.montTotal}</td>
                                                    <td>Gs. {nota.saldo}</td>
                                                    <td>{nota.cantidad}</td>
                                                    <td><button className={styles.bDetalle}>Detalles</button></td>
                                                    <td><button className={styles.bEliminar}>Eliminar</button></td>
                                                </tr>
                                            )
                                        })
                                    }
                                    {/*
                                    <tr>
                                        <td>Carlos Torres</td>
                                        <td>1234567</td>
                                        <td>Pendiante</td>
                                        <td>Gs. 12.500.000</td>
                                        <td>Gs. 12.500.000</td>
                                        <td>1</td>
                                        <td><button className={styles.bDetalle}>Detalles</button></td>
                                        <td><button className={styles.bEliminar}>Eliminar</button></td>
                                    </tr>
                                    */}

                                </tbody>

                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );

}

export default Lista