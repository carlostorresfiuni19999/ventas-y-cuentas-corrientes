import { Fragment, useState } from "react"
import { Modal } from "react-bootstrap";
import { Column, TableWithBrowserPagination,MenuItem } from "react-rainbow-components"
import ModalViewPerson from "./ModalViewPerson";

const PersonList = ({ personas, onDelete, onView }) => {
    const [show, setShow] = useState(false);
    const [selected, setSelected] = useState({});
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    
    return (
        <Fragment>
            <TableWithBrowserPagination keyField="Id" data={personas} pageSize={9} >
                <Column header="Nombre" field="Nombre"></Column>
                <Column header="Apellido" field="Apellido"></Column>
                <Column header="DOC" field="Documento"></Column>
                <Column header="Username" field="UserName"></Column>
                <Column type="action">
                    <MenuItem label="Edit" onClick={(event, data) => console.log(`Edited`)} />
                    <MenuItem label="Delete" onClick={(event, data) => console.log(`Delete `)} />
                    <MenuItem label="View" onClick={(event, data) => {
                        setSelected(data);
                        setShow(true);
                        
                    }}/>
                    

                    
                    
                </Column>
            </TableWithBrowserPagination>
            <Modal show={show} onHide={handleClose}>
                        <Modal.Header closeButton>
                        <Modal.Title>Datos</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <ModalViewPerson person = {selected} />
                        </Modal.Body>

            </Modal>
        </Fragment>
    )
}

export default PersonList;