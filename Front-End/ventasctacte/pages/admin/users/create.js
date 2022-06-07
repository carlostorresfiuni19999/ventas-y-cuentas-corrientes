import React, { Fragment, useEffect, useState } from "react"
import 'bootstrap/dist/css/bootstrap.min.css';
import PersonList from "../../../components/admin/PersonList";
import AdminNavbar from "../../../components/admin/AdminNavbar";
import getPersonas from "../../../API/getPersonas";
import PersonForm from "../../../components/admin/PersonForm";
import { Modal, Button } from "react-bootstrap";
const Create = () => {
    const [personas, setPersonas] = useState([]);
    const [show, setShow] = useState(false);

    const handleShow = () => setShow(true);
    const handleClose = () => setShow(false);
    const loadPeople = (setState) => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        getPersonas(token.access_token)
            .then(r => r.text())
            .then(r => JSON.parse(r))
            .then(r => setState(r))
            .catch(e => console.log(e));
    }

   

    useEffect(() => {
        loadPeople(setPersonas);
    }, [personas]);

    const handleView = (id) => console.log(id);
    const handleDelete = (id) => console.log(id);
   

    const buttonStyle = {
        marginTop: "4%",
        marginLeft: "5%",
        marginBottom:"2%"
    }
    return (
        <Fragment>
            <AdminNavbar />
           
                    <div style={buttonStyle}>
                    <Button variant="primary" onClick={handleShow}>
                        Agregar
                    </Button>
                    </div>
                    
            

            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title style={{ color: "blue" }}>Crear Nuevo Usuario</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <PersonForm onSave={() => console.log("clickme")} />
                </Modal.Body>
            </Modal>
             <PersonList style ={{marginLeft:"30px", marginRight:"30px"}} personas={personas} onView={handleView} onDelete={handleDelete} />

        </Fragment>
    )
}
export default Create;
