import { Fragment, useState } from "react"
import { Modal } from "react-bootstrap";
import { Column, TableWithBrowserPagination, MenuItem } from "react-rainbow-components"
import FormSetPassword from "./FormSetPassword";
import ModalViewPerson from "./ModalViewPerson";
import PersonForm from "./PersonForm";

const PersonList = ({ personas, redirect, onEdit, onChangePassword }) => {
    const [show, setShow] = useState(false);
    const [selected, setSelected] = useState({});
    const [modalEdit, setModalEdit] = useState(false);
    const [modalSetPassword, setModalSetPassword] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const handleOpenModal = () => setModalEdit(true);
    const handleCloseModal = () => setModalEdit(false);
    const handleOpenModalSetPassword = () => setModalSetPassword(true);
    const handleCloseModalSetPassword = () => setModalSetPassword(false);
    return (
        <Fragment>
            <TableWithBrowserPagination keyField="Id" data={personas} pageSize={9} >
                <Column header="Nombre" field="Nombre"></Column>
                <Column header="Apellido" field="Apellido"></Column>
                <Column header="DOC" field="Documento"></Column>
                <Column header="Username" field="UserName"></Column>
                <Column type="action">
                    <MenuItem label="Editar" onClick={(event, data) => {
                        setSelected(data);
                        handleOpenModal();
                    }} />

                    <MenuItem label="Ver" onClick={(event, data) => {
                        setSelected(data);
                        handleShow();

                    }} />

                    <MenuItem label="Cambiar Password" onClick={(event, data) => {
                        setSelected(data);
                        handleOpenModalSetPassword();

                    }} />

                </Column>
            </TableWithBrowserPagination>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Datos</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <ModalViewPerson person={selected} />
                </Modal.Body>
            </Modal>

            <Modal show={modalEdit} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Editar</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <PersonForm
                        redirect={redirect}
                        value={selected}
                        editable={true}
                        onEdit={onEdit}
                    />
                </Modal.Body>
            </Modal>

            <Modal show={modalSetPassword} onHide={handleCloseModalSetPassword}>
                <Modal.Header closeButton>
                    <Modal.Title>Cambiar Password</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <FormSetPassword
                        username={selected.UserName}
                        onChangePassword={onChangePassword}
                    />
                </Modal.Body>
            </Modal>
        </Fragment>
    )
}

export default PersonList;