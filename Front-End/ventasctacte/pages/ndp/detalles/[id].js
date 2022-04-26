//librerias
import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import Link from 'next/link'
//componentes
import Navbar from '../../../components/Navbar'
//css
import styles from '../../../styles/ndBody.module.css'
//api
import getProductos from '../../../API/getProductos'
import { loadGetInitialProps } from 'next/dist/shared/lib/utils'

export default function Detalles(props) {
    const [notas, setNotas] = useState([])
    const [productos, setProductos] = useState([])
    
    useEffect(() => {
        getProductos(JSON.parse(sessionStorage.getItem('token')).access_token)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
    }, [])

    console.log(productos)

    return (
        <div>
            <Navbar rango='ndp' page='ndpDetalles' />
            <div className={styles.ndpdbody}>
                {/*La parte de arriba de la lista */}
                <div className={styles.ndpdtop}>
                    <Link href='/'>
                        <a> {'<'} </a>
                    </Link>
                    <label> {'<'} nombre de vendedor {'>'}</label>
                    <label>V</label>
                </div>
                {/*La parte del medio de la lista */}
                <div className={styles.ndpdcenter}>
                    <label>Notas de Pedidos</label>
                    <label> {'>'} </label>
                    <label>Lista</label>
                    <label> {'>'} </label>
                    <label> Detalles </label>
                    <label> {'>'} </label>
                    {Imprimir()}
                </div>
                {/*La parte de abajo de la lista */}
                <div className={styles.ndpdbottom}>
                    <h1>Notas Detalle</h1>
                    {/* lista de los objetos que se pidio, se deberia tener que cambiar se tiene que implementar
                        selectize, para eso necesito la lista de los productos.
                    */ }
                    <div className={styles.ndpdcliente}>
                        <label>Cliente:</label>
                        <label></label>
                        <label className={styles.clienteCin}>CIN:</label>
                        <label>cin</label>
                    </div>

                    <div className={styles.ndpdtablediv}>
                        <table className={styles.ndpdtable}>
                            {//thead
                            }
                            <thead>
                                <tr>
                                    <th>Cantidad</th>
                                    <th>Codigo de Barra</th>
                                    <th>Producto</th>
                                    <th>Precio Unit.</th>
                                    <th>Precio Total</th>
                                </tr>
                            </thead>
                            {//tbody, aca se imprimen los productos que tiene el detalle                        
                            }
                            <tbody>
                                {//en prueba
                                    productos.map(producto=>{ return(
                                        producto.map(prod => {
                                            return(
                                                <tr key={prod.id}>
                                                    <td>{prod.cantProd}</td>
                                                    <td>{prod.codBarra}</td>
                                                    <td>{prod.nombre}</td>
                                                    <td>{prod.precio}</td>
                                                    <td>{prod.precioTotal}</td>
                                                </tr>
                                            )
                                        })
                                    )})
                                }
                                <tr>
                                    <td><button className={styles.ndpdBtnAdd}> + </button></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>

                            </tbody>
                        </table>
                    </div>

                </div>
            </div>
        </div>
    )
}
const Imprimir = () =>{
    const router = useRouter()
    const {id} = router.query

    return <label>{id}</label>
}