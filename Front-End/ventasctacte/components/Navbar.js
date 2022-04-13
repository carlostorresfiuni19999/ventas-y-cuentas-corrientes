import React from "react"
import styles from "../styles/Navbar.module.css"
import Link from 'next/link'

function Navbar() {

    const apareceNav = () => {
        document.getElementById("mainNav").style.width = "250px";
        document.getElementById("subNav").style.width = "0px";
    }

    const desapareceNav = () => {
        document.getElementById("mainNav").style.width = "0px";
        document.getElementById("subNav").style.width = "20px";
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
                        <label className={styles.listMain}>Notas de Pago</label>
                        <div>
                            <div><label className={styles.listSub}>Lista</label></div>
                            <div><label className={styles.listSub}>Agregar</label></div>
                            <div><label className={styles.listSub}>Detalles</label></div>
                        </div>
                    </div>
                    <div className={styles.secciones}>
                        <label className={styles.listMain}>Facturas</label>
                        <div>
                            <div><label className={styles.listSub}>Lista</label></div>
                            <div><label className={styles.listSub}>Pagos</label></div>
                            <div><label className={styles.listSub}>Detalles</label></div>
                        </div>
                    </div>
                    <div className={styles.secciones}>
                        <label className={styles.listMain}>Stock</label>
                        <div>
                            <div><label className={styles.listSub}>TBD1</label></div>
                            <div><label className={styles.listSub}>TBD2</label></div>
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