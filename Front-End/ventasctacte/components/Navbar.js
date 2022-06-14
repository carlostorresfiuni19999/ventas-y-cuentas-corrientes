import React from "react"
import styles from "../styles/Navbar.module.css"
import Link from 'next/link'
import { useRouter } from 'next/router'
import Head from 'next/head'

function Navbar(props) {
    const router = useRouter()

    const apareceNav = () => {
        document.getElementById("mainNav").style.width = "300px";
        document.getElementById("subNav").style.width = "0px";
        document.getElementById(props.page).style.color = "black"
        document.getElementById(props.rango).style.color = "black"
    }

    const desapareceNav = () => {
        document.getElementById("mainNav").style.width = "0px";
        document.getElementById("subNav").style.width = "1.5%";
    }

    const handleLogOut = () => {
        sessionStorage.clear();
        router.push("/LogIn")
    }

    switch (props.rol) {
        case 'v':
            return (
                <div>
                    <div className={styles.mainNavV} id="mainNav">
                        <div className={styles.Titulo}>
                            
                            <a className={styles.aTitulo}>Ventas y Cta.</a>
                            <a className={styles.aClose} onClick={desapareceNav}>{'<'}</a>
                            
                        </div>
                        <div className={styles.noTitulo}>
                            <div className={styles.secciones}>
                                <label className={styles.listMain}>Vendedor</label>
                                <label className={styles.listMain} id='ndp'>Notas de Pago</label>
                                <div>
                                    <div><Link href='/ndp/Lista' passHref><label className={styles.listSub} id='ndpLista'>Lista</label></Link></div>
                                    <div><Link href='/ndp/Agregar' passHref><label className={styles.listSub} id='ndpAgregar'>Agregar</label></Link></div>
                                    <div><label className={styles.listSub} id='ndpDetalles'>Detalles</label></div>
                                </div>
                            </div>
                            <button className="btn btn-sm btn-secondary mx-2 float-end" onClick={() => { handleLogOut() }}>LogOut</button>
                        </div>
                    </div>
                    <div className={styles.apareceNavV} id="subNav" onClick={apareceNav}>
                        <div>

                            <a className={styles.aOpen}>{'>'}</a>

                        </div>
                    </div>
                </div>
            )
        case 'c':
            return (
                <div>
                    <div className={styles.mainNavC} id="mainNav">
                        <div className={styles.Titulo}>
                            <a className={styles.aTitulo}>Ventas y Cta.</a>
                            <a className={styles.aClose} onClick={desapareceNav}>{'<'}</a>
                        </div>
                        <div className={styles.noTitulo}>
                            
                            <div className={styles.secciones}>
                                <label className={styles.listMain} id='fac'>Facturas</label>
                                <div>
                                    <div><Link href='/factura/Lista' passHref><label className={styles.listSub} id='facLista' >Lista</label></Link></div>
                                </div>
                            </div>
                            <button className="btn btn-sm btn-secondary mt-2 mx-3 float-end" onClick={() => { handleLogOut() }}>LogOut</button>
                        </div>
                    </div>
                    <div className={styles.apareceNavC} id="subNav" onClick={apareceNav}>
                        <div>

                            <a className={styles.aOpen}>{'>'}</a>

                        </div>
                    </div>
                </div>
            )

        case 'a':
            return (
                <div>
                    <div className={styles.mainNavA} id="mainNav">
                        <div className={styles.Titulo}>
                            <Link href="/">
                                <a className={styles.aTitulo}>Ventas y Cta.</a>
                            </Link>
                            <Link href="#">
                                <a className={styles.aClose} onClick={desapareceNav}>{'<'}</a>
                            </Link>
                        </div>
                        <div className={styles.noTitulo}>
                            <div className={styles.secciones}>
                                <label className={styles.listMain} id='ndp'>Notas de Pago</label>
                                <div>
                                    <div><Link href='/ndp/Lista' passHref><label className={styles.listSub} id='ndpLista'>Lista</label></Link></div>
                                    <div><Link href='/ndp/Agregar' passHref><label className={styles.listSub} id='ndpAgregar'>Agregar</label></Link></div>
                                    <div><label className={styles.listSub} id='ndpDetalles'>Detalles</label></div>
                                </div>
                            </div>
                            <div className={styles.secciones}>
                                <label className={styles.listMain} id='fac'>Facturas</label>
                                <div>
                                    <div><Link href='/factura/Lista' passHref><label className={styles.listSub} id='facLista' >Lista</label></Link></div>
                                    <div><label className={styles.listSub} id='facDetalles' >Detalles</label></div>

                                </div>
                            </div>
                            <button className="btn btn-sm btn-secondary mx-2 float-end" onClick={() => { handleLogOut() }}>LogOut</button>
                        </div>
                    </div>
                    <div className={styles.apareceNavA} id="subNav" onClick={apareceNav}>
                        <div>

                            <a className={styles.aOpen}>{'>'}</a>

                        </div>
                    </div>
                </div>
            )

    }


}

export default Navbar