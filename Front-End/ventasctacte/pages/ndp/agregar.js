import React, { useEffect, useState } from 'react'
import Link from 'next/link'
import Navbar from '../../components/Navbar'
import styles from '../../styles/naBody.module.css'
import getPersonas from '../../API/getPersonas'
import getProductos from '../../API/getProductos'
import agregarPedido from '../../API/agregarPedido'
//import selectize from 'selectize'

export default function agregar() {

    const [personas, setPersonas] = useState([])
    const [productos, setProductos] = useState([])
    const [listaProductos, setListaProductos] = useState([])

    useEffect(() => {
        getPersonas(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(response => response.text())
            .then(result => {
                const res = JSON.parse(result)
                //console.log(result)
                setPersonas(res.map(persona => {
                    const newpersona = {
                        id: persona.Id,
                        nombre: persona.Nombre + ' ' + persona.Apellido,
                        cin: persona.Documento
                    }
                    return newpersona
                }))
            })
            .catch(error => console.log('error', error));


        getProductos(JSON.parse(sessionStorage.getItem('token')).access_token)
            .then(res => res.text())
            .then(response => {
                const res = JSON.parse(response)
                //console.log(res)
                setProductos(res.map(producto => {
                    const newProducto = {
                        id: producto.Id,
                        nombre: producto.MarcaProducto + ' ' + producto.NombreProducto,
                        codigoBar: producto.CodigoDeBarra,
                        precio: producto.Precio,
                        precioIva: producto.Precio + producto.Iva
                    }
                    return newProducto
                }))

            })
            .catch(error => console.log('error', error));


    }, [])

    //console.log(productos)
    const [cliente, setCliente] = useState({ id: '', cin: '' })
    const [selectProd, setSelectProd] = useState({})
    const [idListaProd, setIdListaProd] = useState(1)


    const generarPedido = () => {

        setListaProductos([...listaProductos, {
            id: idListaProd,
            idProd: selectProd.id,
            cantidad: document.getElementById('cantidadInput').value,
            codBar: selectProd.codBar,
            prod: selectProd.prod,
            precio: selectProd.precio,
            precioTotal: selectProd.precio * document.getElementById('cantidadInput').value,
            cantCuota: document.getElementById('cantCuota').value
        }])
        setIdListaProd(idListaProd + 1)
        document.getElementById('cantidadInput').value = 0
        document.getElementById('cantCuota').value = 0
    }

    const handleBorrar = (id) => {
        setListaProductos(listaProductos.filter(producto => {
            return producto.id != id
        }))
        console.log(listaProductos.filter(producto => {
            return producto.id != id
        }))
    }

    const generarPeticion = () => {
        const raw = JSON.stringify({
            "ClienteId": cliente.id,
            "Descripcion": "sample string 2",
            "CondicionVenta": "sample string 3",
            "Estado": "PENDIENTE",
            "Pedidos": listaProductos.map(producto =>{
                const productoNew ={
                    "ProductoId": producto.id,
                    "CantidadProducto": parseInt(producto.cantidad, 10),
                    "CantidadCuotas": parseInt(producto.cantCuota, 10)
                }
                return productoNew
            }) 
        });
        console.log(JSON.parse(raw))
        agregarPedido(JSON.parse(sessionStorage.getItem('token')).access_token, raw)
    }

    return (

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
                        <select onChange={() => { setCliente(JSON.parse(document.getElementById('idSelectCliente').value)) }} id='idSelectCliente'>
                            <option value=''>seleccionar...</option>
                            {
                                personas.map((persona) => {
                                    return (
                                        <option key={persona.id} value={JSON.stringify({
                                            id: persona.id,
                                            cin: persona.cin,
                                        })}>{persona.nombre}</option>
                                    )
                                })
                            }
                        </select>
                        <label>CIN: </label>
                        <label>{cliente.cin}</label>
                    </div>
                    <div className={styles.ndpatablediv} >
                        <div>
                            <table className={styles.ndpatable}>
                                <thead>
                                    <tr>
                                        <th>Cantidad</th>
                                        <th>Codigo De Barras</th>
                                        <th>Producto</th>
                                        <th>Precio</th>
                                        <th>PrecioTotal</th>
                                        <th>Cantidad de Cuotas</th>
                                        <th>boton</th>
                                    </tr>
                                </thead>
                                <tbody className={styles.ndpatbody}>
                                    {
                                        listaProductos.map(producto => {
                                            return (
                                                <tr key={producto.id}>
                                                    <td>{producto.cantidad}</td>
                                                    <td>{producto.codBar}</td>
                                                    <td>{producto.prod}</td>
                                                    <td>{producto.precio}</td>
                                                    <td>{producto.precioTotal}</td>
                                                    <td>{producto.cantCuota}</td>
                                                    <td><button className={styles.bCancelar} onClick={() => handleBorrar(producto.id)} > x </button></td>
                                                </tr>
                                            )
                                        })
                                    }
                                    <tr className='Agregar'>
                                        <td><input type='number' min='0' defaultValue="0" id='cantidadInput' /></td>
                                        <td><label>{selectProd.codBar}</label></td>
                                        <td>
                                            <select name='producto' onChange={() => { setSelectProd(JSON.parse(document.getElementById('idSelectProducto').value)) }} id='idSelectProducto' >
                                                <option value={
                                                    JSON.stringify({
                                                        id: '',
                                                        codBar: '',
                                                        prod: '',
                                                        precio: ''
                                                    })
                                                }> selecciona... </option>
                                                {
                                                    productos.map((producto) => {
                                                        return (
                                                            <option key={producto.id} value={
                                                                JSON.stringify({
                                                                    id: producto.id,
                                                                    codBar: producto.codigoBar,
                                                                    prod: producto.nombre,
                                                                    precio: producto.precioIva
                                                                })
                                                            }>{producto.nombre}</option>
                                                        )
                                                    })
                                                }
                                            </select>
                                        </td>
                                        <td>{selectProd.precio}</td>
                                        <td> - </td>
                                        <td><input type='number' defaultValue={0} id='cantCuota' /></td>
                                        <td><button className={styles.bAceptar} onClick={() => {generarPedido()}}>v</button></td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </div>
                    <div className={styles.btnDiv}>
                        <button className={styles.bCancelar}>Cancelar</button>
                        <button className={styles.bAceptar} onClick={() => {generarPeticion()}}>Agregar</button>
                    </div>
                </div>
            </div>
        </div>
    );
}
