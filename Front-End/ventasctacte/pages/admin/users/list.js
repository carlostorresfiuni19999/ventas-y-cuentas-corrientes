import React, { Fragment, useEffect, useState, useCallback } from "react"
import PersonList from "../../../components/admin/PersonList";
import AdminNavbar from "../../../components/admin/AdminNavbar";
import hasRole from "../../../API/hasRole";
import getAllPeople from "../../../API/getAllPeople";
import { Button, Modal, Tab, Tabs } from "react-bootstrap";
import { useRouter } from "next/router";
import PersonForm from "../../../components/admin/PersonForm";
import PostPersona from "../../../API/postPersona";
import { editPersona } from "../../../API/editPersona";
import setPassword from "../../../API/setPassword";
const List = () => {

    const [band, setBand] = useState(true);
    const [personas, setPersonas] = useState([]);
    const [cajeros, setCajeros] = useState([]);
    const [vendedores, setVendedores] = useState([]);
    const [logged, setLogged] = useState(false);
    const [key, setKey] = useState("Clientes");
    const router = useRouter();
    const [show, setShow] = useState(false);

    const open = _ => setShow(true);
    const close = _ => setShow(false);

    const buttonStyle = {
        marginTop: "4%",
        marginLeft: "5%",
        marginBottom: "2%"
    }

    const handleSave = (value) => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        PostPersona(value, token.access_token).then(r => {
            if(r.ok){
                alert("Agregado con exito")
                loadData();
            }
            else{
                alert("Ocurrio un error al agregar al usuario, vuelva a intentarlo");
            }
        });
        
    }

    const handleChangePassword = (username, value) => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        setPassword(token.access_token, username, JSON.stringify(value));
    }

    const handleEdit = (username, value) => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        editPersona(token.access_token, username, value)
        .then(r => {
            if(r.ok){
                alert("Editado con exito");
                loadData();
            }else{
                alert("Ocurrio un error al editar el dato, vuelva a intentarlo");
            }
        });
        
    }

    const loadData = useCallback(() => {
        if (sessionStorage.getItem("token")) {
            setLogged(true);
            const token = JSON.parse(sessionStorage.getItem("token"));

            getAllPeople(token.access_token, "Cajero")
            .then(r =>{
                if(r.ok){
                    r.json()
                    .then(c => setCajeros(c))
                    .catch(console.log);
                } else{
                    router.push("LogIn");
                }
            });

            getAllPeople(token.access_token, "Cliente")
            .then(r =>{
                if(r.ok){
                    r.json()
                    .then(c => setPersonas(c))
                    .catch(console.log);
                } else{
                    router.push("LogIn");
                }
            });

            getAllPeople(token.access_token, "Vendedor")
            .then(r =>{
                if(r.ok){
                    r.json()
                    .then(c => setVendedores(c))
                    .catch(console.log);
                } else{
                    router.push("LogIn");
                }
            });

        } else {
            alert("Primero Inicie Sesion");
            router.push("/LogIn");
        }




        return () => setBand(false);
    }, [])

    //Efecto para que haga solo una precarga

    //Efecto para cargar en caso de que cambie la persona
    useEffect(() => {
        loadData();
    }, []);

    const rendered = () => {
        const element =
            <Fragment>
                <AdminNavbar />

                <div style={buttonStyle}>
                    <Button variant="primary" onClick={open}>
                        Agregar
                    </Button>
                    <Modal show={show} onHide={close}>
                        <Modal.Header closeButton>
                            <Modal.Title>Agregar</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <PersonForm
                                onEdit={handleEdit}
                                onSave={handleSave}
                                editable={false}
                            />
                        </Modal.Body>
                    </Modal>
                </div>

                <Tabs
                    id="controlled-tab-example"
                    activeKey={key}
                    onSelect={(k) => setKey(k)}
                    className="mb-3"
                >
                    <Tab eventKey="Clientes" title="Clientes">
                        <PersonList
                            onChangePassword={handleChangePassword}
                            style={{ marginLeft: "30px", marginRight: "30px" }}
                            personas={personas}
                            onEdit={handleEdit}
                        />
                    </Tab>
                    <Tab
                        eventKey="Vendedores"
                        title="Vendedores">
                        <PersonList
                            onChangePassword={handleChangePassword}
                            style={{ marginLeft: "30px", marginRight: "30px" }}
                            personas={vendedores}
                            onEdit={handleEdit}
                        />
                    </Tab>
                    <Tab eventKey="Cajeros"
                        title="Cajeros"
                    >
                        <PersonList
                            onChangePassword={handleChangePassword}
                            style={{ marginLeft: "30px", marginRight: "30px" }}
                            personas={cajeros}
                            onEdit={handleEdit}
                        />

                    </Tab>

                </Tabs>

            </Fragment>
        return element;
    }

    return logged ? rendered() : <h1>Cargando ...</h1>
}
export default List;
