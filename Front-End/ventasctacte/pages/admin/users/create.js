import { useRouter } from "next/router";
import { Fragment, useEffect } from "react";
import ValidateLogin from "../../../API/validateLogin";
import AdminNavbar from "../../../components/admin/AdminNavbar";
import PersonForm from "../../../components/admin/PersonForm";

const Create = () => {
    const router = useRouter();

    const handleRedirect = () => router.push("/admin/users/list");

    useEffect(() => {
        const notValid = () => router.push("../../LogIn");
        ValidateLogin(
            JSON.parse(sessionStorage.getItem("token")),
            "Administrador",
            () => {},
            notValid
        ); 
        
    });
    return(
        <Fragment>
            <AdminNavbar />
            <h1>Crear Usuario</h1>
            <PersonForm  onRedirect={handleRedirect}/>
        </Fragment>
    )
}

export default Create;