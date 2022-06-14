import { Form, Field, Formik, ErrorMessage } from "formik";
import { Fragment } from "react";
import * as Yup from "yup";
import "bootstrap/dist/css/bootstrap.css"
import { Col, Row, Button } from "react-bootstrap";


const PersonFormUpdate = ({ onEdit, editable = false, value = null }) => {

    const initialValues = {
        Nombre: editable ? value.Nombre : "",
        Apellido: editable ? value.Apellido : "",
        Doc: editable ? value.Documento : "",
        DocumentoTipo: editable ? value.DocumentoTipo : "",
        LineaDeCredito: editable ? value.LineaDeCredito : 0,
        Roles: editable ? value.Roles : [],
        Telefono: editable ? value.Telefono : "",
        UserName: editable ? value.UserName : "",
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
        UserName: Yup.string().email("Formato de email no valido").required("Requerido")
    });

    const handleClick = values => {
        console.log(values);
        onEdit(value.UserName, values);
    };


    const renderEdit = () => {
        return (
            <Fragment>
                <Formik
                    initialValues={initialValues}
                    validationSchema={validationSchema}
                    onSubmit={handleClick}
                >
                    <Form>
                        <Row>
                            <Col sm={6}>
                                <label htmlFor="Nombre">Nombre</label>
                            </Col>

                            <Col sm={6}>
                                <label htmlFor="Apellido">Apellido</label>
                            </Col>

                        </Row>
                        <Row>
                            <Col sm={6}>
                                <Field name="Nombre" type="text" />
                            </Col>
                            <Col sm={6}>
                                <Field name="Apellido" type="text" />
                            </Col>

                        </Row>
                        <Row>

                            <Col sm={6}>
                                <ErrorMessage name="Nombre" />
                            </Col>

                            <Col sm={6}>
                                <ErrorMessage name="Apellido" />
                            </Col>
                        </Row>
                        <Row>
                            <Col sm={6}>
                                <label htmlFor="Doc">Documento</label>
                            </Col>
                            <Col md={6}>
                                <label htmlFor="DocoumentoTipo">Tipo</label>
                            </Col>
                        </Row>
                        <Row>

                            <Col md={6}>
                                <Field name="Doc" type="text" />
                            </Col>

                            <Col md={6}>
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
                            <Col sm={6} >
                                <ErrorMessage name="Doc" />
                            </Col>
                            <Col sm={6} >
                                <ErrorMessage name="DocumentoTipo" />
                            </Col>
                        </Row>
                        <Row>

                        </Row>
                        <Row>
                            <Col sm={6}>
                                <label name="LineaDeCredito">Creditos</label>
                            </Col>
                            <Col sm={6}>
                                <label>Roles</label>
                            </Col>

                        </Row>

                        <Row>

                            <Col sm={6}>
                                <Field name="LineaDeCredito" type="number" min={0} />
                            </Col>
                            <Col sm={6}>
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

                            </Col>
                        </Row>
                        <Row>

                            <Col sm={6}>
                                <ErrorMessage name="LineaDeCredito" />
                            </Col>

                            <Col sm={6}>
                                <ErrorMessage name="Roles" />
                            </Col>
                        </Row>

                        <Row>
                            <Col sm={6}>
                                <label htmlFor="UserName">Username</label>
                            </Col>
                            <Col sm={6}>
                                <label htmlFor="Telefono">Telefono</label>
                            </Col>
                        </Row>

                        <Row>

                            <Col sm={6}>
                                <Field name="UserName" type="text" />
                            </Col>

                            <Col sm={6}>
                                <Field name="Telefono" type="text" />
                            </Col>
                        </Row>
                        <Row>

                            <Col sm={6}>
                                <ErrorMessage name="UserName" />
                            </Col>

                            <Col sm={6}>
                                <ErrorMessage name="Telefono" />
                            </Col>
                        </Row>
                        <Button type="input">Editar</Button>


                    </Form>
                </Formik>

            </Fragment>)
    }

    return renderEdit();



}

export default PersonFormUpdate;