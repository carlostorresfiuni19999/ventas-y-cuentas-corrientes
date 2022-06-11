import { Form, Field, Formik, ErrorMessage } from "formik";
import { Fragment } from "react";
import * as Yup from "yup";
import "bootstrap/dist/css/bootstrap.css"
import { Col, Row, Button } from "react-bootstrap";
import PostPersona from "../../API/postPersona";

const PersonForm = ({ onRedirect }) => {
    const initialValues = {
        Nombre: "",
        Apellido: "",
        Doc: "",
        DocumentoTipo: "",
        LineaDeCredito: 0,
        Roles: [],
        Telefono: "",
        UserName: "",
        password: "",
        confirmPassword: ""
    };

    const validationSchema = Yup.object({
        Nombre: Yup.string().required("requerido"),
        Apellido: Yup.string().required("requerido"),
        Doc: Yup.string().required("requerido"),
        DocumentoTipo: Yup.string().required("requerido"),
        LineaDeCredito: Yup.number()
            .min(0, "Valor minimo 0")
            .required("requerido"),
        Roles: Yup.array().required("requeridos"),
        Telefono: Yup.string().required("requerido"),
        UserName: Yup.string().email("Formato de email no valido").required("Requerido"),
        password: Yup.string()
            .matches(
                /^.*(?=.{8,})((?=.*[!@#$%^&*()\-_=+{};:,<.>]){1})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$/,
                "Por lo menos 8 caracteres, Una Mayuscula y Minuscula, un caracter especial y un numerico"
            )
            .required("Requerido"),
        confirmPassword: Yup.string()
            .when("password", {
                is: val => (val && val.length > 0 ? true : false),
                then: Yup.string().oneOf(
                    [Yup.ref("password")],
                    "No coinciden los password"
                )
            })
    });

    const handleClick = values => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        PostPersona(values, token.access_token);
        console.log(values);
        onRedirect();
    };

    return (
        <Fragment>
            <Formik
                initialValues={initialValues}
                validationSchema={validationSchema}
                onSubmit={handleClick}
            >
                <Form>
                    <Row>
                        <Col sm={2}>
                            <label htmlFor="Nombre">Nombre</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="Nombre" type="text" />
                        </Col>
                        <Col sm={2}>
                            <label htmlFor="Apellido">Apellido</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="Apellido" type="text" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={2}>
                        </Col>
                        <Col sm={4}>
                            <ErrorMessage name="Nombre" />
                        </Col>
                        <Col sm={2}>
                        </Col>
                        <Col sm={4}>
                            <ErrorMessage name="Apellido" />
                        </Col>
                    </Row>

                    <Row>
                        <Col sm={2}>
                            <label htmlFor="Doc">Documento</label>
                        </Col>
                        <Col md={4}>
                            <Field name="Doc" type="text" />
                        </Col>
                        <Col md={2}>
                            <label htmlFor="DocoumentoTipo">Tipo</label>
                        </Col>
                        <Col md={4}>
                            <div style={{ display: "inLine" }}>
                                <Field type="radio" name="DocumentoTipo" value="CI" />
                                <label>CI</label>
                                <Field type="radio" name="DocumentoTipo" value="RUC" />
                                <label>RUC</label>
                                <Field type="radio" name="DocumentoTipo" value="DNI" />
                                <label>DNI</label>
                            </div>
                        </Col>
                    </Row>

                    <Row>
                        <Col sm={2}>
                        </Col>
                        <Col sm={4} >
                            <ErrorMessage name="Doc" />
                        </Col>
                        <Col sm={2}>
                        </Col>
                        <Col sm={4} >
                            <ErrorMessage name="DocumentoTipo" />
                        </Col>
                    </Row>

                    <Row>
                        <Col sm={2}>
                            <label name="LineaDeCredito">Creditos</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="LineaDeCredito" type="number" />
                        </Col>
                        <Col sm={6}>
                            <Row>
                                <Col sm={2}></Col>
                                <label name="Roles">Roles</label>
                            </Row>
                            <Row>
                                <div id="checkbox-group" style={{ display: "inLine" }}>
                                    <div role="group" aria-labelledby="checkbox-group">
                                        
                                        <label name="Roles">
                                            <Field type="checkbox" name="Roles" value="Cliente" />
                                            Cliente</label>
                                        
                                        <label name="Roles">
                                        <Field type="checkbox" name="Roles" value="Vendedor" />
                                            Vendedor</label>
                                        
                                        <label name="Roles">
                                        <Field type="checkbox" name="Roles" value="Cajero" />
                                            Cajero</label>
                                    </div>

                                </div>
                            </Row>
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={2}>
                        </Col>
                        <Col sm={2}>
                            <ErrorMessage name="LineaDeCredito" />
                        </Col>
                        <Col sm={2}>
                        </Col>
                        <Col sm={6}>
                            <ErrorMessage name="Roles" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={2}>
                            <label htmlFor="UserName">Username</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="UserName" type="text" />
                        </Col>
                        <Col sm={2}>
                            <label htmlFor="Telefono">Telefono</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="Telefono" type="text" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={2} />
                        <Col sm={4}>
                            <ErrorMessage name="UserName" />
                        </Col>
                        <Col sm={2} />
                        <Col sm={4}>
                            <ErrorMessage name="Telefono" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={2}>
                            <label htmlFor="password">Password</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="password" type="password" />
                        </Col>
                        <Col sm={2}>
                            <label htmlFor="confirmPassword">Confirm Password</label>
                        </Col>
                        <Col sm={4}>
                            <Field name="confirmPassword" type="password" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={2} />
                        <Col sm={4}>
                            <ErrorMessage name="password" />
                        </Col>
                        <Col sm={2} />
                        <Col sm={4}>
                            <ErrorMessage name="confirmPassword" />
                        </Col>
                    </Row>
                    <Button type="input">Guardar</Button>


                </Form>
            </Formik>

        </Fragment>
    )


}

export default PersonForm;