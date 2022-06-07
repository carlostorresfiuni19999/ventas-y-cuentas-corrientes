import { useFormik, Form, Field } from "formik";
import { Fragment } from "react";
import * as Yup from "yup";
import "bootstrap/dist/css/bootstrap.css"
const PersonForm = ({ onSave }) => {
    /*
    const formik = useFormik({
        initialValues: {
            nombre: "",
            apellido: "",
            username: "",
            password: "",
            confirmPassword: "",
            lineaCredito: 0,
            roles: []
        },
        onSubmit: onSave,
        validationSchema: {
            nombre: Yup.string().required("El nombre es obligatiorio"),
            apellido: Yup.string().required("El apellido es obligatorio"),
            username: Yup.string().email("Debe ser de formato Email"),
            password: Yup.string().required("El Password es requerido")
                .uppercase("Debe tener mayusculas")
                .min(6, "Debe tener de longitud de caracteres 6"),
            confirmPassword: Yup.string().required("El Password es requerido")
                .uppercase("Debe tener mayusculas")
                .min(6, "Debe tener de longitud de caracteres 6"),
                

            roles: Yup.array().required("Los roles son requeridos")
        }
    });
    */
    return (
        <Fragment>
            <form>
                <div className="row">
                    <div className="col-sm-2">
                        <label htmlFor="Nombre" className="form-label">Nombre</label>
                    </div>
                    <div className="col-sm-2">
                        <input type="text" className="form-control" id="Nombre"></input>
                    </div>
                    <div className="col-sm-2">
                        <label htmlFor="Apellido" className="form-label">Apellido</label>
                    </div>
                    <div className="col-sm-2">
                        <input type="text" className="form-control" id="Apellido"></input>
                    </div>
                    <div className="col-sm-2">
                        <label htmlFor="UserName" className="form-label">Username</label>
                    </div>
                    <div className="col-sm-2">
                        <input type="text" className="form-control" id="UserName"></input>
                    </div>
                </div>

                <div className="row">
                    <div className="col-sm-3">
                        <label htmlFor="Password" className="form-label">Password</label>
                    </div>
                    <div className="col-sm-3">
                        <input type="password" className="form-control" id="Password"></input>
                    </div>

                    <div className="col-sm-3">
                        <label htmlFor="ConfirmPassword" className="form-label">ConfirmPassword</label>
                    </div>
                    <div className="col-sm-3">
                        <input type="password" className="form-control" id="ConfirmPassword"></input>
                    </div>
                </div>

                <div className="row">
                    <div className="col-sm-3">
                        <label htmlFor="" className="form-label">Linea de Credito</label>
                    </div>
                    <div className="col-sm-3">
                        <input type="number" min={0} className="form-control" id="Linea de Credito"></input>
                    </div>
                    <div className="col-sm-6">
                    <div role="group" aria-labelledby="checkbox-group">
                        <label>
                            <input type="checkbox"  value="Cliente" />
                            Cliente
                        </label>
                        <label>
                            <input type="checkbox"  value="Vendedor" />
                            Vendedor
                        </label>
                        <label>
                            <input type="checkbox" value="Cajero" />
                            Cajero
                        </label>
                    </div>
                    </div>

                </div>
            </form>
        </Fragment>
    )

}

export default PersonForm;