import { Navbar, Container, Nav, NavDropdown } from "react-bootstrap";
import { Fragment } from "react";

const AdminNavbar = () => {

    return (
        <Fragment>
            <Navbar bg="light" expand="lg">
                <Container>
                    <Navbar.Brand href="/admin/users/list">Admin</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link href="/admin/users/list">Usuarios</Nav.Link>
                            <Nav.Link href="/admin/cajas/list">Cajas</Nav.Link>
                            <NavDropdown title="Historicos" id ="basic-nav-dropdown">
                                <NavDropdown.Item href="/ndp/Lista">Pedidos</NavDropdown.Item>
                                <NavDropdown.Item href="/factura/Lista">
                                   Facturas
                                </NavDropdown.Item>
                            </NavDropdown>
                            <Nav.Link href = "/LogIn" onClick= {sessionStorage.clear()}>Log Out</Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </Fragment>
    )

}

export default AdminNavbar;