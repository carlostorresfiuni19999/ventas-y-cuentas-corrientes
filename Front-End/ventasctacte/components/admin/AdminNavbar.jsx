import { Navbar, Container, Nav } from "react-bootstrap";
import { Fragment } from "react";

const AdminNavbar = () => {
   
    return (
        <Fragment>
            <Navbar bg="light" expand="lg">
                <Container>
                    <Navbar.Brand href="/admin/users/create">Admin</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link href="/admin/users/create">Usuarios</Nav.Link>
                            <Nav.Link href="#link">Cajas</Nav.Link>
                            
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </Fragment>
    )

}

export default AdminNavbar;