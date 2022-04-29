import React from "react"
import styles from "../styles/Navbar.module.css"
import Link from 'next/link'

function Navbar(props) {
    
    const apareceNav = () => {
        document.getElementById("mainNav").style.width = "15%";
        document.getElementById("subNav").style.width = "0px";
        document.getElementById(props.page).style.color= "black"
        document.getElementById(props.rango).style.color= "black"
    }

    const desapareceNav = () => {
        document.getElementById("mainNav").style.width = "0px";
        document.getElementById("subNav").style.width = "1.5%";
    }

    return (
        <div>
            <div className={styles.mainNav} id="mainNav">
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
                            <div><Link href='/ndp/Lista'><label className={styles.listSub} id='ndpLista'>Lista</label></Link></div>
                            <div><Link href='/ndp/Agregar'><label className={styles.listSub} id='ndpAgregar'>Agregar</label></Link></div>
                            <div><label className={styles.listSub} id='ndpDetalles'>Detalles</label></div>
                        </div>
                    </div>
                    <div className={styles.secciones}>
                        <label className={styles.listMain} id='fac'>Facturas</label>
                        <div>
                            <div><label className={styles.listSub} id='facLista' >Lista</label></div>
                            <div><label className={styles.listSub} id='facPagos' >Pagos</label></div>
                            <div><label className={styles.listSub} id='facDetalles' >Detalles</label></div>
                        </div>
                    </div>
                    <div className={styles.secciones}>
                        <label className={styles.listMain}id='db'>Stock</label>
                        <div>
                            <div><label className={styles.listSub} id='db1'>TBD1</label></div>
                            <div><label className={styles.listSub} id='db2'>TBD2</label></div>
                        </div>
                    </div>
                </div>
            </div>
            <div className={styles.apareceNav} id="subNav" onClick={apareceNav}>
                <div>

                    <a className={styles.aOpen}>{'>'}</a>

                </div>
            </div>
        </div>
    )

    
}

export default Navbar