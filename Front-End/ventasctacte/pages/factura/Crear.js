//react
import React, { useEffect, useState } from 'react'

//next
import Head from 'next/head'
import { useRouter } from 'next/router'

//components
import Navbar from '../../components/Navbar'
import FDPControl from '../../components/fdpControl'

//api
import getPedidosSF from '../../API/getPedidosSF'
import crearFactura from '../../API/crearFactura'
import NavMain from '../../components/NavMain'
import hasRole from '../../API/hasRole'

export default function Crear() {

    //router
    const Router = useRouter()

    //estados
    const [notas, setNotas] = useState([])
    const [condPago, setCondPago] = useState("CONTADO")
    const [selectNota, setSelectNota] = useState({
        id: 0,
        cliente: "",
        cin: "",
        vendedor: "",
        precioTotal: 0,
        pedidoDesc: 0,
        productos: []
    })


    //recive las notas
    useEffect(() => {
        if (sessionStorage.getItem('token') == null) {
            Router.push('/LogIn')
        } else {
            const token = JSON.parse(sessionStorage.getItem('token'));
            hasRole(token.access_token, token.userName, "Cajero")
            .then(r => {
                if(r == 'false'){
                    hasRole(token.access_token, token.userName, "Vendedor")
                    .then(k => {
                        if(k == 'false'){
                            Router.push("LogIn");
                        }
                    }).catch(console.log)
                    
                } 
            }).catch(console.log);

            getPedidosSF(JSON.parse(sessionStorage.getItem('token')).access_token)
                .then(res => res.text()).
                then(result => {
                    const n = JSON.parse(result)
                    setNotas(n.map(nota => {
                        const notaNew = {
                            id: nota.Id,
                            cliente: nota.Cliente.Nombre + ' ' + nota.Cliente.Apellido,
                            cin: nota.Cliente.Documento,
                            vendedor: nota.Vendedor.Nombre + ' ' + nota.Vendedor.Apellido,
                            fecha: nota.FechePedido.split('T')[0],
                            precioTotal: nota.CostoTotal,
                            pedidoDesc: nota.PedidoDescripcion,
                            productos: nota.PedidosDetalles.map((detalle) => {
                                const newProdRow = {
                                    id: detalle.Id,
                                    cantidad: detalle.CantidadProductos,
                                    nombre: detalle.Producto.MarcaProducto + ' ' + detalle.Producto.NombreProducto,
                                    precio: detalle.Producto.Precio + detalle.Producto.Iva,
                                    codBarra: detalle.Producto.CodigoDeBarra,
                                    precioTotal: (detalle.Producto.Precio + detalle.Producto.Iva) * detalle.CantidadProductos
                                }
                                return newProdRow
                            })
                        }
                        return notaNew
                    }))

                })
                .catch(error => console.log(error))
        }
    }, [notas, Router])

    useEffect(() => {
        if (sessionStorage.getItem('token') == null) {
            Router.push('/LogIn')
        } else {
            const token = JSON.parse(sessionStorage.getItem('token'));
            hasRole(token.access_token, token.userName, "Cajero")
            .then(r => {
                if(r == 'false'){
                    hasRole(token.access_token, token.userName, "Vendedor")
                    .then(k => {
                        if(k == 'false'){
                            Router.push("LogIn");
                        }
                    }).catch(console.log)
                    
                } 
            }).catch(console.log);
            
            getPedidosSF(JSON.parse(sessionStorage.getItem('token')).access_token)
                .then(res => res.text()).
                then(result => {
                    const n = JSON.parse(result)
                    setNotas(n.map(nota => {
                        const notaNew = {
                            id: nota.Id,
                            cliente: nota.Cliente.Nombre + ' ' + nota.Cliente.Apellido,
                            cin: nota.Cliente.Documento,
                            vendedor: nota.Vendedor.Nombre + ' ' + nota.Vendedor.Apellido,
                            fecha: nota.FechePedido.split('T')[0],
                            precioTotal: nota.CostoTotal,
                            pedidoDesc: nota.PedidoDescripcion,
                            productos: nota.PedidosDetalles.map((detalle) => {
                                const newProdRow = {
                                    id: detalle.Id,
                                    cantidad: detalle.CantidadProductos,
                                    nombre: detalle.Producto.MarcaProducto + ' ' + detalle.Producto.NombreProducto,
                                    precio: detalle.Producto.Precio + detalle.Producto.Iva,
                                    codBarra: detalle.Producto.CodigoDeBarra,
                                    precioTotal: (detalle.Producto.Precio + detalle.Producto.Iva) * detalle.CantidadProductos
                                }
                                return newProdRow
                            })
                        }
                        return notaNew
                    }))

                })
                .catch(error => console.log(error))
        }
    }, [Router])

    //funciones
    const formatfecha = (dateStr) => {
        if (dateStr == null) {
            return ''
        }
        const dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[2] + "/" + dArr[1] + "/" + dArr[0].substring(2); //ex out: "18/01/10"
    }

    const formatNum = (changeInt) => {
        if (changeInt == null) {
            return '---'
        }
        return new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(changeInt)
    }

    const getCantCuotas = () => {
        if (condPago == "CONTADO") {
            return 1
        } else {
            return document.getElementById("cantCuotasCred").value
        }
    }

    const crearPeticionFactura = () => {
        if (selectNota.id == 0) {
            alert("faltan datos, seleccione una nota de pedido")
        } else {
            const raw = JSON.stringify({
                "IdPedido": selectNota.id,
                "CantidadCuotas": getCantCuotas()
            })
            crearFactura(JSON.parse(sessionStorage.getItem('token')).access_token, raw)
            Router.back()
        }
    }

    console.log(notas)

    return (
        <div>
            <Head>
                <title>Crear Nueva Factura</title>
            </Head>
            <div>
                <Navbar rango='fac' page='facCrear' />
            </div>
            <div className='ms-4'>
                {/*La parte de arriba*/}
                <NavMain person="Vendedor" pag="Crear Factura" />

                <div className='pt-4 container'>
                    <div className='row'>
                        <div className='col-8'>
                            <h5 className='pb-2'>Crear una Nueva Factura</h5>
                            {/* lista de los objetos que se pidio, se deberia tener que cambiar se tiene que implementar
                                selectize, para eso necesito la lista de los productos.
                            */ }
                            <select className='form-select form-select-sm mt-2 mb-3' name='producto' onChange={() => { setSelectNota(JSON.parse(document.getElementById('idSelectPedido').value)) }} id='idSelectPedido' >
                                <option value={
                                    JSON.stringify({
                                        id: 0,
                                        cliente: "",
                                        cin: "",
                                        vendedor: "",
                                        precioTotal: 0,
                                        pedidoDesc: 0,
                                        productos: []
                                    })
                                }> selecciona... </option>
                                {
                                    notas.map((nota) => {
                                        return (
                                            <option key={nota.id} value={
                                                JSON.stringify(nota)
                                            }>{nota.cliente}{'('}{formatNum(nota.cin)}{')'} - {formatfecha(nota.fecha)} - {formatNum(nota.precioTotal)}</option>
                                        )
                                    })
                                }
                            </select>
                            <div className='py-2'>
                                <div>
                                    <label className='ps-2'>Cliente:</label>
                                    <label className='px-3'>{selectNota.cliente}</label>
                                    <label className=''>CIN:</label>
                                    <label className='px-3 pe-5'>{new Intl.NumberFormat('us-US', { style: 'decimal', currency: 'PGS' }).format(selectNota.cin)}</label>
                                </div>
                                <div>
                                    <label className='ps-2'>Fecha:</label>
                                    <label className='px-3'>{formatfecha(selectNota.fecha)}</label>
                                    <label className=''>Vendedor:</label>
                                    <label className='px-3 pe-5'>{selectNota.vendedor}</label>
                                </div>
                            </div>

                            <div className='pe-3'>
                                <table className='table'>
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

                                        {
                                            selectNota.productos.map(prod => {
                                                return (
                                                    <tr key={prod.id}>
                                                        <td>{prod.cantidad}</td>
                                                        <td>{prod.codBarr}</td>
                                                        <td>{prod.nombre}</td>
                                                        <td className='text-center'>{formatNum(prod.precio)}</td>
                                                        <td className='text-center'>{formatNum(prod.precioTotal)}</td>
                                                    </tr>
                                                )
                                            })
                                        }

                                    </tbody>
                                </table>
                            </div>
                            <div>
                                <h6 className='float-end pe-5'>{formatNum(selectNota.precioTotal)}</h6>
                                <h6 className='float-end pe-2'>Total:</h6>
                                <h6>Descripcion:</h6>
                                <label>{selectNota.pedidoDesc}</label>
                            </div>

                        </div>
                        <div className='col-4 bg-light h-100 '>
                            <h5 className='py-2'>Seleccione la forma de pago</h5>
                            <select className='form-select form-select-sm ' name='formaPago' onChange={() => { setCondPago(document.getElementById('formaPago').value) }} id='formaPago' >
                                <option value='CONTADO'> Contado </option>
                                <option value='CREDITO'> Credito </option>
                            </select>

                            <FDPControl fdp={condPago} />

                            <div className='float-end pe-2 pt-2 pb-5'>
                                <button className='btn btn-success me-2' onClick={() => { crearPeticionFactura() }}>Crear</button>
                                <button className='btn btn-danger ' onClick={() => { Router.back() }}>Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}