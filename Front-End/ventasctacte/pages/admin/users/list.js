import React, { Fragment, useEffect, useState} from "react"
import 'bootstrap/dist/css/bootstrap.min.css';
import PersonList from "../../../components/admin/PersonList";
import AdminNavbar from "../../../components/admin/AdminNavbar";
import getPersonas from "../../../API/getPersonas";
import ValidateLogin from "../../../API/validateLogin";
import { Button } from "react-bootstrap";
import { useRouter } from "next/router";
const List = () => {
 
    const [band, setBand] = useState(true);
    const [personas, setPersonas] = useState([]);
    const router = useRouter();
    
    const loadPeople = () => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        getPersonas(token.access_token)
            .then(r => r.text())
            .then(r => JSON.parse(r))
            .then(r => {
                band && setPersonas(r)
            })
            .catch(e => console.log(e));
    }

    const redirect = _ => {
        router.push("create");
        setBand(false);
    }

    const buttonStyle = {
        marginTop: "4%",
        marginLeft: "5%",
        marginBottom:"2%"
    }
    useEffect(() => {
        const notValid = () => router.push("../../LogIn");
        
        ValidateLogin(
             
            JSON.parse(sessionStorage.getItem("token")),
            "Administrador",
            loadPeople,
            notValid
        ) 
        return () => setBand(false);
    });
    
    return (
        <Fragment>
            <AdminNavbar />
           
                    <div style={buttonStyle}>
                    <Button variant="primary" onClick={redirect}>
                        Agregar
                    </Button>
                    </div>
                    
            
             <PersonList style ={{marginLeft:"30px", marginRight:"30px"}} personas={personas} />

        </Fragment>
    )
}
export default List;
