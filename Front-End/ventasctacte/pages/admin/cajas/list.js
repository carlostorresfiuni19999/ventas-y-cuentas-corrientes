import React, { Fragment, useCallback, useEffect, useState } from "react";
import { useRouter } from "next/router";
import getCajas from "../../../API/getCajas";
import CajaList from "../../../components/admin/CajaList";
import AdminNavBar from "../../../components/admin/AdminNavbar";
import getUsersDisponibles from "../../../API/getUsersDisponibles";
import PostCajaForm from "../../../components/admin/PostCajaForm";
import { Button } from "react-bootstrap";
import postCaja from "../../../API/postCaja";
import { Modal } from "react-bootstrap";

export default function List() {
    const [cajas, setCajas] = useState([]);
    const router = useRouter();
    const [showModal, setShowModal] = useState(false);
    const [auth, setAuth] = useState("");
    const [users, setUsers] = useState([]);
    const [usersDisponibles, setUsersDisponibles] = useState(false);
    const [rolCargado, setRolCargado] = useState(false);

    const handleLoad = useCallback(() => {
        if (sessionStorage.getItem("token")) {
            const token = JSON.parse(sessionStorage.getItem("token"));
            setAuth(token.access_token);
            getCajas(token.access_token)
                .then(result => {
                    if (result.ok) {
                        setRolCargado(true);
                        return result.json();

                    } else if (result.status == 401 || result.status == 403) {
                        alert("No cuenta con los privilegios suficientes");
                        router.push("/LogIn");
                    }

                    else {
                        console.log("Error");
                    }
                })
                .then(result => setCajas(result))
                .catch(console.log);
        } else { router.push("/LogIn") }

    });
    const handleShowModal = () => setShowModal(true);
    const handleCloseModal = () => setShowModal(false);
    const handlePostCaja = (value) => {
        const token = JSON.parse(sessionStorage.getItem("token"));

        postCaja(JSON.stringify(value), token.access_token)
            .then(result => {
                if (result.ok) {
                    alert("Guardado con exito");
                    handleLoad();

                }
            })
            .catch(console.log);
    }

    const handleLoadUsers = () => {
        
        return getUsersDisponibles()
        .then(result => {
            if (result.ok) {
                console.log(result);
                result.json()
                .then(r => {
                    console.log(r);
                    setUsers(r);
                    setUsersDisponibles(true);
                })
            }
            else{
                console.log("error");
            }
        })
    }
    

    useEffect(() => {
        handleLoad();
    }, []);


    return (
        <Fragment>
            <AdminNavBar />

            <Button onClick={() =>{
                handleLoadUsers();
                handleShowModal();
            }}>Nueva Caja</Button>
            <Modal show = {showModal} onHide = {handleCloseModal}>
                <Modal.Header closeButton>
                <Modal.Title>Agregar</Modal.Title>
                </Modal.Header>
                
                    <Modal.Body>
                      { usersDisponibles ?  <PostCajaForm options = {users} onSave = {handlePostCaja}/> : <h1>...</h1>}
                    </Modal.Body>
            </Modal>
            <div style={{ marginLeft: "10%", marginRight: "10%", marginTop: "40px" }}>
                <CajaList cajas={cajas} />
            </div>


        </Fragment>

    )

}