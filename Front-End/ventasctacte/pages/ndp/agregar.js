import React from 'react'
import Link from 'next/link'
import Navbar from '../../components/Navbar'
import styles from '../../styles/naBody.module.css'

export default function agregar(){
    return(
        
        <div>
            <Navbar rango='ndp' page='ndpAgregar' />
            <div className={styles.ndpabody}>
                {/*La parte de arriba de la agregar */}
                <div className={styles.ndpatop}>
                    <Link href='/'>
                        <a> {'<'} </a>
                    </Link>
                    <label> {'<'} nombre de vendedor {'>'}</label>
                    <label>V</label>
                </div>
                {/*La parte del medio de la agregar */}
                <div className={styles.ndpacenter}>
                    <label>Notas de Pedidos</label>
                    <label> {'>'} </label>
                    <label>Agregar</label>
                </div>
                {/*La parte de abajo de la agregar */}
                <div className={styles.ndpabottom}>
                    <div>
                        <h1>Agregar pedido</h1>
                    </div>
                    <div>
                        <label>Cliente: </label>
                        <input type="text" />
                        <label>CIN: </label>
                        <input type="text" />
                    </div>
                    <div className={styles.ndpatablediv} >
                        <div>
                            <table className={styles.ndpatable}>
                                <thead>
                                    <tr>
                                        <th>Cantidad</th>
                                        <th>Producto</th>
                                        <th>Prec. Unit</th>
                                        <th>Prec. Total</th>
                                        <th>Borrar</th>
                                    </tr>
                                </thead>
                                <tbody className={styles.ndpatbody}>
                                    <tr className='Agregar'>
                                        <td><input type='text' placeholder='selectize?'/></td>
                                        <td><input type='text' /></td>
                                        <td><input type='text' /></td>
                                        <td><input type='text' /></td>
                                        <td><button className={styles.bElim}>x</button></td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </div>
                    <div className={styles.btnDiv}>
                        <button className={styles.bCancelar}>Cancelar</button>
                        <button className={styles.bAceptar}>Agregar</button>
                    </div>
                </div>
            </div>
        </div>
    );
}