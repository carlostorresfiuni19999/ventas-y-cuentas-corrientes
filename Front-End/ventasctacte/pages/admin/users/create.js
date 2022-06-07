import React, { Fragment, useEffect, useState } from "react"
import 'bootstrap/dist/css/bootstrap.min.css';
import PersonList from "../../../components/admin/PersonList";
import AdminNavbar from "../../../components/admin/AdminNavbar";
import getPersonas from "../../../API/getPersonas";
import PersonForm from "../../../components/admin/PersonForm";
const Create = () =>{
    const [personas, setPersonas] = useState([]);
    const loadPeople = (setState)=> {
        const token = JSON.parse(sessionStorage.getItem("token"));
        getPersonas(token.access_token)
        .then(r => r.text())
        .then(r => JSON.parse(r))
        .then(r => setState(r))
        .catch(e => console.log(e));
    }
    
    useEffect(() => {
        loadPeople(setPersonas);
    }, []);

    useEffect(() => {
        loadPeople(setPersonas);
    }, [personas]);

    const handleView = (id) => console.log(id);
    const handleDelete = (id) => console.log(id);
    const divStyle = {
        marginTop:"20%",
        marginBottom:"20%",
        marginLeft: "5%",
        marginRight: "5%"
    }
    return(
        <Fragment>
            <AdminNavbar />
            <PersonForm onSave={()=> console.log("saved")}/>
            <div style={divStyle}>
                <PersonList personas = {personas} onView= {handleView} onDelete = {handleDelete}/>
            </div>
            
        </Fragment>
    )
}
export default Create;
