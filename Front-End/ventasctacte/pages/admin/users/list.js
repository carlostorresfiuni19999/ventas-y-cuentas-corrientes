import React, { Fragment, useEffect, useState } from "react"
import PersonList from "../../../components/admin/PersonList";
import AdminNavbar from "../../../components/admin/AdminNavbar";
import ValidateLogin from "../../../API/validateLogin";
import getAllPeople from "../../../API/getAllPeople";
import { Button, Tab, Tabs } from "react-bootstrap";
import { useRouter } from "next/router";
const List = () => {

    const [band, setBand] = useState(true);
    const [personas, setPersonas] = useState([]);
    const [cajeros, setCajeros] = useState([]);
    const [vendedores, setVendedores] = useState([]);
    const [key, setKey] = useState("Clientes");
    const router = useRouter();




    const redirect = _ => {
        router.push("create");
        setBand(false);
    }

    const buttonStyle = {
        marginTop: "4%",
        marginLeft: "5%",
        marginBottom: "2%"
    }
    useEffect(() => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        getAllPeople(token.access_token)
            .then(r => r.text())
            .then(r => JSON.parse(r))
            .then(r => {
                if (band) {
                    console.log(r);
                    setPersonas(r.filter(p => {
                        for (let rol of p.Roles) {
                            if (rol === "Cliente") return true;
                        }
                        return false;
                    }));
                

                    setVendedores(r.filter(p => {
                        for (let rol of p.Roles) {
                            if (rol === "Vendedor") return true;
                        }
                        return false;
                    }));
                    
                    setCajeros(r.filter(p => {
                        for (let rol of p.Roles) {
                            if (rol === "Cajero") return true;
                        }
                        return false;
                    }));

                    
                    setBand(false);

                }
            })
            .catch(e => console.log(e));


        

        return () => setBand(false);
    }, [band, cajeros, personas, vendedores]);

    return (
        <Fragment>
            <AdminNavbar />

            <div style={buttonStyle}>
                <Button variant="primary" onClick={redirect}>
                    Agregar
                </Button>
            </div>

            <Tabs
                id="controlled-tab-example"
                activeKey={key}
                onSelect={(k) => setKey(k)}
                className="mb-3"
            >
                <Tab eventKey="Clientes" title="Clientes">
                    <PersonList
                        style={{ marginLeft: "30px", marginRight: "30px" }}
                        personas={personas}
                    />
                </Tab>
                <Tab
                    eventKey="Vendedores"
                    title="Vendedores">
                    <PersonList
                        style={{ marginLeft: "30px", marginRight: "30px" }}
                        personas={vendedores}
                    />
                </Tab>
                <Tab eventKey="Cajeros"
                    title="Cajeros"
                >
                    <PersonList
                        style={{ marginLeft: "30px", marginRight: "30px" }}
                        personas={cajeros}
                    />

                </Tab>

            </Tabs>

        </Fragment>
    )
}
export default List;
